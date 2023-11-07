using System;
using System.Collections.Generic;
using System.Drawing;

class GunRegistry
{
    public static Dictionary<string, Gun> Guns = new Dictionary<string, Gun>()
    {
        { "Assault Rifle", new Gun(new Point(-2, 4), new Point(-3, 7)) },
        { "M29 Pistol", new Gun(new Point(-1, 12), new Point(-1, 10)) },
    };

    public static Gun Get(string name)
    {
        Gun _out;

        if (Guns.TryGetValue(name, out _out))
            return _out;
        else
        {
            Console.WriteLine($"Invalid gun type caught {name}, line 19, GunRegistry.cs");
            return null;
        }
    }
}