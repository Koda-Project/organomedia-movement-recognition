using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MovementRecognition
{
    public class TextBoxHelper
    {
        private class RangeValue
        { 
            public double? Min { get; set; }
            public double? Max { get; set; }
            public bool IsInteger { get; set; }
        }
        private static Dictionary<TextBox, RangeValue> _cached = new Dictionary<TextBox, RangeValue>();
        public static void ClearCache()
        {
            _cached.Clear();
        }
        public static void SetupNumericTextBox(TextBox textBox, bool isInteger = false, double? min = null, double? max = null)
        {
            textBox.PreviewTextInput -= textBox_PreviewTextInput;
            textBox.PreviewTextInput += textBox_PreviewTextInput;

            if (min != null || max != null)
            {
                var range = new RangeValue()
                {
                    Min = min,
                    Max = max,
                    IsInteger = isInteger
                };
                _cached[textBox] = range;
            }
        }

        static void textBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string format = "[0-9]+(\\.)?[0-9]*";
            var t = sender as TextBox;
            if (_cached.ContainsKey(t))
            {
                var range = _cached[t];
                if (range.IsInteger)
                {
                    format = "[0-9]+";
                }
            }

            Regex regex = new Regex(format);
            e.Handled = !regex.IsMatch(e.Text);
            if (!e.Handled)
            {
                if (_cached.ContainsKey(t))
                {
                    var range = _cached[t];
                    double d = 0;
                    if (double.TryParse(e.Text, out d))
                    {
                        if (range.Min != null)
                        {
                            if (d < range.Min)
                            {
                                e.Handled = true;
                            }
                        }
                        if (!e.Handled)
                        {
                            if (range.Max != null)
                            {
                                if (d > range.Max)
                                {
                                    e.Handled = true;
                                }
                            }
                        }
                    }
                }
            }
        } 
    }
}
