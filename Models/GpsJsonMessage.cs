using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MauiApp1.Models
{
    public class GpsJsonMessage
    {
        public long T { get; }
        public double[] P { get; }

        public double V { get; }

        public int Q { get; }

        [JsonConstructor]
        public GpsJsonMessage(long t, double[] p, double v, int q) =>
            (T, P, V, Q) = (t,p,v,q);


    }
}
