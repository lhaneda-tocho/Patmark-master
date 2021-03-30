
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TokyoChokoku.Patmark.Droid.Presenter.Embossment
{
	using EmbossmentKit;
	using FieldEditor;
	using FieldPreview;
	using Ruler;


	public class TextSizeSelecter
	{
		public const int SmallID = Resource.Id.segmentOf_small;
		public const int MediumID = Resource.Id.segmentOf_medium;
		public const int LargeID = Resource.Id.segmentOf_large;
        public RadioGroup segment { get; private set; }

		public event Action<TextSizeSelecter> Changed;

		public TextSizeSelecter(RadioGroup segment)
		{
			this.segment = segment ?? throw new NullReferenceException();
			segment.CheckedChange += (sender, ev) =>
			{
				if (ev.CheckedId == -1)
					return;
				if (Changed != null)
				    Changed(this);
			};
		}

		public void Invalidate()
		{
			segment.RequestLayout();
			segment.Invalidate();
        }

        public void SetEnabled(Boolean enabled)
        {
            for (int i = 0; i < segment.ChildCount; i++)
            {
                ((RadioButton)segment.GetChildAt(i)).Enabled = enabled;
            }
        }

		public TextSizeLevel Level
		{
			get
			{
				var id = segment.CheckedRadioButtonId;
				TextSizeLevel level;
				switch (id)
				{
					case SmallID:
						level = TextSizeLevel.Small;
						break;
					case MediumID:
						level = TextSizeLevel.Medium;
						break;
					case LargeID:
						level = TextSizeLevel.Large;
						break;
					default:
						throw new ArgumentOutOfRangeException(id.ToString());
				}
				return level;
			}

			set
			{
				int id = value.Match(
					small: (it) => SmallID,
					medium: (it) => MediumID,
					large: (it) => LargeID
				);
				segment.ClearCheck();
				segment.Check(id);
			}
		}
	}

	public class ForceSelecter
	{
		public const int WeakID = Resource.Id.segmentOf_weak;
		public const int MediumID = Resource.Id.segmentOf_medium;
		public const int StrongID = Resource.Id.segmentOf_strong;
        public RadioGroup segment { get; private set; }

		public event Action<ForceSelecter> Changed;

		public ForceSelecter(RadioGroup segment)
		{
			this.segment = segment ?? throw new NullReferenceException();
			segment.CheckedChange += (sender, ev) =>
			{
				if (ev.CheckedId == -1)
					return;
				if (Changed != null)
				    Changed(this);
			};
		}

		public void Invalidate()
		{
			segment.RequestLayout();
			segment.Invalidate();
		}

		public void SetEnabled(Boolean enabled)
        {
            for (int i = 0; i < segment.ChildCount; i++)
            {
                ((RadioButton)segment.GetChildAt(i)).Enabled = enabled;
            }
        }

		public ForceLevel Level
		{
			get
			{
				var id = segment.CheckedRadioButtonId;
				ForceLevel level;
				switch (id)
				{
					case WeakID:
						level = ForceLevel.Weak;
						break;
					case MediumID:
						level = ForceLevel.Medium;
						break;
					case StrongID:
						level = ForceLevel.Strong;
						break;
					default:
						throw new ArgumentOutOfRangeException(id.ToString());
				}
				return level;
			}

			set
			{
				int id = value.Match(
					weak: (it) => WeakID,
					medium: (it) => MediumID,
					strong: (it) => StrongID
				);
				segment.ClearCheck();
				segment.Check(id);
			}
		}
	}


	public class QualitySelecter
	{
		public const int DotID = Resource.Id.segmentOf_dot;
		public const int MediumID = Resource.Id.segmentOf_medium;
		public const int LineID = Resource.Id.segmentOf_line;
        public RadioGroup segment { get; private set; }

		public event Action<QualitySelecter> Changed;

		public QualitySelecter(RadioGroup segment)
		{
			this.segment = segment ?? throw new NullReferenceException();
			segment.CheckedChange += (sender, ev) =>
			{
				if (ev.CheckedId == -1)
					return;
				if (Changed != null)
				    Changed(this);
			};
		}

		public void Invalidate()
		{
			segment.RequestLayout();
			segment.Invalidate();
		}

		public void SetEnabled(Boolean enabled){
            for (int i = 0; i < segment.ChildCount; i++)
            {
                ((RadioButton)segment.GetChildAt(i)).Enabled = enabled;
            }
        }

		public QualityLevel Level
		{
			get
			{
				var id = segment.CheckedRadioButtonId;
				QualityLevel level;
				switch (id)
				{
					case DotID:
						level = QualityLevel.Dot;
						break;
					case MediumID:
						level = QualityLevel.Medium;
						break;
					case LineID:
						level = QualityLevel.Line;
						break;
					default:
						throw new ArgumentOutOfRangeException(id.ToString());
				}
				return level;
			}

			set
			{
				int id = value.Match(
					dot: (it) => DotID,
					medium: (it) => MediumID,
					line: (it) => LineID
				);
				segment.ClearCheck();
				segment.Check(id);
			}
		}
	}
}
