using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Microsoft.Phone.Tasks;


namespace SilverLightLocalBitmap {
    public partial class MainPage : PhoneApplicationPage {

        CameraCaptureTask camera = new CameraCaptureTask();

        // Constructor
        public MainPage() {
            InitializeComponent();

            
            camera.Completed += new EventHandler<PhotoResult>((sender, e) => {
                if (e.TaskResult == TaskResult.OK) {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(e.ChosenPhoto);    // set the bitmap image to the photo
                    img.Source = bitmap;
                }
            });
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e) {

            //// load the images from the XAP resources
            //Uri imageUri = new Uri("Images/Hello.png", UriKind.Relative);
            //StreamResourceInfo resourceInfo = Application.GetResourceStream(imageUri);
            //BitmapImage bmp = new BitmapImage();
            //bmp.SetSource(resourceInfo.Stream);

            //// Set the Image element's source
            //img.Source = bmp;

            // User tapped the screen, start the camera task.
            camera.Show();

            // Handler cleanup
            e.Complete();
            e.Handled = true;

            base.OnManipulationStarted(e);
        }
    }
}