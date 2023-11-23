
# TripTracker

This is a small .NET MAUI application written in C# that displays data from an logging service (also created by myself, see https://gitlab.com/mh.joosten666/triptracker ) that is used to implement a simple GPS/trip tracker.

The logging service posts messages with a certain format to a MQTT topic that is read and processed by the application, more specific the messages are parsed into model classes (the code responsible for that is found in the `Services\DataService`, `Services\MessageProcessors` and `Services\MessageQueue` directories)
The parsed data is then used in a view model (the application uses the MVVM pattern), see `ViewModels\LiveDataViewModel` to show it in a view (defined in XAML, see `Views\LiveDataPage.cs`)


An example of the UI is shown in the [video](video.mp4)
