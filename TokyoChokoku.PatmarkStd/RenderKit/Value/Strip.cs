using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static System.Math;
using TokyoChokoku.Patmark.UnitKit;
using MathNet.Numerics.LinearAlgebra.Double;
using TokyoChokoku.Patmark.RenderKit.Transform;

namespace TokyoChokoku.Patmark.RenderKit.Value
{
    public interface IStrip : IEnumerable<Pos2D>
    {
        bool IsLoop { get; }
        int Count { get; }
        Pos2D this[int index] { get; }
    }


    public abstract class BaseStrip : IStrip
    {
        protected readonly Pos2D[] path;

        public abstract bool IsLoop { get; set; }

        public int Count { get { return path.Length; }}
		
        public BaseStrip(Pos2D[] path)
		{
			if (path == null) throw new NullReferenceException();
			this.path = (Pos2D[])path.Clone();
        }

        public abstract Pos2D this[int index] { get; set; }

        public IEnumerator<Pos2D> GetEnumerator() {
            return path.Cast<Pos2D>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(var i in path) {
                sb.Append(i.ToString());
            }
            return string.Format("[Path: Count={0}, this[]=[{1}]]", Count, sb.ToString());
        }
    }



    public class MutableStrip: BaseStrip
    {
        public override bool IsLoop { get; set; }

        public override Pos2D this[int index]
        {
            get {
                return path[index];
            }
            set {
                path[index] = value;
            }
        }

        public MutableStrip(params Pos2D[] path): base(path){}
    }



    public class Strip : BaseStrip
	{
        private readonly bool isLoopField;
        public override bool IsLoop
        {
            get
            {
                return isLoopField;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override Pos2D this[int index]
        {
            get {
                return path[index];
            }
			set
			{
				throw new NotImplementedException();
            }
        }

        public Strip(bool isLoop, params Pos2D[] path): base(path){
            this.isLoopField = isLoop;
        }

		public Strip(params Pos2D[] path) : this(false, path)
		{

		}
    }
}