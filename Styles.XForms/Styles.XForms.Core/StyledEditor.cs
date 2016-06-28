using System;
using System.Collections.Generic;
using Styles.Core.Text;
using Xamarin.Forms;

namespace Styles.XForms.Core
{
	public class StyledEditor : Editor, IStyledView
	{
		public string RawText { get; set; }
		public string CssStyle { get; set; }
		public string TextStyleInstance { get; set; }
		public List<CssTagStyle> CustomTags { get; set; }
	}
}