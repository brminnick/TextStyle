using System;
using Xamarin.Forms;

namespace Styles.XForms.Core
{
	public static class ColorExtensions
	{
		/// <summary>
		/// Converts a UIColor value to a string hex value
		/// </summary>
		/// <returns>A string hex value</returns>
		/// <param name="color">Target UIColor</param>
		public static string ToHex (this Color color)
		{
			return string.Format ("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
		}

		/// <summary>
		/// Creates a UIColor from a int hex value
		/// </summary>
		/// <returns>UIColor</returns>
		/// <param name="color">Extension UIColor reference</param>
		/// <param name="hexValue">Hex value as an int</param>
		public static Color FromHex (this Color color, int hexValue)
		{
			return new Color (
				(((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
				(((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
				(((float)(hexValue & 0xFF)) / 255.0f)
			);
		}

		/// <summary>
		/// Creates a UIColor from a string hex value
		/// </summary>
		/// <returns>UIColor</returns>
		/// <param name="color">Extension UIColor reference</param>
		/// <param name="hexValue">Hex value as an int</param>
		/// <param name="alpha">Alpha value of the color</param>
		public static Color FromHex (this Color color, string hexValue, float alpha = 1.0f)
		{
			var colorString = hexValue.Replace ("#", "");
			if (alpha > 1.0f) {
				alpha = 1.0f;
			} else if (alpha < 0.0f) {
				alpha = 0.0f;
			}

			float red, green, blue;

			switch (colorString.Length) {
			case 3: // #RGB
				{
					red = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (0, 1)), 16) / 255f;
					green = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (1, 1)), 16) / 255f;
					blue = Convert.ToInt32 (string.Format ("{0}{0}", colorString.Substring (2, 1)), 16) / 255f;
					return new Color (red, green, blue, alpha);
				}
			case 6: // #RRGGBB
				{
					red = Convert.ToInt32 (colorString.Substring (0, 2), 16) / 255f;
					green = Convert.ToInt32 (colorString.Substring (2, 2), 16) / 255f;
					blue = Convert.ToInt32 (colorString.Substring (4, 2), 16) / 255f;
					return new Color (red, green, blue, alpha);
				}

			default:
				throw new ArgumentOutOfRangeException (string.Format ("Invalid color value {0} is invalid. It should be a hex value of the form #RBG, #RRGGBB", hexValue));
			}
		}
	}
}

