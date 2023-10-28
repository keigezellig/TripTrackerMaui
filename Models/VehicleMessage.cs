using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public abstract class VehicleMessage
    {
        public double Value { get; }
                

        protected VehicleMessage(double value)
        {
            Value = value;
        }
    }
}
