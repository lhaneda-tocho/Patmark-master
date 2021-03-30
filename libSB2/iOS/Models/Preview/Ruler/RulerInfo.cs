using System;
using System.Collections;
using System.Collections.Generic;

namespace TokyoChokoku.MarkinBox.Sketchbook.iOS
{
    public struct RulerInfo: IEnumerable <double>
    {
        public double Start          { get; set; }
        public double End            { get; set; }
        public double GridScale      { get; set; }

        public RulerInfo (double start, double end)
        {
            var builder = new RulerInfoBuilder {
                RulerStart = start,
                RulerEnd = end,
                MaxGridCount = 12
            };
            this = builder.BuildInfo ();
        }

        public RulerInfo (double start, double end, double scale)
        {
            Start = start;
            End   = end;
            GridScale = scale;
        }

        public double FirstGrid {
            get {
                var div = Start / GridScale;
                var floor = Math.Floor (div);

                // div ~= floor
                if (div - floor < 0.001)
                    return floor * GridScale;
                else
                    return (floor + 1) * GridScale;
            }
        }

        public double LastGrid 
        {
            get {
                var div = End / GridScale;
                var roof = Math.Floor (div) + 1;

                // div ~= roof
                if (roof - div < 0.001)
                    return roof * GridScale;
                else
                    return (roof - 1) * GridScale;
            }
        }

        public int CountOfGrid
        {
            get {
                var len = LastGrid - FirstGrid;
                var count = 1 + len / GridScale;
                if (count <= int.MaxValue)
                    return (int)count;
                
                return int.MaxValue;
            }
        }

        public bool IsStart (double value, double range)
        {
            return value - Start < range && Start - value < 0.001;
        }

        public bool IsEnd (double value, double range)
        {
            return End - value < range && value - End < 0.001;
        }

        public bool IsStartInView (double value, double range, double viewStart, double viewEnd)
        {
            range = range * (End - Start) / (viewEnd - viewStart);
            return IsStart (value ,range);
        }


        public bool IsEndInView (double value, double range, double viewStart, double viewEnd)
        {
            range = range * (End - Start) / (viewEnd - viewStart);
            return IsEnd (value, range);
        }

        public double GridToView (double grid, double viewStart, double viewEnd) {
            return (grid - Start) * (viewEnd - viewStart) / (End - Start);
        }

        IEnumerator<double> IEnumerable<double>.GetEnumerator ()
        {
            return new GridEnumerator (this);
        }

        public IEnumerator GetEnumerator ()
        {
            return new GridEnumerator (this);
        }

        class GridEnumerator : IEnumerator<double>
        {
            RulerInfo info;
            int num;
            int count;

            public GridEnumerator (RulerInfo info)
            {
                this.info = info;
                num = info.CountOfGrid;
                count = -1;
            }

            public double Current {
                get {
                    return count * info.GridScale + info.FirstGrid;
                }
            }

            object IEnumerator.Current {
                get {
                    return Current;
                }
            }

            public void Dispose ()
            {
                return;
            }

            public bool MoveNext ()
            {
                if (count < num) {
                    count++;
                    return true;
                }
                return false;
            }

            public void Reset ()
            {
                count = 0;
            }
        }
    }
}

