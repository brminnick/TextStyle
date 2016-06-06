using System;
using TextStyles.Core;

namespace TextStyles.Droid
{
	public static class TextStyleParametersExtension
	{
		public static bool RequiresHtmlTags (this TextStyleParameters target)
		{
			if (target.TextDecoration != TextStyleDecoration.None)
				return true;

			if (Math.Abs (target.LetterSpacing) > 0)
				return true;

			if (target.FontStyle == TextStyleFontStyle.Italic)
				return true;

			if (target.FontWeight == TextStyleFontWeight.Bold)
				return true;

			return false;
		}
	}
}

