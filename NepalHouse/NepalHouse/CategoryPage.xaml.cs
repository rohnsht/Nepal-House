using NepalHouse.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        WCObject wc;
        private bool isLoading = false;

        public CategoryPage()
        {
            InitializeComponent();
            RestAPI rest = new RestAPI(Constants.baseUrl, Constants.ConsumerKey, Constants.ConsumerSecret);
            wc = new WCObject(rest);

            GetCategories();
        }

        private async void GetCategories()
        {
            if (isLoading)
                return;
            isLoading = true;
            try
            {
                var categories = await wc.Category.GetAll(new Dictionary<string, string>() {
                    { "per_page", "50"}
                });
                categoryView.ItemsSource = categories;
                isLoading = false;
                if (categoryView.IsRefreshing)
                    categoryView.EndRefresh();
                activityIndicator.IsVisible = false;
            }
            catch (Exception ex) {
                isLoading = false;
                if (categoryView.IsRefreshing)
                    categoryView.EndRefresh();
                Debug.WriteLine("ERROR {0}", ex.Message);
            }
        }

        private async void categoryView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            var category = e.SelectedItem as ProductCategory;
            await Navigation.PushAsync(new ProductsPage(category));
            ((ListView)sender).SelectedItem = null; //uncomment line if you want to disable the visual selection state.
        }

        private void categoryView_Refreshing(object sender, EventArgs e)
        {
            GetCategories();
        }
    }
}