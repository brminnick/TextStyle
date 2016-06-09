using System.Collections.Generic;
using TextStyles.Core;
using Xamarin.Forms;

namespace TextStyles.XForms.Core
{
	public class StyledLabel : Label
	{
		public string CssStyle { get; set; }
		public string TextStyleInstance { get; set; }
		public List<CssTagStyle> CustomTags { get; set; }
	}

	public interface IStyledLabel
	{

	}
}
