using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.Windows.Forms;

class Keymap
{
    public static int e = 0;

    public static Keymap handle;
    public static EventHandler<KeyEvent> globalKeyEvent;

    private readonly Dictionary<char, uint> _dBuff = new Dictionary<char, uint>();
    private readonly Dictionary<char, bool> _noKey = new Dictionary<char, bool>();

    private readonly Dictionary<char, uint> _rBuff = new Dictionary<char, uint>();
    private readonly Dictionary<char, bool> _yesKey = new Dictionary<char, bool>();

    public Keymap()
    {
        handle = this;
        for (var c = (char)0; c < 0xFF; c++)
        {
            _rBuff.Add(c, 0);
            _dBuff.Add(c, 0);
            _noKey.Add(c, true);
            _yesKey.Add(c, true);
        }
    }

    public void KeyTick()
    {
        for (var c = (char)0; c < 0xFF; c++)
        {
            _noKey[c] = true;
            _yesKey[c] = false;

            if (GetAsyncKeyState(c))
            {
                if (globalKeyEvent != null)
                    globalKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyHeld));

                _noKey[c] = false;
                if (_dBuff[c] > 0)
                    continue;

                _dBuff[c]++;

                if (globalKeyEvent != null)
                    globalKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyDown));
            }
            else
            {
                _yesKey[c] = true;

                if (_rBuff[c] > 0)
                    continue;

                _rBuff[c]++;

                if (globalKeyEvent != null)
                    globalKeyEvent.Invoke(this, new KeyEvent(c, VKeyCodes.KeyUp));
            }

            if (_noKey[c])
                _dBuff[c] = 0;

            if (!_yesKey[c])
                _rBuff[c] = 0;
        }
    }

    public static void MoveMouse(int dx, int dy) // deltax, deltay
    {
        mouse_event(0x1, dx, dy, 0, 0);
    }

    [DllImport("user32.dll")]
    public static extern bool GetAsyncKeyState(char v);

    [DllImport("user32.dll")]
    public static extern bool GetAsyncKeyState(Keys v);

    [DllImport("user32.dll")]
    public static extern bool GetAsyncKeyState(int v);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
}

public class KeyEvent : EventArgs // flare's key events
{
    public Keys key;
    public VKeyCodes vkey;

    public KeyEvent(char v, VKeyCodes c)
    {
        key = (Keys)v;
        vkey = c;
    }

    public KeyEvent(Keys v, VKeyCodes c)
    {
        key = v;
        vkey = c;
    }
}

public enum VKeyCodes
{
    KeyDown = 0,
    KeyHeld = 1,
    KeyUp = 2
}