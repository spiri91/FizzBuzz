using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


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
            var range = Enumerable.Range(1, 100);
            var conditions = GetConditionActions();

            var iterator = range.GetEnumerator();
            while (iterator.MoveNext())
            {
                string result = string.Empty;
                conditions.ToList().ForEach(c =>
                {

                    result += c.InvokeMember("GetResult", BindingFlags.InvokeMethod | BindingFlags.Public, null, c, new object[] { (object)iterator.Current }).ToString();
                });

                if (String.IsNullOrWhiteSpace(result)) result = iterator.Current.ToString();

                Console.WriteLine(result);
            }

            Console.ReadKey();
        }

        private static IEnumerable<Type> GetConditionActions()
        {
            var type = typeof(ICheckCondition);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            return types;
        }
    }

    public class CheckForFizz : ICheckCondition
    {
        public CheckForFizz() { }

        public string GetResult(object[] val)
        {
            if (val.Length == 0) return string.Empty;

            var _ = (int)val[0];

            if (_ % 3 == 0)
                return "Fizz";

            return string.Empty;
        }
    }

    public class CheckForBuzz : ICheckCondition
    {
        public CheckForBuzz() { }

        public string GetResult(object[] val)
        {
            if (val.Length == 0) return string.Empty;

            var _ = (int)val[0];

            if (_ % 5 == 0)
                return "Buzz";

            return string.Empty;
        }
    }

    public interface ICheckCondition
    {
        string GetResult(object[] val);
    }
}


