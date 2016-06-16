using System;
using System.Collections.Generic;
using Styles.Droid.Text;
using Android.Text;
using Styles.Core.Text;

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

