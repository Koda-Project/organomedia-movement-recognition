using OrganomediaComponentAPI;
using OrganomediaLibrary.Logic;
using OrganomediaLibrary;
using OrganomediaLibrary.LogicUC;
using OrganomediaLibrary.OrganomediaInterFace;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.Reflection;

namespace MovementRecognition
{
    public class clsGames : IOrganomedia, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Delegate
        public DelClickGameLevel EventClickSeq { get; set; }
        public DelClickGameLevel EventClickGameLevel { get; set; }
        public DelColumnParaeters ColumnParaeters { get; set; }
        public ActivityActionEvent activityActionEvent { get; set; }
        public DelEventSeqStatus EventSeqStatus { get; set; }
        public DelUpdateEndTime UpdateEndTime { get; set; }
        public ControlMouseDown EventMouseDown { get; set; }
        public DelNavigationButtonClick EventNavigationButtonClick { get; set; }
        public ReportPressedKey ReportPressedKey { get; set; }
        public GetPatientSetting GetPatientSetting { get; set; }
        public SetPatientSetting SetPatientSetting { get; set; }
        public DeletePatientSetting DeletePatientSetting { get; set; }

        public DelSequenceTransition EventSequenceTransition { get; set; }
        #endregion

        private ResourceManager _localResourceManager;

        public static clsGames API { get; private set; }

        void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string _fontSizeParameter;
        public string FontSizeParameter
        {
            get { return _fontSizeParameter; }
            set { _fontSizeParameter = value; CommonStatic.fontSizeParameter = value; }
        }

        private bool isTherapist;
        public bool IsTherapist//
        {
            get { return isTherapist; }
            set
            {
                isTherapist = value;
                if (ucmain != null)
                {
                    ucmain.DataContext = this;
                }
                CommonStatic.IsTherapist = value;
                
            }
        }

        public string gameName = "זיהוי תנועה";
        public string GameName
        {
            get { return gameName; }
            set { gameName = value; }
        }

        public string displayName = "זיהוי תנועה";
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string ConnectionString { get; set; }

        private UCMain ucmain;

        public clsGames()
        {
            API = this;
            _localResourceManager = new ResourceManager("MovementRecognition.Resource.Res", Assembly.GetExecutingAssembly());
        }

        static Dictionary<string, string> _english;
        static Dictionary<string, string> _hebrew;
        public string TranslateLocalResource(string name)
        {
            if (_localResourceManager != null)
            {
                try
                {
                    var item = _localResourceManager.GetString(name, CommonStatic.Cul);
                    if (item != null)
                    {
                        return item;
                    }
                }
                catch (Exception)
                {
                    Dictionary<string, string> dict = null;
                    if (CommonStatic.Cul == null || CommonStatic.Cul.Name == "en-US")
                    {
                        if (_english == null)
                        {
                            _english = new Dictionary<string, string>();
                            _english.Add("ColorsChooser", "Colors Chooser");
                            _english.Add("GameName", "זיהוי תנועה");
                            _english.Add("Key", "Key");
                            _english.Add("RandomHeight", "Random Height");
                            _english.Add("RandomPosition", "Random Position");
                            _english.Add("IdenticalType", "Identical Type");
                            _english.Add("Resume", "RESUME");
                            _english.Add("Save", "Save");
                            _english.Add("SequenceLength", "Sequence Length");
                            _english.Add("SpeedOfPlayingKeys", "Speed of Playing Keys");
                            _english.Add("SpeedOfSong", "Speed of Song");
                            _english.Add("Start", "START");
                            _english.Add("Suspend", "SUSPEND");
                            _english.Add("NumberRange", "Number Range");
                            _english.Add("GameDescription1", "Raise and lower your hands according to the picture");                            
                            _english.Add("SameColors", "Same Colors");
                            _english.Add("SameColorsOrder", "Same order of colors");
                            _english.Add("SameNumberAppearancesColors", "Same number of appearances of each color");
                            _english.Add("Delete", "Delete");
                            _english.Add("SaveAs", "Save As");
                            _english.Add("Sequence1", "Sequence 1");
                            _english.Add("Sequence2", "Sequence 2");
                        }
                        dict = _english;
                    }
                    else
                    {
                        if (_hebrew == null)
                        {
                            _hebrew = new Dictionary<string, string>();
                            _hebrew.Add("ColorsChooser", "צבע פנימי");
                            _hebrew.Add("GameName", "זיהוי תנועה");
                            _hebrew.Add("RandomHeight", "גובה אקראי");
                            _hebrew.Add("RandomPosition", "מיקום אקראי");
                            _hebrew.Add("Key", "קליד");
                            _hebrew.Add("IdenticalType", "סוג זהות");
                            _hebrew.Add("Resume", "המשך");
                            _hebrew.Add("Save", "שמור");
                            _hebrew.Add("SequenceLength", "אורך רצף");
                            _hebrew.Add("SpeedOfPlayingKeys", "זמן לתשובה");
                            _hebrew.Add("SpeedOfSong", "מהירות");
                            _hebrew.Add("Start", "התחל");
                            _hebrew.Add("Suspend", "עצור");
                            _hebrew.Add("NumberRange", "טווח מספרים");
                            _hebrew.Add("GameDescription1", "הרם והורד את הידיים בהתאם לתמונה");                            
                            _hebrew.Add("SameColors", "זיהוי תנועה");
                            _hebrew.Add("SameColorsOrder", "זהים בצבעים ובסדר");
                            _hebrew.Add("SameNumberAppearancesColors", "מופעים זהים");
                            _hebrew.Add("Delete", "מחק");
                            _hebrew.Add("SaveAs", "שמור בשם");
                            _hebrew.Add("Sequence1", "רצף 1");
                            _hebrew.Add("Sequence2", "רצף 2");
                        }
                        dict = _hebrew;
                    }

                    if (dict.ContainsKey(name))
                    {
                        return dict[name];
                    }
                }
            }
            return name;
        }

        public clsGames(string connectionString)
            : this()
        {
            ucmain = new UCMain(connectionString, DisplayName);
            ucmain.activityActionEvent += ucmain_actionEvent;
            ucmain.EventSeqStatus += ucmain_EventSeqStatus;
            ucmain.UpdateEndTime += ucmain_UpdateEndTime;
        }

        void ucmain_UpdateEndTime(DateTime dateTime)
        {
            UpdateEndTime(dateTime);
        }

        private void ucmain_actionEvent(string parameter)
        {
            activityActionEvent(parameter);
            ColumnParaeters(CommonStatic.ListColumnParameter);
        }

        public clsGames(string connectionString, string gameSuccessSoundFileName, string gameErrorSoundFileName, CultureInfo cul, ResourceManager resMan, int PatientId, bool _isTherapist, string fontSizeParameter)
            : this()
        {
            FontSizeParameter = fontSizeParameter;
            GlobalStatic.currCul = cul;
            CommonStatic.currPatientId = PatientId;
            CommonStatic.GameSuccessSoundFileName = gameSuccessSoundFileName;
            CommonStatic.GameErrorSoundFileName = gameErrorSoundFileName;
            CommonStatic.Cul = cul;
            CommonStatic.resMan = resMan;
            this.DisplayName = TranslateLocalResource("GameName");
            IsTherapist = _isTherapist;
            ucmain = new UCMain(connectionString, DisplayName);
            ucmain.DataContext = this;
            ucmain.EventSeqStatus += ucmain_EventSeqStatus;
            ucmain.activityActionEvent += ucmain_actionEvent;
            ucmain.UpdateEndTime += ucmain_UpdateEndTime;
            ucmain.EventMouseDown += ucmain_EventMouseDown;
            ucmain.EventClickGameLevel += ucmain_EventClickGameLevel;
            ucmain.EventClickSeq += ucmain_EventClickSeq;
            ucmain.EventNavigationButtonClick += ucmain_EventNavigationButtonClick;
            ucmain.EventSequenceTransition += ucmain_EventSequenceTransition;
        }

        private void ucmain_EventSequenceTransition(int seqId)
        {
            EventSequenceTransition(seqId);
        }

        void ucmain_EventNavigationButtonClick(NavigationButton usercontrol)
        {
            EventNavigationButtonClick(usercontrol);
        }

        void ucmain_EventClickSeq(string gameName, int level)
        {
            EventClickSeq(gameName, level);
        }

        void ucmain_EventClickGameLevel(string gameName, int level)
        {
            EventClickGameLevel("MovementRecognition", level);
        }

        private void ucmain_EventMouseDown(string strResumeOrStop)
        {
            EventMouseDown(strResumeOrStop);
        }


        void ucmain_EventSeqStatus(int seqId, int completedStatus)
        {
            EventSeqStatus(seqId, completedStatus);
        }

        public UserControl UCMain
        {
            get
            {
                return ucmain;
            }
        }

        public NavigationBar navBreadCrumb
        {
            get
            {
                return ucmain.uCNavigationBar;
            }
        }

        public void Stop()
        {
            if (UpdateEndTime != null)
                UpdateEndTime(DateTime.Now);
            ucmain.StopGame();
        }

        public void SelectGameLevel(int level, string gameName)
        {
        }

        public void ClickOnSeq(string gameName, int seqTag)
        {

        }

        public void SetFocus()
        {
            if (ucmain != null)
                ucmain.SetFocus();
        }
        public void SetVolume(int volume)
        {
            OrganomediaComponentAPI.Play.Instance.ChangeVolumeOfSound(volume);
            if (ucmain != null)
                ucmain.SetVolume(volume);
        }

        private int? patientId = null;
        public int? PatientId
        {
            get
            {
                return patientId;
            }
            set
            {
                patientId = value;
            }
        }

        private PlayingType _playingType;
        public PlayingType PlayingType
        {
            get
            {
                return _playingType;
            }
            set
            {
                _playingType = value;
            }
        }

        private int? _therapist = null;
        public int? TherapistId
        {
            get
            {
                return _therapist;
            }
            set
            {
                _therapist = value;
            }
        }
    }
}
