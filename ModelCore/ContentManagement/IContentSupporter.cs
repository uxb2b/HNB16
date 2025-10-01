using System;
using System.Collections;

namespace ModelCore.ContentManagement
{
	/// <summary>
	/// IContentSupport ���K�n�y�z�C
	/// </summary>
	public interface IContentSupporter
	{
		IList GetSupportedContentList();
		object GetSupportedContent();
	}
}
