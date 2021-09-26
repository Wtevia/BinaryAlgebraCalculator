using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2Console
{
    class Program
    {
        static void Main(string[] args)
        {
            restart:
            BooleanAlgebraCalculator calc = new BooleanAlgebraCalculator();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            try
            {
                //^((^x+^y)*^(t+^z))
                Console.WriteLine("Enter formula:");
                String formula = Console.ReadLine();
                calc.setDictionaryAndRPNByFormula(formula);
                Console.WriteLine("Enter values of variables separated by the space in the next order: \n" + string.Join(" ", calc.binDict.Keys));
                String values = Console.ReadLine();
                calc.setElementsInDictionary(values);
                Console.WriteLine("\nDo you want to change value of any variable? (Enter: yes/no)");
                string shouldChangeSmth = Console.ReadLine();
                while (!shouldChangeSmth.Contains("no"))
                {
                    Console.WriteLine("Enter name of variable and its new value separated by space: ");
                    string[] keyAndValue = Console.ReadLine().Split(' ');
                    calc.setValueByKey(keyAndValue[0], Int32.Parse(keyAndValue[1]));
                    Console.WriteLine("Do you want to change value of any variable? (Enter \"no\" to quit)");
                    shouldChangeSmth = Console.ReadLine();
                }
                calc.setValuesOfElementsInRPN();
                Console.WriteLine("\nResult: " + calc.getResultFromRPN());
                Dictionary<string, string> results = calc.getAllPossiblePairsOfValuesAndResults();
                Console.WriteLine("All possible values and results:");
                Console.WriteLine("\t" + string.Join(" ", calc.binDict.Keys) + "  Result");
                for (int i = 0; i < results.Count; i++)
                {
                    Console.WriteLine("\t" + results.ElementAt(i).Key + "  " + results.ElementAt(i).Value);
                }
                Console.WriteLine("Do you want to restart? (Enter \"yes\" to restart)");
                if (Console.ReadLine().Contains("yes"))
                    goto restart;
            } catch(Exception e)
            {
                Console.WriteLine("Exception! Try again from beginning!");
                Console.WriteLine(e.StackTrace);
                goto restart;
            }
        }
    }
}
