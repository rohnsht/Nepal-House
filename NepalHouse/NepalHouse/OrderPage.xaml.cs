using NepalHouse.Models;
using NepalHouse.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        private List<OrderLineItem> lineItemsList { get; set; }
        private List<OrderShippingLine> shippingLinesList { get; set; }
        private List<PaymentGateway> getways { get; set; }
        private string products;
        private double subtotal = 0.0, total = 0.0;

        public OrderPage(ObservableCollection<Cart> cartList)
        {
            InitializeComponent();
            lineItemsList = new List<OrderLineItem>();
            shippingLinesList = new List<OrderShippingLine>();

            foreach (Cart cart in cartList) {
                products += cart.name + " x" + cart.count + "\n";
                subtotal += cart.count * Convert.ToDouble(cart.price);

                lineItemsList.Add(new OrderLineItem { product_id = cart.id, quantity = cart.count });
            }

            product_lbl.Text = products;
            subtotal_lbl.Text = String.Format("$ {0}", subtotal);
            if (subtotal < 100)
            {
                shipping_lbl.Text = "Flat Rate 2-3 Days: $10";
                total = subtotal + 10;

                shippingLinesList.Add(new OrderShippingLine { method_id = "flat_rate",
                    method_title = "Flat Rate 2-3 Days",
                    total = 10
                });
            }
            else {
                shipping_lbl.Text = "Free Shipping";

                shippingLinesList.Add(new OrderShippingLine
                {
                    method_id = "free",
                    method_title = "Free shipping",
                    total = 0
                });
                total = subtotal;
            }
            total_lbl.Text = String.Format("$ {0}", total);

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                getways = await App.wc.PaymentGateway.GetAll();
                foreach (PaymentGateway gateway in getways)
                {
                    if (!(bool)gateway.enabled)
                        continue;
                    payment_picker.Items.Add(gateway.title);
                    payment_picker.SelectedIndex = 0;
                }
            }
            catch (Exception ex) {
                Debug.WriteLine("ERROR {0}", ex.Message);
            }
             
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            activityIndicator.IsVisible = true;

            var firstname = first_entry.Text;
            var lastname = last_entry.Text;
            var companyname = company_entry.Text;
            var country = country_entry.Text;
            var address1 = address1_entry.Text;
            var address2 = address2_entry.Text;
            var city = suburb_entry.Text;
            string state = state_picker.Items[state_picker.SelectedIndex];
            var postcode = postcode_entry.Text;
            var phone = phone_entry.Text;
            var email = email_entry.Text;
            var note = note_entry.Text;

            if (String.IsNullOrWhiteSpace(firstname)
                || String.IsNullOrWhiteSpace(lastname)
                || String.IsNullOrWhiteSpace(lastname)
                || String.IsNullOrWhiteSpace(address1)
                || String.IsNullOrWhiteSpace(city)
                || String.IsNullOrWhiteSpace(postcode)
                || String.IsNullOrWhiteSpace(phone)
                || String.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Error", "Please input all the required fields.", "OK");
                activityIndicator.IsVisible = false;
                return;
            }
            content.IsVisible = false;
            if (getways.Count <= 0)
                return;
            var getway = getways[payment_picker.SelectedIndex];

            OrderBilling billing = new OrderBilling
            {
                first_name = firstname,
                last_name = lastname,
                company = companyname,
                address_1 = address1,
                address_2 = address2,
                city = city,
                state = state,
                postcode = postcode,
                phone = phone,
                email = email
            };

            OrderShipping shipping = new OrderShipping
            {
                first_name = firstname,
                last_name = lastname,
                company = companyname,
                address_1 = address1,
                address_2 = address2,
                city = city,
                state = state,
                postcode = postcode
            };


            Order order = new Order
            {
                payment_method = getway.id,
                payment_method_title = getway.method_title,
                set_paid = true,
                billing = billing,
                shipping = shipping,
                line_items = lineItemsList,
                shipping_lines = shippingLinesList,
                customer_note = note
            };
            try
            {
                var response = await App.wc.Order.Add(order);
          
                await Navigation.PushModalAsync(new CheckoutPage(response));
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERROR {0}", ex.Message);
                activityIndicator.IsVisible = false;
                content.IsVisible = true;
                await DisplayAlert("Error", "Could not place the order. Please check you input and try again.", "Ok");
            }
        }
    }
}