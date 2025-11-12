using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ModelCore.UploadManagement;
using ModelCore.DataModel;
using CommonLib.Utility;
using CommonLib.Core.DataWork;
using ModelCore.Locale;

namespace ModelCore.UserManagement
{
    public class FpgBeneficiaryUploadManager : XmlUploadManager<LcEntityDbContext, Organization>
    {
        public BeneficiaryServiceGroup.ServiceDefinition? ServiceGroup { get; set; } = BeneficiaryServiceGroup.ServiceDefinition.Fpg;

        public FpgBeneficiaryUploadManager() : base()
        {
        }

        public FpgBeneficiaryUploadManager(GenericManager<LcEntityDbContext> manager)
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

        protected override bool validate(ItemUpload<Organization> item)
        {

            if (string.IsNullOrEmpty(item.Entity.CompanyName))
            {
                item.Status = String.Join("、", item.Status, "公司名稱錯誤");
                _bResult = false;
            }

            if (String.IsNullOrEmpty(item.Entity.ReceiptNo) || item.Entity.ReceiptNo.Length != 8 || !ValidityAgent.ValidateString(item.Entity.ReceiptNo, 20))
            {
                item.Status = String.Join("、", item.Status, "統編格式錯誤");
                _bResult = false;
            }


            if (string.IsNullOrEmpty(item.Entity.Addr))
            {
                item.Status = String.Join("、", item.Status, "地址格式錯誤");
                _bResult = false;
            }

            //if (!GroupID.HasValue)
            //{
            //    item.Status = String.Join("、", item.Status, "未指定集團名稱");
            //    _bResult = false;
            //}

            if (item.Entity.OrganizationStatus == null || !item.Entity.OrganizationStatus.GroupID.HasValue)
            {
                item.Status = String.Join("、", item.Status, "未指定集團名稱");
                _bResult = false;
            }
            else if (ServiceGroup.HasValue)
            {
                if (!this.GetTable<BeneficiaryGroup>().Any(s => s.GroupID == item.Entity.OrganizationStatus.GroupID
                         && s.ServiceID == (int?)ServiceGroup))
                {
                    item.Status = String.Join("、", item.Status, "集團總公司名稱錯誤");
                    _bResult = false;
                }
            }


            if (_bResult)
            {
                var currentItem = this.EntityList.Where(o => o.ReceiptNo == item.Entity.ReceiptNo).FirstOrDefault();

                if (currentItem == null)
                {
                    if (item.Entity.OrganizationStatus == null)
                    {
                        item.Entity.OrganizationStatus = new OrganizationStatus
                        {
                        };
                    }
                    item.Entity.OrganizationStatus.FpgNegoBeneficiary = true;
                    //item.Entity.OrganizationStatus.GroupID = GroupID;

                    this.EntityList.Add(item.Entity);
                }
                else
                {
                    currentItem.CompanyName = item.Entity.CompanyName;
                    currentItem.ReceiptNo = item.Entity.ReceiptNo;
                    currentItem.UndertakerName = item.Entity.UndertakerName;
                    currentItem.Addr = item.Entity.Addr;
                    if (currentItem.OrganizationStatus == null)
                    {
                        currentItem.OrganizationStatus = new OrganizationStatus
                        {
                        };
                    }
                    currentItem.OrganizationStatus.FpgNegoBeneficiary = true;
                    //currentItem.OrganizationStatus.GroupID = GroupID;

                    if (item.Entity.OrganizationStatus != null)
                    {
                        currentItem.OrganizationStatus.ReserveAccount = item.Entity.OrganizationStatus.ReserveAccount;
                        currentItem.OrganizationStatus.GroupID = item.Entity.OrganizationStatus.GroupID;
                    }
                }
            }

            return _bResult;
        }
    }
}