using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using static Keymap;

// use at your own risk
class Program
{
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
        if (e.key == Keys.LControlKey)
        {
            if (e.vkey == VKeyCodes.KeyDown) Active = true;
            else if (e.vkey == VKeyCodes.KeyUp) Active = false;
        }

        if (e.key == Keys.LButton)
        {
            if (e.vkey == VKeyCodes.KeyDown) Zoomed = true;
            else if (e.vkey == VKeyCodes.KeyUp) Zoomed = false;
        }

        if (e.key == Keys.LButton && e.vkey == VKeyCodes.KeyHeld)
        {
            if (Active)
            {
                if (akStopwatch.ElapsedMilliseconds > (133 / 6)) // gonna smooth it out 6 times per bullet
                {
                    akStopwatch = Stopwatch.StartNew();
                    Console.WriteLine("Bullet fired");

                    if (!Zoomed)
                    {
                        // bullet is firing so its time to adjust for the recoil AK gives without including zoom
                        MoveMouse(-2, 3);
                    }
                    else
                    {
                        MoveMouse(-2, 5);
                    }
                }
            }
        }
    }
}