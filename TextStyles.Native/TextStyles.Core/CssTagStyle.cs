using System;

namespace TextStyles.Core
{
	/// <summary>
	/// Css tag style.
	/// </summary>
	public class CssTagStyle
	{
		/// <summary>
		/// Optionally set an existing StyleID to map this CssTagStyle to
		/// </summary>
		/// <value>The style I.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets the Tag ID to be used in the CSS
		/// </summary>
		/// <value>The tag.</value>
		public string Tag { get; private set; }

		/// <summary>
		/// Gets or sets the raw CSS value for the Tag
		/// </summary>
		/// <value>The CS.</value>
		public string CSS { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Occur.TextStyles.Core.CssTagStyle"/> class.
		/// </summary>
		/// <param name="tag">Tag.</param>
		public CssTagStyle (string tag)
		{
			Tag = tag;
		}

	}
}

