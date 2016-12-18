using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Drawing;


namespace Two_Way_Communication_App
{
    
    public sealed partial class MainPage : Page
    {
        
        static string connectionString = "HostName=SecuritySysteHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=8xNbj+v0tzkzwLMlvLJiucEsbuy1pPUDdfMYtGzpRk0=";
        static string iotHubUri = "SecuritySysteHub.azure-devices.net";
        static string deviceKey = "sCcOb7MNMbisRGIyTSHFMM5cCi7UNdHkebqv9Wi7rEk=";
        static DeviceClient deviceClient;
        static ServiceClient serviceClient;
        

        public MainPage()
        {
            InitializeComponent();
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("NajjaciSmo", deviceKey));
            ReceiveC2dAsync();
        }

        private async static Task SendCloudToDeviceMessageAsync(string podaci)
        {
            var commandMessage = new Microsoft.Azure.Devices.Message(Encoding.ASCII.GetBytes(podaci));
            await serviceClient.SendAsync("NajjaciSmo", commandMessage);
        }

        private async void ReceiveC2dAsync()
        {
            while (true)
            {

                Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;
                Byte[] bajtovi = receivedMessage.GetBytes();
                Stream receivedData = new MemoryStream(bajtovi);

                Bitmap bmp = new Bitmap(receivedData);
                System.Windows.Forms.PictureBox pictureBox1 = new System.Windows.Forms.PictureBox();
                pictureBox1.Image = bmp;

                await deviceClient.CompleteAsync(receivedMessage);
            }

        }

        private async void Uslikaj_Click(object sender, RoutedEventArgs e)
        {
            await SendCloudToDeviceMessageAsync("1");
        }
    }
}
