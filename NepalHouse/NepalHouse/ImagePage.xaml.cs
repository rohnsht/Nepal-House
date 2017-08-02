using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NepalHouse
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImagePage : ContentPage
    {
        public ImagePage(String imageUrl)
        {
            InitializeComponent();

            image_view.Source = ImageSource.FromUri(new Uri(imageUrl));
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void Image_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            image_view.ScaleTo(e.Scale);
        }
    }
}