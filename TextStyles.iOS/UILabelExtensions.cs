using System;
using TextStyles.iOS;

namespace UIKit
{
	public static class UILabelExtensions
	{
		// style an existing UILabel
		public static void Style (this UILabel target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<UILabel> (target, cssSelector, text);
		}

		public static void Style (this UILabel target, string instanceID, string cssSelector, string text = null)
		{
			TextStyle.Instances [instanceID]?.Style<UILabel> (target, cssSelector, text);
		}

		public static void Style (this UITextField target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<UITextField> (target, cssSelector, text);
		}

		public static void Style (this UITextField target, string instanceID, string cssSelector, string text = null)
		{
			TextStyle.Instances [instanceID]?.Style<UITextField> (target, cssSelector, text);
		}

		public static void Style (this UITextView target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<UITextView> (target, cssSelector, text);
		}

		public static void Style (this UITextView target, string instanceID, string cssSelector, string text = null)
		{
			TextStyle.Instances [instanceID]?.Style<UITextView> (target, cssSelector, text);
		}
	}
}

