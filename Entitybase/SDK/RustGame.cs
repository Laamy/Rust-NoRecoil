using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using static User32;

class RustGame
{
    // information about rust like its process ID

    /// <summary>
    /// Unsigned int (To store the rust handle in for winhooks)
    /// </summary>
    public static uint GameId;

    /// <summary>
    /// Int Pointer (32bit number) (To store the rust window handle in for winhooks, again)
    /// </summary>
    public static IntPtr WinHandle;

    public static void OpenGame()
    {
        // locate rust in the memory
        Process game = Process.GetProcessesByName("RustClient")[0];

        GameId = (uint)game.Id; // game id
        WinHandle = game.MainWindowHandle; // window handle
    }

    public static ProcessRectangle GetGameRect()
    {
        // get window rect into "rect" variable
        ProcessRectangle rect;
        User32.GetWindowRect(WinHandle, out rect);

        // return window dimensions
        return rect;
    }

    public static IntPtr IsGameFocusedInsert()
    {
        var sb = new StringBuilder("Rust".Length + 1);
        GetWindowText(GetForegroundWindow(), sb, "Rust".Length + 1);
        if (sb.ToString() == "Rust")
            return (IntPtr)(-1);
        return (IntPtr)(-2);
    }

    #region Structs
    /// <summary>
    /// Window Rectangle
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ProcessRectangle
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
        public ProcessRectangle(Point position, Point size) // this is most likely wrong
        {
            this.Left = position.X;
            this.Top = position.X + size.X;
            this.Right = position.Y;
            this.Bottom = position.Y + size.Y;

            // Left, Top, Right, Bottom
            // X, X - X, Y, Y - Y

            // Left, Top,
            // Right, Bottom
            // X, X - X,
            // Y, Y - Y
        }
    }
    #endregion
}