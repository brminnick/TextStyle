using System;
using Styles.iOS.Text;
using Styles.XForms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer (typeof (StyledEditor), typeof (Styles.XForms.iOS.StyledEditorRenderer))]
namespace Styles.XForms.iOS
{
	public class StyledEditorRenderer : EditorRenderer
	{
		StyledEditor _styledElement;
		TextStyle _textStyle;

		public string RawText { get; set; }

		protected override void OnElementChanged (ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged (e);

			_styledElement = _styledElement ?? (Element as StyledEditor);

			if (Control != null) {
				SetStyle ();
				_textStyle.Style (Control, _styledElement.CssStyle, null, _styledElement.CustomTags);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			RawText = Control.Text;
		}

		// TODO look at implementing a better way of updating the view when writing HTML
		public void UpdateStyle ()
		{
			_textStyle.Style (Control, _styledElement.CssStyle, null, _styledElement.CustomTags);
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

