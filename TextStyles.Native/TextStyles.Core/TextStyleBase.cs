using System;
using System.Collections.Generic;
using System.Linq;

namespace TextStyles.Core
{
	public class TextStyleBase
	{
		protected static readonly string MainID = "main";

		// <summary>
		/// The default size for text.
		/// </summary>
		public float DefaultTextSize = 18f;

		public event EventHandler StylesChanged;

		protected Dictionary<string, TextStyleParameters> _textStyles;

		public TextStyleBase ()
		{
			_textStyles = new Dictionary<string, TextStyleParameters> ();
		}

		/// <summary>
		/// Sets the CSS string
		/// </summary>
		/// <param name="css">Css Style Sheet</param>
		public virtual void SetCSS (string css)
		{
			var styles = CssTextStyleParser.Parse (css);
			SetStyles (styles);
		}

		/// <summary>
		/// Sets the styles dictionary
		/// </summary>
		/// <param name="styles">Styles dictionary</param>
		public virtual void SetStyles (Dictionary<string, TextStyleParameters> styles)
		{
			_textStyles = styles;
			Refresh ();
		}

		/// <summary>
		/// Sets the style.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="style">Style.</param>
		/// <param name="refresh">If set to <c>true</c> refresh.</param>
		public virtual void SetStyle (string id, TextStyleParameters style, bool refresh = false)
		{
			_textStyles [id] = style;
			if (refresh) {
				Refresh ();
			}
		}

		/// <summary>
		/// Sets the body css style for the customTags.
		/// </summary>
		/// <param name="baseStyleID">The CSS selector name for the body style</param>
		/// <param name="customTags">A list of CSSTagStyle custom tags</param>
		public void SetBaseStyle (string baseStyleID, ref List<CssTagStyle> customTags)
		{
			if (customTags == null)
				customTags = new List<CssTagStyle> ();

			if (!customTags.Any (x => x.Tag == "body")) {
				customTags.Add (new CssTagStyle (HtmlTextStyleParser.BODYTAG) { Name = baseStyleID });
			}
		}

		/// <summary>
		/// Gets the styles.
		/// </summary>
		/// <returns>The styles.</returns>
		public virtual List<TextStyleParameters> GetStyles ()
		{
			return _textStyles.Select (v => v.Value).ToList ();
		}

		/// <summary>
		/// Gets a style by its selector
		/// </summary>
		/// <returns>The style.</returns>
		/// <param name="selector">Selector.</param>
		public TextStyleParameters GetStyle (string selector)
		{
			return _textStyles.ContainsKey (selector) ? _textStyles [selector] : null;
		}

		/// <summary>
		/// Signals that the styles have been updated.
		/// </summary>
		public void Refresh ()
		{
			StylesChanged?.Invoke (this, EventArgs.Empty);
		}
	}
}

