using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using static Keymap;

// use at your own risk
class Program
{
    static Stopwatch bulletStopwatch = Stopwatch.StartNew();

    public static int selected = 0;

    static void Main(string[] args)
    {
        new Keymap(); // init

        globalKeyEvent += OnKeyEvent;// global key events

        RustGame.OpenGame();// store important info about rust

        Task.Factory.StartNew(() =>
        {
            while (true)
            {
                Thread.Sleep(1);
                handle.KeyTick(); // tick the global keymap hook
            }
        }); // global keyhooks thread

        Application.Run(new Overlay());// visuals
    }

    public static Font font = new Font("Arial", 16);

    public static void DrawText(Graphics g, string text, Color colour, PointF point)
    {
        Bitmap textBitmap = new Bitmap(Overlay.handle.ClientSize.Width, Overlay.handle.ClientSize.Height);

        using (Graphics textGraphics = Graphics.FromImage(textBitmap))
        {
            // Set up font and brushes for rendering text
            Brush textBrush = new SolidBrush(colour); // Set your desired text color

            // Draw the text onto the bitmap without background
            textGraphics.Clear(Color.Transparent);
            textGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            textGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            textGraphics.DrawString(text, font, textBrush, point);
        }

        // Copy the bitmap onto the form
        g.DrawImage(textBitmap, 0, 0);
    }

    public static void OnUpdate(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

        int centerX = e.ClipRectangle.Width / 2;
        int centerY = e.ClipRectangle.Height / 2;

        Pen pen = new Pen(Color.Red, 2);

        g.DrawLine(pen, centerX - 10, centerY, centerX + 10, centerY);

        g.DrawLine(pen, centerX, centerY - 10, centerX, centerY + 10);

        int y = 2;
        int _i = 0;

        foreach (var gun in GunRegistry.Guns)
        {
            if (_i == selected)
                DrawText(g, $"{gun.Key}", Color.Green, new Point(0, y));
            else
                DrawText(g, $"{gun.Key}", Color.Red, new Point(0, y));

            y += TextRenderer.MeasureText($"{gun.Key}", font).Height + 2;
            _i++;
        }
    }

    static bool Active = false;
    static bool Zoomed = false;

    public static void OnKeyEvent(object sender, KeyEvent e) // made on UKN (DONT USE)
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

            if (Overlay.handle != null)
            {
                Overlay.handle.Invalidate();
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

        Gun SelGun = GunRegistry.Get(selected);

        if (SelGun.Repeatable)
        {
            if (e.key == Keys.LButton && e.vkey == VKeyCodes.KeyHeld) // currently firing the gun
            {
                if (Active)
                {
                    if (bulletStopwatch.ElapsedMilliseconds > (60000 / SelGun.Firerate / 3)) // gonna smooth it out 3 times per bullet
                    {
                        bulletStopwatch = Stopwatch.StartNew(); // reset timer for next bullet
                        // Console.WriteLine("Bullet fired"); // debugging stuff

                        // bullet is firing so its time to adjust for the recoil AK gives
                        MoveMouse(Zoomed ? SelGun.RecoilVecZoom : SelGun.RecoilVec);
                    }
                }
            }
        }
        else
        {
            if (e.key == Keys.LButton && e.vkey == VKeyCodes.KeyDown) // currently firing the gun
            {
                if (Active)
                {
                    MoveMouse(Zoomed ? SelGun.RecoilVecZoom : SelGun.RecoilVec);
                }
            }
        }
    }
}
