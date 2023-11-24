using System.Globalization;

namespace MauiApp1.Views.Converters;

public class LocationToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return GetPosition((Location)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }


    private string GetPosition(Location location)
    {
        string result = "";

        if (location == null)
        {
            return "-";
        }

        if (location.Latitude < 0)
        {
            result += $"{Math.Abs(location.Latitude):F4}° S ";
        }
        else if (location.Latitude > 0)
        {
            result += $"{Math.Abs(location.Latitude):F4}° N ";
        }
        else
        {
            result += $"{Math.Abs(location.Latitude):F4}° ";
        }

        if (location.Longitude < 0)
        {
            result += $"{Math.Abs(location.Longitude):F4}° W ";
        }
        else if (location.Longitude > 0)
        {
            result += $"{Math.Abs(location.Longitude):F4}° E ";
        }
        else
        {
            result += $"{Math.Abs(location.Longitude):F4}° ";
        }

        return result;
    }
}