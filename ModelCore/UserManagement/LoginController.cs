using System;
using System.Web;

using ModelCore.Service;

namespace ModelCore.UserManagement
{
	/// <summary>
	/// Summary description for LoginController.
	/// </summary>
	public class LoginController
	{
		public LoginController()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool ProcessLogin(string pid,string password,out string msg)
		{
			bool bAuth = false;
			msg = null;
//
//			UserProfile up = LogonSvc.CreateUserProfile(pid,password);
//			if(up!=null)	//new login
//			{
//				//��User�n�J���\��,�N�NUser����Ʀs��Session��������귽.
//				HttpContext.Current.Session[Model.Service.LogonSvc.USER_PROFILE_SESSION_KEY] = up;
//				bAuth = true;
//			}
//
//			if(bAuth)
//			{
//				msg = "�n�J���\!!";
//			}
//			else
//			{
//				msg = "�z���b���αK�X���~�A�Э��s��J!!";
//			}

			return bAuth;

		}
	}
}
