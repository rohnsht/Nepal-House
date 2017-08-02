using NepalHouse.Models;
using NepalHouse.Persistence;
using NepalHouse.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckoutPage : ContentPage
    {
        private SQLiteAsyncConnection DbConnection;
        public CheckoutPage(Order order)
        {
            if (order == null)
                throw new ArgumentNullException();
            InitializeComponent();
            DbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            BindingContext = order;

            string products = null;
            double? subtotal = 0.0;
            foreach (OrderLineItem item in order.line_items) {
                products += item.name + " x" + item.quantity + "\n";
                subtotal += item.quantity * (double?)item.price;
            }
            product_lbl.Text = products;
            subtotal_lbl.Text = String.Format("$ {0}",subtotal);

            billing_name_lbl.Text = String.Format("{0} {1}", order.billing.first_name, order.billing.last_name);
            billing_address_lbl.Text = String.Format("{0} {1} {2}", order.billing.city,
                order.billing.state, order.billing.postcode);

            shipping_name_lbl.Text = String.Format("{0} {1}", order.shipping.first_name, order.shipping.last_name);
            shipping_address_lbl.Text = String.Format("{0} {1} {2}", order.shipping.city,
                order.shipping.state, order.shipping.postcode);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            emptyCarts();
        }

        private async void emptyCarts()
        {
            await DbConnection.ExecuteAsync("delete from Cart");
            MessagingCenter.Send(new Cart(), "BadgeUpdate");
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}