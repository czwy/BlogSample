using System;
using System.Runtime.InteropServices;

namespace DragDropAssist
{
    public class WindowNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Win32Point pt);
        
        [DllImport("user32", EntryPoint = "ScreenToClient")]
        public static extern int ScreenToClient(IntPtr hwnd,ref Win32Point lpPoint);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Win32Point
    {
        public Int32 X;
        public Int32 Y;
    };
}
