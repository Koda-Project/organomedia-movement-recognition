using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrganomediaLibrary;
using System.Resources;
using System.Globalization;
using OrganomediaLibrary.Entity;

namespace MovementRecognition
{
    public static class CommonStatic
    {
        static public bool IsTherapist = false;
        static public string GameSuccessSoundFileName = "";
        static public string GameErrorSoundFileName = "";

        static public CultureInfo Cul;
        static public ResourceManager resMan;

        static public int currPatientId = 0;

        static public List<string> ListColumnParameter = new List<string>()
        {
            "Game group"
        };

        static public string fontSizeParameter = "1.0";
        static double FontSizeAdjustment = -1;
        static public double StandardFontSize(double fontSize)
        {
            if (FontSizeAdjustment < 0)
            {
                var number = fontSizeParameter;
                if (number != null)
                {
                    double f = 1.0;
                    if (double.TryParse(number, out f))
                    {
                        FontSizeAdjustment = f;
                    }
                }

                if (FontSizeAdjustment < 0)
                {
                    FontSizeAdjustment = 1.1;
                }
            }
            return FontSizeAdjustment * fontSize * (System.Windows.SystemParameters.WorkArea.Width / (1920.0));
        }

        static public List<SeqOfScores> listSeqOfScoresLevel1OfPatient = new List<SeqOfScores>();
        static public List<SeqOfScores> listSeqOfScoresLevel2OfPatient = new List<SeqOfScores>();
    }
}
