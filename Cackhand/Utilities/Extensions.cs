using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cackhand.Utilities
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection,  Action<T> operation)
        {
            foreach (var item in collection)
            {
                operation(item);
            }
        }
    }
}
