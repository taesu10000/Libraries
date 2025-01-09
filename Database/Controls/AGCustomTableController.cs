using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using System.Data.Entity.Migrations;
using DominoDatabase.DSM;
using System.Data.SqlTypes;

namespace DominoDatabase.Controls
{
    public class SPRBJoinTableController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<SpRb> SelectLocal(string orderNo, string seqNo, int take, string stdCode = "", string serial = "", bool RaiseException = false)
        {
            List<SpRb> retList = new List<SpRb>();
            try
            {
                using (DominoDBLocal db = new DominoDBLocal())
                {
                    var serialPoolList = db.Dmn_SerialPool
                                          .Where(a => a.OrderNo == orderNo && a.SeqNo == seqNo)
                                          .OrderByDescending(a => a.UseDate)
                                          .Take(take);

                    var list = from a in serialPoolList
                               join b in db.Dmn_ReadBarcode
                               on new { a.OrderNo, a.SeqNo, Stdcode = a.ProdStdCode, Serial = a.SerialNum }
                               equals new { b.OrderNo, b.SeqNo, Stdcode = b.ParentProdStdCode, Serial = b.ParentSerialNum }
                               into bb
                               from b in bb.DefaultIfEmpty()
                               select new { a, b };

                    if (!string.IsNullOrEmpty(stdCode))
                        list = list.Where(q => q.a.ProdStdCode == stdCode);
                    if (!string.IsNullOrEmpty(serial))
                        list = list.Where(q => q.a.SerialNum == serial);

                    var sprbGroup = list.ToList().GroupBy(q => q.a);
                    foreach (var sp in sprbGroup)
                    {
                        var serialPool = new SpRb(sp.Key);
                        foreach (var rb in sp)
                        {
                            if (rb.b != null)
                                serialPool.ReadBarcodes.Add(new ReadBarcode(rb.b));
                        }
                        retList.Add(serialPool);
                    }
                }
                return retList;
            }
            catch (Exception ex)
            {
                log.InfoFormat("SPRBJoinTableController SelectLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("SPRBJoinTableController SelectLocal Exception", ex);
                return new List<SpRb>();
            }
        }
    }
}
