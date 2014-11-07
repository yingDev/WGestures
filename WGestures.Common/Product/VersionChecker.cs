using System;
using System.Net;
using Newtonsoft.Json;

namespace WGestures.Common.Product
{
    public class VersionChecker : IDisposable
    {
        private string _url;
        private TimeOutWebClient _client;

        public VersionChecker(string url, int timeOutSeconds = 15)
        {
            _url = url;
            _client = new TimeOutWebClient(){TimeOutSecs = timeOutSeconds};
            _client.Encoding = System.Text.Encoding.UTF8;

            _client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Cancelled)
                {
                    OnCanceled();
                    return;
                }

                if (args.Error != null)
                {
                    OnErrorHappened(args.Error);
                    return;
                }

                VersionInfo versionInfo = null;

                try
                {
                    versionInfo = JsonConvert.DeserializeObject<VersionInfo>(args.Result);
                }
                catch (Exception e)
                {
                    OnErrorHappened(e);
                    return;
                }

                OnFinished(versionInfo);

            };


        }

        public event Action<VersionInfo> Finished;
        public event Action Canceled;
        public event Action<Exception> ErrorHappened;


        protected virtual void OnFinished(VersionInfo obj)
        {
            var handler = Finished;
            if (handler != null) handler(obj);
        }
        protected virtual void OnCanceled()
        {
            var handler = Canceled;
            if (handler != null) handler();
        }
        protected virtual void OnErrorHappened(Exception e)
        {
            var handler = ErrorHappened;
            if (handler != null) handler(e);
        }

        public void CheckAsync()
        {
            _client.DownloadStringAsync(new Uri(_url));
        }

        public void Cancel()
        {
            _client.CancelAsync();
            
        }

        public bool IsBusy
        {
            get { return _client.IsBusy; }
        }

        public void Dispose()
        {
            if(_client.IsBusy) _client.CancelAsync();
            _client.Dispose();
            _client = null;
        }
    }

    internal class TimeOutWebClient : WebClient
    {
        public int TimeOutSecs { get; set; }

        public TimeOutWebClient()
        {
            TimeOutSecs = 30;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest w = base.GetWebRequest(address);
            
            w.Timeout = TimeOutSecs * 1000;
            return w;
        }
    }
}
