using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MauiApp1.Models;
using MauiApp1.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MauiApp1.Models.TripEvents;

namespace MauiApp1
{
    public partial class DataViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private DateTime _time;
        [ObservableProperty]
        private string _position;
        [ObservableProperty]
        private int _gpsSpeed;
        
        [ObservableProperty]
        private int _vehicleSpeed;
        [ObservableProperty]
        private int _vehicleRpm;
        [ObservableProperty]
        private int _vehicleCoolantTemp;
        public DataViewModel()
        {
            IsActive = true;
        }

        protected override void OnActivated()
        {
            Messenger.Register<DataViewModel, GpsModel>(this, (r, m) => r.UpdateGpsData(m));
            Messenger.Register<DataViewModel, StatusMessage>(this, (r, m) => r.UpdateStatus(m));
            Messenger.Register<DataViewModel, VehicleModel>(this, (r, m) => r.UpdateVehicleData(m));
        }

        private void UpdateVehicleData(VehicleModel model)
        {
            if (model is VehicleSpeedModel)
            {
                VehicleSpeed = (int)model.Value;
            }
            
            if (model is VehicleRpmModel)
            {
                VehicleRpm = (int)model.Value;
            }
            
            if (model is VehicleCoolantTempModel)
            {
                VehicleCoolantTemp = (int)model.Value - 273;
            }
        }

        private void UpdateStatus(StatusMessage m)
        {
            
            if (!m.IsStarted)
            {
                //Position = null;
               // GpsSpeed = GpsAltitude = VehicleSpeed = VehicleRpm = VehicleCoolantTemp = null;
            }
          //  IsActive = m.IsStarted;
        }

        private void UpdateGpsData(GpsModel model)
        {
            Time = model.Location.Timestamp.LocalDateTime;
            SetPosition(model.Location.Latitude, model.Location.Longitude);
            GpsSpeed = (int)(model.GpsSpeed * 3.6);            
            

        }

        private void SetPosition(double lat, double lon)
        {
            string result = "";
            
            if (lat < 0)
            {
                result += $"{Math.Abs(lat):F4}° S ";
            }
            else if (lat > 0)
            {
                result += $"{Math.Abs(lat):F4}° N ";
            }
            else
            {
                result += $"{Math.Abs(lat):F4}° ";
            }

            if (lon < 0)
            {
                result += $"{Math.Abs(lon):F4}° W ";
            }
            else if (lon > 0)
            {
                result += $"{Math.Abs(lon):F4}° E ";
            }
            else
            {
                result += $"{Math.Abs(lon):F4}° ";
            }

            Position = result;
        }



    }
}
