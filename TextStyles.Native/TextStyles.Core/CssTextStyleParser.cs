using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TextStyles.Core
{
	public static class CssTextStyleParser
	{
		// Dictionary of all properties found on TextStyleParameters
		static Dictionary<string, PropertyInfo> _textStyleProperties = typeof(TextStyleParameters).GetRuntimeProperties ()
			.Select (p => new { p, attr = p.GetCustomAttributes (typeof(CssAttribute), true) })
			.Where (prop => prop.attr.Count () == 1)
			.Select (obj => new { Property = obj.p, Attribute = obj.attr.First () as CssAttribute })
			.ToDictionary (t => t.Attribute.Name, t => t.Property);

		/// <summary>
		/// Parses the specified CSS
		/// </summary>
		/// <param name="css">Css.</param>
		public static Dictionary<string, TextStyleParameters> Parse (string css)
		{
			var parser = new CssParser ();
			var rules = parser.ParseAll (css);

			var textStyles = new Dictionary<string, TextStyleParameters> ();

			foreach (var rule in rules) {
				foreach (var selector in rule.Selectors) {

					// If it doesnt exist, create it
					if (!textStyles.ContainsKey (selector))
						textStyles [selector] = new TextStyleParameters (selector);

					var curStyle = textStyles [selector];
					ParseCSSRule (ref curStyle, rule);
				}

			}

			return textStyles;
		}

		/// <summary>
		/// Merges a css rule.
		/// </summary>
		/// <returns>The rule.</returns>
		/// <param name="curStyle">Target TextStyleParameters</param>
		/// <param name="css">Css value</param>
		/// <param name="clone">If set to <c>true</c> clone the style</param>
		public static TextStyleParameters MergeRule (TextStyleParameters curStyle, string css, bool clone)
		{
			var parser = new CssParser ();
			var rules = parser.ParseAll (css);
			if (rules.Count () != 1) {
				throw new NotSupportedException ("Only a single css class may be merged at a time");
			}

			var mergedStyle = clone ? curStyle.Clone () : curStyle;

			var rule = rules.FirstOrDefault ();
			if (rule != null) {
				ParseCSSRule (ref mergedStyle, rule);
			}

			return mergedStyle;
		}

		/// <summary>
		/// Parses the CSS rule.
		/// </summary>
		/// <param name="curStyle">Current style.</param>
		/// <param name="rule">Rule.</param>
		internal static void ParseCSSRule (ref TextStyleParameters curStyle, CssParserRule rule)
		{
			foreach (var declaration in rule.Declarations) {
				if (_textStyleProperties.ContainsKey (declaration.Property)) {
					var cleanedValue = declaration.Value.Replace ("\"", "");
					cleanedValue = cleanedValue.Trim ();
					var prop = _textStyleProperties [declaration.Property];
					switch (prop.PropertyType.Name) {
					case "String":
						curStyle.SetValue (prop.Name, cleanedValue);
						break;
					case "Int32":
						int numInt;
						if (int.TryParse (cleanedValue, out numInt)) {
							curStyle.SetValue (prop.Name, numInt);
						}
						break;
					case "Single":
						cleanedValue = cleanedValue.Replace ("px", "");
						float numFloat;
						if (float.TryParse (cleanedValue, out numFloat)) {
							curStyle.SetValue (prop.Name, numFloat);
						} else
							throw new Exception ("Failed to Parse Single value " + cleanedValue);
						break;
					case "Single[]":
						var parts = cleanedValue.Split (new char [] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
						var parsedValues = new float [parts.Length];
						for (int i = 0; i < parts.Length; i++) {
							float numArrayFloat;
							if (float.TryParse (parts [i], out numArrayFloat)) {
								parsedValues [i] = numArrayFloat;
							}
						}
						curStyle.SetValue (prop.Name, parsedValues);
						break;
					case "TextStyleAlign":
						curStyle.TextAlign = EnumUtils.FromDescription<TextStyleAlign> (cleanedValue);
						break;
					case "TextStyleDecoration":
						curStyle.TextDecoration = EnumUtils.FromDescription<TextStyleDecoration> (cleanedValue);
						break;
					case "TextStyleTextTransform":
						curStyle.TextTransform = EnumUtils.FromDescription<TextStyleTextTransform> (cleanedValue);
						break;
					case "TextStyleTextOverflow":
						curStyle.TextOverflow = EnumUtils.FromDescription<TextStyleTextOverflow> (cleanedValue);
						break;
					case "TextStyleFontStyle":
						curStyle.FontStyle = EnumUtils.FromDescription<TextStyleFontStyle> (cleanedValue);
						break;
					case "TextStyleFontWeight":
						curStyle.FontWeight = EnumUtils.FromDescription<TextStyleFontWeight> (cleanedValue);
						break;
					default:
						throw new InvalidCastException ("Could not find the appropriate type " + prop.PropertyType.Name);
					}
				}
			}
		}

		/// <summary>
		/// Parses to CSS string.
		/// </summary>
		/// <returns>The to CSS string.</returns>
		/// <param name="tagName">Tag name.</param>
		/// <param name="style">Style.</param>
		public static string ParseToCSSString (string tagName, TextStyleParameters style)
		{
			var builder = new StringBuilder ();
			builder.Append (tagName + "{");

			string cast;
			var runtimeProperties = style.GetType ().GetRuntimeProperties ();
			foreach (var prop in runtimeProperties) {
				try {
					var value = prop.GetValue (style);

					if (value != null) {
						string parsedValue = null;
						switch (prop.PropertyType.Name) {
						case "String":
							if ((value as string).StartsWith ("#"))
								parsedValue = (string)value;
							else
								parsedValue = "'" + value + "'";
							break;
						case "Single":
							if (Convert.ToSingle (value) > float.MinValue) {
								parsedValue = Convert.ToString (value);
								if (prop.Name == "FontSize") // Dirty, I really need a list of things that can be set in pixel values
									parsedValue += "px";
							}
							break;
						case "Int32":
							if (Convert.ToInt32 (value) > int.MinValue)
								parsedValue = Convert.ToString (value);
							break;
						case "Single[]":
							parsedValue = Convert.ToString (value);
							break;
						case "TextStyleAlign":
						case "TextStyleDecoration":
						case "TextStyleTextTransform":
						case "TextStyleTextOverflow":
							cast = Convert.ToString (value);
							if (cast != "None")
								parsedValue = cast.ToLower ();
							break;
						case "TextStyleFontStyle":
						case "TextStyleFontWeight":
							cast = Convert.ToString (value);
							if (cast != "Normal")
								parsedValue = cast.ToLower ();
							break;
						default:
							throw new InvalidCastException ("Could not find the appropriate type " + prop.PropertyType.Name);
						}

						var attributes = (CssAttribute[])prop.GetCustomAttributes (
							                 typeof(CssAttribute), false);

						if (attributes.Length > 0 && parsedValue != null)
							builder.Append (attributes [0].Name + ":" + parsedValue + ";");
					}
				} catch (Exception ex) {
					throw ex;
				}
			}

			builder.Append ("}");

			return builder.ToString ();
		}
	}
}

