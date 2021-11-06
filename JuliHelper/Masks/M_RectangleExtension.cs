namespace JuliHelper
{
    public static class M_RectangleExtension
    {
        /// <summary>Also considers if any is null.</summary>
        public static bool EqualValue(this M_Rectangle rectA, M_Rectangle rectB)
        {
            if ((rectA == null) != (rectB == null))
                return false;

            if (rectA == null)
                return true;

            return rectA.X == rectB.X
                && rectA.Y == rectB.Y
                && rectA.Width == rectB.Width
                && rectA.Height == rectB.Height;
        }
    }
}
