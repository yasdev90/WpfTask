using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var products = new List<ProductDetails>
            {
                new ProductDetails{Name="apple", Code="v001", Unit="KG", Price=1.2 , ImagePath="https://mangomart-autocount.myboostorder.com/wp-content/plugins/woocommerce/assets/images/placeholder.png"},
                new ProductDetails{Name="grape", Code="v003", Unit="KG", Price=9.9},
                new ProductDetails{Name="orange", Code="v002", Unit="KG", Price=1.4},
            };
            repeater.ItemsSource = products;
           TryApi();
        }

        public async Task TryApi()
        {
            using (var httpClient = new HttpClient())
            {
                var apiUrl = "https://mangomart-autocount.myboostorder.com/wp-json/wc/v1/products";
                
                var username = "ck_2682b35c4d9a8b6b6effac126ac552e0bfb315a0";
                var password = "cs_cab8c9a729dfb49c50ce801a9ea41b577c00ad71";
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}")));
                var products = await httpClient.GetAsync(apiUrl);
                var code = products.StatusCode;
                if (code == System.Net.HttpStatusCode.OK)
                {
                    var x = await products.Content.ReadAsStringAsync();
                    MessageBox.Show("all ok");
                }
                else
                {
                    MessageBox.Show("not ok");
                }
            }
        }
    }
}
