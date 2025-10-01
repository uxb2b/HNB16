using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelCore.UploadManagement;
using ModelCore.DataModel;
using CommonLib.Utility;
using CommonLib.DataAccess;

namespace ModelCore.UserManagement
{
    public class GroupDepartmentUploadManager : XmlUploadManager<LcEntityDataContext, GroupDepartment>
    {
        public GroupDepartmentUploadManager() : base()
        {
        }

        public GroupDepartmentUploadManager(GenericManager<LcEntityDataContext> manager)
            : base(manager)
        {
        }

        public override void ParseData(UserProfile userProfile, string fileName, System.Text.Encoding encoding)
        {
            _userProfile = userProfile;
            base.ParseData(userProfile, fileName, encoding);
        }

        protected override void doSave()
        {
            this.SubmitChanges();

        }

        protected override bool validate(ItemUpload<GroupDepartment> item)
        {

            if (string.IsNullOrEmpty(item.Entity.Department))
            {
                item.Status = String.Join("、", item.Status, "部門名稱錯誤");
                _bResult = false;
            }

            if (string.IsNullOrEmpty(item.Entity.DepartID))
            {
                item.Status = String.Join("、", item.Status, "部門代號錯誤");
                _bResult = false;
            }


            var beneGroup = this.GetTable<BeneficiaryGroup>().Where(g => g.GroupID == item.Entity.GroupID).FirstOrDefault();
            if (beneGroup==null)
            {
                item.Status = String.Join("、", item.Status, "總公司＼集團名稱未設定");
                _bResult = false;
            }


            if (_bResult)
            {
                var currentItem = this.EntityList.Where(o => o.DepartID == item.Entity.DepartID && o.GroupID==item.Entity.GroupID).FirstOrDefault();

                if (currentItem == null)
                {
//                    this.EntityList.InsertOnSubmit(item.Entity);
                    item.Entity.BeneficiaryGroup = beneGroup;
                }
                else
                {
                    currentItem.Department = item.Entity.Department;
                }
            }

            return _bResult;
        }
    }
}