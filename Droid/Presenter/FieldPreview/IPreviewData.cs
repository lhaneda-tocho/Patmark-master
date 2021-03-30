using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using TokyoChokoku.Patmark.Presenter.Preview;
using TokyoChokoku.Patmark.TextData;
using TokyoChokoku.Patmark.EmbossmentKit;
using TextData = TokyoChokoku.Patmark.TextData;
using TokyoChokoku.FieldModel;
using TokyoChokoku.Patmark.MachineModel;

namespace TokyoChokoku.Patmark.Droid.Presenter.FieldPreview
{
	public interface IPreviewData : ICommonPreviewData
	{ }

	sealed class PreviewDataWrapper : WrappedPreviewData, IPreviewData
	{
		public PreviewDataWrapper(ICommonPreviewData data) : base(data) { }
	}

	public static class PreviewDataProvider
	{
		public static IPreviewData Empty  { get; } = CastOrWrap(new EmptyPreviewData());

		public static IPreviewData CastOrWrap(ICommonPreviewData data)
		{ 
			if (data is IPreviewData)
				return (IPreviewData)data;
			else
				return new PreviewDataWrapper(data);
		}

		public static IPreviewData Create(EmbossArea area, EmbossmentTextSize textSize, IList<FieldText> contents)
		{
            return CastOrWrap(new CommonPreviewData(area, textSize, contents));
		}

        public static IPreviewData From(PatmarkMachineModel spec, EmbossmentData data) 
        {
            return CastOrWrap(new CommonEmbossmentPreviewData(spec, data));
        }
	}
}
