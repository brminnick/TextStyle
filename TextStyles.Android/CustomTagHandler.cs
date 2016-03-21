﻿using System;
using Android.Text;
using Android.Graphics;
using Java.Lang;
using System.Linq;
using TextStyles.Android;
using TextStyles.Core;
using System.Collections.Generic;

namespace TextStyles.Android
{
	public class CustomTagHandler : Java.Lang.Object, Html.ITagHandler
	{
		Dictionary<string, TextStyleParameters> _textStyles;

		public CustomTagHandler (Dictionary<string, TextStyleParameters> textStyles)
		{
			_textStyles = textStyles;
		}

		#region ITagHandler implementation

		/// <summary>
		/// Handles a custom tag
		/// </summary>
		/// <returns>void</returns>
		/// <param name="opening">bool</param>
		/// <param name="tag">string</param>
		/// <param name="output">IEditable</param>
		/// <param name="xmlReader">IXMLReader</param>
		public void HandleTag (bool opening, string tag, IEditable output, Org.Xml.Sax.IXMLReader xmlReader)
		{
			TextStyleParameters style;

			// Body overwrites the inline styles so we set that at the textview level
			if (_textStyles.TryGetValue (tag, out style)) {
				var text = output as SpannableStringBuilder;

				if (opening) {
					Start (text, new TextStylesObject ());
				} else {
					// Retrieve font
					Typeface font = null;
					if (!string.IsNullOrEmpty (style.Font)) {
						TextStyle.Instance._typeFaces.TryGetValue (style.Font, out font);
					}

					var customSpan = new CustomTypefaceSpan ("", font, style);
					End (style, text, Class.FromType (typeof (TextStylesObject)), customSpan);
				}
			}
		}

		/// <summary>
		/// Start the tag
		/// </summary>
		/// <param name="text">SpannableStringBuilder</param>
		/// <param name="mark">Java.Lang.Object</param>
		static void Start (SpannableStringBuilder text, Java.Lang.Object mark)
		{
			var length = text.Length ();
			text.SetSpan (mark, length, length, SpanTypes.MarkMark);
		}

		/// <summary>
		/// End the specified tag
		/// </summary>
		/// <param name="style">TextStyleParameters</param>
		/// <param name="text">SpannableStringBuilder</param>
		/// <param name="kind">Class</param>
		/// <param name="newSpan">Java.Lang.Object</param>
		static void End (TextStyleParameters style, SpannableStringBuilder text, Class kind, Java.Lang.Object newSpan)
		{
			var length = text.Length ();
			var span = GetLast (text, kind);
			var start = text.GetSpanStart (span);
			text.RemoveSpan (span);

			// Parse the text in the span
			var parsedString = TextStyle.ParseString (style, text.SubSequence (start, length)); // Note this hardcodes the text this way and only works on parsed tags!
			text.Replace (start, length, parsedString);

			if (start != length)
				text.SetSpan (newSpan, start, length, SpanTypes.ExclusiveExclusive);
		}

		/// <summary>
		/// Gets the last instance of an Object
		/// </summary>
		/// <returns>Java.Lang.Object</returns>
		/// <param name="text">ISpanned</param>
		/// <param name="kind">Class</param>
		static Java.Lang.Object GetLast (ISpanned text, Class kind)
		{
			var length = text.Length ();
			var spans = text.GetSpans (0, length, kind);
			return spans.Length > 0 ? spans.Last () : null;
		}

		#endregion

		#region IDisposable implementation

		//		public void Dispose ()
		//		{
		//			throw new NotImplementedException ();
		//		}

		#endregion

	}

	/* 
	* Notice this class. It doesn't really do anything when it spans over the text. 
	* The reason is we just need to distinguish what needs to be spanned, then on our closing
	* tag, we will apply the spannable. For each of your different spannables you implement, just 
	* create a class here. 
	*/
	class TextStylesObject : Java.Lang.Object
	{
	}
}

