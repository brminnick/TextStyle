using System;
using System.Collections.Generic;
using Foundation;
using Styles.Core.Text;
using Styles.iOS.Text;
using UIKit;

namespace Styles.iOS
{
	[Foundation.Preserve (AllMembers = true)]
	class ViewStyle : IDisposable
	{
		public string StyleID { get; private set; }

		public string TextValue { get; private set; }

		public NSAttributedString AttributedValue { get; private set; }

		public List<CssTagStyle> CustomTags { get; set; }

		public UIView Target { get; set; }

		public bool ContainsHtml { get; private set; }

		UITextField _textField;

		string _rawText;

		bool _updateConstraints;

		TextStyle _instance;

		bool _enableHtmlEditing;
		public bool EnableHtmlEditing {
			get { return _enableHtmlEditing; }
			set {
				_enableHtmlEditing = value;

				if (_textField != null) {
					if (value) {
						_textField.EditingChanged += TextEditingChanged;
						_textField.EditingDidBegin += TextEditingStarted;
						_textField.EditingDidEnd += TextEditingEnded;

					} else {
						_textField.EditingChanged -= TextEditingChanged;
						_textField.EditingDidBegin -= TextEditingStarted;
						_textField.EditingDidEnd -= TextEditingEnded;
					}
				}
			}
		}

		public ViewStyle (TextStyle instance, UIView target, string styleID, string text, bool updateConstraints)
		{
			_instance = instance;
			_updateConstraints = updateConstraints;
			_textField = target as UITextField;

			StyleID = styleID;
			Target = target;
			UpdateText (text);
		}

		public void UpdateText (string value = null)
		{
			if (!String.IsNullOrEmpty (value)) {
				_rawText = value;
				ContainsHtml = (!String.IsNullOrEmpty (value) && Common.MatchHtmlTags.IsMatch (value));
			} else {
				return;
			}

			var style = _instance.GetStyle (StyleID);
			TextValue = TextStyle.ParseString (style, _rawText);

			AttributedValue = ContainsHtml ? _instance.CreateHtmlString (TextValue, CustomTags) : _instance.CreateStyledString (style, TextValue);
		}

		public void UpdateFrame ()
		{
			var style = _instance.GetStyle (StyleID);

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
			_instance.Style (Target, StyleID, _rawText, CustomTags);
		}

		public void Dispose ()
		{
			EnableHtmlEditing = false;
			Target = null;
		}

		void TextEditingChanged (object sender, EventArgs e)
		{
			_rawText = _textField.Text;
		}

		void TextEditingStarted (object sender, EventArgs e)
		{
			_instance.StyleUITextField (_textField, StyleID, _rawText, ignoreHtml: true);
		}

		void TextEditingEnded (object sender, EventArgs e)
		{
			_instance.StyleUITextField (_textField, StyleID, _rawText, CustomTags);
		}
	}
}

