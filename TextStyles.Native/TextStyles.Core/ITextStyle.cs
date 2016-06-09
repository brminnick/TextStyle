using System;
using System.Collections.Generic;

namespace TextStyles.Core
{
	public interface ITextStyle
	{
		void SetCSS (string css);

		void SetStyles (Dictionary<string, TextStyleParameters> styles);

		List<TextStyleParameters> GetStyles ();

		TextStyleParameters GetStyle (string selector);

		void Refresh ();

		void Dispose ();

		void SetBaseStyle (string baseStyleID, ref List<CssTagStyle> customTags);

		T Create<T> (string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true);

		void Style<T> (T target, string styleID, string text = null, List<CssTagStyle> customTags = null, bool useExistingStyles = true);

		event EventHandler StylesChanged;
	}
}

