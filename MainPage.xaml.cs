using Microsoft.Maui.Controls;
using Project_2.Services;
using Project_2.Models;
using System;
using System.Collections.ObjectModel;

namespace Project_2
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<EventItem> events;
        private DateTime currentMonth;
        private DatabaseService databaseService;
        private User currentUser;

        public MainPage()
        {
            InitializeComponent();
            events = new ObservableCollection<EventItem>();
            eventsCollectionView.ItemsSource = events;
            currentMonth = DateTime.Now;
            databaseService = DatabaseService.Instance;

            // Assume you have a way to get the current user (e.g., from login)
            string username = Preferences.Get("LoggedInUser", string.Empty); // Retrieve logged-in user from preferences
            currentUser = databaseService.GetUser(username);

            if (currentUser != null)
            {
                events = new ObservableCollection<EventItem>(currentUser.Events);
                eventsCollectionView.ItemsSource = events;
            }

            BuildCalendar(currentMonth);
            UpdateMonthLabel(currentMonth); // Update month label
        }

        private async void OnDayClicked(object sender, EventArgs e)
        {
            DateTime selectedDate = (DateTime)((Button)sender).BindingContext;
            string result = await DisplayPromptAsync("New Event", $"Enter event details for {selectedDate:yyyy-MM-dd}:");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var eventItem = new EventItem { Date = selectedDate.ToString("yyyy-MM-dd"), Event = result, UserId = currentUser.Id };
                databaseService.SaveEvent(eventItem);
                events.Add(eventItem);
                await DisplayAlert("Event Created", $"Event: {result} on {selectedDate:yyyy-MM-dd}", "OK");
            }
        }

        private void BuildCalendar(DateTime month)
        {
            calendarGrid.Children.Clear();

            // Add header for days of the week
            var daysOfWeek = new[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                var label = new Label
                {
                    Text = daysOfWeek[i],
                    HorizontalOptions = LayoutOptions.Center
                };
                calendarGrid.Add(label, i, 0);
            }

            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            DateTime firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            int row = 1;
            int col = startDayOfWeek;

            for (int i = 1; i <= daysInMonth; i++)
            {
                Button dayButton = new Button
                {
                    Text = i.ToString(),
                    BindingContext = new DateTime(month.Year, month.Month, i)
                };
                dayButton.Clicked += OnDayClicked;

                calendarGrid.Add(dayButton, col, row);

                col++;
                if (col == 7)
                {
                    col = 0;
                    row++;
                }
            }

            UpdateMonthLabel(month); // Update month label after building calendar
        }

        private void UpdateMonthLabel(DateTime month)
        {
            monthLabel.Text = month.ToString("MMMM yyyy");
        }

        private void OnDisplayAllEventsClicked(object sender, EventArgs e)
        {
            if (eventsCollectionView.IsVisible)
            {
                eventsCollectionView.IsVisible = false;
                ((Button)sender).Text = "Display All Events";
            }
            else
            {
                eventsCollectionView.IsVisible = true;
                ((Button)sender).Text = "Hide Events";
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Clear user info from preferences
            Preferences.Remove("LoggedInUser");

            // Navigate back to login page
            await Navigation.PopToRootAsync();
        }
    }
}
