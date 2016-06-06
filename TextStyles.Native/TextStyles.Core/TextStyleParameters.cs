using System;
using System.Reflection;
using System.Linq;

namespace TextStyles.Core
{
	/// <summary>
	/// Text alignment enum.
	/// </summary>
	public enum TextStyleAlign
	{
		[CssAttribute ("left")]
		Left,
		[CssAttribute ("right")]
		Right,
		[CssAttribute ("center")]
		Center,
		[CssAttribute ("justified")]
		Justified
	}

	/// <summary>
	/// Text decoration enum.
	/// </summary>
	public enum TextStyleDecoration
	{
		[CssAttribute ("none")]
		None,
		[CssAttribute ("underline")]
		Underline,
		[CssAttribute ("line-through")]
		LineThrough
	}

	/// <summary>
	/// Text transform enum
	/// </summary>
	public enum TextStyleTextTransform
	{
		[CssAttribute ("none")]
		None,
		[CssAttribute ("capitalize")]
		Capitalize,
		[CssAttribute ("uppercase")]
		UpperCase,
		[CssAttribute ("lowercase")]
		LowerCase
	}

	/// <summary>
	/// Text overflow enum
	/// </summary>
	public enum TextStyleTextOverflow
	{
		[CssAttribute ("none")]
		None,
		[CssAttribute ("clip")]
		Clip,
		[CssAttribute ("ellipsis")]
		Ellipsis
	}

	/// <summary>
	/// Font Style enum
	/// </summary>
	public enum TextStyleFontStyle
	{
		[CssAttribute ("normal")]
		Normal,
		[CssAttribute ("italic")]
		Italic
	}

	/// <summary>
	/// Font Weight enum
	/// </summary>
	public enum TextStyleFontWeight
	{
		[CssAttribute ("normal")]
		Normal,
		[CssAttribute ("bold")]
		Bold
	}

	/// <summary>
	/// Text style parameters.
	/// </summary>
	public class TextStyleParameters : Object
	{
		#region CSS Properties

		/// <summary>
		/// Sets all the font properties in one declaration
		/// </summary>
		/// <value>The font.</value>
		[CssAttribute ("font-family")]
		public string Font { get; set; }

		/// <summary>
		/// Specifies the font size of text
		/// </summary>
		/// <value>The size of the font.</value>
		[CssAttribute ("font-size")]
		public float FontSize { get; set; }

		/// <summary>
		///  Specifies the font style
		/// </summary>
		/// <value>The font style.</value>
		[CssAttribute ("font-style")]
		public TextStyleFontStyle FontStyle { get; set; }

		/// <summary>
		///  Specifies the font weight
		/// </summary>
		/// <value>The font style.</value>
		[CssAttribute ("font-weight")]
		public TextStyleFontWeight FontWeight { get; set; }

		/// <summary>
		/// Sets the color of text
		/// </summary>
		/// <value>The color.</value>
		[CssAttribute ("color")]
		public string Color { get; set; }

		/// <summary>
		/// Increases or decreases the space between characters in a text
		/// </summary>
		/// <value>The letter spacing.</value>
		[CssAttribute ("letter-spacing")]
		public float LetterSpacing { get; set; }

		/// <summary>
		/// line-height	Sets the line height
		/// </summary>
		/// <value>The height of the line.</value>
		[CssAttribute ("line-height")]
		public float LineHeight { get; set; }

		/// <summary>
		/// Specifies the horizontal alignment of text
		/// </summary>
		/// <value>The text align.</value>
		[CssAttribute ("text-align")]
		public TextStyleAlign TextAlign { get; set; }

		/// <summary>
		/// Specifies the decoration added to text
		/// </summary>
		/// <value>The text decoration.</value>
		[CssAttribute ("text-decoration")]
		public TextStyleDecoration TextDecoration { get; set; }

		/// <summary>
		/// Specifies the indentation of the first line in a text-block
		/// </summary>
		/// <value>The text indent.</value>
		[CssAttribute ("text-indent")]
		public float TextIndent { get; set; }

		/// <summary>
		/// Specifies the how excess text is displayed
		/// </summary>
		/// <value>The text overflow.</value>
		[CssAttribute ("text-overflow")]
		public TextStyleTextOverflow TextOverflow { get; set; }

		/// <summary>
		/// Controls the capitalization of text
		/// </summary>
		/// <value>The text transform.</value>
		[CssAttribute ("text-transform")]
		public TextStyleTextTransform TextTransform { get; set; }

		/// <summary>
		/// Specifies the color of the background.
		/// </summary>
		/// <value>The color of the background.</value>
		[CssAttribute ("background-color")]
		public string BackgroundColor { get; set; }

		/// <summary>
		/// A shorthand property for setting all the padding properties in one declaration
		/// </summary>
		/// <value>The padding.</value>
		[CssAttribute ("padding")]
		public float [] Padding { get; set; }

		/// <summary>
		/// Sets the bottom padding of an element
		/// </summary>
		/// <value>The padding bottom.</value>
		[CssAttribute ("padding-bottom")]
		public float PaddingBottom { get; set; }

		/// <summary>
		/// Sets the left padding of an element
		/// </summary>
		/// <value>The padding left.</value>
		[CssAttribute ("padding-left")]
		public float PaddingLeft { get; set; }

		/// <summary>
		/// Sets the right padding of an element
		/// </summary>
		/// <value>The padding right.</value>
		[CssAttribute ("padding-right")]
		public float PaddingRight { get; set; }

		/// <summary>
		/// Sets the top padding of an element
		/// </summary>
		/// <value>The padding top.</value>
		[CssAttribute ("padding-top")]
		public float PaddingTop { get; set; }

		#endregion

		#region Custom Properties

		/// <summary>
		/// Custom CSS atribute that specifies the number of lines
		/// </summary>
		/// <value>The lines.</value>
		[CssAttribute ("lines")]
		public int Lines { get; set; }

		/// <summary>
		/// Specifies the color of the decoration added to text
		/// </summary>
		/// <value>The color of the text decoration.</value>
		[CssAttribute ("text-decoration-color")]
		public string TextDecorationColor { get; set; }

		/// <summary>
		/// Stores reference to the raw CSS
		/// </summary>
		/// <value>The raw CS.</value>
		public string RawCSS { get; set; }

		#endregion


		/// <summary>
		/// Name of the Selector
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Occur.TextStyles.Core.TextStyleParameters"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		public TextStyleParameters (string name)
		{
			Name = name;
			PaddingBottom = float.MinValue;
			PaddingLeft = float.MinValue;
			PaddingRight = float.MinValue;
			PaddingTop = float.MinValue;
			Lines = int.MinValue;
		}

		/// <summary>
		/// Sets a value by name
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		/// <param name="value">Value.</param>
		public void SetValue (string propertyName, object value)
		{
			Type myType = typeof (TextStyleParameters);
			PropertyInfo myPropInfo = myType.GetRuntimeProperty (propertyName);
			myPropInfo.SetValue (this, value, null);
		}

		/// <summary>
		/// Gets a value by name.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="propertyName">Property name.</param>
		public object GetValue (string propertyName)
		{
			Type myType = typeof (TextStyleParameters);
			PropertyInfo myPropInfo = myType.GetRuntimeProperty (propertyName);
			return myPropInfo.GetValue (this, null);
		}

		/// <summary>
		/// Gets the line height offset.
		/// </summary>
		/// <returns>The line height offset.</returns>
		public float GetLineHeightOffset ()
		{
			return FontSize - LineHeight;
		}

		/// <summary>
		/// Merge the specified style with this instance and optionally ovwerite any conflicting parameters
		/// </summary>
		/// <param name="style">Source.</param>
		/// <param name="overwriteExisting">Overwrite existing.</param>
		public void Merge (TextStyleParameters style, bool overwriteExisting)
		{
			Type t = typeof (TextStyleParameters);

			var properties = t.GetRuntimeProperties ().Where (prop => prop.CanRead && prop.CanWrite);

			foreach (var prop in properties) {
				var sourceValue = prop.GetValue (style, null);

				if (sourceValue != null) {
					var targetValue = prop.GetValue (this, null);

					switch (prop.Name) {
					case "TextAlign":
						if ((TextStyleAlign)sourceValue != TextStyleAlign.Left)
							targetValue = null;
						break;
					case "TextDecoration":
						if ((TextStyleDecoration)sourceValue != TextStyleDecoration.None)
							targetValue = null;
						break;
					case "TextOverflow":
						if ((TextStyleTextOverflow)sourceValue != TextStyleTextOverflow.None)
							targetValue = null;
						break;
					case "TextTransform":
						if ((TextStyleTextTransform)sourceValue != TextStyleTextTransform.None)
							targetValue = null;
						break;
					}

					if (targetValue != null && !overwriteExisting)
						continue;

					prop.SetValue (this, sourceValue, null);
				}
			}
		}

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public TextStyleParameters Clone ()
		{
			return (TextStyleParameters)MemberwiseClone ();
		}
	}

	/// <summary>
	/// Css attribute.
	/// </summary>
	[System.AttributeUsage (System.AttributeTargets.All)]
	public class CssAttribute : System.Attribute
	{
		/// <summary>
		/// Name of the CSS selector
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Occur.TextStyles.Core.CssAttribute"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		public CssAttribute (string name)
		{
			this.Name = name;
		}
	}
}

