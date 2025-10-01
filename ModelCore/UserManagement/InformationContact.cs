using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Utility;

namespace ModelCore.UserManagement
{
    public class InformationContact
    {
        private static readonly String _ContactFileName = Path.Combine(CommonLib.Core.Utility.Logger.LogPath, "ContactList.json");

        static InformationContact()
        {
            lock (typeof(InformationContact))
            {
                if (!File.Exists(_ContactFileName))
                {
                    File.WriteAllText(_ContactFileName, JsonConvert.SerializeObject(new ContactAdress[] { }));
                }
            }
        }

        public static ContactAdress[] Items
        {
            get
            {
                try
                {
                    return JsonConvert.DeserializeObject<ContactAdress[]>(File.ReadAllText(_ContactFileName));
                }
                catch(Exception ex)
                {
                    ModelCore.Helper.Logger.Error(ex);
                    return new ContactAdress[] { };
                }
            }
            set
            {
                lock (typeof(InformationContact))
                {
                    File.WriteAllText(_ContactFileName, JsonConvert.SerializeObject(value ?? new ContactAdress[] { }));
                }
            }
        }
    }

    public class ContactAdress
    {
        public String UserName { get; set; }
        public String Email { get; set; }
    }
}
