using System;
using System.Linq;
using NUnit.Framework;

namespace TokyoChokoku.Patmark.RuleKit.Test
{
    [TestFixture()]
    public class SendRuleTest
    {
        /// <summary>
        /// ValueList が不変であることを確認します。
        /// </summary>
        [Test()]
        public void ValueListShouldBeImmutableTest()
        {
            // should be immutable
            Assert.Throws<NotSupportedException>(() =>
            {
                SendRule.ValueList.Add(SendRule.Size);
            });
        }

        /// <summary>
        /// ValueList に Size オブジェクトが含まれないことを確認します。
        /// </summary>
        [Test()]
        public void ValueListShouldNotContainSizeObjectTest()
        {
            int i = 0;
            foreach (var value in SendRule.ValueList)
            {
                Assert.AreNotSame(SendRule.Size, value, $"ValueList should not contain `Size` object (identity): ValueList[{i}]");
                Assert.AreNotEqual(SendRule.Size, value, $"ValueList should not contain `Size` object (equals): ValueList[{i}]");
                i++;
            }
        }

        /// <summary>
        /// ValueList の要素数を検証します。
        /// </summary>
        [Test()]
        public void ElementCountTest()
        {
            Assert.AreEqual(
                SendRule.Size.Index,
                SendRule.ValueList.Count,
                "SendRule.ValueList.Count should equals to SendRule.Size.Ordinal"
            );
        }

        /// <summary>
        /// 名前の設定が適切であるか確認するテスト。
        /// (変数名と、列挙値の Name プロパティが一致する必要があります)
        /// </summary>
        [Test()]
        public void NameTest()
        {
            var list = SendRule.ValueList.ToList();
            list.Add(SendRule.Size);
            var t = typeof(SendRule);


            foreach (var expectValue in list)
            {
                var name = expectValue.Name;
                var resultValue = t.GetProperty(name).GetValue(null);
                Assert.AreSame(expectValue, resultValue, $"name mismatched (identity): {expectValue} --> {resultValue}");
                Assert.AreEqual(expectValue, resultValue, $"name mismatched (equals): {expectValue} --> {resultValue}");
            }
        }


        /// <summary>
        /// Index の設定が適切であるか確認するテスト。
        /// </summary>
        [Test()]
        public void IndexTest()
        {
            var list = SendRule.ValueList.ToList();
            list.Add(SendRule.Size);

            int expectIndex = 0;
            foreach (var expectValue in list)
            {
                var resultIndex = expectValue.Index;
                Assert.AreEqual(expectIndex, resultIndex, $"invalid index: ValueList[{expectIndex}] != {expectValue}");
                expectIndex++;
            }
        }

        /// <summary>
        /// ID の設定が適切であるか確認するテスト。(一意であることを保証する必要がある)
        /// </summary>
        [Test()]
        public void IDTest()
        {
            var set = SendRule.ValueList.Select(it => it.Id).ToHashSet();

            // ID に重複があった場合は HashSet に変換した際に、要素数が減ります。
            // この特性を利用して、IDの重複を検出します。
            Assert.AreEqual(SendRule.ValueList.Count, set.Count, "Ids are conflicted!!");
        }






    }
}
