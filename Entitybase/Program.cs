using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

using static Keymap;

// use at your own risk
class Program
{
    static Stopwatch akStopwatch = Stopwatch.StartNew();

    public static int selected = 0;

    static void Main(string[] args)
    {
        new Keymap(); // init

        globalKeyEvent += OnKeyEvent;

        while (true)
        {
            Thread.Sleep(1);
            handle.KeyTick(); // tick the global keymap hook

            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

            int cur = 0;
            foreach (var GunPair in GunRegistry.Guns)
            {
                string key = GunPair.Key;

                if (selected == cur)
                    Console.WriteLine($"[X] {key}");
                else Console.WriteLine($"[ ] {key}");
                cur++;
            }
        }
    }

    static bool Active = false;
    static bool Zoomed = false;

    private static void OnKeyEvent(object sender, KeyEvent e) // made on UKN (DONT USE)
    {
        // arrow keys for gun selection here!
        if (e.vkey == VKeyCodes.KeyDown)
        {
            if (e.key == Keys.Up)
            {
                selected--;
                if (selected < 0)
                    selected = GunRegistry.Guns.Count - 1;
            }
            else if (e.key == Keys.Down)
            {
                selected++;
                if (selected >= GunRegistry.Guns.Count)
                    selected = 0;
            }
        }

        // actual norecoil stuff here
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
                if (akStopwatch.ElapsedMilliseconds > (133 / 3)) // gonna smooth it out 3 times per bullet
                {
                    akStopwatch = Stopwatch.StartNew(); // reset timer for next bullet
                    // Console.WriteLine("Bullet fired"); // debugging stuff

                    // bullet is firing so its time to adjust for the recoil AK gives
                    Gun AK = GunRegistry.Get(selected);

                    MoveMouse(Zoomed ? AK.RecoilVecZoom : AK.RecoilVec);
                }
            }
        }
    }
}
