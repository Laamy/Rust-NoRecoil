using System;
using System.Windows.Forms;
using System.Drawing;

using static RustGame;
using static User32;
using static Debug;

class Overlay : Form
{
    public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

    public static WinEventDelegate overDel;

    public static Overlay handle;

    public Overlay()
    {
        handle = this;

        Log("Initailizing UI..");

        InitializeComponents();

        Log("Initailizing Paint Hooks..");
        Paint += Program.OnUpdate;

        Log("Initailizing Delegate..");

        Focus(); // bring the overlay window into focus

        // click through
        int initStyle = GetWindowLong(Handle, -20);
        SetWindowLong(Handle, -20, initStyle | 0x80000 | 0x20);

        overDel = new WinEventDelegate(OnAdjust);

        Log("Initailizing WinHooks..");

        SetWinEventHook((uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, (uint)SWEH_Events.EVENT_OBJECT_LOCATIONCHANGE, IntPtr.Zero, overDel, GameId, GetWindowThreadProcessId(WinHandle, IntPtr.Zero), (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
        SetWinEventHook((uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, (uint)SWEH_Events.EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, overDel, 0, 0, (uint)SWEH_dwFlags.WINEVENT_OUTOFCONTEXT | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNPROCESS | (uint)SWEH_dwFlags.WINEVENT_SKIPOWNTHREAD);
    }

    public void InitializeComponents()
    {
        // initialize winform here
        TopMost = true; // not needed
        TransparencyKey = BackColor;

        Text = "Rust";

        DoubleBuffered = true; // no tearing cuz this is gay
    }

    public void OnAdjust(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
        // adjust window (this is a hook so hopefully they dont detect it..?)

        //Console.WriteLine("Rust window movement");

        ProcessRectangle rect = GetGameRect();

        int x = rect.Left; // wont do fullscreen yet
        int y = rect.Top;
        int width = rect.Right - rect.Left;
        int height = rect.Bottom - rect.Top;

        SetWindowPos(Handle, IsGameFocusedInsert(), x, y, width, height, 0x40); // might be working
    }
}