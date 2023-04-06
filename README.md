# RaspAlarm
Just a clock and alarm written in .NET Framwork 4.8.1 following MVVM pattern.
It supports showing the current time, waking you up, adding and removing alarms.

![Program image](/RaspAlarm/Screenshots/NewAlarm.png)


## Functionality
### New alarm
Click "New Alarm" button.
Type an hour, or a day or both to add a new alarm.

### Alarm
The app will keep beeping in an async loop until stopped by the user.

![Alarm message](/RaspAlarm/Screenshots/Beep.png)

### Snooze
Click "Yes" when a message appears, to stop the alarm and set a new one for 5 minutes.

### Time formatting
The application will use the time format defined by your system's culture.
