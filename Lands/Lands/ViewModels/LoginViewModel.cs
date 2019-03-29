namespace Lands.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Lands.Services;
    using Views;
    using Xamarin.Forms;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion


        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        #endregion


        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();

            this.IsRemembered = true;
            this.IsEnabled = true;

            this.Email = "paulinoenrique@gmail.com";
            this.Password = "123456";
        }
        #endregion


        #region Commands
        public System.Windows.Input.ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a email.",
                    "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter a password.",
                    "Accept");
                return;
            }

            //this.IsRemembered = true;
            //this.IsEnabled = false;

            //if (this.Email != "paulinoenrique@gmail.com" || this.Password != "123456")
            //{
            //    this.IsRemembered = false;
            //    this.IsEnabled = true;

            //    await Application.Current.MainPage.DisplayAlert(
            //        "Error",
            //        "Email or password incorrect.",
            //        "Accept");
            //    this.Password = string.Empty;
            //    return;
            //}

            this.IsRemembered = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRemembered = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                      "Error",
                      connection.Message,
                      "Accept");
                this.Password = string.Empty;
                return;
            }

            var token = await this.apiService.GetToken(
                "http://www.landsapi.somee.com/",
                this.Email,
                this.Password);

            if (token == null)
            {
                this.IsRemembered = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Something was wroun, please try later.",
                    "Accept");
                return;
            }

            if (string .IsNullOrEmpty(token.AccessToken))
            {
                this.IsRemembered = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    token.ErrorDescription,
                    "Accept");
                return;
            }

            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.Token = token;
            mainViewModel.Lands = new LandsViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());

            this.IsRemembered = false;
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;
        }
        #endregion
    }
}
