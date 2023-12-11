using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        List<object> testList = new List<object>
        {
            1, 2, 3, new List<object> { 4, 5 }, 6, new List<object> { 7, 8, new List<object> { 9, 10 } }
        };

        int result = Sum(testList);
        Console.WriteLine($"La somme est : {result}");
    }

    public static int Sum(IEnumerable<object> values)
    {
        var sum = 0;
        foreach (var item in values)
        {
            switch (item)
            {
                case 0:
                    break;
                case int val:
                    sum += val;
                    break;
                case IEnumerable<object> subList when subList.Any():
                    sum += Sum(subList.Cast<object>());
                    break;
            }
        }
        return sum;
    }
}
