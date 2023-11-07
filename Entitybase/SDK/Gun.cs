using System.Drawing;

class Gun
{
    public Point RecoilVecZoom;
    public Point RecoilVec;

    public Gun(Point p, Point pZoom)
    {
        RecoilVec = p;
        RecoilVecZoom = pZoom;
    }
}