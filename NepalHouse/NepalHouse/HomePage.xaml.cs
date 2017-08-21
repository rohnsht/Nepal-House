using NepalHouse.Models;
using NepalHouse.Persistence;
using NepalHouse.Utils;
using NepalHouse.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private SQLiteAsyncConnection DbConnection;

        private ObservableCollection<Product> productList { get; set; }
        private int page = 1;
        private bool isLoading = false;

        public HomePage()
        {
            InitializeComponent();
            DbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            productList = new ObservableCollection<Product>();

            GetProducts();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing(); 
            await DbConnection.CreateTableAsync<Cart>();
        }

        private async void GetProducts()
        {
            isLoading = true;
            try
            {
                var products = await App.wc.Product.GetAll();
                productList = new ObservableCollection<Product>(products);
                productView.ItemsSource = productList;
                isLoading = false;
                if (productView.IsRefreshing) {
                    productView.EndRefresh();
                }
            }
            catch (Exception ex) {
                isLoading = false;
                if (productView.IsRefreshing)
                {
                    productView.EndRefresh();
                }
                Debug.WriteLine("ERROR: {0}", ex.Message);
                await DisplayAlert("Error", "Connection problem. Pull to refresh again later.", "OK");
            }
            
        }

        private void productView_Refreshing(object sender, EventArgs e)
        {
            if (!isLoading)
                GetProducts();
            else
                productView.EndRefresh();
        }

        async void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isLoading)
                return;

            if (productList != null && e.Item == productList[productList.Count - 1]) {
                isLoading = true;
                ++page;
                try
                {
                    var products = await App.wc.Product.GetAll(new Dictionary<string, string>() {
                        { "page", String.Format("{0}", page) }
                    });
                    foreach (Product p in products)
                    {
                        productList.Add(p);
                    }
                    isLoading = false;
                }
                catch (Exception ex) {
                    --page;
                    isLoading = false;
                    Debug.WriteLine("ERROR: {0}", ex.Message);
                    await DisplayAlert("Error", "Connection problem. Pull to refresh again later.", "OK");
                }
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

            if (!(bool)product.in_stock) {
                await DisplayAlert("Sorry", "The product is out of stock.", "OK");
                return;
            }
        
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

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}