using System;
using TextStyles.iOS;

namespace UIKit
{
	public static class UILabelExtensions
	{
		// style an existing UILabel
		public static void Style (this UILabel target, string cssSelector, string text = null)
		{
			TextStyle.Style<UILabel> (target, cssSelector, text);
		}

		public static void Style (this UITextField target, string cssSelector, string text = null)
		{
			TextStyle.Style<UITextField> (target, cssSelector, text);
		}

		public static void Style (this UITextView target, string cssSelector, string text = null)
		{
			TextStyle.Style<UITextView> (target, cssSelector, text);
		}
	}
}

