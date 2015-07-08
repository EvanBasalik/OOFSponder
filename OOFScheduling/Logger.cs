using System;
using System.Diagnostics;

namespace OOFSponder
{
    public static class Logger
    {
        public static void Error(string message)
        {
            WriteEntry(message, "error", new System.Diagnostics.StackFrame(1).GetMethod().Name);
        }

        public static void Error(Exception ex)
        {
            WriteEntry(ex.Message, "error", new System.Diagnostics.StackFrame(1).GetMethod().Name);
        }

        public static void Warning(string message)
        {
            WriteEntry(message, "warning", new System.Diagnostics.StackFrame(1).GetMethod().Name);
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
        }
    }
}
