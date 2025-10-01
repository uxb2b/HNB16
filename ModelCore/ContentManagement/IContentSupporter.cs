using System;
using System.Collections;

namespace ModelCore.ContentManagement
{
	/// <summary>
	/// IContentSupport ªººK­n´y­z¡C
	/// </summary>
	public interface IContentSupporter
	{
		IList GetSupportedContentList();
		object GetSupportedContent();
	}
}
