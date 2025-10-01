using System;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using ModelCore.UserManagement;
//
using CommonLib.Utility;
using ModelCore.CommonManagement;
using ModelCore.Locale;

namespace ModelCore.EventMessageApp
{
	/// <summary>
	/// IMessageApp ªººK­n´y­z¡C
	/// </summary>
	public interface IMessageApp
	{
		void ProcessEvent(int? docID);
	}

	public class MessageType
	{

		public static string[] MSG_TYPE;
		internal static Hashtable MailControl;
		internal static Hashtable MailManager;
		internal static Hashtable MessageManager;
		internal static Hashtable MessageControl;

		static MessageType()
		{
			MessageManager = new Hashtable();
			MessageControl = new Hashtable();
			MailControl = new Hashtable();
			MailManager = new Hashtable();

			//			Assembly currAsm = Assembly.GetExecutingAssembly();
			object[] args = new object[1];

			using(CommonInboxManager dalc = new CommonInboxManager())
			{
				var items = dalc.EntityList;

				MSG_TYPE = new string[items.Count()+1];
				MSG_TYPE[0] = "";
				int i=1;

				foreach(var row in items)
				{
					MSG_TYPE[i++] = (string)row.Subject;

					if(ValidityAgent.IsSignificantString(row.MessageProcessor))
					{
						args[0] = row.TypeID;
						if(!String.IsNullOrEmpty(row.MessageProcessor))
							MessageManager.Add((int)row.TypeID,(IMessageApp)System.Activator.CreateInstance(Type.GetType((string)row.MessageProcessor),args));

						if(!String.IsNullOrEmpty(row.MailProcessor))
							MailManager.Add((int)row.TypeID,(IMessageApp)System.Activator.CreateInstance(Type.GetType((string)row.MailProcessor),args));

						if(!String.IsNullOrEmpty(row.MessageControl))
							MessageControl.Add((int)row.TypeID,row.MessageControl);

						if(!String.IsNullOrEmpty(row.MailControl))
							MailControl.Add((int)row.TypeID,row.MailControl);
					}
				}
			}

			//					MessageManager[i] = (IMessageApp)currAsm.CreateInstance(MSG_PROCESSOR_TYPE[i]);

		}




	}

}