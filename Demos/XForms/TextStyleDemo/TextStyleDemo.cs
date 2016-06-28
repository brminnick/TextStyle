using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Styles.Core.Text;
using Styles.XForms.Core;
using Xamarin.Forms;

namespace TextStyleDemo.XForms
{
	public class App : Application
	{
		public App ()
		{
			var style = DependencyService.Get<ITextStyle> ();
			var css = LoadStringAsset ("TextStyleDemo.StyleOne.css");
			style.SetCSS (css);

			// The root page of your application
			var content = new ContentPage {
				Title = "TextStyleDemo XForms",
				Content = new StackLayout {
					Padding = new Thickness (20, 40, 20, 0),
					VerticalOptions = LayoutOptions.StartAndExpand,
					Children = {
						new StyledLabel {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "The difference between",
							CssStyle ="h2"
						},
						new StyledLabel {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Ordinary & Extraordinary",
							CssStyle ="h1"
						},
						new StyledLabel {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Is that little <spot>extra</spot>",
							CssStyle ="h2",
							CustomTags = new List<CssTagStyle> {
								new CssTagStyle ("spot"){ CSS = "spot{color:#ff6c00}" }
							}
						},
						new BoxView{
							HeightRequest = 20
						},
						new BoxView{
							HeightRequest = .5,
							BackgroundColor = Color.Gray
						},
						new StyledLabel {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Geometry can produce legible letters but <i>art alone</i> makes them beautiful.<br/><br/>Art begins where geometry ends, and imparts to letters a character trascending mere measurement.",
							CssStyle ="body"
						},
						new StyledEntry{
							Text = "<h1>Lorum</h1> Ipsum Facto",
							CssStyle = "body"
						}
					}
				}
			};

			MainPage = new NavigationPage (content);
		}

		string LoadStringAsset (string id)
		{
			var assembly = typeof (App).GetTypeInfo ().Assembly;
			Stream stream = assembly.GetManifestResourceStream (id);
			string text = "";
			using (var reader = new StreamReader (stream)) {
				text = reader.ReadToEnd ();
			}

			return text;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

