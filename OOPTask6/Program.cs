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
                string userInput = Console.ReadLine();

                switch (userInput)
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
        protected Random Random = new Random();
        protected Dictionary<Product, int> Rroducts;

        public void ShowProducts()
        {
            foreach (var product in Rroducts)
            {
                product.Key.ShowInfo();
                Console.Write($" Количество товара - {product.Value}\n");
            }
            Console.WriteLine($"Денег: {Money}");
        }

        public Product GetProduct(int articleNumber)
        {
            foreach (var product in Rroducts)
            {
                if (product.Key.ArticleNumber == articleNumber)
                {
                    return product.Key;
                }
            }

            Console.WriteLine("Товар не найден");
            return null;
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

        protected int GetNuber()
        {
            int number;
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out number) == false)
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
            Rroducts = new Dictionary<Product, int>();
            Rroducts.Add(new Product("Хлеб", Random.Next(naxPrice), ++_nextArticleNumber), Random.Next(maxProductCount));
            Rroducts.Add(new Product("Колбаса", Random.Next(naxPrice), ++_nextArticleNumber), Random.Next(maxProductCount));
            Rroducts.Add(new Product("Молоко", Random.Next(naxPrice), ++_nextArticleNumber), Random.Next(maxProductCount));
            Rroducts.Add(new Product("Сыр", Random.Next(naxPrice), ++_nextArticleNumber), Random.Next(maxProductCount));
            Rroducts.Add(new Product("Масло", Random.Next(naxPrice), ++_nextArticleNumber), Random.Next(maxProductCount));
        }

        public bool IsCanSell(Product product, int number)
        {
            return Rroducts[product] >= number;
        }

        public void Sell(Product product, int number)
        {
            Rroducts[product] -= number;
            AddMoney(product.Price * number);
        }
    }

    class Player : Inventory
    {   
        public Player()
        {
            Rroducts = new Dictionary<Product, int>();
            int maxMoney = 10000;
            SetMoney(Random.Next(maxMoney));
        }

        public bool IsCanBuy(Product product, int number)
        {
            return Money >= product.Price * number;
        }

        public void Buy(Product product, int number)
        {
            if (Rroducts.ContainsKey(product))
            {
                Rroducts[product] += number;
            }
            else
            {
                Rroducts.Add(product, number);
            }

            SubtractMoney(product.Price * number);
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
                seller.Sell(product, number);
                player.Buy(product, number);
                Console.WriteLine("Сделка совершена");
            } 
            else
            {
                Console.WriteLine("Не возможно провести сделку");
            }
        }

    }
}
