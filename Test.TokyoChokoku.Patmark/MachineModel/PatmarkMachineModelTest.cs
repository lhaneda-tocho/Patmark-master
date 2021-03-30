using System;
using TokyoChokoku.Patmark.MachineModel;
using NUnit.Framework;
namespace TokyoChokoku.PatmarkTest.MachineModel
{
    [TestFixture()]
    public class PatmarkMachineModelTest
    {
        [Obsolete("Disabled_A001", error: true)]
        [Test()]
        public void GetWithVersionTest()
        {
            var eldest  = PatmarkVersion.Of(0,0,0);
            var elder   = PatmarkVersion.Of(3, int.MaxValue, int.MaxValue);
            var p1515s  = PatmarkVersion.Of(4,  0, 0);
            var p1515f  = PatmarkVersion.Of(4,  9, int.MaxValue);
            var p3315s  = PatmarkVersion.Of(4, 10, 0);
            var latest  = PatmarkVersion.Of(int.MaxValue, int.MaxValue, int.MaxValue);

            Assert.True(PatmarkMachineModel.FromVersion(eldest) == PatmarkMachineModel.Patmark1515);
            Assert.True(PatmarkMachineModel.FromVersion(elder ) == PatmarkMachineModel.Patmark1515);
            Assert.True(PatmarkMachineModel.FromVersion(p1515s) == PatmarkMachineModel.Patmark1515);
            Assert.True(PatmarkMachineModel.FromVersion(p1515f) == PatmarkMachineModel.Patmark1515);
            Assert.True(PatmarkMachineModel.FromVersion(p3315s) == PatmarkMachineModel.Patmark3315);
            Assert.True(PatmarkMachineModel.FromVersion(latest) == PatmarkMachineModel.Patmark8020);
        }


        [Test]
        public void ContentCheck1515()
        {
            var m = PatmarkMachineModel.Patmark1515;
            var p = m.Profile;
            Assert.AreEqual("Patmark Mini", p.Name());
            Assert.AreEqual(15.0m, p.MaxTextSize());
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void ContentCheck3315()
        {
            var m = PatmarkMachineModel.Patmark3315;
            var p = m.Profile;
            Assert.AreEqual("Patmark", p.Name());
            Assert.AreEqual(15.0m, p.MaxTextSize());
            Console.WriteLine(p.ToString());
        }

        [Test]
        public void ContentCheck8020()
        {
            var m = PatmarkMachineModel.Patmark8020;
            var p = m.Profile;
            Assert.AreEqual("Patmark Plus", p.Name());
            Assert.AreEqual(20.0m, p.MaxTextSize());
            Console.WriteLine(p.ToString());
        }
    }
}
