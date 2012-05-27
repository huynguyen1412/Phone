using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.IO;


namespace IsolatedStorageDemo {
    public class ViewModel : INotifyPropertyChanged {

        WebClient webClient;
        IOStorage phoneStorage;

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

            LoadImageFromWeb();
        }

        public ICommand LoadImageFromUrl {
            get {
                return this.getImage;
            }
        }
        public ViewModel() {
           // ImageUrl = new Uri("http://res1.newagesolution.net/Portals/0/twitter2_icon.jpg");
            ImageUrl = new Uri("http://8020.photos.jpgmag.com/3106321_283814_9433b77615_m.jpg");
            phoneStorage = new IOStorage();
            this.getImage = new DelegateCommand(GetImageFromUrl);
            webClient = new WebClient();

            // install the delegate for the web async call
            LoadImageFromWebResult();
        }

        private void LoadImageFromWeb() {
            BitmapImage image = new BitmapImage();

            try {
                Stream result = phoneStorage.Load(ImageUrl);
                image.SetSource(result);
            }
            catch(Exception e) {
                webClient.OpenReadAsync(ImageUrl);
            }
            finally {
                ImageSource = image;
            }
        }

        // Web IO
        private void LoadImageFromWebResult() {
 
            // Get the image from the web
            webClient.OpenReadCompleted += (s1, e1) =>
                {
                    BitmapImage image = null;

                    try {
                        if(e1.Error == null) {
                            // read the image into memory
                            int imgLength = (int)e1.Result.Length;
                            byte[] b = new byte[imgLength];
                            e1.Result.Read(b, 0, b.Length);

                            image = new BitmapImage();
                            image.SetSource(e1.Result);

                            // Attempt to save the image to IsolatedStorage
                            phoneStorage.Save(b, imgLength);
                        } 
                    }
                    catch(Exception e) {
                    }

                    ImageSource = image;
                };
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
