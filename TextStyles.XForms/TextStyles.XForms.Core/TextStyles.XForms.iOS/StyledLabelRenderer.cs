using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using TextStyles.iOS;
using TextStyles.XForms.Core;

[assembly: ExportRenderer (typeof (StyledLabel), typeof (TextStyles.XForms.iOS.StyledLabelRenderer))]
namespace TextStyles.XForms.iOS
{
	public class StyledLabelRenderer : LabelRenderer
	{
		StyledLabel _styledElement;
		TextStyle _textStyle;

		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);

			_styledElement = _styledElement ?? (Element as StyledLabel);

			if (Control != null) {
				SetStyle ();
				_textStyle.Style<UILabel> (Control, _styledElement.CssStyle);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == "Text") {
				_textStyle.Style<UILabel> (Control, _styledElement.CssStyle);
			}
		}

		protected void SetStyle ()
		{
			if (_textStyle == null) {
				_textStyle = (string.IsNullOrEmpty (_styledElement.TextStyleInstance) && TextStyle.Instances.ContainsKey (_styledElement.TextStyleInstance))
					? TextStyle.Instances [_styledElement.TextStyleInstance] : TextStyle.Main;
			}
		}
	}
}