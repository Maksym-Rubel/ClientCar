using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Configuration;

namespace ClientCar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string serverAddress = "127.0.0.1";
        const int port = 4040;
        IPEndPoint serverPoint;
        TcpClient client;
        NetworkStream ns = null;
        StreamWriter sw = null;
        StreamReader sr = null;
        //ObservableCollection<MessageInfo> messages;
        public MainWindow()
        {
            InitializeComponent();
            client = new TcpClient();
            
            serverPoint = new IPEndPoint(IPAddress.Parse(serverAddress), port);
           
        }

        private void SearchBtn(object sender, RoutedEventArgs e)
        {
            
            string message = srcTextBox.Text;
            srcTextBox.Text = "";
            sw.WriteLine(message);
            sw.Flush();
            

        }
        
        private async void Listener()
        {
            
            while (true)
            {
                try
                {
                    
                    string? message = await sr.ReadLineAsync();
                    lb1.Items.Clear();
                    lb1.Items.Add(message);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    break;
                }

            }
        }

        private void ConnectBtn(object sender, RoutedEventArgs e)
        {
            try
            {
                client.Connect(serverPoint);
                ns = client.GetStream();

                sr = new StreamReader(ns);
                sw = new StreamWriter(ns);

                Listener();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}