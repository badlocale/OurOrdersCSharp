#region usings

using OurOrdersRunner;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace OurOrders
{
    internal class Program
    {
        private static void Main()
        {
            SolutionChecker.Check(new Processor());
            Console.ReadKey();
        }

        /// <summary>
        ///     Реализация алгоритма задачи "Свои заявки"
        /// </summary>
        internal sealed class Processor : IOrderLogProcessor
        {
            private readonly IList<OrderLogEntry> _entries = new List<OrderLogEntry>();

            public void Update(OrderLogEntry orderLogEntry)
            {
                if (orderLogEntry.Deleted || orderLogEntry.Volume is 0)
                {
                    return;
                }

                var oldEntry = _entries.FirstOrDefault(o => o.Id == orderLogEntry.Id);
                if (oldEntry != null)
                {
                    _entries.Remove(oldEntry);
                }

                _entries.Add(orderLogEntry);
            }            

            public IEnumerable<Tuple<decimal, long>> GetLevels() => 
                _entries
                    .GroupBy(o => o.Price, o => o.Volume)
                    .Select(o => new Tuple<decimal, long>(o.Key, o.Sum()));
        }
    }
}