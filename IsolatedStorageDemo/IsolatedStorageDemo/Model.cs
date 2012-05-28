using System;
using System.IO;
using System.Net;
using System.Windows;


namespace IsolatedStorageDemo {
    public class Model {
        IOStorage phoneStorage;
        ImageFromUrl webHandler;
        public ViewModel Controller { get; set; }

        public Model() {
            phoneStorage = new IOStorage();
        }

        /// <summary>
        /// Loads from web asynchronously and installs the async ReadWebRequestHandler
        /// </summary>
        /// <param name="url">The URL.</param>
        public void LoadFromWeb(Uri url) {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestHandler), request); 
        }

        public Stream Load(Uri filename) {
            return phoneStorage.Load(filename);
        }

        public void Save(Stream stream) {
            phoneStorage.Save(stream);
        }

        /// <summary>
        /// Handler to process the web request to read an image.  It will also
        /// dispatch a newly created Stream object to the UI thread.
        /// </summary>
        /// <param name="results">The results.</param>
        private void ReadWebRequestHandler(IAsyncResult results) {
            webHandler = Controller.webHandler;
            HttpWebRequest webRequest = (HttpWebRequest)results.AsyncState;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(results);

            Stream streamCopy = new MemoryStream();
            using(Stream stream = webResponse.GetResponseStream()) {
                try {
                    stream.CopyTo(streamCopy);
                    stream.Close();
                    Deployment.Current.Dispatcher.BeginInvoke(webHandler, new Object[] { streamCopy });
                }
                catch(Exception e) {
                }
            }

            webResponse.Close();
        }
    }
}
