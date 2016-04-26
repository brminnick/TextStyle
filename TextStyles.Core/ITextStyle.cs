using System;
using System.Collections.Generic;

namespace TextStyles.Core
{
	public interface ITextStyle
	{
		void SetCSS (string css);

		void SetStyles (Dictionary<string, TextStyleParameters> styles);

		void Refresh ();
	}
}

