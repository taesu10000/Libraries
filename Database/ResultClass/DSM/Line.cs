using System;
using DominoFunctions.ExtensionMethod;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace DominoDatabase
{
    [Serializable]
    public class Line
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string PlantCode { get; set; }
        public string LineID { get; set; }
        public string LineName { get; set; }
        public string LineInfo { get; set; }
        public string UseYN { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public string InsertUser { get; set; }
        public DateTime? InsertDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public LineDetailCollection LineDetail { get; set; }
        public string LotNo { get; set; }
        public string ProdName { get; set; }
        public int? Cnt_JobPlan { get; set; }
        public Line()
        {
            LineDetail = new LineDetailCollection();
        }
        public Line(object list)
            :this()
        {
            this.UnionClass(list);
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.PlantCode, PlantCode);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LineID, LineID);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LineName, LineName);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.LineInfo, LineInfo);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UseYN, UseYN);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved1, Reserved1);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved2, Reserved2);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved3, Reserved3);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved4, Reserved4);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Reserved5, Reserved5);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertUser, InsertUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.InsertDate, InsertDate);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateUser, UpdateUser);
            sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.UpdateDate, UpdateDate);
            foreach (LineDetail dt in this.LineDetail)
            {
                sb.AppendFormat("[{0}:{1}]", LanguagePack.UserInterfaces.Detail, dt.ToString());
            }
            return sb.ToString();
            //return DominoFunctions.ClassFunctions.ClassToString<Line>(this);
        }
        public bool InsertServer()
        {
            try
            {
                Controls.LineController.InsertServer(new DSM.Dmn_Line_M(this), true);
                foreach(LineDetail dtl in this.LineDetail)
                {
                    Controls.LineController.InsertServer(new DSM.Dmn_Line_D(dtl) { PlantCode = this.PlantCode, LineID = this.LineID, InsertUser = this.InsertUser, InsertDate = this.InsertDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Line InsertServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public bool UpdateServer()
        {
            try
            {
                Controls.LineController.UpdateServer(new DSM.Dmn_Line_M(this), true);
                Controls.LineController.DeleteServerDetailAll(this.PlantCode, this.LineID, "System");
                foreach (LineDetail dtl in this.LineDetail)
                {
                    Controls.LineController.InsertServer(new DSM.Dmn_Line_D(dtl) { PlantCode = this.PlantCode, LineID = this.LineID, InsertDate = this.InsertDate, InsertUser = this.InsertUser, UpdateUser = this.UpdateUser, UpdateDate = this.UpdateDate });
                }
                return true;
            }
            catch (Exception ex)
            {
                log.InfoFormat("Line UpdateServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                return false;
            }
        }
        public Line Clone()
        {
            return (Line)this.MemberwiseClone();
        }
    }
    [Serializable]
    public class LineDetailCollection : ICollection<LineDetail>
    {

        public LineDetailCollection()
        {
            _item = new List<LineDetail>();
        }

        public LineDetail this[int index]
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
        public LineDetail this[string jobDetailType]
        {
            get
            {
                return _item.Where(x => x.JobDetailType.Equals(jobDetailType)).FirstOrDefault();
            }
            set
            {
                var tmp = _item.Where(x => x.JobDetailType.Equals(jobDetailType)).FirstOrDefault();
                tmp = value;
            }
        }
        public void Add(LineDetail detail)
        {
            _item.Add(detail);
        }
        public void AddRange(IEnumerable<LineDetail> list)
        {
            _item.AddRange(list);
        }
        public void RemoveAt(int index)
        {
            _item.RemoveAt(index);
        }
        public bool Remove(LineDetail detail)
        {
            return _item.Remove(detail);
        }
        public void RemoveRange(int index, int count)
        {
            _item.RemoveRange(index, count);
        }

        protected readonly List<LineDetail> _item;
        public IEnumerator<LineDetail> GetEnumerator()
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
        public bool Contains(LineDetail detail)
        {
            return _item.Contains(detail);
        }
        public void CopyTo(LineDetail[] array, int arrayIndex)
        {
            _item.CopyTo(array, arrayIndex);
        }
        public bool ContainsMachineID(string MachineID)
        {
            return _item.Any(q => q.MachineID.Equals(MachineID));
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
        public LineDetailCollection Clone()
        {
            LineDetailCollection retVal = new LineDetailCollection();
            foreach (LineDetail dt in this)
            {
                retVal.Add(dt.Clone());
            }
            return retVal;
        }
        public override string ToString()
        {
            string retVal = string.Empty;
            foreach(LineDetail dt in this)
            {
                retVal += string.Format("[{0}:{1}]", dt.JobDetailType.ToString(), dt.MachineID);
            }
            return retVal;
        }
    }
    [Serializable]
    public class LineDetail
    {
        public string MachineID { get; set; }
        public string MachineName { get; set; }
        public string JobDetailType { get; set; }
        public int? PackingCount { get; set; }
        public string Reserved1 { get; set; }
        public string Reserved2 { get; set; }
        public string Reserved3 { get; set; }
        public string Reserved4 { get; set; }
        public string Reserved5 { get; set; }
        public int? SeqNo { get; set; }
        public string LastOrderNo { get; set; }
        public string LastSeqNo { get; set; }
        public int? Cnt_Good { get; set; }
        public int? Cnt_Error { get; set; }
        public int? Cnt_Sample { get; set; }
        public int? Cnt_Destroy { get; set; }
        public int? Cnt_Child { get; set; }
        public int? Cnt_Work { get; set; }
        public int? Cnt_Parent { get; set; }
        public string MachineStatus { get; set; }
        public string LotNo { get; set; }
        public string ProdName { get; set; }

        public LineDetail Clone()
        {
            return (LineDetail)this.MemberwiseClone();
        }
        public LineDetail()
        { }
        public LineDetail(object list)
            :this()
        {
            this.UnionClass(list);
        }
    }
}
