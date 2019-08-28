using System;
using System.Collections.Generic;
using System.Linq;
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
using OrganomediaLibrary.LogicUC;
using OrganomediaLibrary.Data;
using OrganomediaLibrary.Entity;
using OrganomediaLibrary.Logic;
using OrganomediaLibrary.OrganomediaInterFace;
using System.Resources;

namespace MovementRecognition
{
    public delegate void ShowSeqGame(int seqId, List<string> listSeq, int color, int level, int typegame, List<SeqOfScores> lissSeqOfScore);
    public delegate void DelGameHomePageClick();
    public delegate void DelBreadCrumbClick(string frameName);

    public partial class UCMain : UserControl
    {
        public event DelClickGameLevel EventClickSeq;
        public event DelClickGameLevel EventClickGameLevel;
        public event ControlMouseDown EventMouseDown;
        public event DelUpdateEndTime UpdateEndTime;
        public event DelEventSeqStatus EventSeqStatus;
        public event DelNavigationButtonClick EventNavigationButtonClick;
        public DelSequenceTransition EventSequenceTransition { get; set; }
        public ActivityActionEvent activityActionEvent { get; set; }
        public DelGameHomePageClick gameHomePage_Click;
        public OrganomediaLibrary.LogicUC.NavigationBar uCNavigationBar = new OrganomediaLibrary.LogicUC.NavigationBar();
        public OrganomediaLibrary.LogicUC.NavigationButton currNavigationButton = new OrganomediaLibrary.LogicUC.NavigationButton();
        public OrganomediaLibrary.LogicUC.NavigationButton HomeButton;
        NavigationButton navChooseSequence;
        NavigationButton navSeqLevel;
        static public ResourceManager resMan;
         
        public UCGameScreen ucGameScreen;

        private string SenderName;

        public UCMain()
        {
            InitializeComponent();
            resMan = new ResourceManager("MovementRecognition.Resource.Res", typeof(UCMain).Assembly);
            SwitchLanguage();
        }
        public void SetFocus()
        {
            if (ucGameScreen != null)
                ucGameScreen.SetFocus();
        }

        void currChooseSeqLevel_EventClickGameLevel(string gameName, int level)
        {
            EventClickGameLevel("MovementRecognition", level);
        }

        private void currChooseSeqLevel_actionEvent(string parameter)
        {
            activityActionEvent(parameter);
        }

        private void currChooseSeqLevel_PickName(object sender)
        {
            SenderName = (sender as Button).Content.ToString();
        }

        private void currChooseSeqLevel_PickTag(object sender)
        {
            SenderName = (sender as Button).Tag.ToString();
        }
         
        public UCMain(string connectionString, string displayName) : this()
        {
            this.DisplayName = displayName;
            DataAccess.SetConnectionString(connectionString);

            ucGameScreen = new UCGameScreen();
            VisibleCollapse(Visibility.Visible, Visibility.Collapsed);
            MainFrame.NavigationService.Navigate(ucGameScreen);
            InitHomeButton();
        }
        public void SetVolume(int volume)
        {
            
        } 

        void currChooseSequence_EventClickSeq(string gameName, int level)
        {
            EventClickSeq(gameName, level);
        } 

        void ucSeqLevel_EventBreadCrumbClick(string frameName)
        {
            if (frameName == "UcChooseSequence")
                NavigationButton_Click(navChooseSequence, null);
        }

        private void ucSeqLevel_EventSequenceTransition(int seqId)
        {
            EventSequenceTransition(seqId);
        }

        private void ucSeqLevel_EventMouseDown(string strResumeOrStop)
        {
            EventMouseDown(strResumeOrStop);
        }


        void ucSeqLevel_UpdateEndTime(DateTime dateTime)
        {
            UpdateEndTime(dateTime);
        }

        void ucSeqLevel_EventSeqStatus(int seqId, int completedStatus)
        {
            EventSeqStatus(seqId, completedStatus);
            if (completedStatus == 2)
            {
                NavigationButton_Click(navChooseSequence, null);
            }
        }

        private void VisibleCollapse(Visibility v_mainFrame, Visibility v_ugMain)
        {
            MainFrame.Visibility = v_mainFrame;
            UGMain.Visibility = v_ugMain;
        }

        public string DisplayName = "Error";
        private void InitHomeButton()
        {
            HomeButton = new NavigationButton(ucGameScreen, DisplayName);
            HomeButton.VerticalAlignment = VerticalAlignment.Stretch;
            HomeButton.VerticalContentAlignment = VerticalAlignment.Center;
            HomeButton.HorizontalContentAlignment = HorizontalAlignment.Center;
            HomeButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            //HomeButton.MinHeight = 70;
            HomeButton.Tag = 2;
            HomeButton.Click += NavigationButton_Click;
            HomeButton.Select();
            currNavigationButton = HomeButton;
            uCNavigationBar.Add(HomeButton);
            navigationFrame.NavigationService.Navigate(uCNavigationBar);
            //HomeButton.Visibility = Visibility.Collapsed;
        }

        public void StopGame()
        {
            if (ucGameScreen != null)
            {
                EventMouseDown("Stop");
                //UpdateEndTime(DateTime.Now);
                LedsAndKeys.Instance.AllLedsOff();
                ucGameScreen.stopGame();
            }
        }

        public void NavigationButton_Click(object sender, RoutedEventArgs e)
        {
            //activityActionEvent(e);
            StopGame();
            OrganomediaLibrary.LogicUC.NavigationButton selectedNavigationButton = sender as OrganomediaLibrary.LogicUC.NavigationButton;
            EventNavigationButtonClick(selectedNavigationButton);
            MainFrame.NavigationService.Navigate(selectedNavigationButton.Frame);
            for (int i = uCNavigationBar.NavigationGridCount() - 1; i > (int)((sender as OrganomediaLibrary.LogicUC.NavigationButton).Tag); i--)
            {
                uCNavigationBar.RemoveIndex(i);
            }
            selectedNavigationButton.Select();
            //for (int i = 1; i < intEzer.Count; i++)
            //    uCNavigationBar.navigationGrid.Children.Remove(uCNavigationBar.navigationGrid.Children[intEzer[i]]);
            //currentButton.Select();
        }

        #region translate language
        private void SwitchLanguage()
        {
        }
        #endregion

        private void UserControl_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            EventMouseDown("Resume");
            this.Focusable = true;
            this.Focus();
            if (ucGameScreen != null)
                ucGameScreen.SetFocus();
        }
    } 
}
