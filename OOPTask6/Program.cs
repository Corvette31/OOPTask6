using System;
using System.Collections.Generic;

namespace OOPTask6
{
    internal class Program
    {
        private const string _commandViewSellarProducts = "1";
        private const string _commandViewPlayerProducts = "2";
        private const string _commanDdeal = "3";
        private const string _commandExit = "4";

        static void Main(string[] args)
        {
            bool isRun = true;
            Player player = new Player();
            Seller seller = new Seller();
            Shop shop = new Shop(seller, player);   

            while (isRun)
            {
                Console.WriteLine($"Доступные команды:\n{_commandViewSellarProducts} Посмотреть товары продавца\n{_commandViewPlayerProducts} Посмотреть свои товары\n{_commanDdeal} Купить товары\n{_commandExit} Выйти");
                Console.Write("Введите команду: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case _commandViewSellarProducts:
                        seller.ShowInventory();
                        break;
                    case _commandViewPlayerProducts:
                        player.ShowInventory();
                        break;
                    case _commanDdeal:
                        shop.Deal();
                        break;
                    case _commandExit:
                        isRun = false;
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

    class Inventory
    {
        private List<Cell> _cells;

        public Inventory()
        {
            _cells = new List<Cell>();
        }

        public int Count => _cells.Count;

        public void Add(Product product, int count)
        {
            int itemIndex = GetIndexProduct(product);

            if (itemIndex >= 0)
            {
                _cells[itemIndex].AddProductCount(count);
            }
            else
            {
                _cells.Add(new Cell(product, count));
            }
        }

        public void Show()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                Console.WriteLine($"№ {i} - {_cells[i].Product.Title} , цена : {_cells[i].Product.Price} , кол-во : {_cells[i].Count} шт.");
            }
        }

        public void RemoveProduct(Product product, int count)
        {
            int itemIndex = GetIndexProduct(product);

            if (itemIndex >= 0)
            {
                if (count > 0 && _cells[itemIndex].Count - count >= 0)
                {
                    _cells[itemIndex].SubtractProductCount(count);

                    if (_cells[itemIndex].Count == 0)
                    {
                        _cells.Remove(_cells[itemIndex]);
                    }
                }
                else
                {
                    Console.WriteLine("Не корректное значение! Ошибка выполнения опреции!");
                }
            }
            else
            {
                Console.WriteLine("Ошибка выполнения опреции!");
            }
        }
        public Product GetProduct(int productIndex)
        {
            return _cells[productIndex].Product;
        }
        public int GetProductCount(int productIndex)
        {
            if (_cells.Count > 0 && productIndex < _cells.Count && productIndex >= 0)
                return _cells[productIndex].Count;

            return -1;
        }
        private int GetIndexProduct(Product product)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i].Product.Equals(product))
                {
                    return i;
                }
            }

            return -1;
        }
    }

    class Product
    {
        public Product(string title, int price)
        {
            Title = title;
            Price = price;
        }

        public string Title { get; private set; }
        public int Price { get; private set; }
    }

    class Cell
    {
        public Cell(Product product, int count)
        {
            Product = product;
            Count = count;
        }

        public Product Product { get; private set; }
        public int Count { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine($"Товар {Product.Title} , Количество {Count}");
        }

        public void AddProductCount(int value)
        {
            Count += value;
        }

        public void SubtractProductCount(int value)
        {
            Count -= value;
        }
    }

    abstract class Human
    {
        protected int Money;
        protected Inventory Inventory = new Inventory();
        protected Random Random = new Random();

        public void ShowInventory()
        {
            Console.WriteLine($"Баланс: {Money}\n");
            Inventory.Show();
        }
    }

    class Seller : Human
    {
        public Seller()
        {
            Money = 0;
            int maxProductPrice = 1000;
            int minProductPrice = 50;
            int maxCountProduct = 10;
            int minCountProduct = 1;
            Inventory.Add(new Product("Колбаса", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.Add(new Product("Мясо", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.Add(new Product("Молоко", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.Add(new Product("Сыр", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.Add(new Product("Хлеб", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
        }

        public bool CanSell(int productIndex, int count)
        {
            return Inventory.GetProductCount(productIndex) >= count && count > 0;
        }

        public void Sell(int productIndex, int count)
        {
            Product product = Inventory.GetProduct(productIndex);
            Inventory.RemoveProduct(product, count);
            Money += product.Price * count;
        }

        public Product GetProduct(int productIndex)
        {
            return Inventory.GetProduct(productIndex);
        }
    }

    class Player : Human
    {
        public Player()
        {
            int maxMoney = 3000;
            int minMoney = 1000;
            Money = Random.Next(minMoney, maxMoney);
        }

        public bool CanBuy(int Price, int count)
        {
            return Money >= Price * count;
        }

        public void Buy(Product product, int count)
        {
            Inventory.Add(product, count);
            Money -= product.Price * count;
        }
    }

    class Shop
    {
        private Seller _seller;
        private Player _player;
        public Shop(Seller seller, Player player)
        {
            _seller = seller;
            _player = player;
        }

        public void Deal()
        {
            Console.WriteLine("Введите номер товара который хотите приобрести: ");
            int productNumber = GetNuber();
            Console.WriteLine("Введите количество товара: ");
            int count = GetNuber();       

            if (_seller.CanSell(productNumber, count))
            {
                Product product = _seller.GetProduct(productNumber);

                if (_player.CanBuy(product.Price, count))
                {
                    _seller.Sell(productNumber, count);
                    _player.Buy(product, count);
                    Console.WriteLine("Сделка совершена!");
                }
                else
                {
                    Console.WriteLine("Не достаточно денег!");
                }
            }
            else
            {
                Console.WriteLine("Невозможно совершить сделку!");
            }
        }

        private int GetNuber()
        {
            int number;
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out number) == false)
            {
                Console.WriteLine("Не корректное значение!");
                return -1;
            }

            return number;
        }
    }
}
