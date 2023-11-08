using System.Drawing;

class Gun
{
    public Point RecoilVecZoom = Point.Empty; // zoom recoil adjust
    public Point RecoilVec = Point.Empty; // recoil adjust

    public float Firerate = 0;

    public bool Repeatable = false; // if the gun is like an AK (constantly shoots when held)
}