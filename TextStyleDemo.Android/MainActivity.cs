using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content.PM;
using TextStyles.Core;
using System.Collections.Generic;
using System;
using TextStyles.Android;
using Android.Graphics;

namespace TextStyleDemo.Android
{
	[Activity (Theme = "@android:style/Theme.Holo.Light", Label = "TextStyleDroidDemo", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		const string headingOne = @"The difference between";
		const string headingTwo = @"Ordinary & Extraordinary";
		const string headingThree = @"Is that little <spot>extra</spot>";
		const string textBody = @"Geometry can produce legible letters but <i>art alone</i> makes them beautiful.<p>Art begins where geometry ends, and imparts to letters a character trascending mere measurement.</p>";

		StyleManager _styleManager;
		string _cssOne;
		string _cssTwo;

		bool _isFirstStyleSheet = true;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_cssOne = OpenCSSFile ("StyleOne.css");
			_cssTwo = OpenCSSFile ("StyleTwo.css");

			TextStyle.Main.AddFont ("Archistico", "Archistico_Simple.ttf");
			TextStyle.Main.AddFont ("Avenir-Medium", "Avenir-Medium.ttf");
			TextStyle.Main.AddFont ("Avenir-Book", "Avenir-Book.ttf");
			TextStyle.Main.AddFont ("Avenir-Heavy", "Avenir-Heavy.ttf");
			TextStyle.Main.AddFont ("BreeSerif-Regular", "BreeSerif-Regular.ttf");
			TextStyle.Main.AddFont ("OpenSans-CondBold", "OpenSans-CondBold.ttf");
			TextStyle.Main.AddFont ("OpenSans-CondLight", "OpenSans-CondLight.ttf");

			TextStyle.Main.SetCSS (_cssOne);

			var labelOne = FindViewById<TextView> (Resource.Id.labelOne);
			var labelTwo = FindViewById<TextView> (Resource.Id.labelTwo);
			var labelThree = FindViewById<TextView> (Resource.Id.labelThree);
			var body = FindViewById<TextView> (Resource.Id.body);

			// Create a StyleManager to handle any CSS changes automatically
			_styleManager = new StyleManager ();
			_styleManager.Add (labelOne, "h2", headingOne);
			_styleManager.Add (labelTwo, "h1", headingTwo);
			_styleManager.Add (labelThree, "h2", headingThree, new List<CssTagStyle> {
				new CssTagStyle ("spot"){ CSS = "spot{color:" + Colors.SpotColor.ToHex () + "}" }
			});
			_styleManager.Add (body, "body", textBody);

			var toggleButton = FindViewById<ImageButton> (Resource.Id.refreshIcon);
			toggleButton.SetBackgroundColor (Color.Transparent);
			toggleButton.Click += (sender, e) => {
				Console.WriteLine ("Toggled");

				var css = _isFirstStyleSheet ? _cssTwo : _cssOne;
				_isFirstStyleSheet = !_isFirstStyleSheet;
				TextStyle.Main.SetCSS (css);
			};
		}

		string OpenCSSFile (string filename)
		{
			string style;
			using (var sr = new StreamReader (Assets.Open (filename))) {
				style = sr.ReadToEnd ();
			}

			return style;
		}
	}
}


