using System.Diagnostics;
using System.Text;

namespace JuliHelper
{
    public static class Drawer2
    {
        // getting shapes
        // GetRectangle()
        // GetCircle()
        // GetPolygon()
        // GetLine()



        // filling shapes
        // extruding shapes (rounded corners possible)
        // scaling shapes (for making outlines outside or inside)
        // outlineOutside, outlineInside, outline

        // example
        //public void Test()
        //{
        //    new Primitive(new M_Rectangle(0,0,100,100)).Outline(2f).Draw();
        //}

        public static PrimitiveAreaStrip ToPrimitiveArea(this M_Rectangle rect) => new PrimitiveAreaStrip(rect);
        public static PrimitiveLineRing ToPrimitiveLine(this M_Rectangle rect) => new PrimitiveLineRing(rect);
        public static PrimitiveAreaFan ToPrimitiveArea(this M_Circle circle, int vertexCount) => new PrimitiveAreaFan(circle, vertexCount);
        public static PrimitiveLineRing ToPrimitiveLine(this M_Circle circle, int vertexCount) => new PrimitiveLineRing(circle, vertexCount);
    }
}
