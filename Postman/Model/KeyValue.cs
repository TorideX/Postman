using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postman.Model
{
    public class KeyValue
    {
        public bool IsEnabled { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public KeyValue()
        {
            IsEnabled = true;
        }
    }
}
