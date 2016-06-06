using System;
using Foundation;
using TextStyles.iOS;
using System.Collections.Generic;
using TextStyles.Core;

namespace System
{
	public static class StringExtensions
	{
		// Convert an HTML string to NSAttributedString

		public static NSAttributedString ToAttributedString (this string target, List<CssTagStyle> customStyles = null, bool useExisting = true)
		{
			return TextStyle.Main.CreateHtmlString (target, customStyles, useExisting);
		}

		public static NSAttributedString ToAttributedString (this string target, TextStyle instance, List<CssTagStyle> customStyles = null, bool useExisting = true)
		{
			return instance.CreateHtmlString (target, customStyles, useExisting);
		}
	}
}

