using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1
{
    public class Dimensions
    {
        public string length { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

    public class Image
    {
        public string src { get; set; }
        public bool? hash { get; set; }
        public string src_small { get; set; }
        public string src_medium { get; set; }
        public string src_large { get; set; }
    }

    public class MixedSkuVolumePricingGroup
    {
        public List<object> product_ids { get; set; }
        public List<object> product_attributes { get; set; }
    }

    public class CustomFields
    {
        public string sales_item { get; set; }
        public string internal_sales_item { get; set; }
        public string inventory_item { get; set; }
        public string to_hide_during_picking_and_packing { get; set; }
        public string source { get; set; }
        public string disallow_children_backorders { get; set; }
        public string customer_tiers { get; set; }
        public string barcode { get; set; }
        public string is_rack_barcode { get; set; }
        public string customers { get; set; }
        public string price_tags { get; set; }
    }

    public class CompositeProductDetails
    {
        public string per_item_pricing { get; set; }
        public List<object> components { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Collection
    {
        public string href { get; set; }
    }

    public class Links
    {
        public List<Self> self { get; set; }
        public List<Collection> collection { get; set; }
    }

    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date_modified { get; set; }
        public string type { get; set; }
        public string status { get; set; }
        public string catalog_visibility { get; set; }
        public string description { get; set; }
        public string sku { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }
        public string date_on_sale_from { get; set; }
        public string date_on_sale_to { get; set; }
        public string tax_class { get; set; }
        public bool manage_stock { get; set; }
        public object stock_quantity { get; set; }
        public bool in_stock { get; set; }
        public string backorders { get; set; }
        public bool backorders_allowed { get; set; }
        public bool backordered { get; set; }
        public string weight { get; set; }
        public Dimensions dimensions { get; set; }
        public string shipping_class { get; set; }
        public int shipping_class_id { get; set; }
        public List<object> cross_sell_ids { get; set; }
        public List<object> categories { get; set; }
        public List<object> tags { get; set; }
        public List<Image> images { get; set; }
        public List<object> attributes { get; set; }
        public List<object> default_attributes { get; set; }
        public List<object> variations { get; set; }
        public int menu_order { get; set; }
        public string composite_layout { get; set; }
        public List<object> composite_components { get; set; }
        public List<object> composite_scenarios { get; set; }
        public string bundle_layout { get; set; }
        public List<object> bundled_by { get; set; }
        public List<object> bundled_items { get; set; }
        public MixedSkuVolumePricingGroup mixed_sku_volume_pricing_group { get; set; }
        public CustomFields custom_fields { get; set; }
        public List<object> pricing_groups { get; set; }
        public CompositeProductDetails composite_product_details { get; set; }
        public object bundle_product_details { get; set; }
        public int group_of { get; set; }
        public object minimum_quantity { get; set; }
        public object maximum_quantity { get; set; }
        public string points_earned { get; set; }
        public string points_required { get; set; }
        public string maximum_points_discount { get; set; }
        public List<object> inventory { get; set; }
        public Links _links { get; set; }

        public ProductDetails GetProductDetails()
        {
            var price = 0d;
            double.TryParse(this.regular_price, out price);
            return new ProductDetails
            {
                Name = this.name,
                Id = this.id,
                ImagePath = this.images[0].src,
                Price = price,
                Description = this.description
            };
        }
    }

   
}
