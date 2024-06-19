using Microsoft.Maui.Controls;
using Project_2.Services;
using Project_2.Models;

namespace Project_2
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            var user = DatabaseService.Instance.GetUser(username);

            if (user != null && user.Password == password)
            {
                // Save user info in preferences
                Preferences.Set("LoggedInUser", username);

                await Navigation.PushAsync(new MainPage());
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
