using System.Globalization;
using MauiApp1.ViewModels;
using MauiIcons.Material;

namespace MauiApp1.Views.Converters;

public class TripStatusToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }
        
        var theValue = (LiveDataItemViewModel.TripStatus)value;
        switch (theValue)   
        {
            case LiveDataItemViewModel.TripStatus.Active:
                return (ImageSource)new MauiIcon
                {
                    Icon = MaterialIcons.PlayArrow
                };
            case LiveDataItemViewModel.TripStatus.Paused:
                return (ImageSource)new MauiIcon
                {
                    Icon = MaterialIcons.Pause
                };

            case LiveDataItemViewModel.TripStatus.FuelStop:
                return (ImageSource)new MauiIcon
                {
                    Icon = MaterialIcons.OilBarrel
                };
 
            case LiveDataItemViewModel.TripStatus.Finished:
                return (ImageSource)new MauiIcon
                {
                    Icon = MaterialIcons.Check
                };

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}