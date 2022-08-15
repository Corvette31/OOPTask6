using System;
using System.Collections.Generic;

namespace OOPTask6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isRun = true;
            Player player = new Player();
            Seller seller = new Seller();
            Transaction transaction = new Transaction();

            while (isRun)
            {            
                Console.WriteLine("Доступные команды:\n1-Посмотреть товары продавца\n2-Посмотреть свои товары\n3-Купить товары\n4-Выйти");
                Console.Write("Введите команду: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        seller.ShowProducts();
                        break;
                    case "2":
                        player.ShowProducts();
                        break;
                    case "3":
                        transaction.Deal(player, seller);
                        break;
                    default:
                        Console.WriteLine("Не известная команда!");
                        break;
                }

                Console.Write("\nНажмите любую клавишу для продолжения: ");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    abstract class Inventory
    {
        public float Money { get; private set; }
        protected Random _random = new Random();
        protected Dictionary<Product, int> _products;

        public void ShowProducts()
        {
            foreach (var product in _products)
            {
                product.Key.ShowInfo();
                Console.Write($" Количество товара - {product.Value}\n");
            }
            Console.WriteLine($"Денег: {Money}");
        }

        public Product GetProduct(int articleNumber)
        {
            foreach (var product in _products)
            {
                if (product.Key.ArticleNumber == articleNumber)
                {
                    return product.Key;
                }
            }

            Console.WriteLine("Товар не найден");
            return null;
        }
        public bool IsCanBuy(Product product, int number)
        {
            return Money >= product.Price * number;
        }

        public bool IsCanSell(Product product, int number)
        {
            return _products[product] >= number;
        }

        protected void SetMoney(float money)
        {
            Money = money;
        }

        protected void AddMoney(float money)
        {
            Money += money;
        }

        protected void SubtractMoney(float money)
        {
            Money -= money;
        }

        protected void Sell(Product product, int number)
        {
            _products[product] -= number;
            Money += product.Price * number;
        }

        protected void Buy(Product product, int number)
        {
            if (_products.ContainsKey(product))
            {
                _products[product] += number;
            } 
            else
            {
                _products.Add(product, number);
            }
            Money -= product.Price * number;
        }     

        protected int GetNuber()
        {
            int number;

            if (int.TryParse(Console.ReadLine(), out number) == false)
            {
                Console.WriteLine("Не корректное значение!");
            }

            return number;
        }   
    }

    class Product
    {
        public string Title { get; private set; }
        public float Price { get; private set; }
        public int ArticleNumber { get; private set; }

        public Product(string title, float price, int articleNumber)
        {
            Title = title;
            Price = price;
            ArticleNumber = articleNumber;
        }

        public void ShowInfo()
        {
            Console.Write($"Товар - {Title} , Цена - {Price}, Артикул - {ArticleNumber}");
        }
    }

    class Seller : Inventory
    {
        private int _nextArticleNumber;

        public Seller()
        {
            int maxProductCount = 10;
            int naxPrice = 1000;
            _nextArticleNumber = 0;
            _products = new Dictionary<Product, int>();
            _products.Add(new Product("Хлеб", _random.Next(naxPrice), ++_nextArticleNumber), _random.Next(maxProductCount));
            _products.Add(new Product("Колбаса", _random.Next(naxPrice), ++_nextArticleNumber), _random.Next(maxProductCount));
            _products.Add(new Product("Молоко", _random.Next(naxPrice), ++_nextArticleNumber), _random.Next(maxProductCount));
            _products.Add(new Product("Сыр", _random.Next(naxPrice), ++_nextArticleNumber), _random.Next(maxProductCount));
            _products.Add(new Product("Масло", _random.Next(naxPrice), ++_nextArticleNumber), _random.Next(maxProductCount));
        }

        public void SellProduct(Product product, int number)
        {
            Sell(product, number);
        }
    }

    class Player : Inventory
    {   
        public Player()
        {
            _products = new Dictionary<Product, int>();
            int maxMoney = 10000;
            SetMoney(_random.Next(maxMoney));
        }

        public void BuyProduct(Product product, int count)
        {
            Buy(product, count);
        }
    }

    class Transaction : Inventory
    {
        public void Deal(Player player, Seller seller)
        {
            Console.WriteLine("Введите номер артикула товара который хотите приобрести: ");
            var product = seller.GetProduct(GetNuber());
            Console.WriteLine("Введите количество товара: ");
            int number = GetNuber();

            if (player.IsCanBuy(product, number) && seller.IsCanSell(product, number))
            {
                seller.SellProduct(product, number);
                player.BuyProduct(product, number);
                Console.WriteLine("Сделка совершена");
            } 
            else
            {
                Console.WriteLine("Не возможно провести сделку");
            }
        }

    }
}
