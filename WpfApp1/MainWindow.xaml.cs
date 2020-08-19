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
using System.Net;
using System.Net.NetworkInformation;
using WpfApp1.ViewModels;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string _localDataPath = "../../../data.json";
        List<ProductDetails> _products = new List<ProductDetails>();
        static List<CartItem> _cartItems = new List<CartItem>();
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
            if (CheckConnection())
            {
                using (var httpClient = new HttpClient())
                {
                    var apiUrl = "https://mangomart-autocount.myboostorder.com/wp-json/wc/v1/products";
                    var username = "ck_2682b35c4d9a8b6b6effac126ac552e0bfb315a0";
                    var password = "cs_cab8c9a729dfb49c50ce801a9ea41b577c00ad71";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{username}:{password}")));
                    var products = await httpClient.GetAsync(apiUrl);
                    if (products.StatusCode == HttpStatusCode.OK)
                    {
                        var response = await products.Content.ReadAsStringAsync();
                        WriteDataToLocalFile(response);
                        var jsonResult = JsonConvert.DeserializeObject<List<Product>>(response);
                       _products = new List<ProductDetails>();
                        jsonResult.ForEach(item => _products.Add(item.GetProductDetails()));
                        _products.ForEach(item =>
                        {
                            if (string.IsNullOrEmpty(item.Unit))
                            {
                                item.Unit = "Piece";
                            }
                        });
                        repeater.ItemsSource = _products;
                        MessageBox.Show("data refreshed");
                    }
                    else
                    {
                        ReadLocalData();
                    }
                }
            }
            else
            {
                ReadLocalData();
            }
            return new List<ProductDetails>();
        }

        private void decrease_Click(object sender, RoutedEventArgs e)
        {
            var quantity = 1;
            var txtQuantity = (sender as Control).FindName("quantity") as TextBox;
            if (int.TryParse(txtQuantity.Text, out quantity))
            {
                if (quantity > 1)
                {
                    quantity--;
                    txtQuantity.Text = quantity.ToString();
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
                if (!File.Exists(_localDataPath))
                {
                    using FileStream fileStream = File.Open(_localDataPath, FileMode.Append);
                }
                File.WriteAllText(_localDataPath, data);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnReload_Click(object sender, RoutedEventArgs e)
        {
            TryApi();
        }

        void ReadLocalData()
        {
            if (File.Exists(_localDataPath))
            {
                using (StreamReader r = new StreamReader(_localDataPath))
                {
                    var json = r.ReadToEnd();
                    var items = JsonConvert.DeserializeObject<List<Product>>(json);
                    var itemsDetails = new List<ProductDetails>();
                    items.ForEach(item => itemsDetails.Add(item.GetProductDetails()));
                    itemsDetails.ForEach(item =>
                    {
                        if (string.IsNullOrEmpty(item.Unit))
                        {
                            item.Unit = "Piece";
                        }
                    });
                    repeater.ItemsSource = itemsDetails;
                    MessageBox.Show("data read locally");
                }
            }
        }

        private bool CheckConnection()
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send("8.8.8.8", 5000);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var total = double.Parse(lblTotal.Content.ToString());
                var txtQuantity = (sender as Button).FindName("quantity") as TextBox;
                var quantity = int.Parse(txtQuantity.Text);
                var price = int.Parse(((sender as Button).FindName("price") as TextBlock).Text);
                lblTotal.Content = total + quantity * price;
                var addedItemId = int.Parse(((sender as Button).FindName("Id") as TextBlock).Text);
                var addedItem = _products.Find(item => item.Id == addedItemId);
                _cartItems.Add(new CartItem { Name = addedItem.Name, Price = addedItem.Price, Quantity = quantity});
                MessageBox.Show($"{addedItem.Name} added to cart");
                txtQuantity.Text = "1";
            }
            catch(Exception ex)
            {

            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (_cartItems.Count > 0) {
                var cartItems = string.Empty;
                var total = 0.0;
                _cartItems.ForEach(item =>
                    {
                        cartItems = $"{item.Name} (RM{item.Price}) x {item.Quantity}item(s) => RM{item.Price * item.Price} \n";
                        total += item.Price * item.Price;
                    });
                cartItems += $"Total = RM{total} ";
                MessageBox.Show(cartItems);
            }
            else
            {
                MessageBox.Show("No items in the cart");
            }
        }
    }
}
