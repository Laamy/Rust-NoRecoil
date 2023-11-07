using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static Keymap;

// use at your own risk
class Program
{
    static string detectMePlease = "This is a rust norecoil script, if you see this thing ban this person...";
    
    static Stopwatch akStopwatch = Stopwatch.StartNew();

    static void Main(string[] args)
    {
        new Keymap(); // init

        globalKeyEvent += OnKeyEvent;

        while (true)
        {
            Thread.Sleep(1);
            handle.KeyTick();
        }
    }

    static bool Active = false;
    static bool Zoomed = false;

    private static void OnKeyEvent(object sender, KeyEvent e) // made on UKN (DONT USE)
    {
        if (e.key == Keys.LControlKey) // lets store if LCTRL is held or not so we know if we should move the mouse
        {
            if (e.vkey == VKeyCodes.KeyDown) Active = true;
            else if (e.vkey == VKeyCodes.KeyUp) Active = false;
        }

        if (e.key == Keys.RButton) // lets also store if we're zoomed in or not so we can adjust the movements for that
        {
            if (e.vkey == VKeyCodes.KeyDown) Zoomed = true;
            else if (e.vkey == VKeyCodes.KeyUp) Zoomed = false;
        }

        if (e.key == Keys.LButton && e.vkey == VKeyCodes.KeyHeld) // currently firing the gun
        {
            if (Active)
            {
                if (akStopwatch.ElapsedMilliseconds > (133 / 3)) // gonna smooth it out 6 times per bullet
                {
                    akStopwatch = Stopwatch.StartNew(); // reset timer for next bullet
                    // Console.WriteLine("Bullet fired"); // debugging stuff

                    // bullet is firing so its time to adjust for the recoil AK gives
                    if (!Zoomed) MoveMouse(-5, 8); // these are the best offsets I found for not zoomed
                    else MoveMouse(-6, 12); // these are the best offsets I found for zoomed
                }
            }
        }
    }
}
