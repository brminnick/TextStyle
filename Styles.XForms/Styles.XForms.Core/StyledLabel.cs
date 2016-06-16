using System.Collections.Generic;
using Styles.Core.Text;
using Xamarin.Forms;

namespace Styles.XForms.Core
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
