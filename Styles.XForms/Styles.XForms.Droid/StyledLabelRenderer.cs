using System;
using Android.Widget;
using Styles.Droid.Text;
using Styles.XForms.Core;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof (StyledLabel), typeof (Styles.XForms.Droid.StyledLabelRenderer))]
namespace Styles.XForms.Droid
{
	public class StyledLabelRenderer : LabelRenderer
	{
		public StyledLabelRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged (e);

			var styledElement = Element as StyledLabel;
			var cssStyle = styledElement.CssStyle;

			var textStyle = (!string.IsNullOrEmpty (styledElement.TextStyleInstance) && TextStyle.Instances.ContainsKey (styledElement.TextStyleInstance))
					? TextStyle.Instances [styledElement.TextStyleInstance] : TextStyle.Main;

			if (Control != null) {
				textStyle.Style<TextView> (Control, cssStyle, null);
			}
		}
	}
}

