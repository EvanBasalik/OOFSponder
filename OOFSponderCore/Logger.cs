﻿using OOFScheduling;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OOFSponder
{
    public static class Logger
    {
        internal static readonly string LogFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OOFSponder\\OOFSponder.log");
        readonly static int MaxRolledLogCount = 3;
        private static readonly object _lockforlogger = new object();

        //static bool we can use to control whether or not any one round of logging 
        //goes to AppInsights. This is used to allow very detailed local logging
        //while still reducing the AI cost
        internal static bool shouldSendtoAppInsightsL1 = true;
        internal static bool shouldSendtoAppInsightsL2 = true;

        // 1 * 1024 * 1024 = 1M; <- small enough to make easy to share
        // while still covering lots of real-world time
        readonly static int MaxLogSize = 1 * 1024 * 1024;

        public static void Error(string message)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);

            WriteEntry(ScrubMessage(message), "error", fr.GetMethod().Name + ":" + st.ToString());
        }

        public static void Error(Exception ex)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);

            //make sure InnerException exists
            if (ex.InnerException is null)
            {
                WriteEntry(ScrubMessage(ex.Message), "error", fr.GetMethod().Name + ":" + st.ToString());
            }
            else
            {
                WriteEntry(ScrubMessage(ex.Message) + " due to " + ScrubMessage(ex.InnerException.Message), "error", fr.GetMethod().Name + ":" + st.ToString());
            }

        }

        public static void Error(string message, Exception ex)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);

            if (ex.InnerException == null)
            {
                WriteEntry(message + ": " + ScrubMessage(ex.Message), "error", fr.GetMethod().Name + ":" + st.ToString());
            }
            else
            {
                WriteEntry(message + ": " + ScrubMessage(ex.Message) + " due to " + ScrubMessage(ex.InnerException.Message), "error", fr.GetMethod().Name + ":" + st.ToString());
            }
        }

        /// <summary>
        /// Removes any reference to data the should't be visible in logs such as
        /// the user name in AppData folder tree and ???
        /// </summary>
        /// <param name="UnscrubbedMessage"></param>
        private static string ScrubMessage(string UnscrubbedMessage)
        {
            //edge case where we need to scrub reference to the user name coming from the AppData reference
            return UnscrubbedMessage.Replace(Environment.SpecialFolder.ApplicationData.ToString(), "");
        }

        public static void Warning(string message)
        {
            WriteEntry(ScrubMessage(message), "warning", new System.Diagnostics.StackFrame(1).GetMethod().Name);
        }

        public static void Warning(Exception ex)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);
            WriteEntry(ScrubMessage(ex.Message) + " due to " + ScrubMessage(ex.InnerException.Message), "warning", fr.GetMethod().Name + ":" + st.ToString());
        }

        public static void Info(string message, bool SendToAppInsights = true)
        {
            WriteEntry(ScrubMessage(message), "info", new System.Diagnostics.StackFrame(1).GetMethod().Name, SendToAppInsights);
        }

        public static void InfoValue(string infoName, string infoValue, bool sendToAppInsights = true)
        {
            Info(ScrubMessage(infoName + ": " + infoValue), sendToAppInsights);
        }

        public static void InfoPotentialPII(string property, string value)
        {
            if (value.Length >= 26)
            {
                WriteEntry(property + " = " + value.Substring(0, 25), "info", new System.Diagnostics.StackFrame(1).GetMethod().Name);
            }
            else
            {
                WriteEntry(property + " = " + "Value too short to trim", "info", new System.Diagnostics.StackFrame(1).GetMethod().Name);
            }

        }

        //user preferences, logs, and OOF messages folder
        internal static string PerUserDataFolder()
        {
            return Program.AppDataRoamingFolder;
        }

        private static void WriteEntry(string message, string type, string module, bool isNotSensitive = true)
        {
            try
            {
                lock (_lockforlogger) // should this ever be called by multiple threads
                {
                    RollLogFile(LogFileName);

                    //keep the Trace calls for backward compatability with Legacy
#if NETFRAMEWORK
                    Trace.WriteLine(
                            string.Format("{0},{1},{2},{3}",
                                          DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                                          type,
                                          module,
                                          ScrubMessage(message)));
#endif
#if NET8_0_OR_GREATER
                    using (StreamWriter writer = new StreamWriter(LogFileName, true))
                    {
                        writer.WriteLine(
                            string.Format("{0},{1},{2},{3}",
                                          DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                                          type,
                                          module,
                                          ScrubMessage(message)));
                    }
#endif
                }

                //default is to use the global static to control
                //but need the ability to log sensitive information to the log file only
                Debug.WriteLine("SendtoAppInsightsL1: " + shouldSendtoAppInsightsL1);
                Debug.WriteLine("SendtoAppInsightsL2: " + shouldSendtoAppInsightsL2);
                if (isNotSensitive && shouldSendtoAppInsightsL1 && shouldSendtoAppInsightsL2)
                {
                    //also send everything to AppInsights
                    OOFSponderInsights.Track(
                            string.Format("{0},{1},{2}",
                                          type,
                                          module,
                                          ScrubMessage(message))
                        );
                }
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.MissingLogFileMessage,
                        Resources.MissingLogFileMessageBoxTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new FileNotFoundException("Unable to write to log file");
            }

        }

        //https://stackoverflow.com/questions/16949771/using-streamwriter-to-implement-a-rolling-log-and-deleting-from-top
        //check file size and if greater roll to a new file
        private static void RollLogFile(string logFilePath)
        {
            try
            {
                var length = new FileInfo(logFilePath).Length;

                if (length > MaxLogSize)
                {
                    var path = Path.GetDirectoryName(logFilePath);
                    var wildLogName = Path.GetFileNameWithoutExtension(logFilePath) + "*" + Path.GetExtension(logFilePath);
                    var bareLogFilePath = Path.Combine(path, Path.GetFileNameWithoutExtension(logFilePath));
                    string[] logFileList = Directory.GetFiles(path, wildLogName, SearchOption.TopDirectoryOnly);
                    if (logFileList.Length > 0)
                    {
                        // only take files like logfilename.1.log and logfilename.0.log, so there also can be a maximum of 10 additional rolled files (0..9)
                        var rolledLogFileList = logFileList.Where(fileName => fileName.Length == (logFilePath.Length + 2)).ToArray();
                        Array.Sort(rolledLogFileList, 0, rolledLogFileList.Length);
                        if (rolledLogFileList.Length >= MaxRolledLogCount)
                        {
                            File.Delete(rolledLogFileList[MaxRolledLogCount - 1]);
                            var list = rolledLogFileList.ToList();
                            list.RemoveAt(MaxRolledLogCount - 1);
                            rolledLogFileList = list.ToArray();
                        }
                        // move remaining rolled files
                        for (int i = rolledLogFileList.Length; i > 0; --i)
                            File.Move(rolledLogFileList[i - 1], bareLogFilePath + "." + i + Path.GetExtension(logFilePath));
                        var targetPath = bareLogFilePath + ".0" + Path.GetExtension(logFilePath);
                        // move original file
                        File.Move(logFilePath, targetPath);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
