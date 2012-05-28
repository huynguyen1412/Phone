using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace IsolatedStorageDemo {

    public delegate void ImageFromUrl(object s, StreamEventArgs e);

    /// <summary>
    /// Defines an event type for Stream event
    /// </summary>
    public class StreamEventArgs : EventArgs {
        public StreamEventArgs(Stream stream) {
            this.stream = stream;
        }
        public Stream stream { get; set; }
    }
    public class ViewModel : INotifyPropertyChanged {

        Model dataModel = new Model();
        
        private ICommand getImage;
        private Uri imageUrl;
        public Uri ImageUrl {
            get { 
                return imageUrl; 
            }
            set { 
                imageUrl = value;
                RaisedPropertyChanged("ImageUrl");
            }
        }

        private BitmapImage imageSource;
        public BitmapImage ImageSource {
            get { 
                return imageSource; 
            }
            set { 
                imageSource = value;
                RaisedPropertyChanged("ImageSource");
            }
        }
        private void GetImageFromUrl(object p) {

            LoadImage();
        }

        public ICommand LoadImageFromUrl {
            get {
                return this.getImage;
            }
        }
        public ViewModel() {
           // ImageUrl = new Uri("http://res1.newagesolution.net/Portals/0/twitter2_icon.jpg");
            ImageUrl = new Uri("http://8020.photos.jpgmag.com/3106321_283814_9433b77615_m.jpg");
            this.getImage = new DelegateCommand(GetImageFromUrl);
            dataModel.ImageChanged += ImageFromUrl;
        }

        /// <summary>
        /// Images from URL. This is the dispatch handler for the http web request to get the image.
        /// It also attempts to save the image to Isolated Storage
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void ImageFromUrl(object s, StreamEventArgs e) {
         
            Stream stream = e.stream;
            BitmapImage image = new BitmapImage();
            image.SetSource(stream);
            ImageSource = image;

            dataModel.Save(stream);
            stream.Close();
        }

        /// <summary>
        /// Loads the image from the Url if the image is not in the Isolated Storage and assigns
        /// it to the binding ImageSource.
        /// </summary>
        private void LoadImage() {
            BitmapImage image = new BitmapImage();

            try {
                Stream stream = dataModel.Load(ImageUrl);
                image.SetSource(stream);
                stream.Close();
            }
            catch(IsolatedStorageException e) {
                Debug.WriteLine(e);
                dataModel.LoadFromWeb(imageUrl);  
            }
            finally {
                ImageSource = image;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisedPropertyChanged(String property) {

            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                handler(property, new PropertyChangedEventArgs(property));
            }
        }
    }
}
