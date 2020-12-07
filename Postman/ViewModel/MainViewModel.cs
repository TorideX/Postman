using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Postman.Model;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Postman.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string tabName = "Untitled Request";
        public string TabName
        {
            get => tabName;
            set => Set(ref tabName, value);
        }

        private int requestType = 0;
        public int RequestType
        {
            get => requestType;
            set { Set(ref requestType, value); GetTabName(); }
        }

        private int requestBodyType;
        public int RequestBodyType
        {
            get => requestBodyType;
            set { Set(ref requestBodyType, value); }
        }

        private string requestBody = "";
        public string RequestBody
        {
            get => requestBody;
            set => Set(ref requestBody, value); 
        }

        private string requestUrl = "";
        public string RequestUrl
        {
            get => requestUrl;
            set { Set(ref requestUrl, value); GetTabName();
            }
        }

        private ObservableCollection<KeyValue> requestParams;
        public ObservableCollection<KeyValue> RequestParams
        {
            get { return requestParams; }
            set { requestParams = value; }
        }

        private ObservableCollection<KeyValue> requestHeaders;
        public ObservableCollection<KeyValue> RequestHeaders
        {
            get { return requestHeaders; }
            set { requestHeaders = value; }
        }

        private string responseContent;
        public string ResponseContent
        {
            get => responseContent;
            set => Set(ref responseContent, value);
        }

        private string responseStatus;
        public string ResponseStatus
        {
            get => responseStatus;
            set => Set(ref responseStatus, value);
        }

        private string responseTime;
        public string ResponseTime
        {
            get => responseTime;
            set => Set(ref responseTime, value);
        }

        private string responseSize;
        public string ResponseSize
        {
            get => responseSize;
            set => Set(ref responseSize, value);
        }

        private ObservableCollection<KeyValue> responseHeaders;
        public ObservableCollection<KeyValue> ResponseHeaders
        {
            get { return responseHeaders; }
            set { responseHeaders = value; }
        }

        #region Commands

        public RelayCommand SendBtnClicked { get; set; }

        #endregion


        PostmanService postman = new PostmanService();        // Postman Service   << =======
        Logger log;

        public MainViewModel()
        {
            SetupVariables();
        }

        private void SetupVariables()
        {
            SendBtnClicked = new RelayCommand(SendBtnClickedExecute);

            RequestParams = new ObservableCollection<KeyValue>();

            RequestHeaders = new ObservableCollection<KeyValue>();

            ResponseHeaders = new ObservableCollection<KeyValue>();

            log = new LoggerConfiguration().WriteTo.RollingFile(new JsonFormatter(), "logJson.txt").CreateLogger();//.File("log.txt").CreateLogger();
        }

        private void GetTabName()
        {
            if (string.IsNullOrWhiteSpace(requestUrl))
                TabName = $"{((RequestType)RequestType).ToString()} Untitled Request";
            else if (requestUrl.Length > 23) 
                TabName = $"{((RequestType)RequestType).ToString()} {requestUrl.Substring(0,23)}...";
            else
                TabName = $"{((RequestType)RequestType).ToString()} {requestUrl}";
        }
        
        private string GetFullRequestUrl()
        {
            string fullUrl = "";                         

            foreach (var item in RequestParams)
            {
                if (item.IsEnabled)
                {
                    fullUrl += "&";
                    if (!string.IsNullOrWhiteSpace(item.Key))
                        fullUrl += $"{item.Key}";
                    if (!string.IsNullOrWhiteSpace(item.Value))
                        fullUrl += $"={item.Value}";
                }
            }

            if (!(RequestUrl.Contains("http://") || RequestUrl.Contains("http://"))) return "http://" + RequestUrl.Split('?')[0] + "?" + fullUrl;
            else  return RequestUrl.Split('?')[0] + "?" + fullUrl;
        }

        private float GetResponseSize(HttpResponseMessage response)
        {
            var size = response.Content.ReadAsByteArrayAsync().Result.Length / 1000f;  // MUST CHANGE!!!
            return size;
        }

        private void GetResponseHeaders(HttpResponseHeaders headers)
        {
            ResponseHeaders.Clear();

            var value = "";
            foreach (var item in headers)
            {
                value = "";
                foreach (var itm in item.Value)
                {
                    value += $"{itm} ";
                }
            }

            foreach (var item in headers)
            {
                value = "";
                foreach (var itm in item.Value)
                {
                    value += $"{itm}, ";
                }
                ResponseHeaders.Add(new KeyValue() { Key = item.Key, Value = value });
            }
        }

        private async void SendBtnClickedExecute()
        {
            List<KeyValue> tmp = new List<KeyValue>(); 
            foreach (var item in RequestHeaders)
            {
                tmp.Add(item);
            }

            RequestUrl = GetFullRequestUrl();

            var request = new Request();
            request.BodyType = (BodyType)requestBodyType;
            request.Body = requestBody;
            request.RequestType = (RequestType)requestType;
            request.Target = requestUrl;
            request.Headers = tmp;

            HttpResponseMessage response;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                response = await postman.GetHttpResponse(request);
                var content = await response.Content.ReadAsStringAsync();

                log.Information("{@Request}", request, response);

                ResponseSize = GetResponseSize(response).ToString() + " KB";
                ResponseStatus = (int)response.StatusCode + " " + response.StatusCode.ToString();
                ResponseContent = content.Replace("\n", "").Replace(" ", "");

                GetResponseHeaders(response.Headers);
            }
            catch (Exception ex)
            {
                log.Error("{@Request}", request, ex);

                ResponseSize = "0 KB";
                ResponseStatus = "Error";
                ResponseContent = ex.Message;
            }
            stopwatch.Stop();
            ResponseTime = stopwatch.ElapsedMilliseconds.ToString() + " ms";
        }
    }
}