using NepalHouse.Models;
using NepalHouse.Persistence;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        private SQLiteAsyncConnection DbConnection;
        private ObservableCollection<Cart> cartList;

        public CartPage()
        {
            InitializeComponent();
            DbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await DbConnection.CreateTableAsync<Cart>();

            var carts = await DbConnection.Table<Cart>().ToListAsync();
            cartList = new ObservableCollection<Cart>(carts);
            cartView.ItemsSource = cartList;
            if (cartList.Count() > 0)
            {
                order_layout.IsVisible = true;
                message_lbl.IsVisible = false;
                calculateCost();
            }
            else
            {
                message_lbl.IsVisible = true;
                order_layout.IsVisible = false;
            }
        }

        private void calculateCost()
        {
            double total = 0.0;
            foreach (Cart cart in cartList)
            {
                total += cart.count * Convert.ToDouble(cart.price);
            }

            if (total < 100)
            {
                total_lbl.Text = String.Format("$ {0}", total + 10);
                shipping_lbl.Text = "Flat Rate 2-3 Days ($10)";
            }
            else
            {
                total_lbl.Text = String.Format("$ {0}", total);
                shipping_lbl.Text = "Free shipping";
            }
        }

        private async void Count_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var cart = btn.CommandParameter as Cart;

            var action = await DisplayActionSheet("Quantity", "Cancel", null, "1", "2", "3", "4", "5", "6", "7", "8");
            Debug.WriteLine("Action: " + action);

            if (action.Equals("Cancel"))
                return;
            cart.count = Convert.ToInt32(action);
            await DbConnection.UpdateAsync(cart);
            calculateCost();
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var cart = btn.CommandParameter as Cart;

            await DbConnection.DeleteAsync(cart);
            cartList.Remove(cart);
            if (cartList.Count() > 0)
                calculateCost();
            else
            {
                order_layout.IsVisible = false;
                message_lbl.IsVisible = true;
            }
        }

        private async void Checkout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrderPage(cartList));
        }

        private void cartView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }
    }
}