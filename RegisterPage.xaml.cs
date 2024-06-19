using Microsoft.Maui.Controls;
using Project_2.Services;
using Project_2.Models;

namespace Project_2
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            if (DatabaseService.Instance.IsUsernameExists(username))
            {
                await DisplayAlert("Error", "Username already exists. Please choose a different username.", "OK");
                return;
            }

            var user = new User { Username = username, Password = password };
            DatabaseService.Instance.SaveUser(user);

            await DisplayAlert("Success", "User registered successfully", "OK");
            await Navigation.PopAsync();
        }
    }
}
