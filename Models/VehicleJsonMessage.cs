using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class VehicleJsonMessage
    {
        public string q { get; }
        public double v { get; }
        public string u { get; }

        [JsonConstructor]
        public VehicleJsonMessage(string q, double v, string u)
        {
            this.q = q;
            this.v = v;
            this.u = u;
        }
    }
}
