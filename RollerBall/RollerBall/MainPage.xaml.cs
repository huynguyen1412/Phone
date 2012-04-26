using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace RollerBall {
    public partial class MainPage : PhoneApplicationPage {
        // Constructor

        private Accelerometer _ac;

        public MainPage() {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait;

            ball.SetValue(Canvas.LeftProperty, ContentGrid.Width / 2);
            ball.SetValue(Canvas.TopProperty, ContentGrid.Height / 2);

            _ac = new Accelerometer();
            _ac.CurrentValueChanged +=new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(_ac_CurrentValueChanged);
        }

        private void _ac_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e) {
            Deployment.Current.Dispatcher.BeginInvoke( ()=>ReadingChanged(e));
        }

        private void ReadingChanged(SensorReadingEventArgs<AccelerometerReading> e) {

            Vector3 currentReading = e.SensorReading.Acceleration;

            double distanceTraveled = 2;
            double boundingBoxStrokeThickness = boundingBox.StrokeThickness;
            double rightBumper = ContentGrid.Width - ball.Width - boundingBoxStrokeThickness; // right margin is 0
            double leftBumper = ContentGrid.Margin.Left + boundingBoxStrokeThickness; // left margin is non-zero
            double bottomBumper = ContentGrid.Height - ball.Height - boundingBoxStrokeThickness;
            double topBumper = ContentGrid.Margin.Top + boundingBoxStrokeThickness;

            double acceleration = Math.Abs(currentReading.Z) == 0 ? 0.1 : Math.Abs(currentReading.Z);
          //  acceleration = (acceleration * 2)%2;
            Debug.WriteLine("Accel" + acceleration);
            double ballX = (double)ball.GetValue(Canvas.LeftProperty) +(double)distanceTraveled * (currentReading.X / acceleration);
            double ballY = (double)ball.GetValue(Canvas.TopProperty) - (double)distanceTraveled * (currentReading.Y / acceleration);

            if (ballX < leftBumper) {
                ballX = leftBumper;
            }

            else if (ballX > rightBumper) {
                ballX = rightBumper;
            }

            if (ballY < topBumper) {
                ballY = topBumper;
            }
            else if (ballY > bottomBumper) {
                ballY = bottomBumper;
            }

            ball.SetValue(Canvas.LeftProperty, ballX);
            ball.SetValue(Canvas.TopProperty, ballY);
        }


        private void btnStart_Click(object sender, RoutedEventArgs e) {
            try {
                _ac.Start();
            }
            catch (AccelerometerFailedException) {
                MessageBox.Show("Accelerometer Failed to Start");
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            try {
                _ac.Stop();
            }
            catch (AccelerometerFailedException) {
                MessageBox.Show("Accelerometer Failed to Stop");
            }
        }
    }
}