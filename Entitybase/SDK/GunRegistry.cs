using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

class GunRegistry
{
    public static Dictionary<string, Gun> Guns = new Dictionary<string, Gun>()
    {
        { "Assault Rifle", new Gun(new Point(-2, 4), new Point(-3, 7)) },
        { "M92 Pistol", new Gun(new Point(-1, 12), new Point(-1, 10)) },
        { "MP5", new Gun(new Point(0, 4), new Point(0, 3)) }, // 3 for slightly up 4 for slightly down I need a better way to measure recoil then pixels
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