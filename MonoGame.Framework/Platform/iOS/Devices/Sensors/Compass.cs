using System;

using Microsoft.Xna.Framework;

using CoreMotion;
using Foundation;

namespace Microsoft.Devices.Sensors
{
    public sealed class Compass : SensorBase<CompassReading>
    {
        static readonly int MaxSensorCount = 10;
        static int instanceCount;
        private static bool started = false;
        private static SensorState state = IsSupported ? SensorState.Initializing : SensorState.NotSupported;
        private bool calibrate = false;

        public event EventHandler<CalibrationEventArgs> Calibrate;

        public static bool IsSupported => motionManager.DeviceMotionAvailable;
        public SensorState State => state;

        private static event CMDeviceMotionHandler readingChanged;

        public Compass()
        {
            if (!IsSupported)
                throw new SensorFailedException("Failed to start compass data acquisition. No default sensor found.");
            else if (instanceCount >= MaxSensorCount)
                throw new SensorFailedException("The limit of 10 simultaneous instances of the Compass class per application has been exceeded.");

            ++instanceCount;

            TimeBetweenUpdatesChanged += UpdateInterval;
            readingChanged += ReadingChangedHandler;
        }

        protected override void Dispose (bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (started)
                        Stop();
                    --instanceCount;
                }
            }
            base.Dispose(disposing);
        }

        public override void Start()
        {
            if (started == false)
            {
                // For true north use CMAttitudeReferenceFrame.XTrueNorthZVertical, but be aware that it requires location service
                motionManager.StartDeviceMotionUpdates(CMAttitudeReferenceFrame.XMagneticNorthZVertical, NSOperationQueue.CurrentQueue, MagnetometerHandler);
                started = true;
                state = SensorState.Ready;
            }
            else
                throw new SensorFailedException("Failed to start compass data acquisition. Data acquisition already started.");
        }

        public override void Stop()
        {
            motionManager.StopDeviceMotionUpdates();
            started = false;
            state = SensorState.Disabled;
        }

        private void MagnetometerHandler(CMDeviceMotion magnetometerData, NSError error)
        {
            readingChanged(magnetometerData, error);
        }

        private void ReadingChangedHandler(CMDeviceMotion data, NSError error)
        {
            CompassReading reading = new CompassReading();
            IsDataValid = error == null;
            if (IsDataValid)
            {
                reading.MagnetometerReading = new Vector3((float)data.MagneticField.Field.Y, (float)-data.MagneticField.Field.X, (float)data.MagneticField.Field.Z);
                reading.TrueHeading = Math.Atan2(reading.MagnetometerReading.Y, reading.MagnetometerReading.X) / Math.PI * 180;
                reading.MagneticHeading = reading.TrueHeading;
                switch (data.MagneticField.Accuracy)
                {
                    case CMMagneticFieldCalibrationAccuracy.High:
                        reading.HeadingAccuracy = 5d;
                        break;
                    case CMMagneticFieldCalibrationAccuracy.Medium:
                        reading.HeadingAccuracy = 30d;
                        break;
                    case CMMagneticFieldCalibrationAccuracy.Low:
                        reading.HeadingAccuracy = 45d;
                        break;
                }

                // Send calibrate event if needed
                if (data.MagneticField.Accuracy == CMMagneticFieldCalibrationAccuracy.Uncalibrated)
                {
                    if (calibrate == false)
                        EventHelpers.Raise(this, Calibrate, new CalibrationEventArgs());
                    calibrate = true;
                }
                else if (calibrate == true)
                    calibrate = false;

                reading.Timestamp = DateTime.UtcNow;
                CurrentValue = reading;
            }
        }

        private void UpdateInterval(object sender, EventArgs args)
        {
            motionManager.MagnetometerUpdateInterval = TimeBetweenUpdates.TotalSeconds;
        }
    }
}

