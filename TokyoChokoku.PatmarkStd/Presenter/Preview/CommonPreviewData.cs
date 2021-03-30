using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.Patmark.TextData;
using TokyoChokoku.Patmark.EmbossmentKit;
using TokyoChokoku.FieldModel;
using TokyoChokoku.MachineModel;
using TokyoChokoku.Patmark.Settings;

namespace TokyoChokoku.Patmark.Presenter.Preview
{
    public interface ICommonPreviewData
    {
        /// <summary>
        /// Contentsが空の時 true を返します．
        /// </summary>
		bool IsEmpty { get; }

        EmbossArea EmbossArea { get; }

		/// <summary>
		/// 文字の大きさ
		/// </summary>
		/// <value>The size of the text.</value>
        EmbossmentTextSize TextSize { get; }

        /// <summary>
        /// 内容
        /// </summary>
        /// <value>The contents.</value>
        IList<FieldText>   Contents { get; }
            
		/// <summary>
		/// ページ数を取得する
		/// </summary>
		/// <value>The number of pages</value>
		int CountPages { get; }

        /// <summary>
        /// 指定したページを持つか調べる．
        /// </summary>
        bool HasPage(int page);

        /// <summary>
        /// 指定したページの内容を取得します．
        /// 
        /// </summary>
        /// <returns>A contents of the page or the default.</returns>
        /// <param name="page">A contents of the page.</param>
        /// <param name="def">the default.</param>
        FieldText GetPageOrDefault(int page, string def = "");
    }

    public class EmptyPreviewData: ICommonPreviewData 
    {
        public bool               IsEmpty      { get; } = true;
        public EmbossArea         EmbossArea   { get; } = new EmbossArea(15, 15);
        public EmbossmentTextSize TextSize     { get; } = TextSizeLevel.Medium.DefaultValueFixed().ToEmbossmentTextSize();
        public IList<FieldText>   Contents     { get; } = ImmutableList.Create<FieldText>();
        public int                CountPages   { get; } = 0;

        public bool          HasPage(int page) 
        {
            return false;
        }

        public FieldText          GetPageOrDefault(int page, string def = "")
        {
            return FieldText.Parseless(def);
        } 
    }

    public abstract class WrappedPreviewData: ICommonPreviewData
    {
        readonly ICommonPreviewData wrapped;

        public bool               IsEmpty      { get{ return wrapped.IsEmpty;      } }
        public EmbossArea         EmbossArea   { get{ return wrapped.EmbossArea; } }
        public EmbossmentTextSize TextSize     { get{ return wrapped.TextSize;     } }
        public IList<FieldText>   Contents     { get{ return wrapped.Contents;     } }
        public int                CountPages   { get{ return wrapped.CountPages;   } }

        public bool          HasPage(int page) 
        {
            return wrapped.HasPage(page);
        }

        public FieldText        GetPageOrDefault(int page, string def = "")
        {
            return wrapped.GetPageOrDefault(page, def);
        }

        protected WrappedPreviewData(ICommonPreviewData wrapped) 
        {
            this.wrapped = wrapped ?? throw new NullReferenceException();
        }

    }

    public class CommonPreviewData : ICommonPreviewData
    {
        public bool               IsEmpty      { get{ return CountPages == 0; } }
        public EmbossArea         EmbossArea   { get; }
		public EmbossmentTextSize TextSize     { get; }
		public IList<FieldText>   Contents     { get; }
        public int                CountPages   { get; }

        public bool               HasPage(int page) 
        {
            return 0 <= page && page < CountPages;
        }

        public FieldText        GetPageOrDefault(int page, string def = "")
        {
            return HasPage(page) ? Contents[page] : FieldText.Parseless(def);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TokyoChokoku.Patmark.Presenter.Preview.CommonPreiewData"/> class.
        /// </summary>
        /// <param name="isEmpty">If set to <c>true</c> is empty.</param>
        /// <param name="textSize">Text size.</param>
        /// <param name="contents">Contents.</param>
        public CommonPreviewData(EmbossArea area, EmbossmentTextSize textSize, IList<FieldText> contents)
		{
            if (contents == null)
                throw new NullReferenceException();

			TextSize = textSize;
			Contents = contents.ToImmutableList();
            CountPages = Contents.Count;
            EmbossArea = area;
		}

        //public static CommonPreviewData Sample
        //{
        //    get
        //    {
        //        var size = TextSizeLevel.Medium.DefaultValueFixed().ToEmbossmentTextSize();
        //        var cnt = new FieldText[] {
        //            FieldText.Parseless("たんご"),
        //            FieldText.Parseless("Tango"),
        //            FieldText.Parseless("猫さん"),
        //            FieldText.Parseless("Jirba"),
        //            FieldText.Parseless("Mike")
        //        }.ToImmutableList();


        //        // FIXME: MachineModelSpec との直接依存を切る
        //        MachineModelSpec spec;

        //        try
        //        {
        //            spec = new MachineModelNoRepositoryOfKeyValueStore().PullDirect();
        //        }
        //        catch (Exception)
        //        {
        //            spec = new MachineModelNoRepositoryOfKeyValueStore().PullDirect();
        //        }

        //        return new CommonPreviewData(new EmbossArea(spec.Width, spec.Height), size, cnt);
        //    }
        //}
    }

    public class CommonEmbossmentPreviewData: ICommonPreviewData
    {
        readonly EmbossmentData         theData;
        readonly Lazy<IList<FieldText>> smallText;
        readonly Lazy<IList<FieldText>> mediumText;
        readonly Lazy<IList<FieldText>> largeText;


        public bool               IsEmpty      { get{ return theData.IsEmpty; } }
        public MachineModelSpec   MachineModel { get; }
        public EmbossArea         EmbossArea { 
            get {
                return new EmbossArea(MachineModel.Width, MachineModel.Height);
            }
        }
        public EmbossmentTextSize TextSize     { get{ return theData.EmbossmentTextSize; } }
        public IList<FieldText>   Contents     { get{
                return theData.Mode.TextSize.Match(
                    small : (it) => { return smallText .Value; },
                    medium: (it) => { return mediumText.Value; },
                    large : (it) => { return largeText .Value; }
				);
        }}
        public int           CountPages{ get{ return Contents.Count; } }

        public bool          HasPage(int page) 
        {
            return 0 <= page && page < CountPages;
        }

        public FieldText     GetPageOrDefault(int page, string def = "")
        {
            return HasPage(page) ? Contents[page] : FieldText.Parseless(def);
        }

        public CommonEmbossmentPreviewData(MachineModelSpec machineModelSpec, EmbossmentData src)
        {
            MachineModel = machineModelSpec;
            theData    = src ?? throw new NullReferenceException();
            smallText  = new Lazy<IList<FieldText>>(() => { return theData.Text.CreatePagesForSmall(EmbossArea);  });
            mediumText = new Lazy<IList<FieldText>>(() => { return theData.Text.CreatePagesForMedium(EmbossArea); });
            largeText  = new Lazy<IList<FieldText>>(() => { return theData.Text.CreatePagesForLarge(EmbossArea);  });
        }
    }
}
