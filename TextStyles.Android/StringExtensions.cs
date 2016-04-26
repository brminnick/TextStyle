using System;
using TextStyles.Android;
using System.Collections.Generic;
using TextStyles.Core;
using Android.Text;

namespace System
{
	public static class StringExtensions
	{
		// Convert an HTML string to NSAttributedString

		public static ISpanned ToSpannedString (this string target, string defaultStyle, List<CssTagStyle> customStyles = null, bool useExisting = true)
		{
			return TextStyle.Main.CreateHtmlString (target, defaultStyle, customStyles, useExisting);
		}

		public static ISpanned ToSpannedString (this string target, string instanceID, string defaultStyle, List<CssTagStyle> customStyles = null, bool useExisting = true)
		{
			return TextStyle.Instances [instanceID]?.CreateHtmlString (target, defaultStyle, customStyles, useExisting);
		}
	}
}

