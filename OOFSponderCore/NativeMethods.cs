using System;
using System.Runtime.InteropServices;

namespace OOFScheduling
{
    // http://sanity-free.org/143/csharp_dotnet_single_instance_application.html
    internal static class NativeMethods
    {
        internal const int SW_NORMAL = 1; // see WinUser.h for definitions
        internal const int SW_RESTORE = 9;
        internal const int SW_MINIMIZE = 6;

        [DllImport("User32", EntryPoint = "FindWindow")]
        internal static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("User32", EntryPoint = "SendMessage")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32", EntryPoint = "SetForegroundWindow")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32", EntryPoint = "SetWindowPlacement")]
        internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("User32", EntryPoint = "GetWindowPlacement")]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        internal struct POINTAPI
        {
            public int x;
            public int y;
        }

        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public POINTAPI ptMinPosition;
            public POINTAPI ptMaxPosition;
            public RECT rcNormalPosition;
        }

    }
}
