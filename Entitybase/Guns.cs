using System.Drawing;

class Guns
{
    public static class AK // assault rifle
    {
        public static Point RecoilVecZoom = new Point(-3, 7);
        public static Point RecoilVec = new Point(-2, 4);
    }

    public static class M29 // m29 pistol
    {
        public static Point RecoilVecZoom = new Point(-1, 10);
        public static Point RecoilVec = new Point(-1, 12);
    }
}