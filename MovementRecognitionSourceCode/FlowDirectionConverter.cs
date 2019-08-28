using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.ComponentModel;


namespace MovementRecognition.Converters
{
    [ValueConversion((typeof(FlowDirection)),typeof(FlowDirection))]
    public class FlowDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DesignerProperties.GetIsInDesignMode(value as FrameworkElement))
                return null;
            else
            {
                return CommonStatic.Cul.Name == "he-IL" ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
