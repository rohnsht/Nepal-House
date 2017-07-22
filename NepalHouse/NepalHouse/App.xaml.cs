using NepalHouse.Models;
using NepalHouse.Persistence;
using NepalHouse.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;

namespace NepalHouse
{
    public partial class App : Application
    {
        public static WCObject wc { get; set; }

        public App()
        {
            InitializeComponent();

            MyRestAPI rest = new MyRestAPI(Constants.baseUrl, Constants.ConsumerKey, Constants.ConsumerSecret);
            wc = new WCObject(rest);
            MainPage = new NepalHouse.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
