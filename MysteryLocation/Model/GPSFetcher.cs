﻿using MysteryLocation.ViewModel;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MysteryLocation.Model
{
    public class GPSFetcher
    {
        public static FeedViewModel fvm;
        public static MarkedViewModel mvm;
        public static UnlockedViewModel uvm;
        public static CategoryViewModel cavm;
        public static Position currentPosition;
        public static ViewCompass vc;

        public GPSFetcher()
        {

        }

        public async Task StartListening()
        {
            if (CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true);

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {

            //If updating the UI, ensure you invoke on main thread
            /*var position = e.Position;
            var output = "Full: Lat: " + position.Latitude + " Long: " + position.Longitude;
            output += "\n" + $"Time: {position.Timestamp}";
            output += "\n" + $"Heading: {position.Heading}";
            output += "\n" + $"Speed: {position.Speed}";
            output += "\n" + $"Accuracy: {position.Accuracy}";
            output += "\n" + $"Altitude: {position.Altitude}";
            output += "\n" + $"Altitude Accuracy: {position.AltitudeAccuracy}";*/
            setPositionsAsync(e.Position);
            
            //Console.WriteLine(output);
        }

        private async Task setPositionsAsync(Position position)
        {
            currentPosition = position;
            string latitude = "" + position.Latitude;
            string longitude = "" + position.Longitude;
            if (latitude.Length > 8)
                latitude = latitude.Substring(0, 8);
            if (longitude.Length > 8)
                longitude = longitude.Substring(0, 8);
            string writePosition = latitude + " , " + longitude;
            string writePositionLocation = await WritePositionWithLocation(position) + latitude + ", " + longitude;
            if (fvm != null) fvm.Position = writePosition;
            if (fvm != null) fvm.PositionLocation = writePositionLocation;

            if (mvm != null) mvm.Position = writePosition;
            if (mvm != null) mvm.PositionLocation = writePositionLocation;

            if (uvm != null) uvm.Position = writePosition;
            if (uvm != null) uvm.PositionLocation = writePositionLocation;

            if (cavm != null) cavm.Position = writePosition;
            if (cavm != null) cavm.PositionLocation = writePositionLocation;

            if (vc != null) vc.Position = writePosition;
            if (vc != null) vc.PositionLocation = writePositionLocation;

            if(fvm != null && mvm != null)
            {
                fvm.currentPos = position;
                mvm.currentPos = position;
                GlobalFuncs.gpsOn = true;
            }
        }

        private async Task<String> WritePositionWithLocation(Position p)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(p.Latitude, p.Longitude);

            var placemark = placemarks?.FirstOrDefault();

            if (placemark != null)
            {
                if (placemark.Locality != null) { return placemark.Locality + ", " + placemark.CountryName + " - "; }
                else if (placemark.Locality == null) { return placemark.SubLocality + ", " + placemark.CountryName + " - "; }
                else { return ""; };
            }
            else
            {
                return "";
            }
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine(e.Error);
            //Handle event here for errors
        }

        public async Task StopListening()
        {
            if (!CrossGeolocator.Current.IsListening)
                return;

            await CrossGeolocator.Current.StopListeningAsync();

            CrossGeolocator.Current.PositionChanged -= PositionChanged;
            CrossGeolocator.Current.PositionError -= PositionError;
        }
    }
}
