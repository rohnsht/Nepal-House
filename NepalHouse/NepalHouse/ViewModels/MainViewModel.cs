using NepalHouse.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NepalHouse.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private SQLiteAsyncConnection DbConnection;
        public event PropertyChangedEventHandler PropertyChanged;

        private int _count = 0;

        public MainViewModel(SQLiteAsyncConnection conn) {
            this.DbConnection = conn;

            MessagingCenter.Subscribe<Cart>(this, "BadgeUpdate", getCartCount);
        }


   
        public async void getCartCount(Cart cart) {
            _count = await DbConnection.ExecuteScalarAsync<int>("select count(*) from Cart");
            RaisePropertyChanged(nameof(Count));
        }

        public string Count => _count <= 0 ? string.Empty : _count.ToString();

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
