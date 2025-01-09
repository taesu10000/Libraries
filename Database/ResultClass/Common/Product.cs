using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using DominoFunctions.ExtensionMethod;
using log4net;
using DominoFunctions.Enums;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace DominoDatabase
{
    [Serializable]
    public class Product
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string ProdCode { get; set; }
        public string LineID { get; set; }
        public string ProdStdCode { get; set; }
        public string ProdName { get; set; }
        public string ProdName2 { get; set; }
		public int? MedicineType { get; set; }
        public int AGLevel { get; set; }
        public string Remark { get; set; }
        public int Exp_Day { get; set; }
        public string UseYN { get; set; }
        public string CompanyPrefix { get; set; }
		public string NDCType { get; set; }
		public string NDCValue { get; set; }
		public string InterfaceDetail { get; set; }
		public int Delay_Print { get; set; }
        public int Delay_Print2 { get; set; }
        public int Delay_Shot1 { get; set; }
        public int Delay_Shot2 { get; set; }
        public int Delay_NG { get; set; }
        public int Delay_NG2 { get; set; }
        public int Delay_Shot3 { get; set; }
        public int Delay_Shot4 { get; set; }
		/// <summary>
		/// <list type="bullet">
		/// <item>Usage 1: Used for China XML Report; PackageSpec, cascade, physicDetailType or such. (SCD)</item> 
		/// <item>Usage 2: --</item> 
		/// </list>
		/// </summary>
		public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public ProductDetailCollection ProductDetail { get; set; }

        public string FirstDetailStandardCode
        {
            get
            {
                if (ProductDetail?.Count > 0 && !string.IsNullOrEmpty(ProductDetail[0].ProdStdCode))
                {
                    return ProductDetail[0].ProdStdCode;
                }
                return ProdStdCode;
            }
        }

		[JsonIgnore]
		public EnInterfaceDetail EnInterfaceDetail
		{
			get
			{
				if (Enum.TryParse(InterfaceDetail, out EnInterfaceDetail ifd))
				{
					return ifd;
				}
				else
				{
					return EnInterfaceDetail.None;
				}
			}
			set
			{
				InterfaceDetail = $"{value}";
			}
		}

		public Product()
        {
            ProductDetail = new ProductDetailCollection();
        }
        public Product(object list)
            : this()
        {
            this.UnionClass(list);
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Plant, PlantCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdCode, ProdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LineID, LineID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.StandardCode, ProdStdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdCode, ProdCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdName, ProdName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdName2, ProdName2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.AGLevel, AGLevel);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Remark, Remark);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ExpiryDayOffset, Exp_Day);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            foreach (ProductDetail dt in this.ProductDetail)
            {
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Detail, dt.ToString());
            }
            return sb.ToString();
        }
        public string GetValue(EnVarKey key)
        {
            switch (key)
            {
                case EnVarKey.StandardCode:
                case EnVarKey.oStandardCode:
                    return ProdStdCode;
                case EnVarKey.Capacity:
                    return ProductDetail[0].Capacity;
                case EnVarKey.ProductName1:
                    return ProdName;
                case EnVarKey.ProductName2:
                    return ProdName2;
                case EnVarKey.AiStandardCode:
                    if (ProdStdCode.Length == 14)
                        return string.Format("(01){0}", ProdStdCode);
                    else if (ProdStdCode.Length == 16)
                        return string.Format("(90){0}", ProdStdCode);
                    else
                        return string.Empty;
                case EnVarKey.Extension:
                    return ProductDetail[0].GS1ExtensionCode;
            }
            return string.Empty;
        }
        public string GetValue(EnVarKey key, int index)
        {
            switch (key)
            {
                case EnVarKey.StandardCode:
                case EnVarKey.oStandardCode:
                    return ProdStdCode;
                case EnVarKey.Capacity:
                    return ProductDetail[index].Capacity;
                case EnVarKey.ProductName1:
                    return ProdName;
                case EnVarKey.ProductName2:
                    return ProdName2;
                case EnVarKey.AiStandardCode:
                    if (ProdStdCode.Length == 14)
                        return string.Format("(01){0}", ProdStdCode);
                    else if (ProdStdCode.Length == 16)
                        return string.Format("(90){0}", ProdStdCode);
                    else
                        return string.Empty;
                case EnVarKey.Extension:
                    return ProductDetail[index].GS1ExtensionCode;
            }
            return string.Empty;
        }
        public string GetValue(EnVarKey key, string jobDetail)
        {
            switch (key)
            {
                case EnVarKey.StandardCode:
                case EnVarKey.oStandardCode:
                    return ProdStdCode;
                case EnVarKey.Capacity:
                    return ProductDetail[jobDetail].Capacity;
                case EnVarKey.ProductName1:
                    return ProdName;
                case EnVarKey.ProductName2:
                    return ProdName2;
                case EnVarKey.AiStandardCode:
                    if (ProdStdCode.Length == 14)
                        return string.Format("(01){0}", ProdStdCode);
                    else if (ProdStdCode.Length == 16)
                        return string.Format("(90){0}", ProdStdCode);
                    else
                        return string.Empty;
                case EnVarKey.Extension:
                    return ProductDetail[jobDetail].GS1ExtensionCode;
            }
            return string.Empty;
        }
        public string GetValue(string key, string jobDetail)
        {
            switch (key)
            {
                case "StandardCode":
                case "oStandardCode":
                    return ProductDetail[jobDetail].ProdStdCode;
                case "Capacity":
                    return ProductDetail[jobDetail].Capacity;
                case "ProductName1":
                    return ProdName;
                case "ProductName2":
                    return ProdName2;
                case "AiStandardCode":
                    if (string.IsNullOrEmpty(ProductDetail[jobDetail].ProdStdCode))
                        return string.Empty;
                    if (ProductDetail[jobDetail].ProdStdCode.Length == 14)
                        return string.Format("(01){0}", ProductDetail[jobDetail].ProdStdCode);
                    else if (ProductDetail[jobDetail].ProdStdCode.Length == 16)
                        return string.Format("(90){0}", ProductDetail[jobDetail].ProdStdCode);
                    else
                        return string.Empty;
            }
            return string.Empty;
        }
		public bool InsertServer()
        {
            try
            {
                Controls.ProductController.InsertServer(new DSM.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.InsertServer(new DSM.Dmn_Product_D(dtl) { PlantCode = this.PlantCode, ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.ProductController.UpdateServer(new DSM.Dmn_Product_M(this), true);
                Controls.ProductController.DeleteServerDetailAll(this.PlantCode, this.ProdCode, this.UpdateUser);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    //Controls.ProductController.DeleteServerDetail(this.PlantCode, this.ProdCode, dtl.JobDetailType, this.UpdateUser);
                    Controls.ProductController.InsertServer(new DSM.Dmn_Product_D(dtl) { PlantCode = this.PlantCode, ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateDetailServer()
        {
            try
            {
                Controls.ProductController.UpdateServer(new DSM.Dmn_Product_M(this), true);
                Controls.ProductController.DeleteServerDetailAll(this.PlantCode, this.ProdCode, this.UpdateUser);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    //Controls.ProductController.DeleteServerDetail(this.PlantCode, this.ProdCode, dtl.JobDetailType, this.UpdateUser);
                    Controls.ProductController.InsertServer(new DSM.Dmn_Product_D(dtl) { PlantCode = this.PlantCode, ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool InsertLocalPM()
        {
            try
            {
                Controls.ProductController.InsertLocal(new Local.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.InsertLocal(new Local.Dmn_Product_PM(dtl) { ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product InsertLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool AddOrUpdateLocalPM()
        {
            try
            {
                Controls.ProductController.AddOrUpdateLocal(new Local.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.AddOrUpdateLocal(new Local.Dmn_Product_PM(dtl) { ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product InsertLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool AddOrUpdateLocalAG()
        {
            try
            {
                Controls.ProductController.AddOrUpdateLocal(new Local.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.AddOrUpdateLocal(new Local.Dmn_Product_AG(dtl) { ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product InsertLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool RequireUpdate()
        {
            try
            {
                DateTime? tmp = Controls.ProductController.GetUpdateDate(this.ProdCode);
                if (tmp == null)
                    return true;
                else if (tmp < this.UpdateDate)
                    return true;
                return false;

            }
            catch(Exception ex)
            {
                log.Debug(ex.ToString());
                return true;
            }
        }
        public bool InsertLocalAG()
        {
            try
            {
                Controls.ProductController.InsertLocal(new Local.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.InsertLocal(new Local.Dmn_Product_AG(dtl) { ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product InsertLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocalPM()
        {
            try
            {
                Controls.ProductController.UpdateLocal(new Local.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.DeleteLocalDetail(this.ProdCode, dtl.JobDetailType, this.UpdateUser);
                    Controls.ProductController.InsertLocal(new Local.Dmn_Product_PM(dtl) { ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product UpdateLocalPM Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateLocalAG()
        {
            try
            {
                Controls.ProductController.UpdateLocal(new Local.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.DeleteLocalDetail(this.ProdCode, dtl.JobDetailType, this.UpdateUser);
                    Controls.ProductController.InsertLocal(new Local.Dmn_Product_AG(dtl) { ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser });
                }

                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Product UpdateLocalAG Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool MergeIntoServer()
        {
            try
            {
                Controls.ProductController.MergeIntoServer(new DSM.Dmn_Product_M(this), true);
                foreach (ProductDetail dtl in this.ProductDetail)
                {
                    Controls.ProductController.MergeIntoServer(new DSM.Dmn_Product_D(dtl) { PlantCode = this.PlantCode ,ProdCode = this.ProdCode, InsertUser = this.InsertUser, InsertDate = this.InsertDate, UpdateDate = this.UpdateDate, UpdateUser = this.UpdateUser }, true);
                }
                if(this.ProductDetail.Count > 0)
                    Controls.ProductController.DeleteDetailNonExistingJobDetail(this.PlantCode, this.ProdCode, this.ProductDetail.Select(q => q.JobDetailType).ToList());
                return true; 
            }
            catch(Exception ex)
            {
                log.InfoFormat("Product MergeIntoServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }


        public Product Clone()
        {
            var tmp = new Product(this);
            foreach (var item in this.ProductDetail)
                tmp.ProductDetail.Add(new ProductDetail(item));
            return tmp;
        }
    }
    [Serializable]
    public class ProductDetailCollection : ICollection<ProductDetail>
    {

        public ProductDetailCollection()
        {
            _item = new List<ProductDetail>();
        }

        public ProductDetail this[int index]
        {
            get
            {
                return _item[index];
            }
            set
            {
                _item[index] = value;
            }
        }
        public ProductDetail this[string jobDetail]
        {
            get
            {
                return _item.Where(x => x.JobDetailType == jobDetail).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.JobDetailType == jobDetail).FirstOrDefault();
                tmp = value;
            }
        }
        public ProductDetail this[PackingType jobDetail]
        {
            get
            {
                return _item.Where(x => x.JobDetailType ==jobDetail.ToString()).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.JobDetailType == jobDetail.ToString()).FirstOrDefault();
                tmp = value;
            }
        }
        public void Add(ProductDetail detail)
        {
            _item.Add(detail);
        }
        public void AddRange(IEnumerable<ProductDetail> list)
        {
            _item.AddRange(list);
        }
        public void RemoveAt(int index)
        {
            _item.RemoveAt(index);
        }
        public bool Remove(ProductDetail detail)
        {
            return _item.Remove(detail);
        }
        public void RemoveRange(int index, int count)
        {
            _item.RemoveRange(index, count);
        }

        protected readonly List<ProductDetail> _item;
        public IEnumerator<ProductDetail> GetEnumerator()
        {
            return _item.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        public void Clear()
        {
            _item.Clear();
        }
        public bool Contains(ProductDetail detail)
        {
            return _item.Contains(detail);
        }
        public void CopyTo(ProductDetail[] array, int arrayIndex)
        {
            _item.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get
            {
                return _item.Count;
            }
        }
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        public List<PackingType> DetailTypeList
        {
            get 
            {
                List<PackingType> list = new List<PackingType>();
                foreach (var detail in _item)
                {
                    list.Add((PackingType)Enum.Parse(typeof(PackingType), detail.JobDetailType));
                }
                return list;
            }
        }
        public bool ContainsJobDetail(string jobDetail)
        {
            return _item.Any(q => q.JobDetailType == jobDetail);
        }
        public bool ContainsJobDetail(PackingType packingType)
        {
            return _item.Any(q => q.JobDetailType == packingType.ToString());
        }
        public int IndexOf(string packingType)
        {
            return _item.FindIndex(q => q.JobDetailType == packingType.ToString());
        }
        public int IndexOfStdCode(string stdCode)
        {
            return _item.FindIndex(q => q.StandardCode == stdCode);
        }
        public ProductDetail GetDetailByStdCode(string stdCode)
        {
            return _item.FirstOrDefault(q => q.StandardCode == stdCode);
        }
        public ProductDetailCollection Clone()
        {
            ProductDetailCollection retVal = new ProductDetailCollection();
            foreach(ProductDetail dt in this)
            {
                retVal.Add(dt.Clone());
            }
            return retVal;
        }
    }
    [Serializable]
    public class ProductDetail
    {
        public string JobDetailType { get; set; }
        public string ResourceType { get; set; }
        public string BarcodeType { get; set; }
        public string BarcodeDataFormat { get; set; }
        public string SnType { get; set; }
        public string SnExpressionID { get; set; }
        public string Capacity { get; set; }
        public bool? UsePrinterGroup1 { get; set; }
		public bool? UsePrinterGroup2 { get; set; }
		public string DesignID { get; set; }
        public string DesignID2 { get; set; }
        public string LIC { get; set; }
        public string PCN { get; set; }
        public string ProdStdCodeChild { get; set; }
        public int? ContentCount { get; set; }
        public int? PackingCount { get; set; }
        public string Prefix_SSCC { get; set; }
        public string PrinterName { get; set; }
        public string PrinterName2 { get; set; }
        public int LabelPrintCount { get; set; }
        public string GS1ExtensionCode { get; set; }
        public string Condition { get; set; }
        public string MachineUseYN { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string SnExpressionStr { get; set; }
        public string Price { get; set; }
        public string PharmaCode { get; set; }
        public string ProdStdCode { get; set; }
        public int? ExtractSerialStart { get; set; }
        public int? ExtractSerialEnd { get; set; }
        public decimal? MinimumWeight { get; set; }
        public decimal? MaximumWeight { get; set; }
        public string CustomBarcodeFormat { get; set; }
        [JsonIgnore]
        public EnResourceType EnResourceType
        {
            get
            {
                switch (ResourceType)
                {
                    case "L":
                        return EnResourceType.Local;
                    case "D":
                        return EnResourceType.DSM;
                    case "E":
                        return EnResourceType.ERP;
                    case "F":
                        return EnResourceType.File;
                    case "M":
                        return EnResourceType.Movilita;
                    case "K":
                        return EnResourceType.KEIDAS;
                    case "I":
                        return EnResourceType.Interface;
                    default:
                        throw new NotImplementedException();
                }
            }
            set
            {
                switch (value)
                {
                    case EnResourceType.Local:
                        ResourceType = "L";
                        break;
                    case EnResourceType.DSM:
                        ResourceType = "D";
                        break;
                    case EnResourceType.ERP:
                        ResourceType = "E";
                        break;
                    case EnResourceType.File:
                        ResourceType = "F";
                        break;
                    case EnResourceType.Movilita:
                        ResourceType = "M";
                        break;
                    case EnResourceType.KEIDAS:
                        ResourceType = "K";
                        break;
                    case EnResourceType.Interface:
                        ResourceType = "I";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        [JsonIgnore]
        public EnSerialType EnSerialType
        {
            get
            {
                if (string.IsNullOrEmpty(SnType))
                    return EnSerialType.None;
                switch (SnType)
                {
                    case "N":
                        return EnSerialType.None;
                    case "C":
                        return EnSerialType.CreateSerialNumber;
                    case "R":
                        return EnSerialType.ReceivedSerialNumber;
                    //case "S":
                    //    return EnSerialType.ScanSerialNumber;
                    case "D":
                        return EnSerialType.DSM;
                    case "M":
                        return EnSerialType.Movilitas;
                    case "I":
                        return EnSerialType.Interface;
                    case "H":
                        return EnSerialType.HelpCode;
                    //case "L":
                    //    return EnSerialType.RecievedSerialNumber_Lot_X;
                }
                throw new NotImplementedException();
            }
            set
            {
                switch (value)
                {
                    case EnSerialType.None:
                        SnType = "N";
                        break;
                    case EnSerialType.CreateSerialNumber:
                        SnType = "C";
                        break;
                    case EnSerialType.ReceivedSerialNumber:
                        SnType = "R";
                        break;
                    //case EnSerialType.RecievedSerialNumber_Lot_X:
                    //    SnType = "L";
                    //    break;
                    //case EnSerialType.ScanSerialNumber:
                    //    SnType = "S";
                    //    break;
                    case EnSerialType.DSM:
                        SnType = "D";
                        break;
                    case EnSerialType.Movilitas:
                        SnType = "M";
                        break;
                    case EnSerialType.Interface:
                        SnType = "I";
                        break;
                    case EnSerialType.HelpCode:
                        SnType = "H";
                        break;
                    default:
                        SnType = "N";
                        break;
                }
            }
        }
        [JsonIgnore]
        public EnBarcodeFormatType EnBarcodeFormatType
        {
            get
            {
                if (Enum.TryParse(BarcodeDataFormat, out EnBarcodeFormatType type))
                    return type;
                else
                    return EnBarcodeFormatType.CustomBarcode;
            }
            set
            {
                BarcodeDataFormat = value.ToString();
            }
        }
        [JsonIgnore]
        public EnBarcodeType EnBarcodeType
        {
            get
            {
                return (EnBarcodeType)Enum.Parse(typeof(EnBarcodeType), BarcodeType);
            }
            set
            {
                BarcodeType = value.ToString();
            }
        }
        [JsonIgnore]
        public bool? MachineUse
        {
            get
            {
                if (string.IsNullOrEmpty(MachineUseYN))
                    return null;
                return MachineUseYN == "Y";
            }
        }
        [JsonIgnore]
        public PackingType EnPackingType
        {
            get
            {
                switch (JobDetailType)
                {
                    case "EA":
                        return PackingType.EA;
                    case "LBL":
                        return PackingType.LBL;
                    case "BX1":
                        return PackingType.BX1;
                    case "BX2":
                        return PackingType.BX2;
                    case "BX3":
                        return PackingType.BX3;
                    case "BX4":
                        return PackingType.BX4;
                    case "BX5":
                        return PackingType.BX5;
                    case "BX6":
                        return PackingType.BX6;
                    case "BX7":
                        return PackingType.BX7;
                    case "BX8":
                        return PackingType.BX8;
                }
                throw new NotImplementedException();
            }
            set
            {
                switch (value)
                {
                    case PackingType.EA:
                        JobDetailType = "EA";
                        break;
                    case PackingType.LBL:
                        JobDetailType = "LBL";
                        break;
                    case PackingType.BX1:
                        JobDetailType = "BX1";
                        break;
                    case PackingType.BX2:
                        JobDetailType = "BX2";
                        break;
                    case PackingType.BX3:
                        JobDetailType = "BX3";
                        break;
                    case PackingType.BX4:
                        JobDetailType = "BX4";
                        break;
                    case PackingType.BX5:
                        JobDetailType = "BX5";
                        break;
                    case PackingType.BX6:
                        JobDetailType = "BX6";
                        break;
                    case PackingType.BX7:
                        JobDetailType = "BX7";
                        break;
                    case PackingType.BX8:
                        JobDetailType = "BX8";
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

        }
		[JsonIgnore]
        public string StandardCode
        {
            get
            {
                if (!string.IsNullOrEmpty(ProdStdCode))
                    return ProdStdCode;
                else if (string.IsNullOrEmpty(GS1ExtensionCode) || string.IsNullOrEmpty(ProdStdCodeChild))
                    return string.Empty;
                else
                {
                    string prefix = GS1ExtensionCode + ProdStdCodeChild.Substring(1, ProdStdCodeChild.Length - 2);
                    int WorkCheckDigit = DominoGSLib.GS1.GetCheckDigit(prefix);
                    return prefix + WorkCheckDigit.ToString();
                }

            }
        }
        [JsonIgnore]
        public bool IsGS1Format
        {
            get { return EnBarcodeFormatType != EnBarcodeFormatType.None; }
        }
        [JsonIgnore]
        public PackingType PackingType
        {
            get { return (PackingType)Enum.Parse(typeof(PackingType), JobDetailType); }
        }
        public ProductDetail Clone()
        {
            return (ProductDetail)this.MemberwiseClone();
        }
        public ProductDetail()
        {

        }
        public ProductDetail(object list)
            : this()
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.JobDetailType, JobDetailType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ResourceType, ResourceType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.BarcodeType, BarcodeType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.BarcodeDataFormat, BarcodeDataFormat);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SerialType, SnType);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.DesignID, DesignID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.SnExpressID, SnExpressionID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Capacity, Capacity);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LIC, LIC);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PCN, PCN);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Condition, Condition);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PackingCount, PackingCount);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Prefix_SSCC, Prefix_SSCC);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterName, PrinterName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LabelPrintCount, LabelPrintCount);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.GS1ExtensionCode, GS1ExtensionCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ChildStandardCode, ProdStdCodeChild);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ContentCount, ContentCount);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterName, PrinterName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Printer, DesignID2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PrinterName, PrinterName2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Price, Price);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.ProdStdCode, ProdStdCode);
            return sb.ToString();
        }
    }
}
