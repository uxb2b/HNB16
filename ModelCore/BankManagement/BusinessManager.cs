using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;



using CommonLib.DataAccess;

using EAI.Service.Transaction;
using ModelCore.UserManagement;
using CommonLib.Utility;

using ModelCore.NegoManagement;
using ModelCore.DataModel;
using ModelCore.Helper;
using ModelCore.Properties;
using ModelCore.Locale;

namespace ModelCore.BankManagement
{
	/// <summary>
	/// Bussiness ���K�n�y�z�C
	/// </summary>
	public  class BusinessManager  : LcEntityManager<CreditApplicationDocumentary>
	{
		public BusinessManager() : base()
		{
			//
			// TODO: �b���[�J�غc�禡���{���X
			//
		}

        public BusinessManager(GenericManager<LcEntityDataContext> mgr) : base(mgr) { }

        public UserManagement.UserProfile UserProfile { get; set; }

        public BusinessManager(UserProfile userProfile)
            : this()
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
            UserProfile = userProfile;
        }

		public Organization GetBusinessDetail(String receiptNo)
		{
            return this.GetTable<Organization>().Where(o => o.ReceiptNo == receiptNo).FirstOrDefault();
		}


        public bool ApproveLcApplication(int? appID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<CreditApplicationDocumentary>().Where(a => a.AppID == appID).FirstOrDefault();
            if (item != null)
            {
                item.ApplicationDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ApproveAmendmentApplication(int? amendingID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<AmendingLcApplication>().Where(a => a.AmendingID == amendingID).FirstOrDefault();
            if (item != null)
            {
                item.ApplicationDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }


        public bool ApproveCreditCancellation(int? cancellationID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<CreditCancellation>().Where(a => a.CancellationID == cancellationID).FirstOrDefault();
            if (item != null)
            {
                item.�ӽФ�� = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ApproveNegoDraft(int? draftID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<NegoDraft>().Where(a => a.DraftID == draftID).FirstOrDefault();
            if (item != null)
            {
                item.NegoDraftExtension.NegotiateDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ApproveReimbursement(int? reimID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<Reimbursement>().Where(a => a.ReimID == reimID).FirstOrDefault();
            if (item != null)
            {
                item.ApplicationDate = DateTime.Now;
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }

        public bool ApproveNegoLoanRepayment(int? repaymentID, String approver, Naming.DocumentLevel docLevel)
        {
            var item = this.GetTable<NegoLoanRepayment>().Where(a => a.RepaymentID == repaymentID).FirstOrDefault();
            if (item != null)
            {
                item.Documentary.DoApprove(docLevel, approver, null);
                this.SubmitChanges();
                return true;
            }
            return false;
        }




        //    //�ٴڳq��BY���~�e�C��
        //    public DataSet queryBLCbefore7day()
        //    {
        //        string strSql;
        //        strSql = "select ReceiptNo,CompanyName,Email,ISSUING_BANK_CODE ";
        //        strSql = strSql + "FROM viewBLCbefore7day GROUP BY ReceiptNo,CompanyName,Email,ISSUING_BANK_CODE ";


        //        return this.FillSqlDataSet(strSql);
        //    }
        //    //�ٴڳq��BY���~�ӤH�M��
        //    public DataSet queryBLCdeallineByApp(string app_rno)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM viewBLCbyDealline  where ReceiptNo in ('";
        //        strSql = strSql + app_rno + "') order by ReceiptNo asc";


        //        return this.FillSqlDataSet(strSql);
        //    }
        //    //�ٴڳq���e�C��BY���~�ӤH�M��
        //    public DataSet queryBLCbefore7dayByApp(string app_rno)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM viewBLCbefore7day  where ReceiptNo in ('";
        //        strSql = strSql + app_rno + "') order by ReceiptNo asc";


        //        return this.FillSqlDataSet(strSql);
        //    }
        //    //todolist
        //    public DataSet QueryslcLcApp(string strkey,int status)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM viewDRLIST_TXN where DOC_ID in ('";
        //        strSql = strSql + strkey + "') and STATUS = " + status.ToString() + " order by DRAFT_APP_NO asc";

        //        return this.FillSqlDataSet(strSql);
        //    }

        //    #region �d�ߤ����� ShowBranchData
        //    //
        //    //7.11�[�J�ϥΪ̧P�_.allen
        //    public DataSet ShowBranchData(string keyWord)
        //    {
        //        string strSql;
        //        strSql = "select BranchName,BankCode,Address,Phone from BankData ";
        //        strSql = strSql + "where (BranchType = 2) and ((bankcode like '%" + keyWord + "%') or (branchName like '%" + keyWord + "%'))";
        //        return this.FillSqlDataSet(strSql);		

        //    }
        //    #endregion



        //    #region ���o�ٴڥӽЮѸ�� QueryrlcLcApp
        //    //���o�ٴڥӽЮѸ��
        //    //,draft_detail
        //    public DataSet QueryrlcLcApp(string strkey)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM view_REIM_APPL where app_rno='"+this.userPid+"' and AppNo in ('";
        //        strSql = strSql + strkey + "') order by REIM_NO asc ";
        //        //���ե�
        //        //strSql = "select * FROM viewLcCancellationApplicationToDoList ";

        //        return this.FillSqlDataSet(strSql);
        //    }
        //    //todolist
        //    public DataSet QueryrlcLcApp(string strkey,int status)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM view_REIM_APPL where app_rno='"+this.userPid+"' and AppNo in ('";
        //        strSql = strSql + strkey + "') and STATUS = " + status.ToString() + " order by REIM_NO asc ";
        //        //���ե�
        //        //strSql = "select * FROM viewLcCancellationApplicationToDoList ";

        //        return this.FillSqlDataSet(strSql);
        //    }
        //    public DataSet QueryrlcLcByDocId(string strkey,int status)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM view_REIM_APPL where app_rno='"+this.userPid+"' and sysDocID in ('";
        //        strSql = strSql + strkey + "') and STATUS = " + status.ToString() + " order by REIM_NO asc ";
        //        //���ե�
        //        //strSql = "select * FROM viewLcCancellationApplicationToDoList ";

        //        return this.FillSqlDataSet(strSql);
        //    }



        //    #endregion

        //    #region �d���ٴڥӽ� inq_reimbursement_list
        //    //inq_reimbursement_list
        //    //7.11�[�J�ϥΪ̧P�_.allen
        //    public DataSet queryREIM_APPL(IDictionary param)
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("queryREIM_APPL",_conn);
        //            ModalUtility.AssignCommandParameter(sqlCmd,param);

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;

        //    }

        //    public DataSet queryREIM_APPL(string strkey)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM view_REIM_APPL where app_rno='"+this.userPid+"' and  AppNo = '";
        //        strSql = strSql + strkey + "'";

        //        return this.FillSqlDataSet(strSql);
        //    }		

        //    public DataSet queryREIM_APPLbyDocID(string strkey)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM view_REIM_APPL where   DocID = '";
        //        strSql = strSql + strkey + "'";

        //        return this.FillSqlDataSet(strSql);
        //    }		
        //    #endregion

        //    #region �d�߭ק�ӽ� 

        //    public DataSet InquireAmendingLcApplication(IDictionary paramValue,int startRecord,int maxRecords,out int recordCount)
        //    {
        //        return this.FillSqlDataSet("InquireAmendingLcApplication",paramValue,startRecord,maxRecords,out recordCount);
        //    }

        //    #endregion

        //    #region �d�߶ײ����
        //    //add_draft_reimbursement,add_draft_reimbursement_list
        //    //7.11�[�J�ϥΪ̧P�O
        //    public DataSet queryCp(IDictionary param)
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("queryCp",_conn);
        //            ModalUtility.AssignCommandParameter(sqlCmd,param);

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;

        //    }

        //    public DataSet queryCp(string strkey)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM viewDrlistByRlc where app_rno='"+this.userPid+"' and  DRAFT_NO = '";
        //        strSql = strSql + strkey + "'";

        //        return this.FillSqlDataSet(strSql);
        //    }		



        //    public DataSet queryCpAccount(string appbank)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM viewOrganizationOfBranch where ReceiptNo ='"+this.userPid+"' " ;
        //        if(appbank != "" )
        //        {
        //            strSql = strSql + " and BankCode = '" + appbank +"' ";
        //        }


        //        return this.FillSqlDataSet(strSql);
        //    }		
        //    #endregion

        //    #region �d�߶}���Ȧ�

        //    /*
        //    public DataSet queryAppBank(IDictionary param)
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("queryAppBank",_conn);
        //            ModalUtility.AssignCommandParameter(sqlCmd,param);

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;

        //    }
        //    */

        //    //�d�߶}����
        //    //AddLcApplication,verify_from_old_application,verify_from_old_application
        //    public DataSet ShowBusinessOfApplicant(object companyID)
        //    {
        //        return this.FillSqlDataSet("ShowBusinessOfApplicant",companyID);
        //    }		
        //    #endregion

        //    #region �d�ߨ��q�H
        ///*
        //    public DataSet queryBeneData(IDictionary param)
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("queryBeneData",_conn);
        //            ModalUtility.AssignCommandParameter(sqlCmd,param);

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;

        //    }
        //    */
        //    //���o���q�H���
        //    //AddLcApplication,verify_from_old_application,verify_from_old_application
        //    public DataSet queryBeneData(string strkey2)
        //    {
        //        string strSql;
        //        strSql = "select * ";
        //        strSql = strSql + "FROM _viewBeneficiaryData where appReceiptNo = '";
        //        strSql = strSql + this.userPid  + "' and BankCode = '" ;            
        //        strSql = strSql + strkey2 + "'";
        //        strSql = strSql + " and ReceiptNo <> '30414175'" ; //�����ư�

        //        return this.FillSqlDataSet(strSql);
        //    }		
        //    #endregion

        //    #region �d�߫H�Ϊ���� GetLcDetail
        //    //7.11�[�J�ϥΪ̧P�O allen
        //    //LcDetail,verify_from_old_application,preview_lc_amendment
        //    public DataSet GetLcDetail(object LcNo)
        //    {
        //        DataSet ds = new DataSet();

        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("ShowLcDetailByLcNo",this._conn);
        //            sqlCmd.Parameters["@LcNo"].Value = LcNo;
        //            sqlCmd.Parameters["@PID"].Value = this.userPid;

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;
        //    }
        //    #endregion

        //    #region �d�߫H�Ϊ����v��� GetLcHistory
        //    //7.11�[�J�ϥΪ̧P�O allen
        //    public DataSet GetLcHistory(object LcNo)
        //    {
        //        DataSet ds = new DataSet();

        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("ShowLcHistoryByLcNo",this._conn);
        //            sqlCmd.Parameters["@LcNo"].Value = LcNo;
        //            sqlCmd.Parameters["@PID"].Value = this.userPid;

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;
        //    }

        //    #endregion


        //    #region �d�ߵ��P�ӽи��  GetCancellationApplication
        //    //cancel_lc_detail
        //    //7.11�[�J�ϥΪ̧P�O 
        //    public DataSet GetCancellationApplication(object LcNo)
        //    {
        //        DataSet ds = new DataSet();

        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("ShowCancellationApplication",this._conn);
        //            sqlCmd.Parameters["@LcNo"].Value = LcNo;
        //            sqlCmd.Parameters["@PID"].Value = this.userPid;

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;
        //    }
        //    #endregion

        //    #region �d�ߵ��P�ӽи�� GetCancellationApplicationApply
        //    //inqCancelationDetail
        //    public DataSet GetCancellationApplicationApply(object LcNo)
        //    {
        //        DataSet ds = new DataSet();

        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("quiryCancellationApplication",this._conn);
        //            sqlCmd.Parameters["@LcNo"].Value = LcNo;
        //            sqlCmd.Parameters["@PID"].Value = this.userPid;

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;
        //    }
        //    #endregion

        //    #region �d�ߵ��P�ӽи�� InquireCancellationApplication
        //    public DataSet InquireCancellationApplication(IDictionary param)
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("quiryCancellationApplication",_conn);
        //            ModalUtility.AssignCommandParameter(sqlCmd,param);
        //            sqlCmd.Parameters["@PID"].Value = this.userPid;

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;

        //    }
        //    #endregion

        //    #region �d�߭ק�q���� �̨��q�H
        //    //confirm_notice_list,inq_amendment_notice_list
        //    public DataSet InquireAmendmentInfobyBene(IDictionary param)
        //    {
        //        DataSet ds = new DataSet();
        //        try
        //        {
        //            this._conn.Open();
        //            SqlCommand sqlCmd = ModalUtility.InvokeStoredProcedure("quireAmendmentInfobyBene",_conn);
        //            ModalUtility.AssignCommandParameter(sqlCmd,param);
        //            sqlCmd.Parameters["@PID"].Value = this.userPid;

        //            using (SqlDataAdapter da = new SqlDataAdapter(sqlCmd))
        //            {
        //                da.Fill(ds);
        //            }
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }

        //        return ds;

        //    }
        //    #endregion

        //    #region �d�߭ק�q���� �̥ӽФH
        //    //confirm_notice_list,inq_amendment_notice_list
        //    public DataSet InquireAmendingLcInformation(IDictionary paramValue,int startRecord,int maxRecords,out int recordCount)
        //    {
        //        return this.FillSqlDataSet("InquireAmendingLcInformation",paramValue,startRecord,maxRecords,out recordCount);

        //    }
        //    #endregion

        //    #region �s�W��ר���T��
        //    public  string CHB_MsgApp()
        //    {

        //        try
        //        {

        //            this._conn.Open();



        //            using(SqlCommand sqlCmd = new SqlCommand("_sp_CHB_MsgApp",this._conn))
        //            {
        //                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                SqlCommandBuilder.DeriveParameters(sqlCmd);
        //                sqlCmd.ExecuteNonQuery();
        //                //���o�ӽнs��
        //                return "0001" ;


        //            }


        //        }
        //        catch(Exception ex)
        //        {

        //            return "9999";
        //        }
        //        finally
        //        {
        //            this._conn.Close();
        //        }
        //    }
        //    #endregion


    }


}
