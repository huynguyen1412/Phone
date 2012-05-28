using System;
using System.IO;
using System.Net;
using System.Windows;


namespace IsolatedStorageDemo {
    public delegate void webResponseHandler(Stream stream);
 
    public class Model {

        IOStorage phoneStorage;
        private webResponseHandler webHandlerMethod { get; set; }
  
        public Model() {
            phoneStorage = new IOStorage();

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
        public virtual void Save(Stream stream) {
            phoneStorage.Save(stream);
        }

        /// <summary>
        /// Occurs when [image changed].  Subscribe to this event to 
        /// receive the Image stream when is arrives.
        /// </summary>
        public event ImageFromUrl ImageChanged;
        protected virtual void OnImageChanged(Stream stream) {
            StreamEventArgs e = new StreamEventArgs(stream);

            if(ImageChanged != null)
                ImageChanged(this, e);
        }

        /// <summary>
        /// Handler to process the web request to read an image.  It will also
        /// dispatch a newly created Stream object to the UI thread.
        /// </summary>
        /// <param name="results">The results.</param>
        private void ReadWebRequestHandler(IAsyncResult results) {
            HttpWebRequest webRequest = (HttpWebRequest)results.AsyncState;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(results);

            Stream streamCopy = new MemoryStream();
            using(Stream stream = webResponse.GetResponseStream()) {
                try {
                    stream.CopyTo(streamCopy);
                    stream.Close();
                    Deployment.Current.Dispatcher.BeginInvoke(webHandlerMethod, new Object[] {streamCopy});
                }
                catch(Exception e) {
                }
            }

            webResponse.Close();
        }
    }
}
