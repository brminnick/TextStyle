﻿using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content.PM;
using Styles.Core.Text;
using System.Collections.Generic;
using System;
using Styles.Droid.Text;
using Styles.Droid;
using Android.Graphics;

namespace TextStyleDemo.Droid
{
	[Activity (Theme = "@android:style/Theme.Holo.Light", Label = "TextStyle Demo", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		const string headingOne = @"The difference between";
		const string headingTwo = @"Ordinary & Extraordinary";
		const string headingThree = @"Is that little <spot>extra</spot>";
		const string textBody = @"Geometry can produce legible letters but <i>art alone</i> makes them beautiful.<p>Art begins where geometry ends, and imparts to letters a character trascending mere measurement.</p>";

		bool _isFirstStyleSheet = true;
		StyleManager _styleManager;
		Dictionary<string, TextStyleParameters> _parsedStylesOne;
		Dictionary<string, TextStyleParameters> _parsedStylesTwo;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			TextStyle.Main.AddFont ("Archistico", "Archistico_Simple.ttf");
			TextStyle.Main.AddFont ("Avenir-Medium", "Avenir-Medium.ttf");
			TextStyle.Main.AddFont ("Avenir-Book", "Avenir-Book.ttf");
			TextStyle.Main.AddFont ("Avenir-Heavy", "Avenir-Heavy.ttf");
			TextStyle.Main.AddFont ("BreeSerif-Regular", "BreeSerif-Regular.ttf");
			TextStyle.Main.AddFont ("OpenSans-CondBold", "OpenSans-CondBold.ttf");
			TextStyle.Main.AddFont ("OpenSans-CondLight", "OpenSans-CondLight.ttf");

			_parsedStylesOne = CssTextStyleParser.Parse (OpenCSSFile ("StyleOne.css"));
			_parsedStylesTwo = CssTextStyleParser.Parse (OpenCSSFile ("StyleTwo.css"));
			TextStyle.Main.SetStyles (_parsedStylesOne);

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
				var styles = _isFirstStyleSheet ? _parsedStylesTwo : _parsedStylesOne;
				TextStyle.Main.SetStyles (styles);

				_isFirstStyleSheet = !_isFirstStyleSheet;
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

