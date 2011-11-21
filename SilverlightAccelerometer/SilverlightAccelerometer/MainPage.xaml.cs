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
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;


namespace SilverlightAccelerometer {

    public partial class MainPage : PhoneApplicationPage {
        // Constructor
        public MainPage() {
            InitializeComponent();

            Accelerometer acc = new Accelerometer();
            acc.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(OnAccelerometerReadingChanged);

            try 
	        {	        
		        acc.Start();
	        }
	        catch (Exception e)
	        {
		        txtBlk.Text = e.Message;
	        }
        }


        void OnAccelerometerReadingChanged(object sender, SensorReadingEventArgs<AccelerometerReading> args) {

            String str = String.Format( "X = {0:F2}\n" +
                                        "Y = {1:F2}\n" +
                                        "Z = {2:F2}\n" +
                                        "Magnitude = {3:F2}\n\n" +
                                        "{4}",
                                        args.SensorReading.Acceleration.X, args.SensorReading.Acceleration.Y, args.SensorReading.Acceleration.Z,
                                        Math.Sqrt((args.SensorReading.Acceleration.X * args.SensorReading.Acceleration.X) +
                                            (args.SensorReading.Acceleration.Y * args.SensorReading.Acceleration.Y) + 
                                            (args.SensorReading.Acceleration.Z * args.SensorReading.Acceleration.Z)),
                                        args.SensorReading.Timestamp);

            if (txtBlk.CheckAccess()) {
                txtBlk.Text = str;
            }
            else {
                txtBlk.Dispatcher.BeginInvoke(()=> { 
                    txtBlk.Text = str; });
            }
        }
    }
}