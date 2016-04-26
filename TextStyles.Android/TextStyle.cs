using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Android.Text;
using Android.Widget;
using TextStyles.Core;
using Android.Graphics;
using Android.App;
using Android.Util;
using Android.Views;

namespace TextStyles.Android
{
	public class TextStyle:ITextStyle
	{
		#region Parameters

		/// <summary>
		/// The default size for text.
		/// </summary>
		public static float DefaultTextSize = 18f;

		public event EventHandler StylesChanged;

		internal static Type typeTextView = typeof(TextView);
		internal static Type typeEditText = typeof(EditText);

		static TextStyle instance = null;
		static readonly object padlock = new object ();

		internal Dictionary<string, TextStyleParameters> _textStyles;
		internal Dictionary<string, Typeface> _typeFaces;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStyles.Android.TextStyle"/> class.
		/// </summary>
		TextStyle ()
		{
			_typeFaces = new Dictionary<string, Typeface> ();
		}

		#region Public Methods

		/// <summary>
		/// Sets the CSS string
		/// </summary>
		/// <param name="css">Css Style Sheet</param>
		public virtual void SetCSS (string css)
		{
			var styles = CssTextStyleParser.Parse (css);
			SetStyles (styles);
		}

		/// <summary>
		/// Adds the typeface.
		/// </summary>
		/// <param name="fontName">Font name.</param>
		/// <param name="font">TypeFace</param>
		public virtual void AddFont (string fontName, string fontUrl)
		{
			Typeface font = Typeface.CreateFromAsset (Application.Context.Assets, fontUrl);
			_typeFaces.Add (fontName, font);
		}

		/// <summary>
		/// Gets an instance of the font by font name.
		/// </summary>
		/// <returns>The font.</returns>
		/// <param name="fontName">Font name.</param>
		public virtual Typeface GetFont (string fontName)
		{
			Typeface font = null;
			_typeFaces.TryGetValue (fontName, out font);
			return font;
		}

		/// <summary>
		/// Sets the styles dictionary
		/// </summary>
		/// <param name="styles">Styles dictionary</param>
		public virtual void SetStyles (Dictionary<string, TextStyleParameters> styles)
		{
			_textStyles = styles;

			// Pre-Parse android specific styles
			foreach (var item in _textStyles) {
				item.Value.FontSize = (item.Value.FontSize <= 0f) ? DefaultTextSize : item.Value.FontSize;

				// scale text spacing
				if (Math.Abs (item.Value.LetterSpacing) > 0f) {
					item.Value.LetterSpacing = (item.Value.LetterSpacing / item.Value.FontSize);
				}
			}

			Refresh ();
		}

		/// <summary>
		/// Gets a style by its selector
		/// </summary>
		/// <returns>The style.</returns>
		/// <param name="selector">Selector.</param>
		public static TextStyleParameters GetStyle (string selector)
		{
			return instance._textStyles.ContainsKey (selector) ? instance._textStyles [selector] : null;
		}

		/// <summary>
		/// Returns the main instance of TextStyle 
		/// </summary>
		/// <value>The instance.</value>
		public static TextStyle Instance {
			get {
				lock (padlock) {
					if (instance == null) {
						instance = new TextStyle ();
					}
					return instance;
				}
			}
		}

		public static ISpanned CreateHtmlString (string text, string defaultStyle, List<CssTagStyle> customTags = null, bool mergeExistingStyles = true, bool includeExistingStyles = true)
		{
			var styles = customTags != null ? MergeStyles (defaultStyle, customTags, mergeExistingStyles, includeExistingStyles) : Instance._textStyles;
			if (!styles.ContainsKey (defaultStyle))
				styles.Add (defaultStyle, Instance._textStyles [defaultStyle]);

			var converter = new CustomHtmlParser (text, styles, defaultStyle);
			return converter.Convert ();
		}

		static Dictionary<string, TextStyleParameters> MergeStyles (string defaultStyleID, List<CssTagStyle> customTags, bool mergeExistingStyles = true, bool includeExistingStyles = true)
		{
			var customCSS = new StringBuilder ();
			foreach (var customTag in customTags)
				customCSS.AppendLine (customTag.CSS);

			var customStyles = CssTextStyleParser.Parse (customCSS.ToString ());
			var defaultStyle = Instance._textStyles [defaultStyleID];
			if (defaultStyle == null)
				throw new Exception ("Default Style ID not found: " + defaultStyleID);

			TextStyleParameters existingStyle;
			foreach (var style in customStyles) {

				if (mergeExistingStyles) {
					Instance._textStyles.TryGetValue (style.Key, out existingStyle);

					if (existingStyle != null) {
						style.Value.Merge (existingStyle, false);
					} else {
						style.Value.Merge (defaultStyle, false);
					}
				}

				// If no font, use the default one
				if (string.IsNullOrEmpty (style.Value.Font)) {
					style.Value.Font = defaultStyle.Font;
				}
			}

			return customStyles;
		}

		public static ISpanned CreateStyledString (string styleID, string text, int startIndex = 0, int endIndex = -1)
		{
			var style = GetStyle (styleID);
			return CreateStyledString (style, text, startIndex, endIndex);
		}

		public static ISpanned CreateStyledString (TextStyleParameters style, string text, int startIndex = 0, int endIndex = -1)
		{
			if (endIndex == -1)
				endIndex = text.Length;

			if (startIndex >= endIndex)
				throw new Exception ("Unable to create styled string, StartIndex is too high:" + startIndex);

			// Parse the text
			text = ParseString (style, text);

			var isHTML = (!string.IsNullOrEmpty (text) && Common.MatchHtmlTags.IsMatch (text));

			if (isHTML) {
				return CreateHtmlString (text, style.Name);
			} else {
				var builder = new SpannableStringBuilder (text);
				var font = instance.GetFont (style.Font);
				var span = new CustomTypefaceSpan ("", font, style);

				builder.SetSpan (span, startIndex, endIndex, SpanTypes.ExclusiveExclusive);
				return builder;
			}
		}

		/// <summary>
		/// Signals that the styles have been updated.
		/// </summary>
		public void Refresh ()
		{
			StylesChanged?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Sets the body css style for the customTags.
		/// </summary>
		/// <param name="baseStyleID">The CSS selector name for the body style</param>
		/// <param name="customTags">A list of CSSTagStyle custom tags</param>
		public static void SetBaseStyle (string baseStyleID, ref List<CssTagStyle> customTags)
		{
			if (customTags == null)
				customTags = new List<CssTagStyle> ();

			if (!customTags.Any (x => x.Tag == "body")) {
				customTags.Add (new CssTagStyle (HtmlTextStyleParser.BODYTAG) { Name = baseStyleID });
			}
		}


		public static T Create<T> (string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true, Encoding encoding = null)
		{
			var target = Activator.CreateInstance<T> ();

			Style (target, styleID, text);
			return target;
		}


		public static void Style<T> (T target, string styleID, string text = null, List<CssTagStyle> customTags = null, bool useExistingStyles = true, Encoding encoding = null)
		{
			var style = GetStyle (styleID);
			var type = typeof(T);
			var isHTML = (!string.IsNullOrEmpty (text) && Common.MatchHtmlTags.IsMatch (text));
			var textView = (type == typeTextView) ? target as TextView : target as EditText;

			if (textView == null) {
				throw new NotSupportedException ("The specified type is not supported, please use a TextView or EditText: " + type.ToString ());
			}

			text = text ?? textView.Text;

			// Style the TextView
			if (isHTML && type == typeTextView) {
				StyleTextView (textView, style, false);
				textView.SetText (CreateHtmlString (text, styleID, customTags, useExistingStyles), TextView.BufferType.Spannable);

			} else if (style.RequiresHtmlTags () && type == typeTextView) {
				StyleTextView (textView, style, false);

				var builder = new SpannableStringBuilder (ParseString (style, text));
				var font = instance.GetFont (style.Font);
				var span = new CustomTypefaceSpan ("", font, style);

				builder.SetSpan (span, 0, builder.Length (), SpanTypes.ExclusiveExclusive);
				textView.SetText (builder, TextView.BufferType.Spannable);
			} else {
				StyleTextView (textView, style, true);
				textView.SetText (ParseString (style, text), TextView.BufferType.Normal);
			}
		}

		#endregion

		#region Private Methods

		// TODO implement this function for styling plain text views, for html based views perhaps not
		internal static void StyleTextView (TextView target, TextStyleParameters style, bool isPlainText)
		{
			var fontSize = (style.FontSize <= 0f) ? DefaultTextSize : style.FontSize;
			target.SetTextSize (ComplexUnitType.Sp, fontSize);

			// Plain text fonts and colors
			if (isPlainText) {
				if (!String.IsNullOrEmpty (style.Color))
					target.SetTextColor (Color.White.FromHex (style.Color));

				if (!String.IsNullOrEmpty (style.Font) && instance._typeFaces.ContainsKey (style.Font)) {
					target.Typeface = instance._typeFaces [style.Font];
					target.PaintFlags = target.PaintFlags | PaintFlags.SubpixelText;
				}
			}

			// Lines
			if (style.Lines > 0)
				target.SetLines (style.Lines);

			if (!string.IsNullOrEmpty (style.BackgroundColor))
				target.SetBackgroundColor (Color.White.FromHex (style.BackgroundColor));

			// Does nothing on the string attribs, needs to be part of a NSMutableAttributedString
			if (Math.Abs (style.LineHeight) > 0)
				target.SetLineSpacing (0, style.LineHeight / fontSize);

			// Assing the text gravity
			target.TextAlignment = TextAlignment.Gravity;
			switch (style.TextAlign) {
			case TextStyleAlign.Center:
				target.Gravity = GravityFlags.CenterHorizontal;
				break;
			case TextStyleAlign.Justified:
				target.Gravity = GravityFlags.FillHorizontal;
				break;
			case TextStyleAlign.Right:
				target.Gravity = GravityFlags.Right;
				break;
			default:
				target.Gravity = GravityFlags.Left;
				break;
			}

			// Padding
			target.SetPadding (
				(style.PaddingLeft > float.MinValue) ? (int)style.PaddingLeft : target.PaddingLeft,
				(style.PaddingTop > float.MinValue) ? (int)style.PaddingTop : target.PaddingTop,
				(style.PaddingRight > float.MinValue) ? (int)style.PaddingRight : target.PaddingRight,
				(style.PaddingBottom > float.MinValue) ? (int)style.PaddingBottom : target.PaddingBottom
			);

			// Overflow
			// TODO implement this fully
			switch (style.TextOverflow) {
			case TextStyleTextOverflow.Ellipsis:
				target.Ellipsize = TextUtils.TruncateAt.End;
				break;
			default:
				break;
			}
		}

		internal static string ParseString (TextStyleParameters style, string text)
		{
			// Text transformations
			if (!string.IsNullOrEmpty (text)) {
				if (style.TextTransform != TextStyleTextTransform.None) {
					switch (style.TextTransform) {
					case TextStyleTextTransform.UpperCase:
						text = text.ToUpper ();
						break;
					case TextStyleTextTransform.LowerCase:
						text = text.ToLower ();
						break;
					case TextStyleTextTransform.Capitalize:
						text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase (text.ToLower ());
						break;
					}
				}
			}

			return text;
		}

		/// <summary>
		/// Dummy method to ensure classes are included for the Linker
		/// </summary>
		/// <param name="injector">Injector.</param>
		/// <param name="textView">Text view.</param>
		/// <param name="textField">Text field.</param>
		private void LinkerInclude (TextView textView, EditText textField)
		{
			textView = new TextView (null);
			textField = new EditText (null);
		}

		#endregion
	}

	/// <summary>
	/// Text attributes range.
	/// </summary>
	public class TextAttributesRange
	{
		public int StartIndex;
		public int Length;
		public string Text;
	}
}

