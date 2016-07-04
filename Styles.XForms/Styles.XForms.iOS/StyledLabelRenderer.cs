using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Styles.iOS.Text;
using Styles.XForms.Core;

[assembly: ExportRenderer (typeof (StyledLabel), typeof (Styles.XForms.iOS.StyledLabelRenderer))]
namespace Styles.XForms.iOS
{
	[Foundation.Preserve (AllMembers = true)]
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
				_textStyle.Style (Control, _styledElement.CssStyle, null, _styledElement.CustomTags);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == "Text") {
				_textStyle.Style (Control, _styledElement.CssStyle, null, _styledElement.CustomTags);
			}
		}

		protected void SetStyle ()
		{
			if (_textStyle == null) {
				_textStyle = TextStyle.Main;

				//(string.IsNullOrEmpty (_styledElement.TextStyleInstance) && TextStyle.Instances.ContainsKey (_styledElement.TextStyleInstance))
				//? TextStyle.Instances [_styledElement.TextStyleInstance] as TextStyle : TextStyle.Main;
			}
		}
	}
}