using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NepalHouse.Utils
{
    class Constants
    {
        public const int MaxBufferSize = 256000;

        public const string ConsumerKey = "ck_f0b2862a465dc89b08a93c4326a584d3ba8bfcd9";
        public const string ConsumerSecret = "cs_67204c1ec006530796f5a49c987086979718db3c";

        public const string baseUrl = "http://www.mynepalhouse.com/wp-json/wc/v2/";
        public const string productsUrl = baseUrl + "products";
        public const string categoriesUrl = baseUrl + "products/categories";
        public const string reviewsUrl = baseUrl + "products/{0}/reviews";
        public const string orderUrl = baseUrl + "orders";
        public const string paymentGatewaysUrl = baseUrl + "payment_gateways";
    }
}
