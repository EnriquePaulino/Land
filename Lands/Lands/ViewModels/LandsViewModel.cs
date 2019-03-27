namespace Lands.ViewModels
{
    using Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;

    public class LandsViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Land> lands;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return this.lands; }
            set { SetValue(ref this.lands, value); }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            this.apiService = new ApiService();
            this.LoadLands();
        }
        #endregion

        #region Methods
        private async void LoadLands()
        {
            //var response = await this.apiService.GetList<Land>(
            //    "http://restcountries.eu",
            //    "/rest",
            //    "/v2/all");

            //if (!response.IsSuccess)
            //{
            //    //this.IsRefreshing = false;
            //    await Application.Current.MainPage.DisplayAlert(
            //        "Error",
            //        response.Message,
            //        "Accept");
            //    return;
            //}

            //var list = (List<Land>)response.Result;
            //this.Lands = new ObservableCollection<Land>(list);

            //this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                //this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var response = await this.apiService.GetList<Land>(
                "http://restcountries.eu",
                "/rest",
                "/v2/all");

            if (!response.IsSuccess)
            {
                //this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopAsync();
                return;
            }

            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);

            //MainViewModel.GetInstance().LandsList = (List<Land>)response.Result;
            //this.Lands = new ObservableCollection<LandItemViewModel>(
            //    this.ToLandItemViewModel());
            //this.IsRefreshing = false;
        }
        #endregion
    }
}
