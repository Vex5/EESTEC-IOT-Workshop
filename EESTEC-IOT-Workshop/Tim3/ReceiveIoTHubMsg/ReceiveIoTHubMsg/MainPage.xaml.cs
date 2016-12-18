using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ReceiveIoTHubMsg
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //promjrnit devicekey
        static string iotHubUri = "SecuritySysteHub.azure-devices.net";
        static string deviceKey = "lHQpznHYQIEiP2ytHhnlPSbSXFY82MAcpLuq7QKj2Zo=";
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush dgreenBrush = new SolidColorBrush(Windows.UI.Colors.DarkGreen);
        static DeviceClient deviceClient;

        public MainPage()
        {
            InitializeComponent();       
            //promjenit deviceid     
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("Vedo", deviceKey));
            ReceiveC2dAsync();
        }


        private async void ReceiveC2dAsync()
        {
            while (true)
            {

                Microsoft.Azure.Devices.Client.Message receivedMessage = await deviceClient.ReceiveAsync();
                if (receivedMessage == null) continue;
                string receivedData = Encoding.ASCII.GetString(receivedMessage.GetBytes());

                if (receivedData == "1")
                {
                    Alert.Stroke = redBrush;
                    textBlock.Text = "Belaj!";
                    textBlock.FontSize = 35;
                }
                else
                {
                    Alert.Stroke = dgreenBrush;
                    textBlock.Text = "Normala";
                    textBlock.FontSize = 25;
                }
                await deviceClient.CompleteAsync(receivedMessage);
            }

        }
    }
}
