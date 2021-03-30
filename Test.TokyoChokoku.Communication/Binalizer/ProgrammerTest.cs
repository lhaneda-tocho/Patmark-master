using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TokyoChokoku.Communication;
using TokyoChokoku.MarkinBox;
using TokyoChokoku.SerialModule.Counter;

namespace TokyoChokoku.Communication.Test
{
    public class ProgrammerTest
    {
        private EndianFormatter Formatter { get; } = new PatmarkEndianFormatter();

        byte[] GenerateData(int size)
        {
            return Enumerable.Range(0, size)
                .Select(it => (byte)it)
                .ToArray();
        }

        Programmer CreateProgrammer(byte[] data)
        {
            return new Programmer(Formatter, data);
        }

        MemoryAddress SectorC(int address)
        {
            return new MemoryAddress(CommandDataType.C, address);
        }

        MemoryAddress SectorD(int address)
        {
            return new MemoryAddress(CommandDataType.D, address);
        }

        MemoryAddress SectorF(int address)
        {
            return new MemoryAddress(CommandDataType.F, address);
        }

        MemoryAddress SectorL(int address)
        {
            return new MemoryAddress(CommandDataType.L, address);
        }

        MemoryAddress SectorR(int address)
        {
            return new MemoryAddress(CommandDataType.R, address);
        }

        MemoryAddress SectorT(int address)
        {
            return new MemoryAddress(CommandDataType.T, address);
        }

        List<WritingCommandBuilder> Divide(MemoryAddress address, Programmer p)
        {
            Assert.AreEqual(0, p.ByteCount % address.DataType.DataSize(), $"The specified datagram in {nameof(p)} will be lacked last elements when dividing");
            var divider = new WritingCommandDivider(new WritingCommandBuilder(address, p));
            return divider.Invoke();
        }

        Programmer DenseMerge(List<WritingCommandBuilder> denseList)
        {
            if(denseList.Count < 1)
            {
                Assert.Ignore("[Warning] Specified datagram list is empty.");
            }

            // 連続性のチェック
            int headAddress = denseList[0].Addr;
            foreach(var (i, builder) in Enumerable.Range(0, denseList.Count).Zip(denseList))
            {
                var p = builder.Data;
                var address = new MemoryAddress(builder.DataType, builder.Addr);
                Assert.AreEqual(headAddress, address.Address, $"The first element position in {nameof(denseList)}[{i}] is invalid");
                Assert.AreEqual(0, p.ByteCount % address.DataType.DataSize(), $"The specified datagram in {nameof(denseList)}[{i}] has an ignored element at the tail when merging.");
                headAddress += (int)(p.ByteCount / address.DataType.DataSize());
            }

            // 併合
            var merged = new Programmer(denseList[0].Data.TheFormatter, denseList.SelectMany(it => it.Data.GetAllBytes()).ToArray());
            return merged;
        }

        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// コマンド分割処理のロジックを挟んでまた1つのデータとして併合しても、同じデータが取得できることを確認する。
        /// </summary>
        [Test]
        public void SectorCSplitUT()
        {
            var address     = SectorC(0);
            var count       = 2048;

            var elementSize = address.DataType.DataSize();
            var byteSize    = count * elementSize;
            var input       = CreateProgrammer(GenerateData((int)byteSize));
            var divided     = Divide(address, input);
            var output      = DenseMerge(divided);

            Assert.AreEqual(input.GetAllBytes(), output.GetAllBytes());
        }
        
        /// <summary>
        /// コマンド分割処理のロジックを挟んでまた1つのデータとして併合しても、同じデータが取得できることを確認する。
        /// </summary>
        [Test]
        public void SectorDSplitUT()
        {
            var address     = SectorD(0);
            var count       = 2048;

            var elementSize = address.DataType.DataSize();
            var byteSize    = count * elementSize;
            var input       = CreateProgrammer(GenerateData((int)byteSize));
            var divided     = Divide(address, input);
            var output      = DenseMerge(divided);

            Assert.AreEqual(input.GetAllBytes(), output.GetAllBytes());
        }
        
        /// <summary>
        /// コマンド分割処理のロジックを挟んでまた1つのデータとして併合しても、同じデータが取得できることを確認する。
        /// </summary>
        [Test]
        public void SectorLSplitUT()
        {
            var address     = SectorL(0);
            var count       = 2048;

            var elementSize = address.DataType.DataSize();
            var byteSize    = count * elementSize;
            var input       = CreateProgrammer(GenerateData((int)byteSize));
            var divided     = Divide(address, input);
            var output      = DenseMerge(divided);

            Assert.AreEqual(input.GetAllBytes(), output.GetAllBytes());
        }
        
        /// <summary>
        /// コマンド分割処理のロジックを挟んでまた1つのデータとして併合しても、同じデータが取得できることを確認する。
        /// </summary>
        [Test]
        public void SectorFSplitUT()
        {
            var address     = SectorF(0);
            var count       = 2048;

            var elementSize = address.DataType.DataSize();
            var byteSize    = count * elementSize;
            var input       = CreateProgrammer(GenerateData((int)byteSize));
            var divided     = Divide(address, input);
            var output      = DenseMerge(divided);

            Assert.AreEqual(input.GetAllBytes(), output.GetAllBytes());
        }
        
        /// <summary>
        /// コマンド分割処理のロジックを挟んでまた1つのデータとして併合しても、同じデータが取得できることを確認する。
        /// </summary>
        [Test]
        public void SectorRSplitUT()
        {
            var address     = SectorR(0);
            var count       = 2048;

            var elementSize = address.DataType.DataSize();
            var byteSize    = count * elementSize;
            var input       = CreateProgrammer(GenerateData((int)byteSize));
            var divided     = Divide(address, input);
            var output      = DenseMerge(divided);

            Assert.AreEqual(input.GetAllBytes(), output.GetAllBytes());
        }
        
        /// <summary>
        /// コマンド分割処理のロジックを挟んでまた1つのデータとして併合しても、同じデータが取得できることを確認する。
        /// </summary>
        [Test]
        public void SectorTSplitUT()
        {
            var address     = SectorT(0);
            var count       = 2048;

            var elementSize = address.DataType.DataSize();
            var byteSize    = count * elementSize;
            var input       = CreateProgrammer(GenerateData((int)byteSize));
            var divided     = Divide(address, input);
            var output      = DenseMerge(divided);

            Assert.AreEqual(input.GetAllBytes(), output.GetAllBytes());
        }
    }
}
