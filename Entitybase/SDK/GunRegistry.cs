using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;

class GunRegistry
{
    public static Dictionary<string, Gun> Guns = new Dictionary<string, Gun>()
    {
        { "Assault Rifle", new Gun(new Point(-2, 4), new Point(-3, 7)) },
        { "M29 Pistol", new Gun(new Point(-1, 12), new Point(-1, 10)) },
    };

    public static Gun Get(int index) => Guns.ElementAtOrDefault(index).Value;

    public static Gun Get(string name)
    {
        Gun _out;

        if (Guns.TryGetValue(name, out _out))
            return _out;
        else
        {
            Console.WriteLine($"Invalid gun type caught {name}, GunRegistry.cs");
            return null;
        }
    }
}