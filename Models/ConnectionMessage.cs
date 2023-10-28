using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class StatusMessage
    {
        public bool IsStarted { get; }

        public StatusMessage(bool isStarted)
        {
            IsStarted = isStarted;
        }
    }
}
