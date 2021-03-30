using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.MarkinBox;

namespace TokyoChokoku.SerialModule.Counter
{
    /// <summary>
    /// SCSetting. (Immutable)
    /// </summary>
    public class SCSetting
    {
        public static readonly SCSetting Default;
        public static readonly IList<SCSetting> DefaultList;

        static SCSetting()
        {
            var count = MBSerial.NumOfSerial;
            var data  = Data.Init();
            Default = new SCSetting(ref data);

            var list = new List<SCSetting>(count);
            for (int i = 0; i < count; i++)
                list.Add(Default);
            DefaultList = list.ToImmutableList();
        }


        public struct Data
        {
            public SCFormat     Format;
            public SCResetRule  ResetRule;
            public SCCountRange Range;
            public byte         SkipNumber;
            public byte         RepeatCount;
            public SCTime       ResetTime;

            public static Data From(MBSerialData sdata) => Init(
                format: sdata.Format.ToSCFormat(),
                resetRule: sdata.ResetRule.ToSCResetRule(),
                range: SCCountRange.Init(max: sdata.MaxValue, min: sdata.MinValue),
                skipNumber: sdata.SkipNumber,
                repeatCount: sdata.RepeatCount,
                resetTime: SCTime.From(sdata.ResetTimeUnion)
            );

            public static Data Init(
                SCFormat format, SCResetRule resetRule, SCCountRange range, byte skipNumber, byte repeatCount, SCTime resetTime
            )
            {
                var ins = new Data();
                ins.Format = format;
                ins.ResetRule = resetRule;
                ins.Range = range;
                ins.SkipNumber = skipNumber;
                ins.RepeatCount = repeatCount;
                ins.ResetTime = resetTime;
                return ins;
            }

            public static Data Init() => Init(
                format      : SCFormat.FillZero,
                resetRule   : SCResetRule.ReachingMaximum,
                range       : SCCountRange.Init(min: 1, max:9999),
                skipNumber  : 1,
                repeatCount : 1,
                resetTime   : SCTime.Init(0,0)
            );

            public SCSetting ToImmutable() {
                return new SCSetting(ref this);
            }

            public MBSerialData ToMBForm() {
                var ins = new MBSerialData();
                ins.Format      = Format.ToMBForm();
                ins.ResetRule   = ResetRule.ToMBForm();
                ins.MaxValue    = Range.MaxValue;
                ins.MinValue    = Range.MinValue;
                ins.SkipNumber  = SkipNumber;
                ins.RepeatCount = RepeatCount;
                ins.ResetTimeUnion = ResetTime.ToMBForm();
                return ins;
            }

            public override string ToString()
            {
                return string.Format("[Format={0}, ResetRule={1}, Range={2}, SkipNumber={3}, RepeatCount={4}, ResetTime={5}]", Format.GetName(), ResetRule.GetName(), Range, SkipNumber, RepeatCount, ResetTime);
            }
        }

        Data All;


        public SCFormat     Format      => All.Format;
        public SCResetRule  ResetRule   => All.ResetRule;
        public SCCountRange Range       => All.Range;
        public byte         SkipNumber  => All.SkipNumber;
        public byte         RepeatCount => All.RepeatCount;
        public SCTime       ResetTime   => All.ResetTime;


        public static SCSetting From(MBSerialData scdata)
        {
            var data = Data.From(scdata);
            return new SCSetting(ref data);
        }

        public SCSetting(ref Data all) {
            All = all;
        }

        public Data ToMutable() => All;
        public MBSerialData ToMBForm() => All.ToMBForm();

        public override string ToString()
        {
            return All.ToString();
        }
    }
}
