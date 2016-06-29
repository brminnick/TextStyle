using System;
using Styles.iOS.Text;
using Styles.XForms.Core;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (StyledEntry), typeof (Styles.XForms.iOS.StyledEntryRenderer))]
namespace Styles.XForms.iOS
{
	public class StyledEntryRenderer : EntryRenderer
	{
		StyledEntry _styledElement;
		TextStyle _textStyle;

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			_styledElement = _styledElement ?? (Element as StyledEntry);

			if (Control != null) {
				if (_textStyle == null) {
					_textStyle = (!string.IsNullOrEmpty (_styledElement.TextStyleInstance) && TextStyle.Instances.ContainsKey (_styledElement.TextStyleInstance))
						? (TextStyle)TextStyle.Instances [_styledElement.TextStyleInstance] : TextStyle.Main;
				}

				_styledElement.RawText = Control.Text;
				_textStyle.Style (Control, _styledElement.CssStyle, null, _styledElement.CustomTags);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (_styledElement.EnableHtmlEditing) {
				switch (e.PropertyName) {
				case "Text":
					if (Control.IsFirstResponder) {
						_styledElement.RawText = Control.Text;
					}
					break;
				case "IsFocused":
					if (Control.IsFirstResponder) {
						_textStyle.StyleUITextField (Control, _styledElement.CssStyle, _styledElement.RawText, ignoreHtml: true);
					} else {
						_textStyle.StyleUITextField (Control, _styledElement.CssStyle, _styledElement.RawText, _styledElement.CustomTags);
					}
					break;
				default:
					break;
				}
			}
		}
	}
}

