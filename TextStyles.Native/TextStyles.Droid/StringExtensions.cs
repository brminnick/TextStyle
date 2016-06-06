using System;
using System.Collections.Generic;
using TextStyles.Core;
using Android.Text;
using TextStyles.Droid;

namespace System
{
	public static class StringExtensions
	{
		// Convert an HTML string to NSAttributedString

		public static ISpanned ToSpannedString (this string target, string defaultStyle, List<CssTagStyle> customStyles = null, bool useExisting = true)
		{
			return TextStyle.Main.CreateHtmlString (target, defaultStyle, customStyles, useExisting);
		}

		public static ISpanned ToSpannedString (this string target, TextStyle instance, string defaultStyle, List<CssTagStyle> customStyles = null, bool useExisting = true)
		{
			return instance.CreateHtmlString (target, defaultStyle, customStyles, useExisting);
		}
	}
}

