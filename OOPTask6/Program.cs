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
            

            while (isRun)
            {            
                Console.WriteLine("Доступные команды:\n1-Посмотреть товары продавца\n2-Посмотреть свои товары\n3-Купить товары\n4-Выйти");
                Console.Write("Введите команду: ");
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        seller.ShowInventory();
                        break;
                    case "2":
                        player.ShowInventory();
                        break;
                    case "3":
                        seller.Deal(player);
                        break;
                    case "4":
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
        public int Count { get { return _cells.Count; } } 

        public Inventory()
        {
            _cells = new List<Cell>();
        }

        public void AddInInventary(Product product, int count)
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

        public void ShowInventary()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                Console.WriteLine($"№ {i} - {_cells[i].Product.Title} , цена : {_cells[i].Product.Price} , кол-во : {_cells[i].Count} шт.");
            }
        }

        public void TakeProduct(Product product, int count)
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
            return _cells[productIndex].Count;
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
        public string Title { get; private set; }
        public int Price { get; private set; }

        public Product(string title, int price)
        {
            Title = title;
            Price = price;
        }

    }

    class Cell
    {
        public Product Product { get; private set; }
        public int Count { get; private set; }

        public Cell(Product product, int count)
        {
            Product = product;
            Count = count;
        }

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
        protected Inventory Inventory;
        protected Random Random = new Random();

        public void ShowInventory()
        {
            Console.WriteLine($"Баланс: {Money}\n");
            Inventory.ShowInventary();
        }

        protected int GetNuber()
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

    class Seller : Human
    {
        public Seller()
        {
            Money = 0;
            Inventory = new Inventory();
            int maxProductPrice = 1000;
            int minProductPrice = 50;
            int maxCountProduct = 10;
            int minCountProduct = 1;
            Inventory.AddInInventary(new Product("Колбаса", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.AddInInventary(new Product("Мясо", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.AddInInventary(new Product("Молоко", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.AddInInventary(new Product("Сыр", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
            Inventory.AddInInventary(new Product("Хлеб", Random.Next(minProductPrice, maxProductPrice)), Random.Next(minCountProduct, maxCountProduct));
        }

        public void Deal(Player player)
        {
            Console.WriteLine("Введите номер товара который хотите приобрести: ");
            int productNumber = GetNuber();

            if (productNumber >= 0 && productNumber < Inventory.Count)
            {
                Product product = Inventory.GetProduct(productNumber);
                Console.WriteLine("Введите количество товара: ");
                int count = GetNuber();

                if (count > 0)
                {
                    if (player.IsCanBuy(product, count) && isCanSell(productNumber, count))
                    { 
                        Inventory.TakeProduct(product, count);
                        player.Buy(product, count);
                        Money += product.Price * count;
                        Console.WriteLine("Сделка совершена");
                    }
                    else
                    {
                        Console.WriteLine("Не достаточно денег!/Нет такова количества товара!");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка, не верное количество товара!");
                }
            }
            else
            {
                Console.WriteLine("Ошибка, нет такова товара!");
            }
        }
        private bool isCanSell(int productIndex, int count)
        {
            return Inventory.GetProductCount(productIndex) >= count;
        }
    }

    class Player : Human
    {   
        public Player()
        {
            int maxMoney = 3000;
            int minMoney = 1000;
            Money = Random.Next(minMoney, maxMoney);
            Inventory = new Inventory();
        }

        public bool IsCanBuy(Product product, int count)
        {
            return Money >= product.Price * count;
        }

        public void Buy(Product product, int count)
        {
            Inventory.AddInInventary(product, count); 
            Money -= product.Price * count;
        }
    }

}
