// ---------- IMPORTS ----------
using System.Windows;              // WPF framework — gives us Window, MessageBox, RoutedEventArgs
using Windows.Devices.Geolocation;  // WinRT geolocation API — talks to Windows Location Services

namespace WpfApp1                   // project namespace — just a container name
{
    // "partial" means the other half of this class is auto-generated from the XAML file
    // (the XAML defines the UI layout; this file defines the behaviour)
    public partial class MainWindow : Window
    {
        // Constructor — runs once when the window first opens
        public MainWindow()
        {
            InitializeComponent();  // reads the XAML and builds all the buttons/labels/etc.
        }

        // ---------- BUTTON CLICK HANDLER ----------
        // "async" = this method contains awaitable calls (won't freeze the UI while waiting)
        // "void" = event handlers must return void (even when async)
        // "sender" = the button that was clicked
        // "e" = event metadata (which button, mouse position, etc.)
        private async void ShowMsg(object sender, RoutedEventArgs e)
        {
            // Step 1: Ask Windows for permission to use location
            //         RequestAccessAsync() returns a status enum, not the location itself
            var access = await Geolocator.RequestAccessAsync();

            // Step 2: If the user denied location access, show an error and stop here
            if (access != GeolocationAccessStatus.Allowed)
            {
                MessageBox.Show("Location access denied.");
                return;  // bail out — no point continuing without permission
            }

            // Step 3: Create a Geolocator object and request 10m accuracy
            //         "DesiredAccuracyInMeters = 10" is a REQUEST, not a guarantee
            //         On a laptop with no GPS chip, you'll get WiFi-based accuracy (~30-100m)
            var geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };

            // Step 4: Actually fetch the position from Windows Location Services
            //         Under the hood, Windows checks: GPS chip > WiFi triangulation > IP lookup
            //         This is the slow part — await lets the UI stay responsive while we wait
            var position = await geolocator.GetGeopositionAsync();

            // Step 5: Drill into the nested object to extract lat/lon
            //         position.Coordinate.Point.Position is a BasicGeoposition struct
            //         containing .Latitude, .Longitude, .Altitude
            var coord = position.Coordinate.Point.Position;

            // This prints to the debug output window, NOT visible in the WPF app itself
            Console.WriteLine("Hello World");

            // Step 6: Show a popup with the coordinates
            //         $"..." is string interpolation — embeds variables directly
            //         :F3 formats the number to 3 decimal places
            //         position.Coordinate.Accuracy = radius in metres (how confident Windows is)
            MessageBox.Show(
                $"Latitude:  {coord.Latitude:F3}\n" +
                $"Longitude: {coord.Longitude:F3}\n" +
                $"Accuracy:  {position.Coordinate.Accuracy}m",
                "You have been located...");
        }
    }
}
