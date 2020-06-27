using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using System.Threading;

namespace ArkServers
{
    class Program
    {
        private static List<Server> svList = new List<Server>();
        private static int counter = 0;
        static void Main(string[] args)
        {
            int[] port = new int[] { 27010, 27013, 27015, 27017, 27019, 27021, 27023, 27025, 27028, 27031 };            

            Console.WriteLine("Press enter to get ark pc official servers");
            Console.Read();

            WebRequest webRequest = WebRequest.Create(@"http://arkdedicated.com/officialservers.ini");
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            StreamReader sr = new StreamReader(webResponse.GetResponseStream());
            string[] resultIP = sr.ReadToEnd().Split('\n');
            webResponse.Close();

            Thread searchThread;
            foreach (string ip in resultIP)
            {
                string onlyIP = ip;
                if (ip.Contains("/")) onlyIP = onlyIP.Split('/')[0];
                if (ip.Contains(" ")) onlyIP = onlyIP.Split(' ')[0];
                if (ip.Contains("\r")) onlyIP = onlyIP.Split('\r')[0];
                foreach (int p in port)
                {
                    searchThread = new Thread(() => SearchServerInfo(onlyIP, p));
                    searchThread.Start();
                }
            }
            Thread.Sleep(1000);
            Console.WriteLine("Creating servers.json file...");
            
            File.WriteAllText("servers.json", JsonConvert.SerializeObject(svList));

            sr.Close();
            Console.WriteLine("\n-- That's All Folks --");

            Console.Read();
            
        }

        private static void SearchServerInfo(string onlyIP, int p)
        {
            
            GameServer sv = new GameServer();
            try
            {
                sv = new GameServer(new IPEndPoint(IPAddress.Parse(onlyIP), p));
                
                string name = sv.name;

                if (!(name.Contains("PVE")
                    || name.Contains("Tek")
                    || name.Contains("Raid")
                    || name.Contains("Small")
                    || name.Contains("CrossArk")
                    || name.Contains("PrimPlus")
                    || name.Contains("Hardcore")
                    || name.Contains("Classic")
                    || name.Contains("pocalypse")
                    || name.Contains("LEGACY")
                    || name.Contains("Asia")
                    || name.Contains("Conquest")
                    || name.Contains("Capsule")
                    ))
                {
                    counter++;
                    Console.WriteLine(counter + ")" + name + "(" + onlyIP + ":" + p + ")");
                    svList.Add(new Server(name, onlyIP, p));
                }
                    
                    
            }
            catch { }
        }
    }
}
