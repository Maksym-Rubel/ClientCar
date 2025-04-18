using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Runtime;

public class VinInfo
{
    public string country {  get; set; }
    public string region { get; set; }
    public string year { get; set; }

    // для тесту 5YJSA1DP4CFS00842 або JH4KA7561PC008269

}

public class CharServer
{

    const int port = 4040;

    TcpListener server;

    public CharServer()
    {
        server = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
    }
    public void Start()
    {
       

        server.Start();
        Console.WriteLine("Waiting for connection");
        TcpClient client = server.AcceptTcpClient();
        Console.WriteLine("Connected!!!");
        NetworkStream ns = client.GetStream();
        StreamWriter sw = new StreamWriter(ns);
        StreamReader sr = new StreamReader(ns);
        while (true)
        {
            string vin = sr.ReadLine()!;
            string url = $"https://api.api-ninjas.com/v1/vinlookup?vin={vin}";
            VinInfo? vinInfo = null;
            using (WebClient wclient = new WebClient())
            {
                wclient.Headers.Add("X-Api-Key", "16XbQpeMofTroqIM2xvmIw==8IzpyLQakNDzKW7F");
                try
                {
                    string json = wclient.DownloadString(url);

                    vinInfo = JsonConvert.DeserializeObject<VinInfo>(json);


                }
                catch (Exception ex)
                { Console.WriteLine(ex.Message); }

                Console.WriteLine($"{DateTime.Now.ToLongTimeString()}  -- {vin} --  from {client.Client.LocalEndPoint}");

                if (vinInfo != null)
                {
                    sw.WriteLine($"Year : {vinInfo.year} | Country : {vinInfo.country} | Region : {vinInfo.region}");
                    sw.Flush();
                }
                else
                {
                    sw.WriteLine("Not found");
                }


            }




        }

    }
}

internal class Program
{
    
    private static void Main(string[] args)
    {
        CharServer server_ = new CharServer();
        server_.Start();
    }
}
