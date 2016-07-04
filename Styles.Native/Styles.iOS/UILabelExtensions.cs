using System;
using Styles.iOS.Text;

namespace UIKit
{
	[Foundation.Preserve (AllMembers = true)]
	public static class UILabelExtensions
	{
		// style an existing UILabel
		public static void Style (this UILabel target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<UILabel> (target, cssSelector, text);
		}

		public static void Style (this UILabel target, TextStyle instance, string cssSelector, string text = null)
		{
			instance.Style<UILabel> (target, cssSelector, text);
		}

		public static void Style (this UITextField target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<UITextField> (target, cssSelector, text);
		}

		public static void Style (this UITextField target, TextStyle instance, string cssSelector, string text = null)
		{
			instance.Style<UITextField> (target, cssSelector, text);
		}

		public static void Style (this UITextView target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<UITextView> (target, cssSelector, text);
		}

		public static void Style (this UITextView target, TextStyle instance, string cssSelector, string text = null)
		{
			instance.Style<UITextView> (target, cssSelector, text);
		}
	}
}

