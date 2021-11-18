using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            object list;
            //list = CodedText.GetElements("Hello my , (comma)").ToList();
            //list = CodedText.GetElements("Hello #ColorElement(0)(my , (comma))").ToList();
            //list = CodedText.GetElements("Hello #ColorElement(0)(my , (#ColorElement(f00)(red) comma))");
            list = CodedText.GetElements("Hello #c(0)(my , (#c(f00)(red) comma))");
        }
    }
}
