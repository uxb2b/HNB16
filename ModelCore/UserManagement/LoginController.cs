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
//				//當有User登入成功時,就將User的資料存到Session中當成全域資源.
//				HttpContext.Current.Session[Model.Service.LogonSvc.USER_PROFILE_SESSION_KEY] = up;
//				bAuth = true;
//			}
//
//			if(bAuth)
//			{
//				msg = "登入成功!!";
//			}
//			else
//			{
//				msg = "您的帳號或密碼有誤，請重新輸入!!";
//			}

			return bAuth;

		}
	}
}
