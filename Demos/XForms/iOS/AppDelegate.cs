using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Styles.Core.Text;
using Styles.XForms.iOS;
using TextStyleDemo.XForms;
using UIKit;

namespace TextStyleDemo.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			Xamarin.Forms.DependencyService.Register<ITextStyle, Styles.iOS.Text.TextStyle> ();
			StyledLabelRenderer label; //TODO implement linking

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

