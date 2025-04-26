using System.Net.Sockets;
using System.Net;
using System.Runtime;
using Newtonsoft.Json;
using System.IO;

public class Siriazeble()
{
    public string Words { get; set; }
    public string Vidpovid { get; set; }
}

public class CharServer
{

   
    const int port = 4040;

    TcpListener server;
    private List<Siriazeble> _qaList;

    public CharServer()
    {
        server = new TcpListener(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port));
        var jsonText = File.ReadAllText("questions.json");
        _qaList = JsonConvert.DeserializeObject<List<Siriazeble>>(jsonText)!;
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
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}  -- {vin} --  from {client.Client.LocalEndPoint}");

            bool answeradd = false;
            foreach(var item in _qaList)
            {
                if (item.Words.Contains(vin))
                {
                    sw.WriteLine($"You --> {vin}");

                    sw.WriteLine($"Bot --> {item.Vidpovid}");
                    sw.Flush();
                    answeradd = true;
                    break;
                }

            }
            if(!answeradd)
            {
                sw.WriteLine($"You --> {vin}");
                sw.WriteLine($"Bot --> I don1t know");
                sw.Flush();
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