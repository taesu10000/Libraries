using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

namespace DominoDatabase.Controls
{
    public class CommonCodeController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Common> SelectLocal(string cdCode, string cdCode_Detail, bool? useYN, string code_Value1 = null, string code_Value2 = null, string code_Value3 = null,
             bool RaiseException = false)
        {
            List<Common> retList = new List<Common>();
            try
            {
                using (var db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_CommonCode_M
                               join b in db.Dmn_CommonCode_D
                               on a.CDCode equals b.CDCode
                               select new
                               {
                                   Key = b.CDCode_Dtl,
                                   Value = b.CDCode_Name,
                                   b.UseYN,
                                   b.Code_Value1,
                                   b.Code_Value2,
                                   b.Code_Value3
                               };
                    if (useYN != null)
                    {
                        list = list.Where(q => q.UseYN.Equals((bool)useYN ? "Y" : "N"));
                    }

                    if (code_Value1 != null)
                    {
                        list = list.Where(q => q.Code_Value1.Equals((string)code_Value1));
                    }

                    if (code_Value2 != null)
                    {
                        list = list.Where(q => q.Code_Value2.Equals((string)code_Value2));
                    }

                    if (code_Value3 != null)
                    {
                        list = list.Where(q => q.Code_Value3.Equals((string)code_Value3));
                    }
                    foreach (var item in list)
                    {
                        retList.Add(new Common(item)
                        {
                            CDCode = item.Key == null ? null : item.Key,
                            CDName = item.Value == null ? null : item.Value,
                            UseYN = item.UseYN == null ? null : item.UseYN,
                        });
                    }
                    return retList;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController SelectCommonCodeLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                {
                    throw new Exception("CommonCodeController SelectCommonCodeLocal Exception", ex);
                }
            }
            return retList;
        }
        public static string SelectCommonCodeLocal(string cdCode, string cdCode_Detail, bool? useYN, string code_Value1 = null, string code_Value2 = null, string code_Value3 = null,
            bool RaiseException = false)
        {
            string returnValue = "";
            try
            {
                using (var db = new DominoDBLocal())
                {
                    var list = from a in db.Dmn_CommonCode_M
                               join b in db.Dmn_CommonCode_D
                               on a.CDCode equals b.CDCode
                               where a.CDCode.Equals(cdCode) && a.UseYN.Equals("Y") && b.CDCode_Dtl.Equals(cdCode_Detail)
                               select new
                               {
                                   Key = b.CDCode_Dtl,
                                   Value = b.CDCode_Name,
                                   b.UseYN,
                                   b.Code_Value1,
                                   b.Code_Value2,
                                   b.Code_Value3
                               };
                    if (useYN != null)
                    {
                        list = list.Where(q => q.UseYN.Equals((bool)useYN ? "Y" : "N"));
                    }

                    if (code_Value1 != null)
                    {
                        list = list.Where(q => q.Code_Value1.Equals((string)code_Value1));
                    }

                    if (code_Value2 != null)
                    {
                        list = list.Where(q => q.Code_Value2.Equals((string)code_Value2));
                    }

                    if (code_Value3 != null)
                    {
                        list = list.Where(q => q.Code_Value3.Equals((string)code_Value3));
                    }

                    returnValue = list.FirstOrDefault().Value;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController SelectCommonCodeLocal Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                {
                    throw new Exception("CommonCodeController SelectCommonCodeLocal Exception", ex);
                }
            }
            return returnValue;
        }
        public static string SelectCommonCodeServer(string cdCode, string cdCode_Detail, bool? useYN, string code_Value1 = null, string code_Value2 = null, string code_Value3 = null,
            bool RaiseException = false)
        {
            string returnValue = "";
            try
            {
                using (var db = new DominoDBServer())
                {
                    var list = from a in db.Dmn_CommonCode_M
                               join b in db.Dmn_CommonCode_D
                               on a.CDCode equals b.CDCode
                               where a.CDCode.Equals(cdCode) && a.UseYN.Equals("Y") && b.CDCode_Dtl.Equals(cdCode_Detail)
                               select new
                               {
                                   Key = b.CDCode_Dtl,
                                   Value = b.CDCode_Name,
                                   b.UseYN,
                                   b.Code_Value1,
                                   b.Code_Value2,
                                   b.Code_Value3
                               };
                    if (useYN != null)
                    {
                        list = list.Where(q => q.UseYN.Equals((bool)useYN ? "Y" : "N"));
                    }

                    if (code_Value1 != null)
                    {
                        list = list.Where(q => q.Code_Value1.Equals((string)code_Value1));
                    }

                    if (code_Value2 != null)
                    {
                        list = list.Where(q => q.Code_Value2.Equals((string)code_Value2));
                    }

                    if (code_Value3 != null)
                    {
                        list = list.Where(q => q.Code_Value3.Equals((string)code_Value3));
                    }

                    returnValue = list.FirstOrDefault().Value;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController SelectCommonCodeServer Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                {
                    throw new Exception("CommonCodeController SelectCommonCodeServer Exception", ex);
                }
            }
            return returnValue;
        }
        public static bool InsertLocal(Dmn_CommonCode_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("CommonCodeController InsertLocalMaster by {0}", item.InsertUser);
                    db.Dmn_CommonCode_M.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController InsertLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CommonCodeController InsertLocalMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertLocal(Dmn_CommonCode_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBLocal())
                {
                    log.InfoFormat("CommonCodeController InsertLocalMaster by {0}", item.InsertUser);
                    db.Dmn_CommonCode_D.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController InsertLocalMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CommonCodeController InsertLocalMaster Exception", ex);
                return false;
            }
        }

        public static List<Common> SelectServer(bool RaiseException = false)
        {
            List<Common> retList = new List<Common>();

            using (var db = new DominoDBServer())
            {
                var list = from a in db.Dmn_CommonCode_M
                           where a.InsertUser.Equals("System")
                           select a;
                foreach (var item in list)
                    retList.Add(new Common());
            }
            return retList;
        }

        public static bool InsertServer(Dmn_CommonCode_M item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("CommonCodeController InsertServerMaster by {0}", item.InsertUser);
                    db.Dmn_CommonCode_M.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController InsertServerMaster Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CommonCodeController InsertServerMaster Exception", ex);
                return false;
            }
        }
        public static bool InsertServer(Dmn_CommonCode_D item, bool RaiseException = false)
        {
            try
            {
                using (var db = new DominoDBServer())
                {
                    log.InfoFormat("CommonCodeController InsertServerDetail by {0}", item.InsertUser);
                    db.Dmn_CommonCode_D.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.InfoFormat("CommonCodeController InsertServerDetail Exception : {0} \r\n {1}", ex.InnerException ?? ex, ex.StackTrace);
                if (RaiseException)
                    throw new Exception("CommonCodeController InsertServerDetail Exception", ex);
                return false;
            }
        }

    }
}
