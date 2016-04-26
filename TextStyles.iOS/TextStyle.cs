using System;
using UIKit;
using Foundation;
using System.Collections.Generic;
using System.Globalization;
using TextStyles.Core;
using System.Linq;

namespace TextStyles.iOS
{
	public class TextStyle:ITextStyle
	{
		#region Parameters

		/// <summary>
		/// The default size for text.
		/// </summary>
		public static float DefaultTextSize = 18f;

		public event EventHandler StylesChanged;

		internal static Type typeLabel = typeof(UILabel);
		internal static Type typeTextView = typeof(UITextView);
		internal static Type typeTextField = typeof(UITextField);

		static TextStyle instance = null;
		static readonly object padlock = new object ();

		internal Dictionary<string, TextStyleParameters> _textStyles;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStyles.iOS,TextStyle"/> class.
		/// </summary>
		TextStyle ()
		{
			_textStyles = new Dictionary<string, TextStyleParameters> ();
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
		/// Sets the styles dictionary
		/// </summary>
		/// <param name="styles">Styles dictionary</param>
		public virtual void SetStyles (Dictionary<string, TextStyleParameters> styles)
		{
			_textStyles = styles;
			Refresh ();
		}

		/// <summary>
		/// Sets the style.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="style">Style.</param>
		/// <param name="refresh">If set to <c>true</c> refresh.</param>
		public virtual void SetStyle (string id, TextStyleParameters style, bool refresh = false)
		{
			_textStyles [id] = style;
			if (refresh) {
				Refresh ();
			}
		}

		/// <summary>
		/// Gets the styles.
		/// </summary>
		/// <returns>The styles.</returns>
		public virtual List<TextStyleParameters> GetStyles ()
		{
			return _textStyles.Select (v => v.Value).ToList ();
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

		// TEMP
		internal Dictionary<string, TextStyle> _instances = new Dictionary<string, TextStyle> ();

		public static TextStyle CreateInstance (string id)
		{
			lock (padlock) {
				_instances [id] = new TextStyle ();
				return _instances [id];
			}
		}

		/// <summary>
		/// Creates an NSAttibutedString html string using the custom tags for styling.
		/// </summary>
		/// <returns>NSAttibutedString</returns>
		/// <param name="text">Text to display including html tags</param>
		/// <param name="customTags">A list of custom <c>CSSTagStyle</c> instances that set the styling for the html</param>
		/// <param name="useExistingStyles">Existing CSS styles willl be used If set to <c>true</c></param>
		public static NSAttributedString CreateHtmlString (string text, List<CssTagStyle> customTags = null, bool useExistingStyles = true)
		{
			var error = new NSError ();

			text = HtmlTextStyleParser.StyleString (text, Instance._textStyles, customTags, useExistingStyles);

			var stringAttribs = new NSAttributedStringDocumentAttributes {
				DocumentType = NSDocumentType.HTML,
				StringEncoding = NSStringEncoding.UTF8
			};

			var htmlString = new NSAttributedString (text, stringAttribs, ref error);

			return htmlString;
		}

		/// <summary>
		/// Creates a styled string as an NSAttibutedString 
		/// </summary>
		/// <returns>NSMutableAttributedString</returns>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to style</param>
		/// <param name="startIndex">Style start index</param>
		/// <param name="endIndex">Style end index</param>
		public static NSMutableAttributedString CreateStyledString (string styleID, string text, int startIndex = 0, int endIndex = -1)
		{
			var style = GetStyle (styleID);
			return CreateStyledString (style, text, startIndex, endIndex);
		}

		/// <summary>
		/// Creates a styled string as an NSAttibutedString 
		/// </summary>
		/// <returns>The styled string.</returns>
		/// <param name="style">TextStyleParameters for styling</param>
		/// <param name="text">Text to style</param>
		/// <param name="startIndex">Style start index</param>
		/// <param name="endIndex">Style end index</param>
		public static NSMutableAttributedString CreateStyledString (TextStyleParameters style, string text, int startIndex = 0, int endIndex = -1)
		{
			var attribs = GetStringAttributes (style);
			text = ParseString (style, text);

			if (endIndex == -1) {
				endIndex = text.Length;
			}

			var prettyString = new NSMutableAttributedString (text);
			prettyString.SetAttributes (attribs, new NSRange (startIndex, endIndex));

			return prettyString;
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

		/// <summary>
		/// Creates and styles a new Text container (UIlabel, UITextView, UITextField)
		/// </summary>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to display including html tags</param>
		/// <param name="customTags">A list of custom <c>CSSTagStyle</c> instances that set the styling for the html</param>
		/// <param name="useExistingStyles">Existing CSS styles willl be used If set to <c>true</c></param>
		/// <param name="encoding">String encoding type</param>
		/// <typeparam name="T">Text container type (UIlabel, UITextView, UITextField)</typeparam>
		public static T Create<T> (string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true)
		{
			var isHTML = (!string.IsNullOrEmpty (text) && Common.MatchHtmlTags.IsMatch (text));
			var target = Activator.CreateInstance<T> ();

			// If this is a plain string view, style it and return it
			if (!string.IsNullOrEmpty (text) && !isHTML) {
				Style<T> (target, styleID, text);
				return target;
			} else if (isHTML) {
				SetBaseStyle (styleID, ref customTags);
			}

			var formattedText = isHTML ?
				TextStyle.CreateHtmlString (text, customTags, useExistingStyles) :
				TextStyle.CreateStyledString (styleID, text);

			var type = typeof(T);
			if (type == typeLabel) {
				var label = target as UILabel;
				label.AttributedText = formattedText;
				if (!isHTML)
					StyleUILabel (label, GetStyle (styleID), true);

			} else if (type == typeTextView) {
				var textView = target as UITextView;
				textView.AttributedText = formattedText;
				if (!isHTML)
					StyleUITextView (textView, GetStyle (styleID), true);

			} else if (type == typeTextField) {
				var textField = target as UITextField;
				textField.AttributedText = formattedText;
				if (!isHTML)
					StyleUITextField (textField, GetStyle (styleID), true);

			} else {
				throw new NotSupportedException ("The specified type is not supported, please use a UILabel, UITextView or UITextField: " + type.ToString ());
			}

			return target;
		}

		/// <summary>
		/// Styles a text container (UIlabel, UITextView, UITextField)
		/// </summary>
		/// <param name="target">Target text container</param>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to display</param>
		/// <typeparam name="T">Text container type (UIlabel, UITextView, UITextField)</typeparam>
		public static void Style<T> (T target, string styleID, string text = null)
		{
			var style = GetStyle (styleID);
			var type = typeof(T);

			if (type == typeLabel) {
				var label = target as UILabel;
				label.AttributedText = ParseHtmlString (style, text ?? label.Text);
				StyleUILabel (label, style, false);

			} else if (type == typeTextView) {
				var textView = target as UITextView;
				textView.AttributedText = ParseHtmlString (style, text ?? textView.Text);
				StyleUITextView (textView, style, false);

			} else if (type == typeTextField) {
				var textField = target as UITextField;
				textField.AttributedText = ParseHtmlString (style, text ?? textField.Text);
				StyleUITextField (textField, style, false);

			} else {
				throw new NotSupportedException ("The specified type is not supported, please use a UILabel, UITextView or UITextField: " + type.ToString ());
			}
		}

		#endregion

		#region Private Methods

		static void UpdateMargins (TextStyleParameters style, ref UIEdgeInsets inset)
		{
			inset.Top = (style.PaddingTop > float.MinValue) ? style.PaddingTop : inset.Top;
			inset.Bottom = (style.PaddingBottom > float.MinValue) ? style.PaddingBottom : inset.Bottom;
			inset.Left = (style.PaddingLeft > float.MinValue) ? style.PaddingLeft : inset.Left;
			inset.Right = (style.PaddingRight > float.MinValue) ? style.PaddingRight : inset.Right;
		}

		internal static void StyleUILabel (UILabel target, TextStyleParameters style, bool setFonts)
		{
			// TODO fix this as its not helping as it stands
			var attribs = GetStringAttributes (style);
			target.TextColor = attribs.ForegroundColor;

			// If setting the font attributes
			if (setFonts) {
				target.Font = attribs.Font;
			}

			// Lines
			if (style.Lines > int.MinValue) {
				target.Lines = style.Lines;
			}

			// Text Alignment
			target.TextAlignment = GetAlignment (style.TextAlign);

			// Overflow
			switch (style.TextOverflow) {
			case TextStyleTextOverflow.Ellipsis:
				target.LineBreakMode = UILineBreakMode.TailTruncation;
				break;
			case TextStyleTextOverflow.Clip:
				target.LineBreakMode = UILineBreakMode.Clip;
				break;
			default:
				target.LineBreakMode = UILineBreakMode.WordWrap;
				break;
			}
		}

		internal static void StyleUITextView (UITextView target, TextStyleParameters style, bool setFonts)
		{
			var attribs = GetStringAttributes (style);
			target.Font = attribs.Font;

			// If setting the font attributes
			if (setFonts) {
				target.TextColor = attribs.ForegroundColor;
			}

			// Text Alignment
			target.TextAlignment = GetAlignment (style.TextAlign);

			// Padding
			if (style.Padding != null) {
				var padding = style.Padding;
				target.TextContainerInset = new UIEdgeInsets (padding [0], padding [1], padding [2], padding [3]);
			} else {
				var curInset = target.TextContainerInset;
				UpdateMargins (style, ref curInset);
				target.TextContainerInset = curInset;
			}
		}

		internal static void StyleUITextField (UITextField target, TextStyleParameters style, bool setFonts)
		{
			var attribs = GetStringAttributes (style);

			target.TextColor = attribs.ForegroundColor;

			// If setting the font attributes
			if (setFonts)
				target.Font = attribs.Font;

			target.TextAlignment = GetAlignment (style.TextAlign);

			// Padding
			if (style.Padding != null) {
				var padding = style.Padding;
				target.LayoutMargins = new UIEdgeInsets (padding [0], padding [1], padding [2], padding [3]);
			} else {
				var curInset = target.LayoutMargins;
				UpdateMargins (style, ref curInset);
				target.LayoutMargins = curInset;
			}
		}

		private static UIStringAttributes GetStringAttributes (TextStyleParameters style)
		{
			var stringAttribs = new UIStringAttributes ();

			var fontSize = style.FontSize;
			if (fontSize <= 0f)
				fontSize = DefaultTextSize;

			if (!string.IsNullOrEmpty (style.Font))
				stringAttribs.Font = UIFont.FromName (style.Font, fontSize);

			if (!string.IsNullOrEmpty (style.Color))
				stringAttribs.ForegroundColor = UIColor.Clear.FromHex (style.Color);

			if (!string.IsNullOrEmpty (style.BackgroundColor))
				stringAttribs.BackgroundColor = UIColor.Clear.FromHex (style.BackgroundColor);

			if (style.LetterSpacing > 0f)
				stringAttribs.KerningAdjustment = style.LetterSpacing;

			// Does nothing on the string attribs, needs to be part of a NSMutableAttributedString
			if (style.LineHeight != 0f) {
				var paragraphStyle = new NSMutableParagraphStyle () {
					LineHeightMultiple = style.LineHeight / fontSize,
					Alignment = GetAlignment (style.TextAlign)
				};
				stringAttribs.ParagraphStyle = paragraphStyle;
			}

			if (style.TextDecoration != TextStyleDecoration.None) {
				switch (style.TextDecoration) {
				case TextStyleDecoration.LineThrough:
					stringAttribs.StrikethroughStyle = NSUnderlineStyle.Single;
					break;
				case TextStyleDecoration.Underline:
					stringAttribs.UnderlineStyle = NSUnderlineStyle.Single;
					break;
				}

				if (!string.IsNullOrEmpty (style.TextDecorationColor))
					stringAttribs.StrikethroughColor = UIColor.Clear.FromHex (style.TextDecorationColor);
			}


			return stringAttribs;
		}

		internal static NSAttributedString ParseHtmlString (TextStyleParameters style, string text)
		{
			var attribs = GetStringAttributes (style);
			text = ParseString (style, text);

			var prettyString = new NSMutableAttributedString (text);
			prettyString.AddAttributes (attribs, new NSRange (0, text.Length));
			return prettyString;
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

		private static UITextAlignment GetAlignment (TextStyleAlign alignment)
		{
			switch (alignment) {
			case TextStyleAlign.Left:
				return UITextAlignment.Left;
			case TextStyleAlign.Right:
				return UITextAlignment.Right;
			case TextStyleAlign.Center:
				return UITextAlignment.Center;
			case TextStyleAlign.Justified:
				return UITextAlignment.Justified;
			default:
				return UITextAlignment.Left;
			}
		}

		/// <summary>
		/// Dummy method to ensure classes are included for the Linker
		/// </summary>
		/// <param name="injector">Injector.</param>
		/// <param name="textView">Text view.</param>
		/// <param name="textField">Text field.</param>
		private void LinkerInclude (UILabel injector, UITextView textView, UITextField textField)
		{
			injector = new UILabel ();
			textView = new UITextView ();
			textField = new UITextField ();
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
		public UIStringAttributes Attributes;
	}
}

