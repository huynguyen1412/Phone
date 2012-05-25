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
        private ICommand getImage;
        private Uri imageUrl;
        public Uri ImageUrl {
            get { 
                return imageUrl; 
            }
            set { 
                imageUrl = value;
                ImageFileName = imageUrl.AbsolutePath.Substring(imageUrl.AbsolutePath.LastIndexOf('/') + 1);
                RaisedPropertyChanged("ImageUrl");
            }
        }

        public String ImageFileName { get; set; }

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
            ImageUrl = new Uri("http://res1.newagesolution.net/Portals/0/twitter2_icon.jpg");
            this.getImage = new DelegateCommand(GetImageFromUrl);
            webClient = new WebClient();
            LoadImageFromWebResult();
        }

        private void LoadImageFromWeb() {
            webClient.OpenReadAsync(ImageUrl);
        }

        // Web IO
        private void LoadImageFromWebResult() {
 
            // Get the image from the web
            webClient.OpenReadCompleted += (s1, e1) =>
                {
                    try {
                        if(e1.Error == null) {
                            // read the image into memory
                            int imgLength = (int)e1.Result.Length;
                            byte[] b = new byte[imgLength];
                            e1.Result.Read(b, 0, b.Length);

                            BitmapImage image = new BitmapImage();

                            // Attempt to save the image to IsolatedStorage
                            if(SaveImageStreamToIsolatedStorage(b, imgLength)) {
                                image = LoadImageStreamFromIsolatedStorage();
                            }

                            if(image == null) {
                                // Saving to storage failed, just set image manually
                                image = new BitmapImage();
                                image.SetSource(e1.Result);
                            }

                            ImageSource = image;
                        }
                    }
                    catch(Exception e) {
                    }
                };
        }

        // IsolateStorage 
        private bool SaveImageStreamToIsolatedStorage(byte[] data, int length) {

            bool result = false;
            try {
       
                if(IsSpaceAvailble(length)) {
                    using(IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(ImageFileName, System.IO.FileMode.Create, IsolatedStorageFile.GetUserStoreForApplication())) {

                        // Write it out to the file
                        isfs.Write(data, 0, length);
                        isfs.Flush();
                        result = true;
                    }
                }
            }
            catch(Exception e) {
                MessageBox.Show(e.ToString());
            }

            return result;
        }
        private BitmapImage LoadImageStreamFromIsolatedStorage() {

            BitmapImage result = null;

            try {
                using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication()) {
                    using(IsolatedStorageFileStream ioStream = file.OpenFile(ImageFileName, FileMode.Open)) {
                        result = new BitmapImage();
                        result.SetSource(ioStream);
                    }
                }
            }
            catch(Exception e) {
            }

            return result;
        }
        private bool IsSpaceAvailble(long amount) {
            using(IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication()) {
                 if (amount > store.AvailableFreeSpace)
                     return false;
            }
            return true;
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
