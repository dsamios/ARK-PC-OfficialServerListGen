using System;
using System.Collections.Generic;
using System.Text;

namespace ArkServers
{
    class Server
    {
        public string name;
        public string ip;
        public int port;

        public Server(string name, string ip, int port)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
        }
    }
}
