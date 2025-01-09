using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoDatabase
{
    public class SGTIN : IEquatable<SGTIN>
    {
        public string StdCode { get; set; }
        public string Serial { get; set; }
        public SGTIN(string stdCode, string serial) 
        {
            StdCode = stdCode;
            Serial = serial;
        }
        public static bool operator ==(SGTIN left, SGTIN right)
        {
            return left.StdCode == right.StdCode && left.Serial == right.Serial;
        }

        public static bool operator !=(SGTIN left, SGTIN right)
        {
            return left.StdCode != right.StdCode || left.Serial != right.Serial;
        }
        public bool Equals(SGTIN other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return StdCode == other.StdCode && Serial == other.Serial;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != GetType())
                return false;

            return Equals((SGTIN)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (StdCode != null ? StdCode.GetHashCode() : 0);
                hash = hash * 23 + (Serial != null ? Serial.GetHashCode() : 0);
                return hash;
            }
        }
    }
    public class SGTINComparer : IEqualityComparer<SGTIN>
    {
        public bool Equals(SGTIN x, SGTIN y)
        {
            if (x.StdCode == y.StdCode && x.Serial == y.Serial)
                return true;

            return false;
        }

        public int GetHashCode(SGTIN obj)
        {
            return (obj.StdCode + obj.Serial).GetHashCode();
        }
    }
    public class SerialPoolCompairer : IEqualityComparer<SerialPool>
    {
        public bool Equals(SerialPool x, SerialPool y)
        {
            return x.ProdStdCode == y.ProdStdCode && x.SerialNum == y.SerialNum;
        }

        public int GetHashCode(SerialPool obj)
        {
            return (obj.ProdStdCode + obj.SerialNum).GetHashCode();
        }
    }

}
