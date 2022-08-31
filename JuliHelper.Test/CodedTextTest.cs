//using JuliHelper.Creating;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JuliHelper.Test
{
    [TestClass]
    public class CodedTextTest
    {
        [TestMethod]
        public void Test()
        {
            //string code = "ColorElement(0)(my , (comma))";

            //Creator2 creator = new Creator2();
            //var obj = creator.CreateObject(new ScriptReader(code));


            //Creator creator = new Creator(null);

            //var drawing = new DrawElementCollection(creator, "Hello #c(0)(my , (#c(f00)(red) comma)) #tex(Test)");

            //object list;
            //list = CodedText.GetElements("Hello my , (comma)").ToList();
            //list = CodedText.GetElements("Hello #ColorElement(0)(my , (comma))").ToList();
            //list = CodedText.GetElements("Hello #ColorElement(0)(my , (#ColorElement(f00)(red) comma))");
            //DrawElementCollection drawing = CodedText.GetElements("Hello #c(0)(my , (#c(f00)(red) comma)) #tex(Test)");
            //#tex(Tutorial/Ascend)


            // 1. string
            // 2. elements (string parsed to elements)
            // 3. positioning and scaling
            // () 4. texture

            //Vector2 size = drawing.GetSize(/* align?, font, scale, /*font and other params...?*/);
            //Anchor anchor = Anchor.BottomRight(16, 16);
            //M_Rectangle rect = anchor.Rectangle(size);
            ////Drawer.DrawCoded(
            //drawing.Draw(Anchor.BottomRight(0, 0), 0.5f/* align*/, );// anchor, align, color, font, scale, rotation?, effects?, time
        }
    }
}
