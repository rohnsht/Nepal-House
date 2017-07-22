using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NepalHouse.Models
{
    public class Cart : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey]
        public int? id { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        public string permalink { get; set; }
        public DateTime? date_created { get; set; }
        public DateTime? date_modified { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public bool? featured { get; set; }
        public string catalog_visibility { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public string sku { get; set; }
        public decimal? price { get; set; }
        public decimal? regular_price { get; set; }
        public decimal? sale_price { get; set; }
        public DateTime? date_on_sale_from { get; set; }
        public DateTime? date_on_sale_to { get; set; }
        public string price_html { get; set; }
        public bool? on_sale { get; set; }
        public bool? purchasable { get; set; }
        public int? total_sales { get; set; }
        public string tax_status { get; set; }
        public string tax_class { get; set; }
        public bool? manage_stock { get; set; }
        public int? stock_quantity { get; set; }
        public bool? in_stock { get; set; }
        public string backorders { get; set; }
        public bool? backorders_allowed { get; set; }
        public bool? backordered { get; set; }
        public bool? sold_individually { get; set; }
        public string weight { get; set; }
        public bool? shipping_required { get; set; }
        public bool? shipping_taxable { get; set; }
        public string shipping_class { get; set; }
        public string shipping_class_id { get; set; }
        public bool? reviews_allowed { get; set; }
        public string average_rating { get; set; }
        public int? rating_count { get; set; }
        public int? parent_id { get; set; }
        public string purchase_note { get; set; }
        public int? categories { get; set; }
        public string images { get; set; }
        public int? menu_order { get; set; }

        private int _count;

        public int count {
            get { return _count; }
            set {
                if (_count == value)
                    return;
                _count = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
