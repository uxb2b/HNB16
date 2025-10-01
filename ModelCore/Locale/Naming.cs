using System;
using System.Collections.Generic;

namespace ModelCore.Locale
{
    /// <summary>
    /// Naming ���K�n�y�z�C
    /// </summary>
    public class Naming
    {
        private Naming()
        {
            //
            // TODO: �b���[�J�غc�禡���{���X
            //
        }

        private static int[] __BankProcessLevel = new int[] { 
            (int)DocumentLevel.�ݸg��f��,
            (int)DocumentLevel.�ݥD�޼f��,
            (int)DocumentLevel.��CRC�n��,
            (int)DocumentLevel.�ݩ��,
            (int)DocumentLevel.�w�}��
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
            �ݸg��f�� = 0,
            �ݥD�޼f�� = 1,
            ��CRC�n�� = 2,
            �ݩ�� = 3,
            �w�}�� = 4,
            �ݵ��O = 5,
            �w���P = 6,
            �D�ʾl�B���P_�ݵn�� = 7,
            �D�ʾl�B���P_�ݩ�� = 8,
            �H�Ϊ��l�B���s = 9,
            �w�Ȧs = 50,
            ���~�D�ޮ֩� = 51,
            ���~�D�ްh�^_�f�� = 52,
            �ݽ��֩w = 53,
            �ݶR��֩w = 54,
            ���~�D�ީ�椤 = 55,
            ���~�D�ްh�^_��� = 56,
            ���~���w�R�� = 57,
            ���w�� = 100,
            �Ȧ�w�ڵ� = 101,
            �w�h�^_�D�ްh�^ = 102,
            �w�h�^_CRC�h�^ = 103,
            �w�h�^_CRC�D�ްh�^ = 104,
            �ڵ����_�۰ʰh�^ = 105,
            ��פ��ǳƤ� = 106,
            ��ץӽФw��b = 107,
            �岫��� = 109,
            ��׸�Ƹɵn = 110,
            �{�d�ӽЫݵn�� = 111,
            �{�d�ӽлȦ�w�ڵ� = 112,
            �{�d�ӽ�CRC�D�ްh�^ = 113,
            �Ȥ�ݤw�T�{ = 200,
        }

        public enum DocumentLevelQueryScope
        {
            ForEmployee = 1,
            ForEnterprise,
        }


        public static readonly int?[] DocumentFinalState = new int?[]
        {
            (int)Naming.DocumentLevel.�w�}��,
            (int)Naming.DocumentLevel.���~���w�R��,
            (int)Naming.DocumentLevel.�w���P,
            (int)Naming.DocumentLevel.�{�d�ӽлȦ�w�ڵ�,
            //(int)Naming.DocumentLevel.�Ȧ�w�ڵ�,
        };

        public static readonly int?[] DocumentEffectiveState = new int?[]
        {
            (int)Naming.DocumentLevel.�w�}��,
            (int)Naming.DocumentLevel.�w���P,
        };

        public enum BeneficiaryStatus
        {
            �s�W�ݩ�� = 21,
            �ק�ݩ�� = 22,
            �R���ݩ�� = 23,
            �w�֭� = 24,
            ���q�H�w�R�� = 25,
            �ק�ݩ��_EAI = 26,
            �w�h�^ = 102,
            �w�R�� = 6
        }

        public enum DocumentTypeDefinition
        {
            �}���ӽЮ� = 0,
            �ת��ӽЮ� = 1,
            �H�Ϊ����P�ӽЮ� = 2,
            ��ץӽЮ� = 5,
            �ٴڧ�U�ӽЮ� = 6,
            �ӧI�ײ�����I�� = 7,
            ��U�ٴ� = 8,
        }

        public enum MessageTypeDefinition
        {
            MSG_NONE = 0,
            MSG_CREDIT_APP_READY = 1,//�}���ӽЫݨ��z
            MSG_CREDIT_APP_TO_CHECK = 2,//�}���ӽЫݼf��
            MSG_CREDIT_APP_DENIED = 3,//�}���ӽФw�ڵ�
            MSG_CREDIT_APP_TO_REGISTER = 4,//�}���ӽЫݵn��
            MSG_CREDIT_APP_DAO_WITHDRAW = 5,//�}���ӽХD�ްh�^
            MSG_CREDIT_APP_TO_ALLOW = 6,//�}���ӽЫݩ��
            MSG_CREDIT_APP_CRC_WITHDRAW = 7,//�}���ӽ�CRC�h�^
            MSG_CREDIT_APP_LOAN_WITHDRAW = 8,//�}���ӽаh�^
            MSG_CREDIT_CREATED = 9,//�H�Ϊ��w�}��
            MSG_CREDIT_APP_CANCELLED = 10,//�}���ӽЮѰh�^

            MSG_AMENDMENT_APP_READY = 11,//�窱�ӽЫݨ��z
            MSG_AMENDMENT_APP_TO_CHECK = 12,//�窱�ӽЫݼf��
            MSG_AMENDMENT_APP_DENIED = 13,//�窱�ӽФw�ڵ�
            MSG_AMENDMENT_APP_TO_REGISTER = 14,//�窱�ӽЫݵn��
            MSG_AMENDMENT_APP_DAO_WITHDRAW = 15,//�窱�ӽХD�ްh�^
            MSG_AMENDMENT_APP_TO_ALLOW = 16,//�窱�ӽЫݩ��
            MSG_AMENDMENT_APP_CRC_WITHDRAW = 17,//�窱�ӽ�CRC�h�^
            MSG_AMENDMENT_APP_LOAN_WITHDRAW = 18,//�窱�ӽаh�^
            MSG_AMENDMENT_CREATED = 19,//�窱�ӽФw�}��
            MSG_AMENDMENT_APP_CANCELLED = 20,//�窱�ӽЮѰh�^

            MSG_CANCELLATION_APP_READY = 21,//���P�ӽЫݨ��z
            MSG_CANCELLATION_APP_TO_CHECK = 22,//���P�ӽЫݼf��
            MSG_CANCELLATION_APP_DENIED = 23,//���P�ӽФw�ڵ�
            MSG_CANCELLATION_APP_DAO_WITHDRAW = 24,//���P�ӽХD�ްh�^
            MSG_CANCELLATION_APP_TO_ALLOW = 25,//���P�ӽЫݩ��
            MSG_CANCELLATION_CREATED = 26,//���P�ӽФw�}��
            MSG_CANCELLATION_APP_CANCELLED = 27,//���P�ӽЮѰh�^

            //�ٴ�
            MSG_REIM_APP_READY = 28,//�ٴڥӽЫݨ��z
            MSG_REIM_APP_TO_CHECK = 29,//�ٴڥӽЫݼf��
            MSG_REIM_APP_DENIED = 30,//�ٴڥӽФw�ڵ�
            MSG_REIM_APP_TO_REGISTER = 31,//�ٴڥӽЫݵn��
            MSG_REIM_APP_DAO_WITHDRAW = 32,//�ٴڥӽХD�ްh�^
            MSG_REIM_APP_TO_ALLOW = 33,//�ٴڥӽЫݩ��
            MSG_REIM_APP_CRC_WITHDRAW = 34,//�ٴڥӽ�CRC�h�^
            MSG_REIM_APP_LOAN_WITHDRAW = 35,//�ٴڥӽаh�^
            MSG_REIM_CREATED = 36,//�ٴڤw����(���~��)
            MSG_REIM_APP_CANCELLED = 37,//�ٴڥӽЮѰh�^

            //��U
            MSG_LOAN_APP_READY = 38,//��U�ӽЫݨ��z
            MSG_LOAN_APP_TO_CHECK = 39,//��U�ӽЫݼf��
            MSG_LOAN_APP_DENIED = 40,//��U�ӽФw�ڵ�
            MSG_LOAN_APP_TO_REGISTER = 41,//��U�ӽЫݵn��
            MSG_LOAN_APP_DAO_WITHDRAW = 42,//��U�ӽХD�ްh�^
            MSG_LOAN_APP_TO_ALLOW = 43,//��U�ӽЫݩ��
            MSG_LOAN_APP_CRC_WITHDRAW = 44,//��U�ӽ�CRC�h�^
            MSG_LOAN_APP_LOAN_WITHDRAW = 45,//��U�ӽаh�^
            MSG_LOAN_CREATED = 46,//��U�w����
            MSG_LOAN_APP_CANCELLED = 47,//��U�ӽЮѰh�^

            MSG_CANCELLATION_APP_SELLER = 48,//���P�ӽгq�����

            MSG_CREDIT_APP_APPLY = 49,//�}���ӽЮѫݮ֩�
            MSG_AMENDMENT_APP_APPLY = 50,//�窱�ӽЮѫݮ֩�
            MSG_CANCELLATION_APP_APPLY = 51,//���P�ӽЮѫݮ֩�
            MSG_REIM_APP_APPLY = 52,//�ٴڥӽЮѫݮ֩�
            MSG_DRAFT = 53,//���

            MSG_CREDIT_APP_RETRIAL = 54,//�}���ӽЫݵ��O
            MSG_AMENDMENT_RETRIAL = 55,//�窱�ӽЫݵ��O
            MSG_CANNEL_RETRIAL = 56,//���P�ӽЫݵ��O
            MSG_REIM_RETRIAL = 57,//���O�ٴڥӽЧ���(mail�D���O"�ٴڥӽФw�}��")
            MSG_LOAN_RETRIAL = 58,//���O�ٴڧ�U�ӽЧ���(mail�D���O"��U�ӽФw�}��")
            MSG_AMENDMENT_INFO = 59,//�H�Ϊ��ק�q���ѫݽT�{
            MSG_CANCELLATION_APP_SEELER_OK = 60,//���P�ӽн��w�T�{

            MSG_CANCELLATION_UNASKED = 63,//�D�ʾl�B���P
            MSG_CANCELLATION_NOTICE_BUYER = 64,//�D�ʾl�B���P�q���R��
            MSG_CANCELLATION_NOTICE_SELLER = 65,//�D�ʾl�B���P�q�����
            MSG_BEFEFICIARY_MODIFIED = 66,//���q�H��ƳB�z
            MSG_BENEFICIARY_DELETED = 67,//���q�H��ƧR��

            MSG_NEGO_APP_READY = 68,//��ץӽЫݨ��z
            MSG_NEGO_APP_TO_CHECK = 69,//��ץӽЫݼf��
            MSG_NEGO_APP_DENIED = 70,//��ץӽФw�ڵ�
            MSG_NEGO_APP_TO_REGISTER = 71,//��ץӽЫݵn��
            MSG_NEGO_APP_DAO_WITHDRAW = 72,//��ץӽХD�ްh�^
            MSG_NEGO_APP_TO_ALLOW = 73,//��ץӽЫݩ��
            MSG_NEGO_APP_CRC_WITHDRAW = 74,//��ץӽ�CRC�h�^
            MSG_NEGO_APP_LOAN_WITHDRAW = 75,//��ץӽаh�^
            MSG_NEGO_CREATED = 76,//��ץӽФw�}��
            MSG_NEGO_APP_CANCELLED = 77,//��ץӽЮѰh�^

            MSG_CREDIT_APP_APPLIED = 78,//�}���ӽЮѥ��~�ݤw�e�X
            MSG_AMENDMENT_APP_APPLIED = 79,//�窱�ӽЮѥ��~�ݤw�e�X
            MSG_LOAN_APP_APPLY = 80,//��U�ӽЮѫݮ֩�

            MSG_DRAFT_ACCEPTANCE_READY = 81,//�ӧI�ײ��ݨ��z
            MSG_DRAFT_ACCEPTANCE_CHECK = 82,//�ӧI�ײ��ݼf��
            MSG_DRAFT_ACCEPTED = 83,    //�ײ��w�ӧI
            MSG_DRAFT_ACCEPTANCE_DAO_WITHDRAW = 84, //  ��U�ӽХD�ްh�^
            MSG_APPLY_CREDIT_ALERT = 85,            //  �}���ӽЫH�ζS�z
            MSG_AMENDMENT_CREDIT_ALERT = 86,        //  �ת��ӽЫH�ζS�z
            MSG_LOAN_CREDIT_ALERT = 87,             //  ��U�ӽЫH�ζS�z
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
            �q�l���_CDS�}�� = 1,
            �D�q�l��� = 2,
            �q�l���_�q�l�ӽ� = 3,
            �q�l���_�x�춰�� = 4
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
            �������� = 1,    //
            ELC = 2,
            ���� = 3,
            �x�� = 4,
            �طs�R�� = 5,
            �_�� = 6,
        }



        public enum UploadStatusDefinition
        {
            ���ݶפJ = 0,
            ��ƿ��~ = 1,
            �פJ���\ = 2,
            �פJ���� = 3
        }

        public enum ProcessLogStatus
        {
            ��Ƥw�ǰe = 0
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
            �״ڥ����� = -1,
            �״ګݼf�� = 0,
            �״ڰh�^ = 1,
            �״ڸ�ƷǳƤ� = 2,
            �״ڸ�Ƥw�e�X = 3,
            �״ڤw���� = 4,
            �״ڥ��� = 5,
            �״ڪ��A�d�ߤ� = 6,
        }

        public enum FPGReportCategory
        {
            �ȥ� = 0,
            �q�l�ӽ� = 1,
            �q�l = 2
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
            �}���� = 0x01,
            �q���� = 0x02,
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
            IDELC0000 = 1, //����ݫݿ�ƶ�(�t�\�౶�|(����)�P�T���X)
            IDELC0101, //�}���ӽШ��z
            IDELC0102, //�}���ӽмf��
            IDELC0103, //�}���ӽеn��
            IDELC0104, //�}���ӽЩ��
            IDELC0105, //���O�}���ӽЧ���
            IDELC0106, //�H�Ϊ��}�߳q��
            IDELC0107, //�{�d�}���ӽШ��z
            IDELC0201, //�ת��ӽШ��z
            IDELC0202, //�ת��ӽмf��
            IDELC0203, //�ת��ӽеn��
            IDELC0204, //�ת��ӽЩ��
            IDELC0205, //���O�ת��ӽЧ���
            IDELC0206, //�窱�}�߳q��
            IDELC0207, //�{�d�ת��ӽШ��z
            IDELC0301, //��ץӽШ��z
            IDELC0302, //��ץӽмf��
            IDELC0303, //��ץӽеn��
            IDELC0304, //��ץӽЩ��
            IDELC0305, //���O��ץӽЧ���
            IDELC0401, //�ӧI�ײ�����I�ڨ��z
            IDELC0402, //�ӧI�ײ�����I�ک��
            IDELC0403, //���O�ӧI�ײ�����I�ڧ���
            IDELC0501, //�ꤺ�H�Ϊ������b���ٴڨ��z
            IDELC0502, //�ꤺ�H�Ϊ������b���ٴک��
            IDELC0503, //���O�ꤺ�H�Ϊ������b���ٴڥӽЧ���
            IDELC0601, //�Y���ꤺ�H�Ϊ���U�ӽШ��z
            IDELC0602, //�Y���ꤺ�H�Ϊ���U�ӽмf��
            IDELC0603, //�Y���ꤺ�H�Ϊ���U�ӽеn��
            IDELC0604, //�Y���ꤺ�H�Ϊ���U�ӽЩ��
            IDELC0605, //���O�Y���ꤺ�H�Ϊ���U�ӽЧ���
            IDELC0701, //�{�d��U�ٴڥӽ�
            IDELC0702, //�{�d��U�ٴک��
            IDELC0703, //���O�{�d��U�ٴڧ���
            IDELC0801, //�l�B���P�ӽШ��z
            IDELC0802, //�l�B���P�ӽЮ֩�
            IDELC0803, //�D�ʾl�B���P�n��
            IDELC0804, //�D�ʾl�B���P���
            IDELC0805, //���O�l�B���P�ӽЧ���
            IDELC0901, //�}���ӽЮѬd��
            IDELC0902, //�ת��ӽЮѬd��
            IDELC0903, //��ץӽЬd��
            IDELC0904, //�ꤺ�H�Ϊ������b���ٴڥӽЬd��
            IDELC0905, //�Y���ꤺ�H�Ϊ���U�ӽЬd��
            IDELC0906, //�ꤺ�H�Ϊ���U�ٴڬd��
            IDELC0907, //�l�B���P�d��
            IDELC0908, //�Ȥ��B�׬d��(�t��סB��U�B�׬d��)
            IDELC0909, //�K�ӽФH�T�{�����q�H�M��d��
            IDELC0910, //������B����d��
            IDELC0911, //���q�H�M�U�d��
            IDELC0912, //�}���H���w�ƶ��d��
            IDELC0913, //�q�T�d��
            IDELC0914, //�ϥΪ��v���d��
            IDELC0915, //����M�U�d��
            IDELC0916, //�D������q�H���i��Ƭd��
            IDELC0917, //�H�Ϊ��d��
            IDELC0918, //�ק�q���Ѭd��
            IDELC1001, //�}���H���w�ƶ����@
            IDELC1002, //�}���H���w�ƶ��f��
            IDELC1003, //���q�H�M�U���@
            IDELC1004, //���q�H�M�U�f��
            IDELC1005, //���\�W���v���@
            IDELC1006, //���\�W���v�f��
            IDELC1007, //�q�T�q���H���]�w
            IDELC1008, //����M�U���@
            IDELC1009, //�t�ΪA�ȻP�Ƶ{�ʱ�
            IDELC1101, //�x��岫��ר��z
            IDELC1102, //�x���״ڶץX�@�~
            IDELC1103, //�x���״ڶץX�f��
            IDELC1104, //���O�x���״ڶץX����
            IDELC1105, //�x����q�H�Ʒ~�����ɶפJ
            IDELC1106, //�x���׸ɵn��ƶפJ
            IDELC1107, //�x����v�b����@
            IDELC1108, //�x����v�b��f��
            IDELC1109, //�x���״ڶץX�b�Ȫ��A�d��
            IDELC1110, //�x��H�Ϊ��έp��
            IDELC1111, //�x��q�T�ǿ�Ƶ{�˵�
            IDELC1112, //�]�w�t��ñ������
            IDELC1113, //��״ڶץX����O�]�w
            IDELC1114, //��״ڶץX����O�f��
            IDELC1201, //�ǰe�H�Ϊ��}�߸��
            IDELC1202, //�ǰe��ש��q��
            WDELC0000, //���~�ݫݿ�ƶ�(�t�\�౶�|(����)�P�T���X)
            WDELC0101, //�}���ӽЮѷs�ӽ�
            WDELC0102, //�}���ӽЮѽƻs�w�ӽЫH�Ϊ�
            WDELC0103, //�}���ӽЮѬd��
            WDELC0201, //�ת��ӽЮ�
            WDELC0202, //�ת��ӽЮѬd��
            WDELC0203, //�ק�q���Ѭd��
            WDELC0301, //�l�B���P�ӽ�
            WDELC0302, //�T�{�l�B���P�ӽ�
            WDELC0303, //�l�B���P�d��
            WDELC0401, //��ץӽ�
            WDELC0402, //�f�֩���ץӽ�
            WDELC0403, //��ץӽнu�Wñ��
            WDELC0404, //�d�ߩ�ץӽ�
            WDELC0501, //�Y���ꤺ�H�Ϊ���U�ӽХӽ�
            WDELC0502, //�Y���ꤺ�H�Ϊ���U�ӽЩ��
            WDELC0503, //�d�ߧY���ꤺ�H�Ϊ���U
            WDELC0601, //��U�ٴڥӽ�
            WDELC0602, //��U�ٴک��
            WDELC0603, //�d�ߧ�U�ٴ�
            WDELC0701,//�d�߶}���B�׸��
            WDELC0702,//�d�ߩ���B�׸��
            WDELC0703, //�d�ߧ�U�B�׸��
            WDELC0704, //�T���X���@

        }

        public static Dictionary<TaskID, String> TaskIDNaming = new Dictionary<TaskID, string>
            {
                { Naming.TaskID.IDELC0000, "����ݫݿ�ƶ�" },
                { Naming.TaskID.IDELC0101, "�}���ӽШ��z" },
                { Naming.TaskID.IDELC0102, "�}���ӽмf��" },
                { Naming.TaskID.IDELC0103, "�}���ӽеn��" },
                { Naming.TaskID.IDELC0104, "�}���ӽЩ��" },
                { Naming.TaskID.IDELC0105, "���O�}���ӽЧ���" },
                { Naming.TaskID.IDELC0106, "�H�Ϊ��}�߳q��" },
                { Naming.TaskID.IDELC0107, "�{�d�}���ӽШ��z" },
                { Naming.TaskID.IDELC0201, "�ת��ӽШ��z" },
                { Naming.TaskID.IDELC0202, "�ת��ӽмf��" },
                { Naming.TaskID.IDELC0203, "�ת��ӽеn��" },
                { Naming.TaskID.IDELC0204, "�ת��ӽЩ��" },
                { Naming.TaskID.IDELC0205, "���O�ת��ӽЧ���" },
                { Naming.TaskID.IDELC0206, "�ת��}�߳q��" },
                { Naming.TaskID.IDELC0207, "�{�d�ת��ӽШ��z" },
                { Naming.TaskID.IDELC0301, "��ץӽШ��z" },
                { Naming.TaskID.IDELC0302, "��ץӽмf��" },
                { Naming.TaskID.IDELC0303, "��ץӽеn��" },
                { Naming.TaskID.IDELC0304, "��ץӽЩ��" },
                { Naming.TaskID.IDELC0305, "���O��ץӽЧ���" },
                { Naming.TaskID.IDELC0401, "�ӧI�ײ�����I�ڨ��z" },
                { Naming.TaskID.IDELC0402, "�ӧI�ײ�����I�ک��" },
                { Naming.TaskID.IDELC0403, "���O�ӧI�ײ�����I�ڧ���" },
                { Naming.TaskID.IDELC0501, "�ꤺ�H�Ϊ������b���ٴڨ��z" },
                { Naming.TaskID.IDELC0502, "�ꤺ�H�Ϊ������b���ٴک��" },
                { Naming.TaskID.IDELC0503, "���O�ꤺ�H�Ϊ������b���ٴڥӽЧ���" },
                { Naming.TaskID.IDELC0601, "�Y���ꤺ�H�Ϊ���U�ӽШ��z" },
                { Naming.TaskID.IDELC0602, "�Y���ꤺ�H�Ϊ���U�ӽмf��" },
                { Naming.TaskID.IDELC0603, "�Y���ꤺ�H�Ϊ���U�ӽеn��" },
                { Naming.TaskID.IDELC0604, "�Y���ꤺ�H�Ϊ���U�ӽЩ��" },
                { Naming.TaskID.IDELC0605, "���O�Y���ꤺ�H�Ϊ���U�ӽЧ���" },
                { Naming.TaskID.IDELC0701, "�{�d��(�I)�ڡB��U�ӽ�" },
                { Naming.TaskID.IDELC0702, "�{�d��(�I)�ڡB��U���" },
                { Naming.TaskID.IDELC0703, "���O�{�d�ٴڡB��U����" },
                { Naming.TaskID.IDELC0801, "�l�B���P�ӽШ��z" },
                { Naming.TaskID.IDELC0802, "�l�B���P�ӽЮ֩�" },
                { Naming.TaskID.IDELC0803, "�D�ʾl�B���P�n��" },
                { Naming.TaskID.IDELC0804, "�D�ʾl�B���P���" },
                { Naming.TaskID.IDELC0805, "���O�l�B���P�ӽЧ���" },
                { Naming.TaskID.IDELC0901, "�}���ӽЮѬd��" },
                { Naming.TaskID.IDELC0902, "�ת��ӽЮѬd��" },
                { Naming.TaskID.IDELC0903, "��ץӽЬd��" },
                { Naming.TaskID.IDELC0904, "�ꤺ�H�Ϊ������b���ٴڥӽЬd��" },
                { Naming.TaskID.IDELC0905, "�Y���ꤺ�H�Ϊ���U�ӽЬd��" },
                { Naming.TaskID.IDELC0906, "�ꤺ�H�Ϊ���U�ٴڬd��" },
                { Naming.TaskID.IDELC0907, "�l�B���P�d��" },
                { Naming.TaskID.IDELC0908, "�Ȥ��B�׬d��(�t��סB��U�B�׬d��)" },
                { Naming.TaskID.IDELC0909, "�K�ӽФH�T�{�����q�H�M��d��" },
                { Naming.TaskID.IDELC0910, "������B����d��" },
                { Naming.TaskID.IDELC0911, "���q�H�M�U�d��" },
                { Naming.TaskID.IDELC0912, "�}���H���w�ƶ��d��" },
                { Naming.TaskID.IDELC0913, "�q�T�d��" },
                { Naming.TaskID.IDELC0914, "�ϥΪ��v���d��" },
                { Naming.TaskID.IDELC0915, "����M�U�d��" },
                { Naming.TaskID.IDELC0916, "�D������q�H���i��Ƭd��" },
                { Naming.TaskID.IDELC0917, "�H�Ϊ��d��" },
                { Naming.TaskID.IDELC0918, "�ק�q���Ѭd��" },
                { Naming.TaskID.IDELC1001, "�}���H���w�ƶ����@" },
                { Naming.TaskID.IDELC1002, "�}���H���w�ƶ��f��" },
                { Naming.TaskID.IDELC1003, "���q�H�M�U���@" },
                { Naming.TaskID.IDELC1004, "���q�H�M�U�f��" },
                { Naming.TaskID.IDELC1005, "���\�W���v���@" },
                { Naming.TaskID.IDELC1006, "���\�W���v�f��" },
                { Naming.TaskID.IDELC1007, "�q�T�q���H���]�w" },
                { Naming.TaskID.IDELC1008, "����M�U���@" },
                { Naming.TaskID.IDELC1009, "�t�ΪA�ȻP�Ƶ{�ʱ�" },
                { Naming.TaskID.IDELC1101, "�x��岫��ר��z" },
                { Naming.TaskID.IDELC1102, "�x���״ڶץX�@�~" },
                { Naming.TaskID.IDELC1103, "�x���״ڶץX�f��" },
                { Naming.TaskID.IDELC1104, "�x���״ڶץX�������O" },
                { Naming.TaskID.IDELC1105, "�x����q�H�Ʒ~�����ɶפJ" },
                { Naming.TaskID.IDELC1106, "�x���׸ɵn��ƶפJ" },
                { Naming.TaskID.IDELC1107, "�x����v�b����@" },
                { Naming.TaskID.IDELC1108, "�x����v�b��f��" },
                { Naming.TaskID.IDELC1109, "�x���״ڶץX�b�Ȫ��A�d��" },
                { Naming.TaskID.IDELC1110, "�x��H�Ϊ��έp��" },
                { Naming.TaskID.IDELC1111, "�x��q�T�ǿ�Ƶ{�˵�" },
                { Naming.TaskID.IDELC1112, "�]�w�t��ñ������" },
                { Naming.TaskID.IDELC1113, "��״ڶץX����O�]�w" },
                { Naming.TaskID.IDELC1114, "��״ڶץX����O�f��" },
                { Naming.TaskID.IDELC1201, "�ǰe�H�Ϊ��}�߸��" },
                { Naming.TaskID.IDELC1202, "�ǰe��ש��q��" },
                { Naming.TaskID.WDELC0000, "���~�ݫݿ�ƶ�(�t�\�౶�|(����)�P�T���X)" },
                { Naming.TaskID.WDELC0101, "�s�ӽ�" },
                { Naming.TaskID.WDELC0102, "�ƻs�w�ӽ�" },
                { Naming.TaskID.WDELC0103, "�}���ӽЮѬd��" },
                { Naming.TaskID.WDELC0201, "�ת��ӽЮ�" },
                { Naming.TaskID.WDELC0202, "�ת��ӽЮѬd��" },
                { Naming.TaskID.WDELC0203, "�ק�q���Ѭd��" },
                { Naming.TaskID.WDELC0301, "�l�B���P�ӽ�" },
                { Naming.TaskID.WDELC0302, "�T�{�l�B���P�ӽ�" },
                { Naming.TaskID.WDELC0303, "�l�B���P�d��" },
                { Naming.TaskID.WDELC0401, "��ץӽ�" },
                { Naming.TaskID.WDELC0402, "�f�֩���ץӽ�" },
                { Naming.TaskID.WDELC0403, "��ץӽнu�Wñ��" },
                { Naming.TaskID.WDELC0404, "�d�ߩ�ץӽ�" },
                { Naming.TaskID.WDELC0501, "�ꤺ�H�Ϊ���U�@�ٴڥӽ�" },
                { Naming.TaskID.WDELC0502, "�ꤺ�H�Ϊ���U�@�ٴڼf�֩��" },
                { Naming.TaskID.WDELC0503, "�d�߰ꤺ�H�Ϊ���U�@�ٴ�" },
                { Naming.TaskID.WDELC0601, "��U�ٴڥӽ�" },
                { Naming.TaskID.WDELC0602, "��U�ٴک��" },
                { Naming.TaskID.WDELC0603, "�d�ߧ�U�ٴ�" },
                { Naming.TaskID.WDELC0701,"�d�߶}���B��" },
                { Naming.TaskID.WDELC0702,"�d�ߩ���B��" },
                { Naming.TaskID.WDELC0703, "�d�ߧ�U�B��" },
                { Naming.TaskID.WDELC0704, "�T���X���@" },
        };

        public static Dictionary<TaskID, String> TaskIDNamingEN = new Dictionary<TaskID, string>
            {
                { Naming.TaskID.IDELC0000, "����ݫݿ�ƶ�" },
                { Naming.TaskID.IDELC0101, "�}���ӽШ��z" },
                { Naming.TaskID.IDELC0102, "�}���ӽмf��" },
                { Naming.TaskID.IDELC0103, "�}���ӽеn��" },
                { Naming.TaskID.IDELC0104, "�}���ӽЩ��" },
                { Naming.TaskID.IDELC0105, "���O�}���ӽЧ���" },
                { Naming.TaskID.IDELC0106, "�H�Ϊ��}�߳q��" },
                { Naming.TaskID.IDELC0107, "�{�d�}���ӽШ��z" },
                { Naming.TaskID.IDELC0201, "�ת��ӽШ��z" },
                { Naming.TaskID.IDELC0202, "�ת��ӽмf��" },
                { Naming.TaskID.IDELC0203, "�ת��ӽеn��" },
                { Naming.TaskID.IDELC0204, "�ת��ӽЩ��" },
                { Naming.TaskID.IDELC0205, "���O�ת��ӽЧ���" },
                { Naming.TaskID.IDELC0206, "�窱�}�߳q��" },
                { Naming.TaskID.IDELC0207, "�{�d�ת��ӽШ��z" },
                { Naming.TaskID.IDELC0301, "��ץӽШ��z" },
                { Naming.TaskID.IDELC0302, "��ץӽмf��" },
                { Naming.TaskID.IDELC0303, "��ץӽеn��" },
                { Naming.TaskID.IDELC0304, "��ץӽЩ��" },
                { Naming.TaskID.IDELC0305, "���O��ץӽЧ���" },
                { Naming.TaskID.IDELC0401, "�ӧI�ײ�����I�ڨ��z" },
                { Naming.TaskID.IDELC0402, "�ӧI�ײ�����I�ک��" },
                { Naming.TaskID.IDELC0403, "���O�ӧI�ײ�����I�ڧ���" },
                { Naming.TaskID.IDELC0501, "�ꤺ�H�Ϊ������b���ٴڨ��z" },
                { Naming.TaskID.IDELC0502, "�ꤺ�H�Ϊ������b���ٴک��" },
                { Naming.TaskID.IDELC0503, "���O�ꤺ�H�Ϊ������b���ٴڥӽЧ���" },
                { Naming.TaskID.IDELC0601, "�Y���ꤺ�H�Ϊ���U�ӽШ��z" },
                { Naming.TaskID.IDELC0602, "�Y���ꤺ�H�Ϊ���U�ӽмf��" },
                { Naming.TaskID.IDELC0603, "�Y���ꤺ�H�Ϊ���U�ӽеn��" },
                { Naming.TaskID.IDELC0604, "�Y���ꤺ�H�Ϊ���U�ӽЩ��" },
                { Naming.TaskID.IDELC0605, "���O�Y���ꤺ�H�Ϊ���U�ӽЧ���" },
                { Naming.TaskID.IDELC0701, "�{�d��U�ٴڥӽ�" },
                { Naming.TaskID.IDELC0702, "�{�d��U�ٴک��" },
                { Naming.TaskID.IDELC0703, "���O�{�d��U�ٴڧ���" },
                { Naming.TaskID.IDELC0801, "�l�B���P�ӽШ��z" },
                { Naming.TaskID.IDELC0802, "�l�B���P�ӽЮ֩�" },
                { Naming.TaskID.IDELC0803, "�D�ʾl�B���P�n��" },
                { Naming.TaskID.IDELC0804, "�D�ʾl�B���P���" },
                { Naming.TaskID.IDELC0805, "���O�l�B���P�ӽЧ���" },
                { Naming.TaskID.IDELC0901, "�}���ӽЮѬd��" },
                { Naming.TaskID.IDELC0902, "�ת��ӽЮѬd��" },
                { Naming.TaskID.IDELC0903, "��ץӽЬd��" },
                { Naming.TaskID.IDELC0904, "�ꤺ�H�Ϊ������b���ٴڥӽЬd��" },
                { Naming.TaskID.IDELC0905, "�Y���ꤺ�H�Ϊ���U�ӽЬd��" },
                { Naming.TaskID.IDELC0906, "�ꤺ�H�Ϊ���U�ٴڬd��" },
                { Naming.TaskID.IDELC0907, "�l�B���P�d��" },
                { Naming.TaskID.IDELC0908, "�Ȥ��B�׬d��(�t��סB��U�B�׬d��)" },
                { Naming.TaskID.IDELC0909, "�K�ӽФH�T�{�����q�H�M��d��" },
                { Naming.TaskID.IDELC0910, "������B����d��" },
                { Naming.TaskID.IDELC0911, "���q�H�M�U�d��" },
                { Naming.TaskID.IDELC0912, "�}���H���w�ƶ��d��" },
                { Naming.TaskID.IDELC0913, "�q�T�d��" },
                { Naming.TaskID.IDELC0914, "�ϥΪ��v���d��" },
                { Naming.TaskID.IDELC0915, "����M�U�d��" },
                { Naming.TaskID.IDELC0916, "�D������q�H���i��Ƭd��" },
                { Naming.TaskID.IDELC0917, "�H�Ϊ��d��" },
                { Naming.TaskID.IDELC0918, "�ק�q���Ѭd��" },
                { Naming.TaskID.IDELC1001, "�}���H���w�ƶ����@" },
                { Naming.TaskID.IDELC1002, "�}���H���w�ƶ��f��" },
                { Naming.TaskID.IDELC1003, "���q�H�M�U���@" },
                { Naming.TaskID.IDELC1004, "���q�H�M�U�f��" },
                { Naming.TaskID.IDELC1005, "���\�W���v���@" },
                { Naming.TaskID.IDELC1006, "���\�W���v�f��" },
                { Naming.TaskID.IDELC1007, "�q�T�q���H���]�w" },
                { Naming.TaskID.IDELC1008, "����M�U���@" },
                { Naming.TaskID.IDELC1009, "�t�ΪA�ȻP�Ƶ{�ʱ�" },
                { Naming.TaskID.IDELC1101, "�x��岫��ר��z" },
                { Naming.TaskID.IDELC1102, "�x���״ڶץX�@�~" },
                { Naming.TaskID.IDELC1103, "�x���״ڶץX�f��" },
                { Naming.TaskID.IDELC1104, "���O�x���״ڶץX����" },
                { Naming.TaskID.IDELC1105, "�x����q�H�Ʒ~�����ɶפJ" },
                { Naming.TaskID.IDELC1106, "�x���׸ɵn��ƶפJ" },
                { Naming.TaskID.IDELC1107, "�x����v�b����@" },
                { Naming.TaskID.IDELC1108, "�x����v�b��f��" },
                { Naming.TaskID.IDELC1109, "�x���״ڶץX�b�Ȫ��A�d��" },
                { Naming.TaskID.IDELC1110, "�x��H�Ϊ��έp��" },
                { Naming.TaskID.IDELC1111, "�x��q�T�ǿ�Ƶ{�˵�" },
                { Naming.TaskID.IDELC1112, "�]�w�t��ñ������" },
                { Naming.TaskID.IDELC1201, "�ǰe�H�Ϊ��}�߸��" },
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
