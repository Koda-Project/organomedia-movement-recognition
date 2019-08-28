using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace MovementRecognition
{
    public class KinectHandTracker
    {
        private KinectSensor sensor = null;
        private bool _leftHandUp = false;
        private bool _rightHandUp = false;
        private bool _tracking = false;
        public bool RightHandUp { get { return _rightHandUp; } }
        public bool LeftHandUp { get { return _leftHandUp; } }

        public SkeletonPoint LeftWrist { get; set; }
        public SkeletonPoint RightWrist { get; set; }
        public bool SensorWorking { get; set; }
        public bool Tracking { get { return _tracking; } }

        public KinectHandTracker()
        {
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    sensor = potentialSensor;
                    break;
                }
            }

            if (sensor != null)
            {
                sensor.SkeletonStream.Enable();
                sensor.SkeletonFrameReady += SensorSkeletonFrameReady;

                try
                {
                    this.sensor.Start();
                    SensorWorking = true;
                }
                catch (System.IO.IOException)
                {
                    this.sensor = null;
                    SensorWorking = false;
                }
            }
        }

        ~KinectHandTracker()  // destructor
        {
            if (null != sensor)
            {
                sensor.Stop();
            }
        }


        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                }
            }



            SkeletonPoint leftWrist = new SkeletonPoint();
            SkeletonPoint rightWrist = new SkeletonPoint();

            if (skeletons.Length != 0)
            {
                //go over the skeletons until you find movements in one of them (handle a case where the camera recognize more than one person)
                foreach (Skeleton skel in skeletons)
                {
                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        _tracking = true;
                        leftWrist = skel.Joints[JointType.WristLeft].Position;
                        rightWrist = skel.Joints[JointType.WristRight].Position;

                        LeftWrist = leftWrist;
                        RightWrist = rightWrist;

                        if (LeftWrist.Y > 0.1 || RightWrist.Y > 0.1)
                        {
                            break;
                        }
                        
                    }
                    else
                    {
                        _tracking = false;
                    }

                }
            }

            //Console.WriteLine("r: " + rightWrist.Y + " l: " + leftWrist.Y);
            //_leftHandUp = leftWrist.Y > 0.3;
            //_rightHandUp = rightWrist.Y > 0.3;

            
        }
    }
}
