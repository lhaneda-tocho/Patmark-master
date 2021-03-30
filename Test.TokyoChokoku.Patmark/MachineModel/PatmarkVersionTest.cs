using System;
using TokyoChokoku.Patmark.MachineModel;
using NUnit.Framework;
namespace TokyoChokoku.PatmarkTest.MachineModel
{
    [TestFixture()]
    public class PatmarkVersionTest
    {
        [Test()]
        public void VersionOfTest()
        {
            var a = 329;
            var b = 184;
            var c = 102;
            var d = 932;
            var ans = PatmarkVersion.Of(a, b, c, d);
            Assert.True(ans.Count == 4, "size mismatch");
            Assert.True(ans[0] == a);
            Assert.True(ans[1] == b);
            Assert.True(ans[2] == c);
            Assert.True(ans[3] == d);
        }

        [Test()]
        public void CompareTest()
        {
            {
                var a = PatmarkVersion.Of(4, 7,84, 9234);
                var b = PatmarkVersion.Of(5, 4, 0, 2314);
                var c = PatmarkVersion.Of(6, 0,99, 4398);
                var d = PatmarkVersion.Of(5, 4, 0, 2314);
                
                Assert.True(a < b);  Assert.False(b < a);
                Assert.True(b < c);  Assert.False(c < b);
                Assert.False(b < d);  Assert.False(d < b);

                Assert.True(a <= b); Assert.False(b <= a);
                Assert.True(b <= c); Assert.False(c <= b);
                Assert.True(b <= d); Assert.True(d <= b);


                Assert.False(a > b); Assert.True(b > a);
                Assert.False(b > c); Assert.True(c > b);
                Assert.False(b > d); Assert.False(d > b);

                Assert.False(a >= b); Assert.True(b >= a);
                Assert.False(b >= c); Assert.True(c >= b);
                Assert.True(b >= d); Assert.True(d >= b);

                Assert.False(a == b); Assert.False(b == a);
                Assert.False(b == c); Assert.False(c == b);
                Assert.True (b == d); Assert.True (d == b);
            }

            {
                var a = PatmarkVersion.Of(4, 4, 99, 34238);
                var b = PatmarkVersion.Of(4, 5, 1, 423);
                var c = PatmarkVersion.Of(4, 6, 2, 78961);
                var d = PatmarkVersion.Of(4, 5, 1, 423);

                Assert.True(a < b); Assert.False(b < a);
                Assert.True(b < c); Assert.False(c < b);
                Assert.False(b < d); Assert.False(d < b);

                Assert.True(a <= b); Assert.False(b <= a);
                Assert.True(b <= c); Assert.False(c <= b);
                Assert.True(b <= d); Assert.True(d <= b);


                Assert.False(a > b); Assert.True(b > a);
                Assert.False(b > c); Assert.True(c > b);
                Assert.False(b > d); Assert.False(d > b);

                Assert.False(a >= b); Assert.True(b >= a);
                Assert.False(b >= c); Assert.True(c >= b);
                Assert.True(b >= d); Assert.True(d >= b);

                Assert.False(a == b); Assert.False(b == a);
                Assert.False(b == c); Assert.False(c == b);
                Assert.True(b == d); Assert.True(d == b);
            }

            {
                var a = PatmarkVersion.Of(4, 2, 4, 49);
                var b = PatmarkVersion.Of(4, 2, 5, 6341);
                var c = PatmarkVersion.Of(4, 2, 6, 5143);
                var d = PatmarkVersion.Of(4, 2, 5, 6341);

                Assert.True(a < b); Assert.False(b < a);
                Assert.True(b < c); Assert.False(c < b);
                Assert.False(b < d); Assert.False(d < b);

                Assert.True(a <= b); Assert.False(b <= a);
                Assert.True(b <= c); Assert.False(c <= b);
                Assert.True(b <= d); Assert.True(d <= b);


                Assert.False(a > b); Assert.True(b > a);
                Assert.False(b > c); Assert.True(c > b);
                Assert.False(b > d); Assert.False(d > b);

                Assert.False(a >= b); Assert.True(b >= a);
                Assert.False(b >= c); Assert.True(c >= b);
                Assert.True(b >= d); Assert.True(d >= b);

                Assert.False(a == b); Assert.False(b == a);
                Assert.False(b == c); Assert.False(c == b);
                Assert.True(b == d); Assert.True(d == b);
            }
        }
    }
}
