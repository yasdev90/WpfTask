using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Caching;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ProductDetails> _products = new List<ProductDetails>();
        public MainWindow()
        {
            InitializeComponent();
            header.Background = (Brush)new BrushConverter().ConvertFrom("#285faa");
            _products = new List<ProductDetails>
            {
                new ProductDetails{Name="apple", Description="v001", Unit="KG", Price=1.2 , ImagePath="https://myboostorder.com/wp-content/uploads/sites/446/2020/08/2020-08-05_18h18_01-99x180.png"},
                new ProductDetails{Name="grape", Description="v003", Unit="KG", Price=9.9, ImagePath="https://myboostorder.com/wp-content/uploads/sites/446/2020/08/2020-08-05_18h18_01-99x180.png"},
                new ProductDetails{Name="orange", Description="v002", Unit="KG", Price=1.4, ImagePath="https://myboostorder.com/wp-content/uploads/sites/446/2020/08/2020-08-05_18h18_01-99x180.png"},
            };
            repeater.ItemsSource = _products;
            TryApi();
        }

        public async Task<List<ProductDetails>> TryApi()
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
                    WriteDataToLocalFile(x);
                     var jsonResult = JsonConvert.DeserializeObject<List<Product>>(x);
                    var details = new List<ProductDetails>();
                    jsonResult.ForEach(item => details.Add(item.GetProductDetails()));
                    repeater.ItemsSource = details;
                    MessageBox.Show("data refreshed");
                }
                else
                {
                    MessageBox.Show("not ok");
                }
                return new List<ProductDetails>();
            }
        }

        private void decrease_Click(object sender, RoutedEventArgs e)
        {
            var quantity = 1;
            var txtQuantity = (sender as Control).FindName("quantity") as TextBox;
           if(int.TryParse(txtQuantity.Text, out quantity))
            {
                if (quantity > 1)
                {
                    quantity--;
                    txtQuantity.Text =  quantity.ToString();
                }
            }
        }

        private void increase_Click(object sender, RoutedEventArgs e)
        {
            var quantity = 1;
            var txtQuantity = (sender as Control).FindName("quantity") as TextBox;
            if (int.TryParse(txtQuantity.Text, out quantity))
            {
                    quantity++;
                    txtQuantity.Text = quantity.ToString();
            }
        }

        void WriteDataToLocalFile(string data)
        {
            try
            {
                var localDataPath = "../../../data.json";
                if (!File.Exists(localDataPath))
                {
                    using FileStream fileStream = File.Open(localDataPath, FileMode.Append);
                }
                File.WriteAllText(localDataPath, data);
            }
            catch(Exception ex)
            {
                
            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            TryApi();
        }
    }   
}
