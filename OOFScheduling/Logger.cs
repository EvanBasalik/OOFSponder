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
            WriteEntry(message, "error", fr.GetMethod().Name + ":" + st.ToString());
        }

        public static void Error(Exception ex)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);
            WriteEntry(ex.Message + " due to " + ex.InnerException.Message, "error", fr.GetMethod().Name + ":" + st.ToString());
        }

        public static void Warning(string message)
        {
            WriteEntry(message, "warning", new System.Diagnostics.StackFrame(1).GetMethod().Name);
        }

        public static void Warning(Exception ex)
        {
            StackFrame fr = new StackFrame(1, true);
            StackTrace st = new StackTrace(fr);
            WriteEntry(ex.Message + " due to " + ex.InnerException.Message, "warning", fr.GetMethod().Name + ":" + st.ToString());
        }

        public static void Info(string message)
        {
            WriteEntry(message, "info", new System.Diagnostics.StackFrame(1).GetMethod().Name);
        }

        private static void WriteEntry(string message, string type, string module)
        {
            Trace.WriteLine(
                    string.Format("{0},{1},{2},{3}",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  type,
                                  module,
                                  message));

            //also send everything to AppInsights
            OOFSponderInsights.Track(
                    string.Format("{0},{1},{2}",
                                  type,
                                  module,
                                  message)
                );
        }
    }
}
