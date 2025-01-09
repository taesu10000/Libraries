using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DominoDatabase.Controls;
using DominoFunctions.Enums;

namespace DominoDatabase
{
    public class HelpCodePool
    {
        public string OrderNo { get; set; }
        public string SeqNo { get; set; }
        public string HelpCode { get; set; }
        public string ProdStdCode { get; set; }
        public string SerialNum { get; set; }
        public string UseYN { get; set; } = "N";
        public long? idx_Insert { get; set; }
        public string FilePath { get; set; } = "";
        public string InsertUser { get; set; }
        public string Status { get; set; } = "RE";
        public EnYN EnUseYN
        {
            get
            {
                switch (UseYN)
                {
                    case "Y":
                        return EnYN.Y;
                    case "N":
                    default:
                        return EnYN.N;
                }
            }
            set
            {
                switch (value)
                {
                    case EnYN.Y:
                        UseYN = value.ToString();
                        break;
                    default:
                        UseYN = EnYN.N.ToString();
                        break;
                }
            }
        }
        public EnProductStatus EnStatus
        {
            get
            {
                switch (Status)
                {
                    case "PA":
                        return EnProductStatus.Pass;
                    case "RE":
                    default:
                        return EnProductStatus.Reject;
                }
            }
            set
            {
                switch (value)
                {
                    case EnProductStatus.Pass:
                        Status = "PA";
                        break;
                    case EnProductStatus.Reject:
                    default:
                        Status = "RE";
                        break;
                }
            }
        }
        public bool Printed { get; set; } = false;
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public HelpCodePoolCollection HelpCodePoolDetail { get; set; }
        public int DetailCount { get { return HelpCodePoolDetail.Count; } }
        public HelpCodePool() 
        {
            HelpCodePoolDetail = new HelpCodePoolCollection();
        }
        public HelpCodePool(object obj) : this()
        {
            this.UnionClass(obj);
        }
        public bool Insert(bool raiseException = false)
        {
            return HelpCodepoolController.InsertLocal(this, raiseException);
        }
        public bool Update(string userID, bool raiseException = false)
        {
            UpdateDate = DateTime.Now;
            UpdateUser = userID;
            return HelpCodepoolController.UpdateLocal(this, raiseException);
        }
        public bool UpdateMasterQuery(string userID, bool raiseException = false)
        {
            UpdateDate = DateTime.Now;
            UpdateUser = userID;
            return HelpCodepoolController.UpdateMasterLocal(this, raiseException);
        }
        public bool Delete(string userID, bool raiseException = false)
        {
            return HelpCodepoolController.DeleteLocal(OrderNo, SeqNo, HelpCode, userID, raiseException);
        }
        public bool isExist(bool raiseException = false)
        {
            return HelpCodepoolController.IsExist(this, raiseException);
        }
        public List<string> GetFilePath()
        {
            return FilePath.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries).OrderBy(q => q).ToList();
        }
        public override string ToString()
        {
            return string.Format("[HelpCode : {0}] [ProdStdCode : {1}] [SerialNum : {2}]", HelpCode, ProdStdCode, SerialNum);
        }
    }
    public class HelpCodePoolDetail
    {
        public string ChildProdStdCode { get; set; }
        public string ChildSerialNum { get; set; }
        public string FullChildBarcode_Read { get; set; }
        public string History { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public HelpCodePoolDetail()
        {

        }
        public HelpCodePoolDetail(object obj) : this()
        {
            this.UnionClass(obj);
        }
        public override string ToString()
        {
            return string.Format("[ChildProdStdCode : {0}] [ChildSerialNum : {1}]", ChildProdStdCode, ChildSerialNum);
        }
    }
    public class HelpCodePoolCollection : ICollection<HelpCodePoolDetail>
    {
        List<HelpCodePoolDetail> _items;
        public int Count => ((ICollection<HelpCodePoolDetail>)_items).Count;
        public bool IsReadOnly => ((ICollection<HelpCodePoolDetail>)_items).IsReadOnly;
        public HelpCodePoolCollection()
        {
            _items = new List<HelpCodePoolDetail>();
        }
        public void Add(HelpCodePoolDetail item)
        {
            var isExist = _items.Any(q => q.ChildProdStdCode == item.ChildProdStdCode && q.ChildSerialNum == item.ChildSerialNum);
            if (isExist == false)
                ((ICollection<HelpCodePoolDetail>)_items).Add(item);
        }
        public bool Remove(HelpCodePoolDetail item)
        {
            return ((ICollection<HelpCodePoolDetail>)_items).Remove(item);
        }
        public void Clear()
        {
            ((ICollection<HelpCodePoolDetail>)_items).Clear();
        }
        public int IndexOf(HelpCodePoolDetail item)
        {
            return _items.IndexOf(item);
        }
        public bool Contains(HelpCodePoolDetail item)
        {
            return ((ICollection<HelpCodePoolDetail>)_items).Contains(item, new HelpCodePoolDetailComparer());
        }
        public void CopyTo(HelpCodePoolDetail[] array, int arrayIndex)
        {
            ((ICollection<HelpCodePoolDetail>)_items).CopyTo(array, arrayIndex);
        }
        public IEnumerator<HelpCodePoolDetail> GetEnumerator()
        {
            return ((IEnumerable<HelpCodePoolDetail>)_items).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_items).GetEnumerator();
        }
        public void ForEach(Action<HelpCodePoolDetail> action)
        {
            if (action == null)
                throw new ArgumentException();

            for (int i = 0; i < _items.Count; i++)
            {
                action(_items[i]);
            }
        }
        public override string ToString()
        {
            return string.Format("[Count : {0}]", Count);
        }
    }
    class HelpCodePoolDetailComparer : IEqualityComparer<HelpCodePoolDetail>
    {
        public bool Equals(HelpCodePoolDetail x, HelpCodePoolDetail y)
        {
            return GetHashCode(x).Equals(GetHashCode(y));
        }

        public int GetHashCode(HelpCodePoolDetail obj)
        {
            return new { obj.ChildProdStdCode, obj.ChildSerialNum }.GetHashCode();
        }
    }
}
