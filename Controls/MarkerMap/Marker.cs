using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CoordinateSharp;

namespace MauiApp1.Controls.MarkerMap;

[NotifyPropertyChangedRecipients]
public partial class Marker : ObservableRecipient
{
    
    [ObservableProperty]
    private Coordinate _position;
    [ObservableProperty]
    private Color _color;
    [ObservableProperty] 
    private string _label;
    [ObservableProperty] 
    private string _description;
    [ObservableProperty] 
    private bool _isVisible;
    
    public MarkerSet MarkerSet { get;  }
    public int Id { get; set; }
    

    public Marker(Coordinate position, Color color, string label, string description, bool isVisible, MarkerSet markerSet)
    {
        Position = position;
        Color = color;
        Label = label;
        Description = description;
        IsVisible = isVisible;
        MarkerSet = markerSet;
    }

    public override string ToString()
    {
        return $"{nameof(Position)}: {Position}, {nameof(Color)}: {Color}, {nameof(Label)}: {Label}, {nameof(Description)}: {Description}, {nameof(IsVisible)}: {IsVisible}";
    }
}


public partial class MarkerSet : ObservableObject
{
    [ObservableProperty] private string _id;
    [ObservableProperty] private ObservableCollection<Marker> _markers;
    private bool _isVisible;
    [ObservableProperty] private bool _isSelected;

    public MarkerSet(string id)
    {
        Id = id;
        Markers = new ObservableCollection<Marker>();
        IsVisible = false;
        IsSelected = false;
    }
    
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            SetProperty(ref _isVisible, value);
            foreach (var marker in Markers)
            {
                marker.IsVisible = value;
            }
        }
    }

    
}