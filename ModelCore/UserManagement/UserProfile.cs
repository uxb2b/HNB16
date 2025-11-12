using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Linq;


using CommonLib.Core.DataWork;


using CommonLib.Utility;
using ModelCore.BankManagement;
using ModelCore.Properties;
using ModelCore.DataModel;
using ModelCore.Locale;
using Newtonsoft.Json;
using ModelCore.Schema;
using System.Data.Linq;

namespace ModelCore.UserManagement
{
    /// <summary>
    /// Summary description for UserProfile.
    /// </summary>
    public class UserProfile
    {
        private BankUser _profile;
        private Hashtable _values;
        internal Organization _company;

        public DateTime checkPoint;

        public UserProfile()
        {
            //
            // TODO: Add constructor logic here
            //
            _values = new Hashtable();
        }

        public static UserProfile CreateInstance(string pid, string securedCode)
        {
            var profile = CreateInstance(pid);
            securedCode = securedCode.MakePassword();
            if (securedCode != null && profile?.ProfileData.PASSWORD == securedCode)
            {
                return profile;
            }

            return null;
        }

        public static UserProfile CreateInstance(string pid)
        {
            ModelSource models = new ModelSource();

            var user = models.GetTable<BankUser>()
                            .FirstOrDefault(u => u.PID == pid);

            if (user != null)
            {
                return CreateInstance(user);
            }

            return null;
        }


        public static UserProfile CreateInstance(BankUser user)
        {
            if (user == null)
            {
                return null;
            }

            UserProfile profile = new UserProfile()
            {
                _profile = user,
                checkPoint = DateTime.Now
            };

            return profile;

        }


        private String[] _bank = null;
        public String[] BranchCodeItems
        {
            get
            {
                if(_bank == null)
                {
                    if (_profile?.BankUserBranch != null)
                    {
                        _bank = _profile.BankUserBranch
                                        .Select(b => b.BRANCH_ID)
                                        .ToArray();
                    }
                    else
                    {
                        _bank = [];
                    }
                }
                return _bank;
            }
        }

        public BankUser ProfileData
        {
            get
            {
                return _profile;
            }
        }

        public string PID
        {
            get
            {
                return _profile.PID;
            }
        }

        public object this[object key]
        {
            get
            {
                return _values[key];
            }
            set
            {
                _values[key] = value;
            }
        }

        public void ClearAttributes()
        {
            _values.Clear();
        }

        public void RemoveAttribute(object key)
        {
            _values.Remove(key);
        }


        public string UserName
        {
            get
            {
                return _profile.USER_NAME;
            }
            set
            {
                _profile.USER_NAME = value;
            }
        }


        public XmlDocument SiteMenu
        {
            get;
            private set;
        }

        public string MenuDataPath
        {
            get;
            set;
        }


        public Organization Company
        {
            get
            {
                return _company;
            }
        }

        public int CompanyID
        {
            get
            {
                return _company?.CompanyID ?? -1;
            }
        }

    }

    public static partial class ExtensionMethods
    {
        public static int GetCompanyID(this UserProfile up)
        {
            return up.CompanyID;
        }

    }

}
