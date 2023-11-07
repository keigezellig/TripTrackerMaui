using System.Globalization;

namespace MauiApp1.Views.Converters;

public class DateTimeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTimeOffset dt) return "-";
        return dt == DateTimeOffset.MinValue ? "-" : $"{dt:g}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
    
    
    
}