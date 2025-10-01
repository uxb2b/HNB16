using System;
using System.Collections.Generic;

namespace ModelCore.Locale
{
    /// <summary>
    /// Naming 的摘要描述。
    /// </summary>
    public class Naming
    {
        private Naming()
        {
            //
            // TODO: 在此加入建構函式的程式碼
            //
        }

        private static int[] __BankProcessLevel = new int[] { 
            (int)DocumentLevel.待經辦審核,
            (int)DocumentLevel.待主管審核,
            (int)DocumentLevel.待CRC登錄,
            (int)DocumentLevel.待放行,
            (int)DocumentLevel.已開立
        };

        public static int[] BankProcessLevel
        {
            get
            {
                return __BankProcessLevel;
            }
        }

        public enum DocumentLevel
        {
            待經辦審核 = 0,
            待主管審核 = 1,
            待CRC登錄 = 2,
            待放行 = 3,
            已開立 = 4,
            待註記 = 5,
            已註銷 = 6,
            主動餘額註銷_待登錄 = 7,
            主動餘額註銷_待放行 = 8,
            信用狀餘額為零 = 9,
            已暫存 = 50,
            企業主管核放中 = 51,
            企業主管退回_審核 = 52,
            待賣方核定 = 53,
            待買方核定 = 54,
            企業主管放行中 = 55,
            企業主管退回_放行 = 56,
            企業文件已刪除 = 57,
            文件預覽 = 100,
            銀行已拒絕 = 101,
            已退回_主管退回 = 102,
            已退回_CRC退回 = 103,
            已退回_CRC主管退回 = 104,
            拒絕押匯_自動退回 = 105,
            押匯文件準備中 = 106,
            押匯申請已轉帳 = 107,
            瑕疵押匯 = 109,
            押匯資料補登 = 110,
            臨櫃申請待登錄 = 111,
            臨櫃申請銀行已拒絕 = 112,
            臨櫃申請CRC主管退回 = 113,
            客戶端已確認 = 200,
        }

        public enum DocumentLevelQueryScope
        {
            ForEmployee = 1,
            ForEnterprise,
        }


        public static readonly int?[] DocumentFinalState = new int?[]
        {
            (int)Naming.DocumentLevel.已開立,
            (int)Naming.DocumentLevel.企業文件已刪除,
            (int)Naming.DocumentLevel.已註銷,
            (int)Naming.DocumentLevel.臨櫃申請銀行已拒絕,
            //(int)Naming.DocumentLevel.銀行已拒絕,
        };

        public static readonly int?[] DocumentEffectiveState = new int?[]
        {
            (int)Naming.DocumentLevel.已開立,
            (int)Naming.DocumentLevel.已註銷,
        };

        public enum BeneficiaryStatus
        {
            新增待放行 = 21,
            修改待放行 = 22,
            刪除待放行 = 23,
            已核准 = 24,
            受益人已刪除 = 25,
            修改待放行_EAI = 26,
            已退回 = 102,
            已刪除 = 6
        }

        public enum DocumentTypeDefinition
        {
            開狀申請書 = 0,
            修狀申請書 = 1,
            信用狀註銷申請書 = 2,
            押匯申請書 = 5,
            還款改貸申請書 = 6,
            承兌匯票到期付款 = 7,
            改貸還款 = 8,
        }

        public enum MessageTypeDefinition
        {
            MSG_NONE = 0,
            MSG_CREDIT_APP_READY = 1,//開狀申請待受理
            MSG_CREDIT_APP_TO_CHECK = 2,//開狀申請待審核
            MSG_CREDIT_APP_DENIED = 3,//開狀申請已拒絕
            MSG_CREDIT_APP_TO_REGISTER = 4,//開狀申請待登錄
            MSG_CREDIT_APP_DAO_WITHDRAW = 5,//開狀申請主管退回
            MSG_CREDIT_APP_TO_ALLOW = 6,//開狀申請待放行
            MSG_CREDIT_APP_CRC_WITHDRAW = 7,//開狀申請CRC退回
            MSG_CREDIT_APP_LOAN_WITHDRAW = 8,//開狀申請退回
            MSG_CREDIT_CREATED = 9,//信用狀已開立
            MSG_CREDIT_APP_CANCELLED = 10,//開狀申請書退回

            MSG_AMENDMENT_APP_READY = 11,//改狀申請待受理
            MSG_AMENDMENT_APP_TO_CHECK = 12,//改狀申請待審核
            MSG_AMENDMENT_APP_DENIED = 13,//改狀申請已拒絕
            MSG_AMENDMENT_APP_TO_REGISTER = 14,//改狀申請待登錄
            MSG_AMENDMENT_APP_DAO_WITHDRAW = 15,//改狀申請主管退回
            MSG_AMENDMENT_APP_TO_ALLOW = 16,//改狀申請待放行
            MSG_AMENDMENT_APP_CRC_WITHDRAW = 17,//改狀申請CRC退回
            MSG_AMENDMENT_APP_LOAN_WITHDRAW = 18,//改狀申請退回
            MSG_AMENDMENT_CREATED = 19,//改狀申請已開立
            MSG_AMENDMENT_APP_CANCELLED = 20,//改狀申請書退回

            MSG_CANCELLATION_APP_READY = 21,//註銷申請待受理
            MSG_CANCELLATION_APP_TO_CHECK = 22,//註銷申請待審核
            MSG_CANCELLATION_APP_DENIED = 23,//註銷申請已拒絕
            MSG_CANCELLATION_APP_DAO_WITHDRAW = 24,//註銷申請主管退回
            MSG_CANCELLATION_APP_TO_ALLOW = 25,//註銷申請待放行
            MSG_CANCELLATION_CREATED = 26,//註銷申請已開立
            MSG_CANCELLATION_APP_CANCELLED = 27,//註銷申請書退回

            //還款
            MSG_REIM_APP_READY = 28,//還款申請待受理
            MSG_REIM_APP_TO_CHECK = 29,//還款申請待審核
            MSG_REIM_APP_DENIED = 30,//還款申請已拒絕
            MSG_REIM_APP_TO_REGISTER = 31,//還款申請待登錄
            MSG_REIM_APP_DAO_WITHDRAW = 32,//還款申請主管退回
            MSG_REIM_APP_TO_ALLOW = 33,//還款申請待放行
            MSG_REIM_APP_CRC_WITHDRAW = 34,//還款申請CRC退回
            MSG_REIM_APP_LOAN_WITHDRAW = 35,//還款申請退回
            MSG_REIM_CREATED = 36,//還款已完成(企業端)
            MSG_REIM_APP_CANCELLED = 37,//還款申請書退回

            //改貸
            MSG_LOAN_APP_READY = 38,//改貸申請待受理
            MSG_LOAN_APP_TO_CHECK = 39,//改貸申請待審核
            MSG_LOAN_APP_DENIED = 40,//改貸申請已拒絕
            MSG_LOAN_APP_TO_REGISTER = 41,//改貸申請待登錄
            MSG_LOAN_APP_DAO_WITHDRAW = 42,//改貸申請主管退回
            MSG_LOAN_APP_TO_ALLOW = 43,//改貸申請待放行
            MSG_LOAN_APP_CRC_WITHDRAW = 44,//改貸申請CRC退回
            MSG_LOAN_APP_LOAN_WITHDRAW = 45,//改貸申請退回
            MSG_LOAN_CREATED = 46,//改貸已接受
            MSG_LOAN_APP_CANCELLED = 47,//改貸申請書退回

            MSG_CANCELLATION_APP_SELLER = 48,//註銷申請通知賣方

            MSG_CREDIT_APP_APPLY = 49,//開狀申請書待核放
            MSG_AMENDMENT_APP_APPLY = 50,//改狀申請書待核放
            MSG_CANCELLATION_APP_APPLY = 51,//註銷申請書待核放
            MSG_REIM_APP_APPLY = 52,//還款申請書待核放
            MSG_DRAFT = 53,//押匯

            MSG_CREDIT_APP_RETRIAL = 54,//開狀申請待註記
            MSG_AMENDMENT_RETRIAL = 55,//改狀申請待註記
            MSG_CANNEL_RETRIAL = 56,//註銷申請待註記
            MSG_REIM_RETRIAL = 57,//註記還款申請完成(mail主旨是"還款申請已開立")
            MSG_LOAN_RETRIAL = 58,//註記還款改貸申請完成(mail主旨是"改貸申請已開立")
            MSG_AMENDMENT_INFO = 59,//信用狀修改通知書待確認
            MSG_CANCELLATION_APP_SEELER_OK = 60,//註銷申請賣方已確認

            MSG_CANCELLATION_UNASKED = 63,//主動餘額註銷
            MSG_CANCELLATION_NOTICE_BUYER = 64,//主動餘額註銷通知買方
            MSG_CANCELLATION_NOTICE_SELLER = 65,//主動餘額註銷通知賣方
            MSG_BEFEFICIARY_MODIFIED = 66,//受益人資料處理
            MSG_BENEFICIARY_DELETED = 67,//受益人資料刪除

            MSG_NEGO_APP_READY = 68,//押匯申請待受理
            MSG_NEGO_APP_TO_CHECK = 69,//押匯申請待審核
            MSG_NEGO_APP_DENIED = 70,//押匯申請已拒絕
            MSG_NEGO_APP_TO_REGISTER = 71,//押匯申請待登錄
            MSG_NEGO_APP_DAO_WITHDRAW = 72,//押匯申請主管退回
            MSG_NEGO_APP_TO_ALLOW = 73,//押匯申請待放行
            MSG_NEGO_APP_CRC_WITHDRAW = 74,//押匯申請CRC退回
            MSG_NEGO_APP_LOAN_WITHDRAW = 75,//押匯申請退回
            MSG_NEGO_CREATED = 76,//押匯申請已開立
            MSG_NEGO_APP_CANCELLED = 77,//押匯申請書退回

            MSG_CREDIT_APP_APPLIED = 78,//開狀申請書企業端已送出
            MSG_AMENDMENT_APP_APPLIED = 79,//改狀申請書企業端已送出
            MSG_LOAN_APP_APPLY = 80,//改貸申請書待核放

            MSG_DRAFT_ACCEPTANCE_READY = 81,//承兌匯票待受理
            MSG_DRAFT_ACCEPTANCE_CHECK = 82,//承兌匯票待審核
            MSG_DRAFT_ACCEPTED = 83,    //匯票已承兌
            MSG_DRAFT_ACCEPTANCE_DAO_WITHDRAW = 84, //  改貸申請主管退回
            MSG_APPLY_CREDIT_ALERT = 85,            //  開狀申請信用貶弱
            MSG_AMENDMENT_CREDIT_ALERT = 86,        //  修狀申請信用貶弱
            MSG_LOAN_CREDIT_ALERT = 87,             //  改貸申請信用貶弱
        }

        public enum MessageReceipent
        { 
            ForBank,
            ForApplicant,
            ForBeneficiary,
            ForCustomer,
            ForApplicantAndBank,
            ForAll
        }

        public enum BeneficiaryDraftType
        {
            電子押匯_CDS開狀 = 1,
            非電子押匯 = 2,
            電子押匯_電子申請 = 3,
            電子押匯_台塑集團 = 4
        }

        public enum DraftType
        {
            CDS_CSC = 1,    //
            ELC = 2,
            CHB = 3,
            FPG = 4,
            WASIN = 5,
            CHIMEI = 6,
        }

        public enum DraftTypeName
        {
            中鋼集團 = 1,    //
            ELC = 2,
            網銀 = 3,
            台塑 = 4,
            華新麗華 = 5,
            奇美 = 6,
        }



        public enum UploadStatusDefinition
        {
            等待匯入 = 0,
            資料錯誤 = 1,
            匯入成功 = 2,
            匯入失敗 = 3
        }

        public enum ProcessLogStatus
        {
            資料已傳送 = 0
        }

        public enum TransportDirection
        { 
            Inbound = 0,
            Outbound = 1
        }

        public enum TransportCatalogDefinition
        {
            LetterOfCredit = 0,
            B8500FpgNegoDraft = 1,
            BatchFpgNegoRemittance = 2
        }

        public enum RemittanceStatusDefinition
        {
            匯款未完成 = -1,
            匯款待審核 = 0,
            匯款退回 = 1,
            匯款資料準備中 = 2,
            匯款資料已送出 = 3,
            匯款已完成 = 4,
            匯款失敗 = 5,
            匯款狀態查詢中 = 6,
        }

        public enum FPGReportCategory
        {
            紙本 = 0,
            電子申請 = 1,
            電子 = 2
        }

        public enum FieldDisplayType
        {
            Header = 1,
            DataItem = 2,
            Handler = 3,
            SortBy = 4,
            Query = 5,
            Edit = 6,
            Create = 7,
            DataTableColumn = 8,
            DataTableRow = 9,
        }

        public enum SortType
        {
            None = 0,
            ASC = 1,
            DESC = -1,
        }

        public enum LcUsance
        {
            AtSight = 1,
            AtUsance = 2
        }

        [Flags]
        public enum LcQueryBranch
        {
            開狀行 = 0x01,
            通知行 = 0x02,
        }

        public enum LcStatus
        {
            Effective = 1,
            Expired = 2,
            Revoked = 3,
        }

        [Flags]
        public enum BranchType
        {
            Headquarter = 1,
            Branch = 2,
            CRC = 4,
        }

        public enum TaskID
        {
            IDELC0000 = 1, //行員端待辦事項(含功能捷徑(筆數)與訊息匣)
            IDELC0101, //開狀申請受理
            IDELC0102, //開狀申請審核
            IDELC0103, //開狀申請登錄
            IDELC0104, //開狀申請放行
            IDELC0105, //註記開狀申請完成
            IDELC0106, //信用狀開立通知
            IDELC0107, //臨櫃開狀申請受理
            IDELC0201, //修狀申請受理
            IDELC0202, //修狀申請審核
            IDELC0203, //修狀申請登錄
            IDELC0204, //修狀申請放行
            IDELC0205, //註記修狀申請完成
            IDELC0206, //改狀開立通知
            IDELC0207, //臨櫃修狀申請受理
            IDELC0301, //押匯申請受理
            IDELC0302, //押匯申請審核
            IDELC0303, //押匯申請登錄
            IDELC0304, //押匯申請放行
            IDELC0305, //註記押匯申請完成
            IDELC0401, //承兌匯票到期付款受理
            IDELC0402, //承兌匯票到期付款放行
            IDELC0403, //註記承兌匯票到期付款完成
            IDELC0501, //國內信用狀應收帳款還款受理
            IDELC0502, //國內信用狀應收帳款還款放行
            IDELC0503, //註記國內信用狀應收帳款還款申請完成
            IDELC0601, //即期國內信用狀改貸申請受理
            IDELC0602, //即期國內信用狀改貸申請審核
            IDELC0603, //即期國內信用狀改貸申請登錄
            IDELC0604, //即期國內信用狀改貸申請放行
            IDELC0605, //註記即期國內信用狀改貸申請完成
            IDELC0701, //臨櫃改貸還款申請
            IDELC0702, //臨櫃改貸還款放行
            IDELC0703, //註記臨櫃改貸還款完成
            IDELC0801, //餘額註銷申請受理
            IDELC0802, //餘額註銷申請核放
            IDELC0803, //主動餘額註銷登錄
            IDELC0804, //主動餘額註銷放行
            IDELC0805, //註記餘額註銷申請完成
            IDELC0901, //開狀申請書查詢
            IDELC0902, //修狀申請書查詢
            IDELC0903, //押匯申請查詢
            IDELC0904, //國內信用狀應收帳款還款申請查詢
            IDELC0905, //即期國內信用狀改貸申請查詢
            IDELC0906, //國內信用狀改貸還款查詢
            IDELC0907, //餘額註銷查詢
            IDELC0908, //客戶額度查詢(含押匯、改貸額度查詢)
            IDELC0909, //免申請人確認之受益人清單查詢
            IDELC0910, //交易金額報表查詢
            IDELC0911, //受益人清冊查詢
            IDELC0912, //開狀人約定事項查詢
            IDELC0913, //通訊查詢
            IDELC0914, //使用者權限查詢
            IDELC0915, //分行清冊查詢
            IDELC0916, //非本行受益人推展資料查詢
            IDELC0917, //信用狀查詢
            IDELC0918, //修改通知書查詢
            IDELC1001, //開狀人約定事項維護
            IDELC1002, //開狀人約定事項審核
            IDELC1003, //受益人清冊維護
            IDELC1004, //受益人清冊審核
            IDELC1005, //允許超押比率維護
            IDELC1006, //允許超押比率審核
            IDELC1007, //通訊通知人員設定
            IDELC1008, //分行清冊維護
            IDELC1009, //系統服務與排程監控
            IDELC1101, //台塑瑕疵押匯受理
            IDELC1102, //台塑押匯款匯出作業
            IDELC1103, //台塑押匯款匯出審核
            IDELC1104, //註記台塑押匯款匯出完成
            IDELC1105, //台塑受益人事業部建檔匯入
            IDELC1106, //台塑押匯補登資料匯入
            IDELC1107, //台塑備償帳戶維護
            IDELC1108, //台塑備償帳戶審核
            IDELC1109, //台塑押匯款匯出帳務狀態查詢
            IDELC1110, //台塑信用狀統計表
            IDELC1111, //台塑通訊傳輸排程檢視
            IDELC1112, //設定系統簽章憑證
            IDELC1113, //押匯款匯出手續費設定
            IDELC1114, //押匯款匯出手續費審核
            IDELC1201, //傳送信用狀開立資料
            IDELC1202, //傳送押匯放行通知
            WDELC0000, //企業端待辦事項(含功能捷徑(筆數)與訊息匣)
            WDELC0101, //開狀申請書新申請
            WDELC0102, //開狀申請書複製已申請信用狀
            WDELC0103, //開狀申請書查詢
            WDELC0201, //修狀申請書
            WDELC0202, //修狀申請書查詢
            WDELC0203, //修改通知書查詢
            WDELC0301, //餘額註銷申請
            WDELC0302, //確認餘額註銷申請
            WDELC0303, //餘額註銷查詢
            WDELC0401, //押匯申請
            WDELC0402, //審核放行押匯申請
            WDELC0403, //押匯申請線上簽核
            WDELC0404, //查詢押匯申請
            WDELC0501, //即期國內信用狀改貸申請申請
            WDELC0502, //即期國內信用狀改貸申請放行
            WDELC0503, //查詢即期國內信用狀改貸
            WDELC0601, //改貸還款申請
            WDELC0602, //改貸還款放行
            WDELC0603, //查詢改貸還款
            WDELC0701,//查詢開狀額度資料
            WDELC0702,//查詢押匯額度資料
            WDELC0703, //查詢改貸額度資料
            WDELC0704, //訊息匣維護

        }

        public static Dictionary<TaskID, String> TaskIDNaming = new Dictionary<TaskID, string>
            {
                { Naming.TaskID.IDELC0000, "行員端待辦事項" },
                { Naming.TaskID.IDELC0101, "開狀申請受理" },
                { Naming.TaskID.IDELC0102, "開狀申請審核" },
                { Naming.TaskID.IDELC0103, "開狀申請登錄" },
                { Naming.TaskID.IDELC0104, "開狀申請放行" },
                { Naming.TaskID.IDELC0105, "註記開狀申請完成" },
                { Naming.TaskID.IDELC0106, "信用狀開立通知" },
                { Naming.TaskID.IDELC0107, "臨櫃開狀申請受理" },
                { Naming.TaskID.IDELC0201, "修狀申請受理" },
                { Naming.TaskID.IDELC0202, "修狀申請審核" },
                { Naming.TaskID.IDELC0203, "修狀申請登錄" },
                { Naming.TaskID.IDELC0204, "修狀申請放行" },
                { Naming.TaskID.IDELC0205, "註記修狀申請完成" },
                { Naming.TaskID.IDELC0206, "修狀開立通知" },
                { Naming.TaskID.IDELC0207, "臨櫃修狀申請受理" },
                { Naming.TaskID.IDELC0301, "押匯申請受理" },
                { Naming.TaskID.IDELC0302, "押匯申請審核" },
                { Naming.TaskID.IDELC0303, "押匯申請登錄" },
                { Naming.TaskID.IDELC0304, "押匯申請放行" },
                { Naming.TaskID.IDELC0305, "註記押匯申請完成" },
                { Naming.TaskID.IDELC0401, "承兌匯票到期付款受理" },
                { Naming.TaskID.IDELC0402, "承兌匯票到期付款放行" },
                { Naming.TaskID.IDELC0403, "註記承兌匯票到期付款完成" },
                { Naming.TaskID.IDELC0501, "國內信用狀應收帳款還款受理" },
                { Naming.TaskID.IDELC0502, "國內信用狀應收帳款還款放行" },
                { Naming.TaskID.IDELC0503, "註記國內信用狀應收帳款還款申請完成" },
                { Naming.TaskID.IDELC0601, "即期國內信用狀改貸申請受理" },
                { Naming.TaskID.IDELC0602, "即期國內信用狀改貸申請審核" },
                { Naming.TaskID.IDELC0603, "即期國內信用狀改貸申請登錄" },
                { Naming.TaskID.IDELC0604, "即期國內信用狀改貸申請放行" },
                { Naming.TaskID.IDELC0605, "註記即期國內信用狀改貸申請完成" },
                { Naming.TaskID.IDELC0701, "臨櫃還(付)款、改貸申請" },
                { Naming.TaskID.IDELC0702, "臨櫃還(付)款、改貸放行" },
                { Naming.TaskID.IDELC0703, "註記臨櫃還款、改貸完成" },
                { Naming.TaskID.IDELC0801, "餘額註銷申請受理" },
                { Naming.TaskID.IDELC0802, "餘額註銷申請核放" },
                { Naming.TaskID.IDELC0803, "主動餘額註銷登錄" },
                { Naming.TaskID.IDELC0804, "主動餘額註銷放行" },
                { Naming.TaskID.IDELC0805, "註記餘額註銷申請完成" },
                { Naming.TaskID.IDELC0901, "開狀申請書查詢" },
                { Naming.TaskID.IDELC0902, "修狀申請書查詢" },
                { Naming.TaskID.IDELC0903, "押匯申請查詢" },
                { Naming.TaskID.IDELC0904, "國內信用狀應收帳款還款申請查詢" },
                { Naming.TaskID.IDELC0905, "即期國內信用狀改貸申請查詢" },
                { Naming.TaskID.IDELC0906, "國內信用狀改貸還款查詢" },
                { Naming.TaskID.IDELC0907, "餘額註銷查詢" },
                { Naming.TaskID.IDELC0908, "客戶額度查詢(含押匯、改貸額度查詢)" },
                { Naming.TaskID.IDELC0909, "免申請人確認之受益人清單查詢" },
                { Naming.TaskID.IDELC0910, "交易金額報表查詢" },
                { Naming.TaskID.IDELC0911, "受益人清冊查詢" },
                { Naming.TaskID.IDELC0912, "開狀人約定事項查詢" },
                { Naming.TaskID.IDELC0913, "通訊查詢" },
                { Naming.TaskID.IDELC0914, "使用者權限查詢" },
                { Naming.TaskID.IDELC0915, "分行清冊查詢" },
                { Naming.TaskID.IDELC0916, "非本行受益人推展資料查詢" },
                { Naming.TaskID.IDELC0917, "信用狀查詢" },
                { Naming.TaskID.IDELC0918, "修改通知書查詢" },
                { Naming.TaskID.IDELC1001, "開狀人約定事項維護" },
                { Naming.TaskID.IDELC1002, "開狀人約定事項審核" },
                { Naming.TaskID.IDELC1003, "受益人清冊維護" },
                { Naming.TaskID.IDELC1004, "受益人清冊審核" },
                { Naming.TaskID.IDELC1005, "允許超押比率維護" },
                { Naming.TaskID.IDELC1006, "允許超押比率審核" },
                { Naming.TaskID.IDELC1007, "通訊通知人員設定" },
                { Naming.TaskID.IDELC1008, "分行清冊維護" },
                { Naming.TaskID.IDELC1009, "系統服務與排程監控" },
                { Naming.TaskID.IDELC1101, "台塑瑕疵押匯受理" },
                { Naming.TaskID.IDELC1102, "台塑押匯款匯出作業" },
                { Naming.TaskID.IDELC1103, "台塑押匯款匯出審核" },
                { Naming.TaskID.IDELC1104, "台塑押匯款匯出完成註記" },
                { Naming.TaskID.IDELC1105, "台塑受益人事業部建檔匯入" },
                { Naming.TaskID.IDELC1106, "台塑押匯補登資料匯入" },
                { Naming.TaskID.IDELC1107, "台塑備償帳戶維護" },
                { Naming.TaskID.IDELC1108, "台塑備償帳戶審核" },
                { Naming.TaskID.IDELC1109, "台塑押匯款匯出帳務狀態查詢" },
                { Naming.TaskID.IDELC1110, "台塑信用狀統計表" },
                { Naming.TaskID.IDELC1111, "台塑通訊傳輸排程檢視" },
                { Naming.TaskID.IDELC1112, "設定系統簽章憑證" },
                { Naming.TaskID.IDELC1113, "押匯款匯出手續費設定" },
                { Naming.TaskID.IDELC1114, "押匯款匯出手續費審核" },
                { Naming.TaskID.IDELC1201, "傳送信用狀開立資料" },
                { Naming.TaskID.IDELC1202, "傳送押匯放行通知" },
                { Naming.TaskID.WDELC0000, "企業端待辦事項(含功能捷徑(筆數)與訊息匣)" },
                { Naming.TaskID.WDELC0101, "新申請" },
                { Naming.TaskID.WDELC0102, "複製已申請" },
                { Naming.TaskID.WDELC0103, "開狀申請書查詢" },
                { Naming.TaskID.WDELC0201, "修狀申請書" },
                { Naming.TaskID.WDELC0202, "修狀申請書查詢" },
                { Naming.TaskID.WDELC0203, "修改通知書查詢" },
                { Naming.TaskID.WDELC0301, "餘額註銷申請" },
                { Naming.TaskID.WDELC0302, "確認餘額註銷申請" },
                { Naming.TaskID.WDELC0303, "餘額註銷查詢" },
                { Naming.TaskID.WDELC0401, "押匯申請" },
                { Naming.TaskID.WDELC0402, "審核放行押匯申請" },
                { Naming.TaskID.WDELC0403, "押匯申請線上簽核" },
                { Naming.TaskID.WDELC0404, "查詢押匯申請" },
                { Naming.TaskID.WDELC0501, "國內信用狀改貸＼還款申請" },
                { Naming.TaskID.WDELC0502, "國內信用狀改貸＼還款審核放行" },
                { Naming.TaskID.WDELC0503, "查詢國內信用狀改貸＼還款" },
                { Naming.TaskID.WDELC0601, "改貸還款申請" },
                { Naming.TaskID.WDELC0602, "改貸還款放行" },
                { Naming.TaskID.WDELC0603, "查詢改貸還款" },
                { Naming.TaskID.WDELC0701,"查詢開狀額度" },
                { Naming.TaskID.WDELC0702,"查詢押匯額度" },
                { Naming.TaskID.WDELC0703, "查詢改貸額度" },
                { Naming.TaskID.WDELC0704, "訊息匣維護" },
        };

        public static Dictionary<TaskID, String> TaskIDNamingEN = new Dictionary<TaskID, string>
            {
                { Naming.TaskID.IDELC0000, "行員端待辦事項" },
                { Naming.TaskID.IDELC0101, "開狀申請受理" },
                { Naming.TaskID.IDELC0102, "開狀申請審核" },
                { Naming.TaskID.IDELC0103, "開狀申請登錄" },
                { Naming.TaskID.IDELC0104, "開狀申請放行" },
                { Naming.TaskID.IDELC0105, "註記開狀申請完成" },
                { Naming.TaskID.IDELC0106, "信用狀開立通知" },
                { Naming.TaskID.IDELC0107, "臨櫃開狀申請受理" },
                { Naming.TaskID.IDELC0201, "修狀申請受理" },
                { Naming.TaskID.IDELC0202, "修狀申請審核" },
                { Naming.TaskID.IDELC0203, "修狀申請登錄" },
                { Naming.TaskID.IDELC0204, "修狀申請放行" },
                { Naming.TaskID.IDELC0205, "註記修狀申請完成" },
                { Naming.TaskID.IDELC0206, "改狀開立通知" },
                { Naming.TaskID.IDELC0207, "臨櫃修狀申請受理" },
                { Naming.TaskID.IDELC0301, "押匯申請受理" },
                { Naming.TaskID.IDELC0302, "押匯申請審核" },
                { Naming.TaskID.IDELC0303, "押匯申請登錄" },
                { Naming.TaskID.IDELC0304, "押匯申請放行" },
                { Naming.TaskID.IDELC0305, "註記押匯申請完成" },
                { Naming.TaskID.IDELC0401, "承兌匯票到期付款受理" },
                { Naming.TaskID.IDELC0402, "承兌匯票到期付款放行" },
                { Naming.TaskID.IDELC0403, "註記承兌匯票到期付款完成" },
                { Naming.TaskID.IDELC0501, "國內信用狀應收帳款還款受理" },
                { Naming.TaskID.IDELC0502, "國內信用狀應收帳款還款放行" },
                { Naming.TaskID.IDELC0503, "註記國內信用狀應收帳款還款申請完成" },
                { Naming.TaskID.IDELC0601, "即期國內信用狀改貸申請受理" },
                { Naming.TaskID.IDELC0602, "即期國內信用狀改貸申請審核" },
                { Naming.TaskID.IDELC0603, "即期國內信用狀改貸申請登錄" },
                { Naming.TaskID.IDELC0604, "即期國內信用狀改貸申請放行" },
                { Naming.TaskID.IDELC0605, "註記即期國內信用狀改貸申請完成" },
                { Naming.TaskID.IDELC0701, "臨櫃改貸還款申請" },
                { Naming.TaskID.IDELC0702, "臨櫃改貸還款放行" },
                { Naming.TaskID.IDELC0703, "註記臨櫃改貸還款完成" },
                { Naming.TaskID.IDELC0801, "餘額註銷申請受理" },
                { Naming.TaskID.IDELC0802, "餘額註銷申請核放" },
                { Naming.TaskID.IDELC0803, "主動餘額註銷登錄" },
                { Naming.TaskID.IDELC0804, "主動餘額註銷放行" },
                { Naming.TaskID.IDELC0805, "註記餘額註銷申請完成" },
                { Naming.TaskID.IDELC0901, "開狀申請書查詢" },
                { Naming.TaskID.IDELC0902, "修狀申請書查詢" },
                { Naming.TaskID.IDELC0903, "押匯申請查詢" },
                { Naming.TaskID.IDELC0904, "國內信用狀應收帳款還款申請查詢" },
                { Naming.TaskID.IDELC0905, "即期國內信用狀改貸申請查詢" },
                { Naming.TaskID.IDELC0906, "國內信用狀改貸還款查詢" },
                { Naming.TaskID.IDELC0907, "餘額註銷查詢" },
                { Naming.TaskID.IDELC0908, "客戶額度查詢(含押匯、改貸額度查詢)" },
                { Naming.TaskID.IDELC0909, "免申請人確認之受益人清單查詢" },
                { Naming.TaskID.IDELC0910, "交易金額報表查詢" },
                { Naming.TaskID.IDELC0911, "受益人清冊查詢" },
                { Naming.TaskID.IDELC0912, "開狀人約定事項查詢" },
                { Naming.TaskID.IDELC0913, "通訊查詢" },
                { Naming.TaskID.IDELC0914, "使用者權限查詢" },
                { Naming.TaskID.IDELC0915, "分行清冊查詢" },
                { Naming.TaskID.IDELC0916, "非本行受益人推展資料查詢" },
                { Naming.TaskID.IDELC0917, "信用狀查詢" },
                { Naming.TaskID.IDELC0918, "修改通知書查詢" },
                { Naming.TaskID.IDELC1001, "開狀人約定事項維護" },
                { Naming.TaskID.IDELC1002, "開狀人約定事項審核" },
                { Naming.TaskID.IDELC1003, "受益人清冊維護" },
                { Naming.TaskID.IDELC1004, "受益人清冊審核" },
                { Naming.TaskID.IDELC1005, "允許超押比率維護" },
                { Naming.TaskID.IDELC1006, "允許超押比率審核" },
                { Naming.TaskID.IDELC1007, "通訊通知人員設定" },
                { Naming.TaskID.IDELC1008, "分行清冊維護" },
                { Naming.TaskID.IDELC1009, "系統服務與排程監控" },
                { Naming.TaskID.IDELC1101, "台塑瑕疵押匯受理" },
                { Naming.TaskID.IDELC1102, "台塑押匯款匯出作業" },
                { Naming.TaskID.IDELC1103, "台塑押匯款匯出審核" },
                { Naming.TaskID.IDELC1104, "註記台塑押匯款匯出完成" },
                { Naming.TaskID.IDELC1105, "台塑受益人事業部建檔匯入" },
                { Naming.TaskID.IDELC1106, "台塑押匯補登資料匯入" },
                { Naming.TaskID.IDELC1107, "台塑備償帳戶維護" },
                { Naming.TaskID.IDELC1108, "台塑備償帳戶審核" },
                { Naming.TaskID.IDELC1109, "台塑押匯款匯出帳務狀態查詢" },
                { Naming.TaskID.IDELC1110, "台塑信用狀統計表" },
                { Naming.TaskID.IDELC1111, "台塑通訊傳輸排程檢視" },
                { Naming.TaskID.IDELC1112, "設定系統簽章憑證" },
                { Naming.TaskID.IDELC1201, "傳送信用狀開立資料" },
                { Naming.TaskID.WDELC0000,"To-Do List"},
                { Naming.TaskID.WDELC0101,"New"},
                { Naming.TaskID.WDELC0102,"Copy applied form"},
                { Naming.TaskID.WDELC0103,"L/C issuance application form inquiry"},
                { Naming.TaskID.WDELC0201,"Modify application form"},
                { Naming.TaskID.WDELC0202,"Inquiry on the modified application form"},
                { Naming.TaskID.WDELC0203,"Inquiry on the modified application notification"},
                { Naming.TaskID.WDELC0301,"Balance write-off application"},
                { Naming.TaskID.WDELC0302,"Confirm balance write-off application"},
                { Naming.TaskID.WDELC0303,"Balance write-off inquiry"},
                { Naming.TaskID.WDELC0401,"Negotiation application"},
                { Naming.TaskID.WDELC0402,"Confirm negotiation application"},
                { Naming.TaskID.WDELC0403,"Negotiation L/C applicant online signing"},
                { Naming.TaskID.WDELC0404,"Inquiry on negotiation application"},
                { Naming.TaskID.WDELC0501,"Payment or advance financing loan by L/C application"},
                { Naming.TaskID.WDELC0502,"Confirm payment or advance financing loan application"},
                { Naming.TaskID.WDELC0503,"Inquiry on L/C payment or advance financing loan"},
                { Naming.TaskID.WDELC0601,"L/C at sight advance financing loan repayment application"},
                { Naming.TaskID.WDELC0602,"Confirm L/C at sight advance financing loan repayment application"},
                { Naming.TaskID.WDELC0603,"Inquiry on L/C at sight advance financing loan repayment"},
                { Naming.TaskID.WDELC0701,"Inquiry on L/C amount data"},
                { Naming.TaskID.WDELC0702,"Inquiry on negotiation amount data"},
                { Naming.TaskID.WDELC0703,"Inquiry on Inquiry L/C at sight advance financing loan amount data"},
                { Naming.TaskID.WDELC0704,"Manage inbox"},
        };

        public enum ApplicationFormMode
        {
            Standard = 0,
            ForCRC = 1
        }

        public enum ApplicationViewMode
        {
            Standard = 0,
            Audit = 1,
            GoodsAttachment = 2,
            AuditWithGoodsAttachment = 3,
            Check = 4,
            Preview = 5,
            Review = 6,
            Permit = 7,
            Pending = 8,
        }

        public enum SanctionProcessMode
        {
            Single = 0,
            Batch = 1,
            BatchCheck = 2,
            BatchReject = 3
        }

        public enum DataResultMode
        {
            Display = 0,
            Print = 1,
            Download = 2,
            DataContent = 3,
            ForExcel = 4,
        }

        public enum UnitBehavior
        {
            Bank = 1,
            Enterprise = 2,
        }

        public enum BusinessType
        {
            Applicant = 1,
            Beneficiary = 2
        }

        public enum FpgServiceType
        {
            TXD2SB29 = 1,
            TXD2SB30 = 2,
            TXD2SB31 = 3,
            TXD2SB32 = 4,
            TXD2SB33 = 5,
            TXD2SB38 = 6,
            TXD2SB39 = 7,
            TXD2SB40 = 8,
        }

    }
}
