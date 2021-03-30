using System;
using System.Linq;
using System.Collections.Generic;
using TokyoChokoku.Patmark.RenderKit.Value;
using TokyoChokoku.Patmark.RenderKit.Transform;
using System.Collections;

namespace TokyoChokoku.Patmark.iOS.FieldCanvasForIOS
{
    /// <summary>
    /// 図形の配置位置を表します．
    /// </summary>
    public class FCPlacement {
        Pos2D    Position { get; }
        Size2D   Scale    { get; }
        Affine2D Rotate   { get; }

        public FCPlacement(Pos2D position, Size2D scale, Affine2D rotate)
        {
            Position = position;
            Scale = scale;
            Rotate = rotate;
        }
    }

    public interface IFCPlaceRule : IEnumerable<FCPlacement> {
        
    }

    public class FCPlaceList: IFCPlaceRule {
        public IList<FCPlacement> List;

        public FCPlaceList(IList<FCPlacement> list) {
            List = list;
        }

        public IEnumerator<FCPlacement> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }
    }

    public class FCHorizontalPlaceRule: IFCPlaceRule
    {
        public int     Count    { get; }
        public double  Stride   { get; }
        public Size2D  CharSize { get; }
        public Size2D  TotalSize {
            get {
                if (Count == 0)
                    return Size2D.Init(0, 0);
                return Size2D.Init(
                    Stride*(Count-1) + CharSize.W,
                    CharSize.H
                );
            }
        }

        FCPlaceList List;

        public FCHorizontalPlaceRule(int count, double stride, Size2D charSize)
        {
            Count    = count;
            Stride   = stride;
            CharSize = charSize;

            var list = new List<FCPlacement>();
            for (int i = 0; i < count; i++) {
                var p = i * Stride;
                var placement = new FCPlacement(
                    Pos2D.Init(p, 0),
                    CharSize,
                    Affine2D.Entity()
                );
            }

            List = new FCPlaceList(list);
        }

        public IEnumerator<FCPlacement> GetEnumerator()
        {
            return ((IFCPlaceRule)List).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IFCPlaceRule)List).GetEnumerator();
        }
    }

    public class FCVerticalPlaceRule : IFCPlaceRule
    {
        public int    Count    { get; }
        public double Stride   { get; }
        public Size2D CharSize { get; }
        public Size2D TotalSize
        {
            get
            {
                if (Count == 0)
                    return Size2D.Init(0, 0);
                return Size2D.Init(
                    CharSize.W,
                    Stride * (Count - 1) + CharSize.H
                );
            }
        }

        FCPlaceList List;

        public FCVerticalPlaceRule(int count, double stride, Size2D charSize)
        {
            Count    = count;
            Stride   = stride;
            CharSize = charSize;

            var list = new List<FCPlacement>();
            for (int i = 0; i < count; i++)
            {
                var p = i * Stride;
                var placement = new FCPlacement(
                    Pos2D.Init(0, p),
                    CharSize,
                    Affine2D.Entity()
                );
            }
            List = new FCPlaceList(list);
        }

        public IEnumerator<FCPlacement> GetEnumerator()
        {
            return ((IFCPlaceRule)List).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IFCPlaceRule)List).GetEnumerator();
        }
    }


}
