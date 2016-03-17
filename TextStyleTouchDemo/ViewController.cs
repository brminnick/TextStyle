﻿
using System;
using UIKit;
using System.IO;
using TextStyles.Touch;
using System.Collections.Generic;
using TextStyles.Core;
using CoreGraphics;
using System.Diagnostics;

namespace TextStyleTouchDemo
{
	public partial class ViewController : UIViewController
	{
		const string headingOne = @"The difference between";
		const string headingTwo = @"Ordinary & Extraordinary";
		const string headingThree = @"Is that little <spot>extra</spot>";
		const string textBody = @"Geometry can produce legible letters but <i>art alone</i> makes them beautiful.<br/><br/>Art begins where geometry ends, and imparts to letters a character trascending mere measurement.";

		StyleManager _styleManager;
		UIView _divider;
		bool _isFirstStyleSheet = true;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			// Load the CSS file
			var style = File.ReadAllText ("StyleOne.css");
			TextStyle.Instance.SetCSS (style);

			// TEMP
			var stopwatch = Stopwatch.StartNew ();

			// Create a StyleManager to handle any CSS changes automatically
			_styleManager = new StyleManager ();
			_styleManager.Add (labelOne, "h2", headingOne);
			_styleManager.Add (labelTwo, "h1", headingTwo);
			_styleManager.Add (labelThree, "h2", headingThree, new List<CssTagStyle> {
				new CssTagStyle ("spot"){ CSS = "spot{color:" + Colors.SpotColor.ToHex () + "}" }
			});
			_styleManager.Add (body, "body", textBody);

			Console.WriteLine ("Elapsed time {0}", stopwatch.ElapsedMilliseconds);

			AddUIElements ();
		}

		void AddUIElements ()
		{
			// Add a visual divider
			_divider = new UIView ();
			_divider.BackgroundColor = Colors.Grey;
			_divider.UserInteractionEnabled = false;
			_divider.AutoresizingMask = UIViewAutoresizing.FlexibleWidth;
			Add (_divider);


			// Swapping Button
			var frame = View.Frame;
			var button = new SwapButton ();
			button.Frame = new CGRect (frame.Width / 2 - SwapButton.SIZE * 2, frame.Height - SwapButton.SIZE * 1.5f, SwapButton.SIZE, SwapButton.SIZE);
			button.TouchUpInside += (sender, e) => {
				var cssFileName = _isFirstStyleSheet ? "StyleTwo.css" : "StyleOne.css";
				var css = File.ReadAllText (cssFileName);

				_isFirstStyleSheet = !_isFirstStyleSheet;
				TextStyle.Instance.SetCSS (css);
			};
			Add (button);

			// Next Page Button
			var nextPageButton = new NextButton ();
			nextPageButton.Frame = new CGRect (frame.Width / 2 + SwapButton.SIZE, frame.Height - SwapButton.SIZE * 1.5f, SwapButton.SIZE, SwapButton.SIZE);
			nextPageButton.TouchUpInside += (sender, e) => NavigationController.PushViewController (new ManualViewController (), true);
			Add (nextPageButton);
		}

		public override void ViewDidLayoutSubviews ()
		{
			base.ViewDidLayoutSubviews ();

			_divider.Frame = new CGRect (0, body.Frame.Y, View.Frame.Width, 1.0 / UIScreen.MainScreen.Scale);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden (true, true);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


