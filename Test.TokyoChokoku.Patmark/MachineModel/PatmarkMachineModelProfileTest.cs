using System;
using System.Collections.Generic;
using System.Linq;
using TokyoChokoku.Patmark.MachineModel;
using NUnit.Framework;
namespace TokyoChokoku.PatmarkTest.MachineModel
{

    [TestFixture()]
    public class PatmarkMachineModelProfileTest
    {
        [Test()]
        public void MutableImmutableCopyTest()
        {
            var input = new MutablePatmarkProfile
            {
                NameOrNull        = "Tango",
                MaxTextSizeOrNull = 10.0m,
            };

            var result = input.ToImmutable();

            Assert.AreEqual(input.NameOrNull,        result.NameOrNull       );
            Assert.AreEqual(input.MaxTextSizeOrNull, result.MaxTextSizeOrNull);
        }


        [Test()]
        public void MutableMutableCopyTest()
        {
            var input = new MutablePatmarkProfile
            {
                NameOrNull = "Tango",
                MaxTextSizeOrNull = 10.0m,
            };

            var result = input.ToMutable();

            Assert.AreEqual(input.NameOrNull, result.NameOrNull);
            Assert.AreEqual(input.MaxTextSizeOrNull, result.MaxTextSizeOrNull);
        }




        [Test()]
        public void ProfilePropertyInfoUT()
        {
            var plist = IPatmarkProfile.GetPropertyInfo();
            Console.WriteLine("=== Name List ===");
            foreach (var p in plist)
            {
                Console.WriteLine(p.Name);
            }

            var resultNameList = plist
                .Select(it => it.Name)
                .ToList();
            resultNameList.Sort();

            var expectedNameList = new List<string> {
                "MaxTextSizeOrNull",
                "NameOrNull"
            };
            expectedNameList.Sort();

            foreach(var (e, r) in expectedNameList.Zip(resultNameList))
            {
                Assert.AreEqual(e, r);
            }
        }
    }
}
