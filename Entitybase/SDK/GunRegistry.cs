using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

class GunRegistry
{
    public static Dictionary<string, Gun> Guns = new Dictionary<string, Gun>()
    {
        {
            "Assault Rifle", new Gun()
            {
                Firerate = 450, // ingame firerate (rust tells you)
                Repeatable = true, // does it repeatedly shoot when held?
                RecoilVec = new Point(-2, 4), // in pixels for a 1K monitor (I might scale this depending on the monitor later..)
                RecoilVecZoom = new Point(-3, 7) // in pixels for a 1K monitor ZOOMED (might scale too)
            }
        },
        {
            "M92 Pistol", new Gun() // I'll write some code to smooth this out later
            {
                Firerate = 400,
                Repeatable = false,
                RecoilVec = new Point(-3, 36),
                RecoilVecZoom = new Point(0, 19),
            }
        }
    };

    public static Gun Get(int index) => Guns.ElementAtOrDefault(index).Value;

    public static Gun Get(string name)
    {
        if (Guns.TryGetValue(name, out Gun gun))
            return gun;
        else
        {
            Console.WriteLine($"Invalid gun type caught {name}, GunRegistry.cs");
            return null;
        }
    }
}