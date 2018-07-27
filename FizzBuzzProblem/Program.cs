using System;
using System.Collections.Generic;
using System.Linq;


/*
 * Write a program that prints the numbers from 1 to 100. 
 * But for multiples of three print “Fizz” instead of the number and for the multiples of five print “Buzz”.
 * For numbers which are multiples of both three and five print “FizzBuzz”.
 */

namespace FizzBuzzProblem
{
    class Program
    {
        private static void Main()
        {
            var iterator = Enumerable.Range(1, 100).GetEnumerator();
            var conditions = GetConditionActions();
            var params_List = new object[1];

            while (iterator.MoveNext())
            {
                string result = string.Empty;

                conditions.ForEach(c =>
                {
                    params_List.SetValue(iterator.Current, 0);

                    result += c.GetMethod("GetResult").Invoke(Activator.CreateInstance(c, null), new object[] { params_List });
                });

                Console.WriteLine(string.IsNullOrWhiteSpace(result) ? iterator.Current.ToString() : result);
            }

            Console.ReadKey();
        }

        private static List<Type> GetConditionActions() =>
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
            .Where(p => typeof(ICheckCondition).IsAssignableFrom(p) && !p.IsInterface).ToList();
    }

    public interface ICheckCondition
    {
        string GetResult(object[] val);
    }

    public class CheckForFizz : ICheckCondition
    {
        public string GetResult(object[] val) => (val.Length != 0 && (int)val[0] % 3 == 0) ? "Fizz" : string.Empty;
    }

    public class CheckForBuzz : ICheckCondition
    {
        public string GetResult(object[] val) => (val.Length != 0 && (int)val[0] % 5 == 0) ? "Buzz" : string.Empty;
    }
}


