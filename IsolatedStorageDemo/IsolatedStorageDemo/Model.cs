using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Diagnostics;
using WPToolKit;


namespace IsolatedStorageDemo {
    public delegate void webResponseHandler(StorageStream stream);
   

    public class Model {
        IOStorage phoneStorage;
        private webResponseHandler webHandlerMethod { get; set; }
        public Notification nc;

        public Model() {
            phoneStorage = new IOStorage();
            nc = App.Current.GetApplicationNotificationObject();

            // create a new delegate (OnImageChanged) and assign it.
            webHandlerMethod = new webResponseHandler(this.OnImageChanged);
        }
        /// <summary>
        /// Loads from web asynchronously and installs the async ReadWebRequestHandler
        /// </summary>
        /// <param name="url">The URL.</param>
        public virtual void LoadFromWeb(Uri url) {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.BeginGetResponse(new AsyncCallback(ReadWebRequestHandler), request); 
        }
        /// <summary>
        /// Loads the specified filename from Isolated Storage.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public virtual Stream Load(Uri filename) {
            return phoneStorage.Load(filename);
        }
        /// <summary>
        /// Saves the specified stream to Isolated Storage
        /// </summary>
        /// <param name="stream">The stream.</param>
        public virtual void Save(StorageStream stream) {
            phoneStorage.Save(stream);
        }

        public virtual void Remove(Uri filename) {
            phoneStorage.Remove(filename);
        }
        /// <summary>
        /// Occurs when [image changed].  Subscribe to this event to 
        /// receive the Image stream when is arrives.
        /// </summary>
        protected void OnImageChanged(StorageStream stream) {
            nc.Send<StorageStream>(this, stream);
        }
        /// <summary>
        /// Handler to process the web request to read an image.  It will also
        /// dispatch a newly created Stream object to the UI thread.
        /// </summary>
        /// <param name="results">The results.</param>
        private void ReadWebRequestHandler(IAsyncResult results) {

            try {
                HttpWebRequest webRequest = (HttpWebRequest)results.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(results);

                StorageStream streamCopy;
                
                using (Stream stream = webResponse.GetResponseStream()) {
                    streamCopy = new StorageStream(stream);
                    using (stream) {
                        Deployment.Current.Dispatcher.BeginInvoke(webHandlerMethod, new Object[] { streamCopy });
                    };
                }
                webResponse.Close();
            }
            catch (WebException w) {
                Debug.WriteLine(w);
            }
            finally {
            }
        }
    }
}
