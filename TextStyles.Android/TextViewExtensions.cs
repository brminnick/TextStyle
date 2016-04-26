using System;
using TextStyles.Android;

namespace Android.Widget
{
	public static class TextViewExtensions
	{
		public static void Style (this TextView target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<TextView> (target, cssSelector, text);
		}

		public static void Style (this TextView target, string instanceID, string cssSelector, string text = null)
		{
			TextStyle.Instances [instanceID]?.Style<TextView> (target, cssSelector, text);
		}

		public static void Style (this EditText target, string cssSelector, string text = null)
		{
			TextStyle.Main.Style<EditText> (target, cssSelector, text);
		}

		public static void Style (this EditText target, string instanceID, string cssSelector, string text = null)
		{
			TextStyle.Instances [instanceID]?.Style<EditText> (target, cssSelector, text);
		}
	}
}

