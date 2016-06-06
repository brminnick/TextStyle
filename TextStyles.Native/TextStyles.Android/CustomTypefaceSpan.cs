using Android.Text.Style;
using Android.Graphics;
using System;
using TextStyles.Core;
using Android.Text;

namespace TextStyles.Android
{
	public class CustomTypefaceSpan : TypefaceSpan
	{
		readonly Typeface _typeface;
		readonly TextStyleParameters _style;

		public CustomTypefaceSpan (String family, Typeface typeface, TextStyleParameters style) : base (family)
		{
			_typeface = typeface;
			_style = style;
		}

		public override void UpdateDrawState (TextPaint ds)
		{
			ApplyCustomTypeFace (ds);
		}

		public override void UpdateMeasureState (TextPaint paint)
		{
			ApplyCustomTypeFace (paint);
		}

		void ApplyCustomTypeFace (Paint paint)
		{
			// Color
			if (!String.IsNullOrEmpty (_style.Color))
				paint.Color = Color.White.FromHex (_style.Color);

			// Italic
			if (_style.FontStyle == TextStyleFontStyle.Italic)
				paint.TextSkewX = -.25f;

			// Weight
			paint.FakeBoldText = (_style.FontWeight == TextStyleFontWeight.Bold);

			// Text Decoration
			paint.StrikeThruText = (_style.TextDecoration == TextStyleDecoration.LineThrough);
			paint.UnderlineText = (_style.TextDecoration == TextStyleDecoration.Underline);

			// Letter spacing
#if __ANDROID_21__
			var space = paint.FontSpacing;
			if (Math.Abs (_style.LetterSpacing) > 0) {
				paint.LetterSpacing = _style.LetterSpacing;
			}
#endif

			var flags = paint.Flags | PaintFlags.AntiAlias;// | PaintFlags.SubpixelText;
			if (_style.TextDecoration == TextStyleDecoration.LineThrough)
				flags = flags | PaintFlags.StrikeThruText;
			else if (_style.TextDecoration == TextStyleDecoration.Underline)
				flags = flags | PaintFlags.UnderlineText;

			paint.Flags = flags;

			if (_typeface != null) {
				paint.SetTypeface (_typeface);
			}
		}
	}
}



