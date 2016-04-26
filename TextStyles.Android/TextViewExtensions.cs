using System;
using TextStyles.Android;

namespace Android.Widget
{
	public static class TextViewExtensions
	{
		public static void Style (this TextView target, string cssSelector, string text = null)
		{
			TextStyle.Style<TextView> (target, cssSelector, text);
		}

		public static void Style (this EditText target, string cssSelector, string text = null)
		{
			TextStyle.Style<EditText> (target, cssSelector, text);
		}
	}
}

