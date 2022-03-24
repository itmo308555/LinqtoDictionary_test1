using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_ConsolidateStockUsingJoin();
            Test_ConsolidateStockUsingDictionary();
            Console.ReadLine();
        }
        public class Item
        {
            public string Sku { get; set; }
            public int Quantity { get; set; }
        }

        public static IEnumerable<Item> GetItemsStock()
        {
            for (var i = 1; i < 1000000; i++)
            {
                yield return new Item
                {
                    Sku = $"Sku-{i}",
                    Quantity = i
                };
            }
        }

        public static IEnumerable<Item> ConsolidateStockUsingJoin(List<Item> stock1, List<Item> stock2)
        {
            var consolidatedItems = from s1 in stock1
                                    join s2 in stock2
                                    on s1.Sku equals s2.Sku
                                    select new Item
                                    {
                                        Sku = s1.Sku,
                                        Quantity = s1.Quantity + s2.Quantity
                                    };

            return consolidatedItems;
        }

        public static IEnumerable<Item> ConsolidateStockUsingDictionary(List<Item> stock1, List<Item> stock2)
        {
            var stock2Dict = stock2.ToDictionary(x => x.Sku, x => x.Quantity);

            foreach (var item in stock1)
            {
                var quantityOfItemInStock2 = stock2Dict[item.Sku];
                yield return new Item
                {
                    Sku = item.Sku,
                    Quantity = item.Quantity + quantityOfItemInStock2
                };
            }
        }

        private static void Test_ConsolidateStockUsingJoin()
        {
            var stock1 = GetItemsStock().ToList();
            var stock2 = GetItemsStock().ToList();

            var watch = new Stopwatch();
            watch.Start();
            var items = ConsolidateStockUsingJoin(stock1, stock2);
            watch.Stop();

            Console.WriteLine($"Time taken to use Join - {watch.Elapsed.TotalMilliseconds} ms");

        }


        private static void Test_ConsolidateStockUsingDictionary()
        {
            var stock1 = GetItemsStock().ToList();
            var stock2 = GetItemsStock().ToList();

            var watch = new Stopwatch();
            watch.Start();
            var items = ConsolidateStockUsingDictionary(stock1, stock2);
            watch.Stop();

            Console.WriteLine($"Time taken to use Dictionary - {watch.Elapsed.TotalMilliseconds} ms");

        }

    }
}
