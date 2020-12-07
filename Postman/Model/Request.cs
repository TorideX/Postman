using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Postman.Model
{
    public class Request
    {
        public RequestType RequestType { get; set; }
        public string Target { get; set; }
        public List<KeyValue> Params { get; set; }
        public List<KeyValue> Headers { get; set; }
        public string Body { get; set; }
        public BodyType BodyType { get; set; }
    }
}
