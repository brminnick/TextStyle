using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Styles.Core.Text;
using TextStyleDemo.XForms;

namespace TextStyleDemo.Droid
{
	[Activity (Label = "TextStyleDemo.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);

			Xamarin.Forms.DependencyService.Register<ITextStyle, Styles.Droid.Text.TextStyle> ();

			var style = (Styles.Droid.Text.TextStyle)Xamarin.Forms.DependencyService.Get<ITextStyle> ();
			style.AddFont ("Archistico", "Archistico_Simple.ttf");
			style.AddFont ("Avenir-Medium", "Avenir-Medium.ttf");
			style.AddFont ("Avenir-Book", "Avenir-Book.ttf");
			style.AddFont ("Avenir-Heavy", "Avenir-Heavy.ttf");
			style.AddFont ("BreeSerif-Regular", "BreeSerif-Regular.ttf");
			style.AddFont ("OpenSans-CondBold", "OpenSans-CondBold.ttf");
			style.AddFont ("OpenSans-CondLight", "OpenSans-CondLight.ttf");

			LoadApplication (new App ());
		}
	}
}

