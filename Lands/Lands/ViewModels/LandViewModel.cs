namespace Lands.ViewModels
{
    using Lands.Models;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class LandViewModel : BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Border> borders;
        private ObservableCollection<Currency> currency;
        private ObservableCollection<Language> languages;
        #endregion


        #region Properties
        public Land Land
        {
            get;
            set;
        }

        public ObservableCollection<Border> Borders
        {
            get { return this.borders; }
            set { this.SetValue(ref this.borders, value); }
        }

        public ObservableCollection<Currency> Currency
        {
            get { return this.currency; }
            set { this.SetValue(ref this.currency, value); }
        }

        public ObservableCollection<Language> Languages
        {
            get { return this.languages; }
            set { this.SetValue(ref this.languages, value); }
        }
        #endregion


        #region Constructors
        public LandViewModel(Land land)
        {
            this.Land = land;
            this.LoadBorders();
            this.Currency = new ObservableCollection<Currency>(this.Land.Currencies);
            this.Languages = new ObservableCollection<Language>(this.Land.Languages);
        }
        #endregion


        #region Methods
        private void LoadBorders()
        {
            this.Borders = new ObservableCollection<Border>();
            foreach (var border in this.Land.Borders)
            {
                var land = MainViewModel.GetInstance().LandsList.
                                         Where(l => l.Alpha3Code == border).
                                         FirstOrDefault();
                if (land != null)
                {
                    this.Borders.Add(new Border
                    {
                        Code = land.Alpha3Code,
                        Name = land.Name,
                    });
                }
            }
        } 
        #endregion
    }
}
