using NepalHouse.Models;
using NepalHouse.Persistence;
using NepalHouse.Utils;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        private SQLiteAsyncConnection DbConnection;

        Product product;
        public ProductDetailPage(Product product)
        {
            if (product == null)
                throw new ArgumentNullException();
            InitializeComponent();
            this.product = product;
            BindingContext = product;

            DbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();

            if (product.rating_count == 0)
                ratingBar.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (product.rating_count > 0)
                GetReViews();
            else {
                reviewsView.IsVisible = false;
                emptyReviewsLabel.IsVisible = true;
            }
            GetRelatedProducts();
        }

        private async void GetReViews()
        {
            try
            {
                var reviews = await App.wc.Product.Reviews.GetAll(product.id);
                if (reviews.Count > 0)
                    reviewsView.ItemsSource = reviews;
                else
                {
                    reviewsView.IsVisible = false;
                    emptyReviewsLabel.IsVisible = true;
                }
            }
            catch (Exception ex) {
                Debug.WriteLine("ERROR {0}", ex.Message);
            }

        }

        private async void GetRelatedProducts()
        {
            try
            {
                var products = await App.wc.Product.GetAll(new Dictionary<string, string>() {
                    { "category", String.Format("{0}",  product.categories[0].id) },
                    { "per_page", "3" },
                    { "exclude", String.Format("{0}",  product.id) }
                });
                productView.ItemsSource = products;
            }
            catch (Exception ex) {
                Debug.WriteLine("ERROR {0}", ex.Message);
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

        private async void Add_Cart_Clicked(object sender, EventArgs e)
        {
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

            await DisplayAlert("Success", "Successfully added to the cart", "OK");
        }
    }

}