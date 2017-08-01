using NepalHouse.Models;
using NepalHouse.Persistence;
using NepalHouse.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NepalHouse
{
    public partial class MainPage : TabbedPage
    {
        private SQLiteAsyncConnection DbConnection;

        public MainPage()
        {
            InitializeComponent();
            DbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            BindingContext = new MainViewModel(DbConnection);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await DbConnection.CreateTableAsync<Cart>();
            displayBadge();
        }

        public void displayBadge() {
            (BindingContext as MainViewModel).getCartCount(null);
        }
    }
}
