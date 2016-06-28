using System;
using Styles.Core.Text;
using Styles.Droid.Text;
using Styles.XForms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (StyledEntry), typeof (Styles.XForms.Droid.StyledEntryRenderer))]
namespace Styles.XForms.Droid
{
	public class StyledEntryRenderer : EntryRenderer
	{
		StyledEntry _styledElement;
		ITextStyle _textStyle;

		public bool AutoRenderHtml { get; set; }

		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			AutoRenderHtml = true;

			_styledElement = _styledElement ?? (Element as StyledEntry);

			if (Control != null) {
				if (_textStyle == null) {
					_textStyle = (!string.IsNullOrEmpty (_styledElement.TextStyleInstance) && TextStyle.Instances.ContainsKey (_styledElement.TextStyleInstance))
						? TextStyle.Instances [_styledElement.TextStyleInstance] : TextStyle.Main;
				}
				_styledElement.RawText = Control.Text;
				_textStyle.Style (Control, _styledElement.CssStyle, null, _styledElement.CustomTags);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (AutoRenderHtml) {
				switch (e.PropertyName) {
				case "Text":
					if (Control.IsFocused)
						_styledElement.RawText = Control.Text;
					break;
				case "IsFocused":
					if (Control.IsFocused) {
						Control.Text = _styledElement.RawText;
					} else {
						_textStyle.Style (Control, _styledElement.CssStyle, _styledElement.RawText, _styledElement.CustomTags);
					}
					break;
				default:
					break;
				}
			}
		}
	}
}

