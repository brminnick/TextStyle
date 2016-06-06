using System;
using TextStyles.XForms.Core;
using Xamarin.Forms;

namespace TextStyleDemo.XForms
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			var content = new ContentPage {
				Title = "TextStyleDemo.XForms",
				Content = new StackLayout {
					VerticalOptions = LayoutOptions.Center,
					Children = {
						new StyledLabel {
							HorizontalTextAlignment = TextAlignment.Center,
							Text = "Welcome to Xamarin Forms!"
						}
					}
				}
			};

			MainPage = new NavigationPage (content);
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

