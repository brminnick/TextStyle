using System;
using System.Collections.Generic;

namespace Styles.Core.Text
{
	public interface ITextStyleParser
	{
		Dictionary<string, TextStyleParameters> Parse (string css);

		string ParseToCSSString (string tagName, TextStyleParameters style);
	}
}

