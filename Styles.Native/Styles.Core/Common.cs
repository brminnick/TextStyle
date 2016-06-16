using System;
using System.Text.RegularExpressions;

namespace Styles.Core.Text
{
	public static class Common
	{
		public static readonly Regex MatchHtmlTags = new Regex (@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>");
	}
}

