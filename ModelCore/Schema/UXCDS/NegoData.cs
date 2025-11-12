using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelCore.Schema.UXCDS
{
    public class NegoData
    {
        public NegoLC LC { get; set; }
        public NegoDraft Draft { get; set; }
        public BusinessInvoice[] Invoice { get; set; }
    }

    public class NegoLC
    {
        public System.String LCNo { get; set; }
        public System.DateTime ImportDate { get; set; }
        public System.String IssuingBank { get; set; }
        public System.DateTime DateOfIssue { get; set; }
        public System.DateTime DateOfExpiry { get; set; }
        public System.Decimal Amount { get; set; }
        public System.String ApplicantReceiptNo { get; set; }
        public System.String AdvisingBank { get; set; }
        public System.DateTime ShipmentNoAfter { get; set; }
        public System.String LCType { get; set; }
        public System.Decimal? AvailableAmount { get; set; }
        public System.Int32? DueDays { get; set; }
        public System.String LCFile { get; set; }
        public System.String NotifyingBank { get; set; }
        public System.Int32 BeneficiaryID { get; set; }
        public List<NegoDraft> NegoDraft { get; set; }
        public Organization Applicant { get; set; }
        public Organization BeneficiaryData { get; set; }
    }

    public class NegoDraft
    {
        public System.String DraftNo { get; set; }
        public System.Int32 Status { get; set; }
        public System.DateTime ImportDate { get; set; }
        public System.DateTime? ShipmentDate { get; set; }
        public System.DateTime? NegoDate { get; set; }
        public System.Decimal Amount { get; set; }
        public System.String AmountInChinese { get; set; }
        public System.String ItemName { get; set; }
        public System.Decimal? ItemQuantity { get; set; }
        public System.Decimal? ItemSubtotal { get; set; }
        public System.String ApplicantAddr { get; set; }
        public System.Int16 DownloadFlag { get; set; }
        public System.DateTime InitialDate { get; set; }
        public List<NegoInvoice> NegoInvoice { get; set; }
        public OrganizationBackSeal BackSeal { get; set; }
        public OrganizationFrontSeal FrontSeal { get; set; }
        public System.String KeyID { get; set; }
        public System.String DepositAccount { get; set; }
    }

    public class Organization
    {
        public System.String ContactName { get; set; }
        public System.String Fax { get; set; }
        public System.String LogoURL { get; set; }
        public System.String CompanyName { get; set; }
        public System.String ReceiptNo { get; set; }
        public System.String Phone { get; set; }
        public System.String ContactFax { get; set; }
        public System.String ContactPhone { get; set; }
        public System.String ContactMobilePhone { get; set; }
        public System.String RegAddr { get; set; }
        public System.String UndertakerName { get; set; }
        public System.String Addr { get; set; }
        public System.String EnglishName { get; set; }
        public System.String EnglishAddr { get; set; }
        public System.String EnglishRegAddr { get; set; }
        public System.String ContactEmail { get; set; }
        public System.String UndertakerPhone { get; set; }
        public System.String UndertakerFax { get; set; }
        public System.String UndertakerMobilePhone { get; set; }
        public System.String InvoiceSignature { get; set; }
        public System.String UndertakerID { get; set; }
        public System.String ContactTitle { get; set; }
    }

    public class OrganizationBackSeal
    {
        public System.String SealPath { get; set; }
        public System.String Content { get; set; }
    }

    public class OrganizationFrontSeal
    {
        public System.String SealPath { get; set; }
        public System.String Content { get; set; }
    }

    public class NegoInvoice
    {
        public System.String InvoiceNo { get; set; }
        public System.String LadingNo { get; set; }
        public System.Decimal? InvoiceAmount { get; set; }
        public System.DateTime? InvoiceDate { get; set; }
        public System.DateTime? ImportDate { get; set; }
        public System.String ContractNo { get; set; }
        public System.String TaxNo { get; set; }
        public System.String ContractQuarter { get; set; }
        public System.Boolean? FileReady { get; set; }
    }

    public class BusinessInvoice
    {
        public System.String InvoiceNo { get; set; }
        public System.Int32 Year { get; set; }
        public System.Int32 PeriodNo { get; set; }
        public System.String DataContent { get; set; }
        public Organization Organization { get; set; }
    }
}
