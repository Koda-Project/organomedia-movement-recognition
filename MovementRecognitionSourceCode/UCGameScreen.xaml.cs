using OrganomediaComponentAPI;
using OrganomediaLibrary.Controls;
using OrganomediaLibrary.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Kinect;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace MovementRecognition
{
    /// <summary>
    /// Interaction logic for UCGameScreen.xaml
    /// </summary>
    public partial class UCGameScreen : UserControl
    {
        
        

        // private int _pivotColor = 1;

        private KinectHandTracker kinect;
        private static Random rand = new Random();
        private SoundPlayer successSound;
        private bool leftY,rightY;
        int requiredPose;

        public UCGameScreen()
        {
            
            InitializeComponent();

            successSound = new SoundPlayer(Properties.Resources.game_sound_correct);
            kinect = new KinectHandTracker();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();            
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,0,1,0);
            dispatcherTimer.Start();
            paintBody();
        }

        public void stopGame()
        {          
            successSound.Stop();
            successSound.Dispose();           
        }

        private void paintBody()
        {
            requiredPose = createUniqueRandomNumber(requiredPose,1,4);
            mainFigure1.Source = new BitmapImage(new Uri(@"Images\pose"+requiredPose+".png", UriKind.Relative));
        }

        private int createUniqueRandomNumber(int requiredPose, int minValue, int maxValue)
        {
            int possibleNumber;
            while (true)
            {
                possibleNumber = rand.Next(minValue, maxValue);
                if (possibleNumber != requiredPose)
                {
                    return possibleNumber;
                }
            }
            
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            leftY = kinect.LeftWrist.Y > 0.1;
            rightY = kinect.RightWrist.Y > 0.1;

            if (requiredPose == 1 && leftY && !rightY ||
                        requiredPose == 2 && !leftY && rightY ||
                        requiredPose == 3 && leftY && rightY)
            {
                paintBody();
                successSound.Play();
            }
            //string upOrDownUri = (rightY > 0.1 || leftY > 0.1) ? @"Images\up.png" : @"Images\down.png";

            //mainFigure1.Source = new BitmapImage(new Uri(upOrDownUri, UriKind.Relative));
        }



        public void SetFocus()
        {
            this.Focus();
        }
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
           
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
        private void keyBoard_eventKeyPress(string tav)
        {
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
        }






        private double _scale = 1;
        

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform mt = new ScaleTransform();
            _scale *= 1.1;
            mt.ScaleY = _scale;
            mt.ScaleX = _scale;

            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(mt);
            keysHolder.RenderTransform = myTransformGroup;
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform mt = new ScaleTransform();
            _scale /= 1.1;
            mt.ScaleY = _scale;
            mt.ScaleX = _scale;

            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(mt);
            keysHolder.RenderTransform = myTransformGroup;

        }

        



        
    }
}
