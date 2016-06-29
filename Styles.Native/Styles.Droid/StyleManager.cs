using System;
using System.Collections.Generic;
using System.Text;
using Android.Widget;
using Styles.Core.Text;

namespace Styles.Droid.Text
{
	public class StyleManager : IDisposable
	{
		private Dictionary<object, ViewStyle> _views;
		TextStyle _instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="Occur.TextStyles.Android.StyleManager"/> class.
		/// </summary>
		public StyleManager (TextStyle instance = null)
		{
			_instance = instance ?? TextStyle.Main;
			_views = new Dictionary<object, ViewStyle> ();
			_instance.StylesChanged += TextStyle_Instance_StylesChanged;
		}

		/// <summary>
		/// Creates and styles a new text container (UIlabel, UITextView, UITextField)
		/// </summary>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to display. Plain or with html tags</param>
		/// <param name="customTags">A list of custom <c>CSSTagStyle</c> instances that set the styling for the html</param>
		/// <param name="useExistingStyles">Existing CSS styles willl be used If set to <c>true</c></param>
		/// <param name="encoding">String encoding type</param>
		/// <typeparam name="T">Text container type (UIlabel, UITextView, UITextField)</typeparam>
		public T Create<T> (string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true)
		{
			var target = _instance.Create<T> (styleID, text, customTags, useExistingStyles);
			_instance.SetBaseStyle (styleID, ref customTags);

			var reference = new ViewStyle (_instance, target as TextView, styleID, text, true) {
				StyleID = styleID,
				CustomTags = customTags
			};

			_views.Add (target, reference);

			return target;
		}

		/// <summary>
		/// Adds an existing text container (UILabel, UITextView, UITextField) to the StyleManager and styles it
		/// </summary>
		/// <param name="target">Target text container</param>
		/// <param name="styleID">The CSS selector name for the style</param>
		/// <param name="text">Text to display. Plain or with html tags</param>
		/// <param name="customTags">A list of custom <c>CSSTagStyle</c> instances that set the styling for the html</param>
		/// <param name="useExistingStyles">Existing CSS styles willl be used If set to <c>true</c></param>
		/// <param name="encoding">String encoding type</param>
		public void Add (object target, string styleID, string text = "", List<CssTagStyle> customTags = null, bool useExistingStyles = true, Encoding encoding = null, bool enableHtmlEditing = false)
		{
			var viewStyle = new ViewStyle (_instance, (TextView)target, styleID, text, true) {
				StyleID = styleID,
				CustomTags = customTags,
				EnableHtmlEditing = enableHtmlEditing
			};

			_views.Add (target, viewStyle);
			viewStyle.UpdateText ();
			viewStyle.UpdateDisplay ();
		}

		/// <summary>
		/// Updates the text of a target text container (UILabel, UITextView, UITextField)
		/// </summary>
		/// <param name="target">Target text container</param>
		/// <param name="text">Text</param>
		public void UpdateText (object target, string text)
		{
			var viewStyle = _views [target];
			if (viewStyle == null) {
				return;
			}

			viewStyle.UpdateText (text);
			viewStyle.UpdateDisplay ();
		}

		/// <summary>
		/// Updates the styling and display of all registered text containers
		/// </summary>
		public void UpdateAll ()
		{
			// Update the Attrib strings first as they can take some time
			foreach (var item in _views.Values) {
				item.UpdateText ();
			}

			// Update the displays after so they change all at once
			foreach (var item in _views.Values) {
				item.UpdateDisplay ();
			}
		}

		/// <summary>
		/// Updates the frames of any text containers with line heights smaller than the fonts default
		/// </summary>
		public void UpdateFrames ()
		{
			// Update the frames of any linespaced itemss
			//foreach (var item in _views.Values) {
			//	item.UpdateFrame ();
			//}
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:TextStyles.Android.StyleManager"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:TextStyles.Android.StyleManager"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="T:TextStyles.Android.StyleManager"/> in an unusable state. After
		/// calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:TextStyles.Android.StyleManager"/> so the garbage collector can reclaim the memory that the
		/// <see cref="T:TextStyles.Android.StyleManager"/> was occupying.</remarks>
		public void Dispose ()
		{
			foreach (var item in _views.Values) {
				item.Dispose ();
			}

			_views.Clear ();
			_views = null;

			_instance.StylesChanged -= TextStyle_Instance_StylesChanged;
		}

		void TextStyle_Instance_StylesChanged (object sender, EventArgs e)
		{
			UpdateAll ();
		}

	}
}



