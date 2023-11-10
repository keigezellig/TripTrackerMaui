using System.Collections.ObjectModel;
using MauiApp1.Controls.MarkerMap;

namespace MauiApp1.Helpers;

public static class CollectionExtensions

{
    static int _lastId = 0;

    static int GenerateId()
    {
        return Interlocked.Increment(ref _lastId);
    }
    public static void AddWithId(this ObservableCollection<Marker> markers, Marker newMarker)
    {
            newMarker.Id = GenerateId();

        markers.Add(newMarker);
    }
}