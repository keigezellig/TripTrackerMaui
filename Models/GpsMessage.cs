using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class GpsMessage
    {
        public enum FixQuality
        {
            None, TwoD, ThreeD
        }
        public Location Location { get; }
        public int GpsSpeed { get; }
        public FixQuality TheFixQuality {  get; }
        

        public GpsMessage(Location location, int gpsSpeed, FixQuality fixQuality) =>
            (Location,GpsSpeed, TheFixQuality) = (location,gpsSpeed, fixQuality);   
        
    }
}
