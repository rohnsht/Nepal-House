using NepalHouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;

namespace NepalHouse.Utils
{
    public class Util
    {
        public static Cart productToCart(Product product) {
            Cart cart = new Cart
            {
                id = product.id,
                name = product.name,
                slug = product.slug,
                permalink = product.permalink,
                date_created = product.date_created,
                date_modified = product.date_modified,
                type = product.type,
                status = product.status,
                featured = product.featured,
                catalog_visibility = product.catalog_visibility,
                description = product.description,
                short_description = product.short_description,
                sku = product.sku,
                price = product.price,
                regular_price = product.regular_price,
                sale_price = product.sale_price,
                date_on_sale_from = product.date_on_sale_from,
                date_on_sale_to = product.date_on_sale_to,
                price_html = product.price_html,
                on_sale = product.on_sale,
                purchasable = product.purchasable,
                total_sales = product.total_sales,
                tax_status = product.tax_status,
                tax_class = product.tax_class,
                manage_stock = product.manage_stock,
                stock_quantity = product.stock_quantity,
                in_stock = product.in_stock,
                backorders = product.backorders,
                backordered = product.backordered,
                backorders_allowed = product.backorders_allowed,
                sold_individually = product.sold_individually,
                weight = product.weight,
                shipping_required = product.shipping_required,
                shipping_taxable = product.shipping_taxable,
                shipping_class = product.shipping_class,
                shipping_class_id = product.shipping_class_id,
                reviews_allowed = product.reviews_allowed,
                average_rating = product.average_rating,
                rating_count = product.rating_count,
                parent_id = product.parent_id,
                purchase_note = product.purchase_note,
                categories = product.categories[0].id,
                images = product.images[0].src,
                menu_order = product.menu_order
            };

            return cart;
        }
    }
}
