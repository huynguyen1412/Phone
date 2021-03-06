﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPToolKit;


namespace IsolatedStorageDemo {

    public delegate void ImageFromUrl(object s, StorageStream e);

    public class ViewModel : ViewModelBase {

        Model dataModel = new Model();
        
        private ICommand getImage;
        private ICommand removeImage;
        private Uri imageUrl;
        public Uri ImageUrl {
            get { 
                return imageUrl; 
            }

            set {
                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }
        private BitmapImage imageSource;
        public BitmapImage ImageSource {
            get { 
                return imageSource; 
            }
            set { 
                imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }
        public ICommand LoadImageFromUrl {
            get {
                return this.getImage;
            }
        }
        public ICommand DeleteImageFromStorage {
            get {
                return this.removeImage;
            }
        }
        public ViewModel() {

            // Test url for jpeg image
            ImageUrl = new Uri("http://8020.photos.jpgmag.com/3106321_283814_9433b77615_m.jpg");
            
            // Button ICommand handler
            this.getImage = new DelegateCommand(m => LoadImage());
            this.removeImage = new DelegateCommand(m => DeleteImage());

            // Listen for changes in the model 
            App.Current.GetApplicationNotificationObject().Register<StorageStream>(this.ImageFromUrl);
        }
        /// <summary>
        /// Images from URL. This is the event handler the model sends when a new image is ready.
        /// It also attempts to save the image to Isolated Storage
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void ImageFromUrl(object o, StorageStream e) {
         
            // Extract the stream from the event args update Image control
            StorageStream s = e;

            BitmapImage image = new BitmapImage();
            image.SetSource(s);
            ImageSource = image;

            // Save it to Isolated Storage
            dataModel.Save(s);
            s.Close();
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

            // file name could be null
            catch (ArgumentNullException) {
            }
            // catch the invalid operation exception for parsing the Uri
            catch (InvalidOperationException) {
            }
            // use this exception to try and load the file from the web if it's not in the storage
            catch (IsolatedStorageException e) {
                Debug.WriteLine(e);
                dataModel.LoadFromWeb(imageUrl);
            }
            finally {
                ImageSource = image;
            }
        }

        private void DeleteImage() {
            ApplicationSettings s = new ApplicationSettings();

            try {
                dataModel.Remove(ImageUrl);
            } 
            catch (ArgumentNullException) {
            }

            ImageSource = null;
        }
    }
}
