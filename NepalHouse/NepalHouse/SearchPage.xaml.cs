using NepalHouse.Models;
using NepalHouse.Persistence;
using NepalHouse.Utils;
using NepalHouse.ViewModels;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Runtime.CompilerServices;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        private SQLiteAsyncConnection DbConnection;
        private ObservableCollection<Product> productList { get; set; }

        public SearchPage()
        {
            InitializeComponent();
            DbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
   
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string searchQuery = searchBar.Text;
            if (String.IsNullOrWhiteSpace(searchQuery))
                return;

            activityIndicator.IsVisible = true;
            message_lbl.IsVisible = false;
            try
            {
                var products = await App.wc.Product.GetAll(new Dictionary<string, string>() {
                    { "search", searchQuery },
                    { "per_page", "15" }
                });
                productList = new ObservableCollection<Product>(products);
                productView.ItemsSource = productList;
                if (products.Count() <= 0)
                    message_lbl.IsVisible = true;
                activityIndicator.IsVisible = false;
            }
            catch (Exception ex) {
                activityIndicator.IsVisible = false; 
            }     
        }

        async void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }
            var product = e.SelectedItem as Product;
            await Navigation.PushAsync(new ProductDetailPage(product));
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        private async void MenuItem_Add(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var product = menuItem.CommandParameter as Product;

            var cart = Util.productToCart(product);

            var existingCart = await DbConnection.Table<Cart>()
                .Where(c => c.id == cart.id)
                .FirstOrDefaultAsync();

            if (existingCart != null)
            {
                ++existingCart.count;
                await DbConnection.UpdateAsync(existingCart);
            }
            else
            {
                cart.count = 1;
                await DbConnection.InsertAsync(cart);
            }

            MessagingCenter.Send(cart, "BadgeUpdate");
            await DisplayAlert("Success", "Successfully added to the cart", "OK");
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            String searchQuery = e.NewTextValue;
            if (String.IsNullOrWhiteSpace(searchQuery))
            {
                if(productList != null)
                    productList.Clear();
            }
        }
    }
}