using Xamarin.Forms;

namespace TextStyles.XForms.Core
{
	public class StyledLabel : Label
	{
		public string CssStyle { get; set; }
		public string TextStyleInstance { get; set; }
	}

	public interface IStyledLabel
	{

	}
}
