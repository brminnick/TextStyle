using System;
using System.Collections.Generic;
using Foundation;
using TextStyles.Core;
using UIKit;

namespace TextStyles.iOS
{
	public class StyleManager : IDisposable
	{
		private Dictionary<object, ViewStyle> _views;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextStyles.iOS.StyleManager"/> class.
		/// </summary>
		public StyleManager ()
		{
			_views = new Dictionary<object, ViewStyle> ();
			TextStyle.Instance.StylesChanged += TextStyle_Instance_StylesChanged;
		}

		/// <summary>
		/// Creates and styles a new text container (UIlabel, UITextView, UITextField)
		/// </summary>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to display. Plain or with html tags</param>
		/// <param name="customTags">A list of custom <c>CSSTagStyle</c> instances that set the styling for the html</param>
		/// <param name="useExistingStyles">Existing CSS styles willl be used If set to <c>true</c></param>
		/// <param name="encoding">String encoding type</param>
		/// <typeparam name="T">Text container type (UIlabel, UITextView, UITextField)</typeparam>
		public T Create<T> (string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true)
		{
			var target = TextStyle.Create<T> (styleID, text, customTags, useExistingStyles);
			TextStyle.SetBaseStyle (styleID, ref customTags);

			var reference = new ViewStyle (target as UIView, text, true) {
				StyleID = styleID,
				CustomTags = customTags
			};

			_views.Add (target, reference);

			return target;
		}

		/// <summary>
		/// Adds an existing text container (UILabel, UITextView, UITextField) to the StyleManager and styles it
		/// </summary>
		/// <param name="target">Target text container</param>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to display. Plain or with html tags</param>
		/// <param name="customTags">A list of custom <c>CSSTagStyle</c> instances that set the styling for the html</param>
		/// <param name="useExistingStyles">Existing CSS styles willl be used If set to <c>true</c></param>
		/// <param name="encoding">String encoding type</param>
		public void Add (object target, string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true)
		{
			// Set the base style for the field
			TextStyle.SetBaseStyle (styleID, ref customTags);

			var viewStyle = new ViewStyle ((UIView)target, text, true) {
				StyleID = styleID,
				CustomTags = customTags
			};

			_views.Add (target, viewStyle);
			viewStyle.UpdateText ();
			viewStyle.UpdateDisplay ();
		}

		/// <summary>
		/// Updates the text of a target text container (UILabel, UITextView, UITextField)
		/// </summary>
		/// <param name="target">Target text container</param>
		/// <param name="text">Text</param>
		public void UpdateText (object target, string text)
		{
			var viewStyle = _views [target];
			if (viewStyle == null) {
				return;
			}

			viewStyle.UpdateText (text);
			viewStyle.UpdateDisplay ();
		}

		/// <summary>
		/// Updates the styling and display of all registered text containers
		/// </summary>
		public void UpdateAll ()
		{
			// Update the Attrib strings first as they can take some time
			foreach (var item in _views.Values) {
				item.UpdateText ();
			}

			// Update the displays after so they change all at once
			foreach (var item in _views.Values) {
				item.UpdateDisplay ();
			}
		}

		/// <summary>
		/// Updates the frames of any text containers with line heights smaller than the fonts default
		/// </summary>
		public void UpdateFrames ()
		{
			// Update the frames of any linespaced itemss
			foreach (var item in _views.Values) {
				item.UpdateFrame ();
			}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Occur.TextStyles.iOS.StyleManager"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Occur.TextStyles.iOS.StyleManager"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Occur.TextStyles.iOS.StyleManager"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="Occur.TextStyles.iOS.StyleManager"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Occur.TextStyles.iOS.StyleManager"/> was occupying.</remarks>
		public void Dispose ()
		{
			foreach (var item in _views.Values) {
				item.Target = null;
			}

			_views.Clear ();
			_views = null;

			TextStyle.Instance.StylesChanged -= TextStyle_Instance_StylesChanged;
		}

		void TextStyle_Instance_StylesChanged (object sender, EventArgs e)
		{
			UpdateAll ();
		}

	}

	class ViewStyle
	{
		public string StyleID { get; set; }

		public string TextValue { get; private set; }

		public NSAttributedString AttributedValue { get; private set; }

		public List<CssTagStyle> CustomTags { get; set; }

		public UIView Target { get; set; }

		public bool ContainsHtml { get; private set; }

		string _rawText;

		bool _updateConstraints;

		public ViewStyle (UIView target, string rawText, bool updateConstraints)
		{
			Target = target;
			_rawText = rawText;
			_updateConstraints = updateConstraints;

			ContainsHtml = (!String.IsNullOrEmpty (rawText) && Common.MatchHtmlTags.IsMatch (_rawText));
		}

		public void UpdateText (string value = null)
		{
			if (!String.IsNullOrEmpty (value)) {
				_rawText = value;
			}

			var style = TextStyle.GetStyle (StyleID);
			TextValue = TextStyle.ParseString (style, _rawText);

			AttributedValue = ContainsHtml ? TextStyle.CreateHtmlString (TextValue, CustomTags) : TextStyle.CreateStyledString (style, TextValue);
		}

		public void UpdateFrame ()
		{
			var style = TextStyle.GetStyle (StyleID);

			// Offset the frame if needed
			if (_updateConstraints && style.LineHeight < 0f) {
				var heightOffset = style.GetLineHeightOffset ();
				var targetFrame = Target.Frame;
				targetFrame.Height = (nfloat)Math.Ceiling (targetFrame.Height) + heightOffset;

				if (Target.Constraints.Length > 0) {
					foreach (var constraint in Target.Constraints) {
						if (constraint.FirstAttribute == NSLayoutAttribute.Height) {
							constraint.Constant = targetFrame.Height;
							break;
						}
					}
				} else {
					Target.Frame = targetFrame;
				}
			}
		}

		public void UpdateDisplay ()
		{
			var type = Target.GetType ();
			var style = TextStyle.GetStyle (StyleID);

			if (type == TextStyle.typeLabel) {
				var label = Target as UILabel;
				TextStyle.StyleUILabel (label, style, !ContainsHtml);
				label.AttributedText = AttributedValue;

			} else if (type == TextStyle.typeTextView) {
				var textView = Target as UITextView;
				TextStyle.StyleUITextView (textView, style, !ContainsHtml);
				textView.AttributedText = AttributedValue;

			} else if (type == TextStyle.typeTextField) {
				var textField = Target as UITextField;
				TextStyle.StyleUITextField (textField, style, true);
				textField.AttributedText = AttributedValue;

			} else {
				throw new NotSupportedException ("The specified type is not supported, please use a UILabel, UITextView or UITextField: " + type.ToString ());
			}
		}
	}
}

