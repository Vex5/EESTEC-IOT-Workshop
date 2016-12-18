using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MsgToIoTHub
{
    public sealed partial class MainPage : Page
    {

        static string connectionString = "HostName=SecuritySysteHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=8xNbj+v0tzkzwLMlvLJiucEsbuy1pPUDdfMYtGzpRk0=";
    
        static ServiceClient serviceClient;

        private static int i = 2;

        private DispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        /*
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (i % 2 == 0)
                await SendCloudToDeviceMessageAsync("1");
            else
                await SendCloudToDeviceMessageAsync("0");
            i++;

        }*/
        private async static Task SendCloudToDeviceMessageAsync(string podaci)
        {
            var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(podaci));
            //promjenit deviceid
            await serviceClient.SendAsync("Vedo", commandMessage);
        }

        private async void Timer_Tick(object sender, object e)
        {
            if (i % 2 == 0)
                await SendCloudToDeviceMessageAsync("1");
            else
                await SendCloudToDeviceMessageAsync("0");
            i++;
            /*
                Bitmap slika = new Bitmap("photo.jpg");
                byte[] buffer = slika.ToByteArray(ImageFormat.Bmp);
                string s = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                await SendCloudToDeviceMessageAsync(s);
            */
            /*
             Primanje na mobitelu:
             Byte[] bajtovi = receivedMessage.GetBytes();
             Stream receivedData = new MemoryStream(bajtovi);

             Bitmap bmp = new Bitmap(receivedData);
            */
        }
    }
}
