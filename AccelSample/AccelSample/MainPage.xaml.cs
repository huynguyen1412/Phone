using System;
using System.Windows;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;

namespace AccelSample {
    public partial class MainPage : PhoneApplicationPage {

        Accelerometer _ac;

        // Constructor
        public MainPage() {
            InitializeComponent();
            _ac = new Accelerometer();
            _ac.CurrentValueChanged +=new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(_ac_CurrentValueChanged);
        }


        private void _ac_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e) {
        
            // This handler is called on a different thread
            Deployment.Current.Dispatcher.BeginInvoke(() => ProcessAccelerometerReading(e));
        }
        
        private void ProcessAccelerometerReading(SensorReadingEventArgs<AccelerometerReading> e) {

            Vector3 reading = e.SensorReading.Acceleration;
            txtTime.Text = e.SensorReading.Timestamp.ToString();
            txtX.Text = reading.X.ToString();
            txtY.Text = reading.Y.ToString();
            txtZ.Text = reading.Z.ToString();

            ProcessPRY(e);
        }

        private void ProcessPRY(SensorReadingEventArgs<AccelerometerReading> e) {

            Vector3 reading = e.SensorReading.Acceleration;

            txtPitch.Text = ((180 / Math.PI) * (

                Math.Atan(reading.X/Math.Sqrt(Math.Pow(reading.Y,2))) + Math.Pow(reading.Z,2))).ToString();

            txtRoll.Text = ((180 / Math.PI) * (

                Math.Atan(reading.Y / Math.Sqrt(Math.Pow(reading.X, 2))) + Math.Pow(reading.Z, 2))).ToString();

            txtYaw.Text = ((180 / Math.PI) * (

                Math.Atan(Math.Sqrt(Math.Pow(reading.X, 2))) + Math.Pow(reading.Y, 2)/reading.Z)).ToString();

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