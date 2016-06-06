using System;
using System.Collections.Generic;
using TextStyles.Core;

namespace TextStyles.Core
{
	public interface ITextStyleParser
	{
		Dictionary<string, TextStyleParameters> Parse (string css);

		string ParseToCSSString (string tagName, TextStyleParameters style);
	}
}

