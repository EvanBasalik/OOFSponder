using OOFScheduling;
using System;
using System.Diagnostics;

namespace OOFSponder
{
    public static class Logger
    {
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

            WriteEntry(ScrubMessage(ex.Message) + " due to " + ScrubMessage(ex.InnerException.Message), "error", fr.GetMethod().Name + ":" + st.ToString());
        }

        public static void Error(string message, Exception ex)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);
            WriteEntry(message + ": " + ScrubMessage(ex.Message) + " due to " + ScrubMessage(ex.InnerException.Message), "error", fr.GetMethod().Name + ":" + st.ToString());
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

        public static void Info(string message)
        {
            WriteEntry(ScrubMessage(message), "info", new System.Diagnostics.StackFrame(1).GetMethod().Name);
        }

        public static void InfoPotentialPII(string property, string value)
        {
            if (value.Length >= 21)
            {
                WriteEntry(property + " = " + value.Substring(0, 20), "info", new System.Diagnostics.StackFrame(1).GetMethod().Name);
            }
            else
            {
                WriteEntry(property + " = " + "Value too short to trim", "info", new System.Diagnostics.StackFrame(1).GetMethod().Name);
            }

        }

        private static void WriteEntry(string message, string type, string module)
        {
            Trace.WriteLine(
                    string.Format("{0},{1},{2},{3}",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  type,
                                  module,
                                  ScrubMessage(message)));

            //also send everything to AppInsights
            OOFSponderInsights.Track(
                    string.Format("{0},{1},{2}",
                                  type,
                                  module,
                                  ScrubMessage(message))
                );
        }
    }
}
