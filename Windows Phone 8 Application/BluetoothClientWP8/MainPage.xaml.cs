using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BluetoothClientWP8.Resources;
using Windows.Networking.Sockets;
using Windows.Networking.Proximity;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using BluetoothConnectionManager;
using System.Windows.Media;
using Windows.Devices.Geolocation;
using System.Xml.Linq;
using Microsoft.Phone.Maps.Services;
using System.Device.Location;
using System.Text;
using Windows.Phone.Devices.Notification;

namespace BluetoothClientWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ConnectionManager connectionManager;
        private StateManager stateManager;
        Geolocator myGeoLocator;

        Double Lat, Long;
        int life = 10;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            connectionManager = new ConnectionManager();
            connectionManager.MessageReceived += connectionManager_MessageReceived;
            stateManager = new StateManager();

        }

        async void connectionManager_MessageReceived(string message)
        {
            Debug.WriteLine("Message received:" + message);
            //string[] messageArray = message.Split(':');
            //switch (messageArray[0])

            switch (message)
            {
                case "f":
                    // send emergency alert
                    Dispatcher.BeginInvoke(delegate()
                    {
                        // find GPS location - continuous
                        // find address - Reverse Geocoding
                        ReverseGeocodeQuery query = new ReverseGeocodeQuery()
                        {
                            GeoCoordinate = new GeoCoordinate(Lat, Long)
                        };
                        query.QueryCompleted += query_QueryCompleted;
                        query.QueryAsync();

                    });
                break;
                case "s":
                    //reduce life
                    life--;
                    
                    Dispatcher.BeginInvoke(delegate()
                    {
                        // make ui changes
                        Life.Text = life.ToString();

                        if (life <= 0)
                            Life.Text = "Lost";
                        
                    });
                break;
                default :
                //if (float.Parse(message) > 0.0 && float.Parse(message) < 99.0)
                //{
                //    Temp.Text = message;
                //}
                Dispatcher.BeginInvoke(delegate()
                {
                    // make ui changes
                    Temp.Text = message;

                });
                
                break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            connectionManager.Initialize();
            stateManager.Initialize();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            connectionManager.Terminate();

            // GPS deconstructor
            //myGeoLocator.PositionChanged -= myGeoLocator_PositionChanged;
            //myGeoLocator = null;
        }

        private void ConnectAppToDeviceButton_Click_1(object sender, RoutedEventArgs e)
        {
            AppToDevice();
        }

        private async void AppToDevice()
        {
            ConnectAppToDeviceButton.Content = "Connecting...";
            PeerFinder.AlternateIdentities["Bluetooth:Paired"] = "";
            var pairedDevices = await PeerFinder.FindAllPeersAsync();

            if (pairedDevices.Count == 0)
            {
                Debug.WriteLine("No paired devices were found.");
            }
            else
            { 
                foreach (var pairedDevice in pairedDevices)
                {
                    if (pairedDevice.DisplayName == DeviceName.Text)
                    {
                        connectionManager.Connect(pairedDevice.HostName);
                        ConnectAppToDeviceButton.Content = "Connected";
                        DeviceName.IsReadOnly = true;
                        ConnectAppToDeviceButton.IsEnabled = false;
                        continue;
                    }
               } 
            }
        }



        // Tile tap callbacks
        private async void Tile1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string command = "g";
            await connectionManager.SendCommand(command);
            life = 10;
            Dispatcher.BeginInvoke(delegate()
            {
                // make ui changes
                Life.Text = life.ToString();

            });
        }

        private async void Tile2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string command = "h";
            await connectionManager.SendCommand(command);

            myGeoLocator = new Geolocator();
            myGeoLocator.DesiredAccuracy = PositionAccuracy.Default;
            myGeoLocator.MovementThreshold = 50;
            myGeoLocator.PositionChanged += myGeoLocator_PositionChanged;

        }

        private async void Tile3_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string command = "t";
            await connectionManager.SendCommand(command);

        }

        void myGeoLocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Lat = args.Position.Coordinate.Latitude;
            Long = args.Position.Coordinate.Longitude;

            Dispatcher.BeginInvoke(delegate()
            {
                // make UI changes
                Coordinates.Text = Lat+";"+Long;
            });
        }


        void query_QueryCompleted(object sender, QueryCompletedEventArgs<IList<MapLocation>> e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("An emergency situation is observed at following location-");
            foreach (var item in e.Result)
            {
                //sb.AppendLine(item.GeoCoordinate.ToString());
                sb.AppendLine(item.Information.Name);
                sb.AppendLine(item.Information.Description);
                sb.AppendLine(item.Information.Address.BuildingFloor);
                sb.AppendLine(item.Information.Address.BuildingName);
                sb.AppendLine(item.Information.Address.BuildingRoom);
                sb.AppendLine(item.Information.Address.BuildingZone);
                sb.AppendLine(item.Information.Address.City);
                //sb.AppendLine(item.Information.Address.Continent);
                sb.AppendLine(item.Information.Address.Country);
                //sb.AppendLine(item.Information.Address.CountryCode);
                sb.AppendLine(item.Information.Address.County);
                sb.AppendLine(item.Information.Address.District);
                sb.AppendLine(item.Information.Address.HouseNumber);
                sb.AppendLine(item.Information.Address.Neighborhood);
                sb.AppendLine(item.Information.Address.PostalCode);
                sb.AppendLine(item.Information.Address.Province);
                sb.AppendLine(item.Information.Address.State);
                //sb.AppendLine(item.Information.Address.StateCode);
                sb.AppendLine(item.Information.Address.Street);
                sb.AppendLine(item.Information.Address.Township);
            }

            Dispatcher.BeginInvoke(delegate()
            {
                // the below call puts the device to vibration mode for 2 seconds
                Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromSeconds(2.0));

                // make UI changes
                MessageBox.Show(sb.ToString());

                // send email and sms
                Uri EmailUri = new Uri(" http://antonioasaro.site50.net/mail_to_sms.php?cmd=send&who=nikhilesh1992@gmail.com&msg=Emergency%20at%20UPenn");
                WebClient wc = new WebClient();
                // Write to the WebClient
                wc.OpenWriteAsync(EmailUri, "GET");

            });
            
        }

    }
}