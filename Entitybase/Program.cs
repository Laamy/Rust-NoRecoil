using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

// use at your own risk
class Program
{
    Stopwatch akStopwatch = new Stopwatch();

    static void Main(string[] args)
    {
        new Keymap(); // init

        Keymap.globalKeyEvent += OnKeyEvent;

        while (true)
        {
            Thread.Sleep(1);
            Keymap.handle.KeyTick();
        }
    }

    private static void OnKeyEvent(object sender, KeyEvent e)
    {
        if (e.key == Keys.LControlKey && e.vkey == VKeyCodes.KeyDown)
        {
            Console.WriteLine("lctrl down");
        }
    }
}