using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Functions.TCP
{
    struct NetworkConnectionParameter
    {
        public NetworkStream stream;        //For TCP-Connection only
        public Byte[] bytes;
        public int portIn;                  //For UDP-Connection only
        public IPAddress ipAddressIn;       //For UDP-Connection only
    }

}
