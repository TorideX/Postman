using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Postman.Model
{
    public class PostmanService
    {
        private HttpClient client;
        public PostmanService()
        {
            client = new HttpClient();
        }

        private StringContent GetRequestContent(Request request)
        {
            string mediaType = "";

            if (request.BodyType == BodyType.JSON)
            {
                mediaType = "application/json";
                
            }
            else if (request.BodyType == BodyType.Text)
            {
                mediaType = "text/plain";
            }
            else if (request.BodyType == BodyType.XML)
            {
                mediaType = "application/xml";

            }

            return new StringContent(request.Body, Encoding.UTF8, mediaType);
        }
        private void AddHeadersToMessage(ref HttpRequestMessage message, Request request)
        {
            foreach (var item in request.Headers)
            {
                if (item.IsEnabled == false) continue;
                message.Headers.Add(item.Key, item.Value);
            }
        }
        private HttpRequestMessage GetRequestMessage(Request request)
        {
            var msg = new HttpRequestMessage();

            msg.RequestUri = new Uri(request.Target);
            switch (request.RequestType)
            {
                case RequestType.GET:
                    msg.Method = HttpMethod.Get;
                    break;
                case RequestType.POST:
                    msg.Method = HttpMethod.Post;
                    break;
                case RequestType.PUT:
                    msg.Method = HttpMethod.Put;
                    break;
                case RequestType.DELETE:
                    msg.Method = HttpMethod.Delete;
                    break;
                default:
                    break;
            }

            if (request.RequestType != RequestType.GET)
            {
                msg.Content = GetRequestContent(request);
                AddHeadersToMessage(ref msg, request);
            }

            return msg;
        }

        public async Task<HttpResponseMessage> GetHttpResponse(Request request)
        {
            var response = await client.SendAsync(GetRequestMessage(request));

            return response;
        }
    }
}
