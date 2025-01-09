using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoDatabase
{
    public class DasanJobOrder
    {
        public string 포장지시번호 { get; set; }
        public string 제품코드 { get; set; }
        public string 제품명 { get; set; }
        public string 개정번호 { get; set; }
        public string GTIN코드 { get; set; }
        public string 포장단위 { get; set; }
        public string 단위 { get; set; }
        public string 제조번호 { get; set; }
        public string 제조제품제조번호 { get; set; }
        public DateTime 제조일자 { get; set; }
        public DateTime 유효기한 { get; set; }
        public DateTime 등록일자 { get; set; }
        public string 지시수량 { get; set; }
        public string 승인단계 { get; set; }
        public string 상태 { get; set; }
        public string 불출지시상태 { get; set; }
        public string 원자재투입상태 { get; set; }
        public string 제조지시수량 { get; set; }
        public string 발행사유 { get; set; }
        public string 안전성배치확인 { get; set; }
        public string 안전성샘플량 { get; set; }
    }
}
