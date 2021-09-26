using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace lab2Console
{
    class BooleanAlgebraCalculator
    {

        public Dictionary<string, int> binDict = new Dictionary<string, int>();
        private List<string> formulaRPN = new List<string>();
        private List<string> stackRPN = new List<string>();

        //This function updates value returned by key in binDict
        public void setValueByKey(string key, int newValue)
        {
            if (binDict.ContainsKey(key))
            {
                binDict.Remove(key);
                binDict.Add(key, newValue);
                Console.WriteLine("New values are:\n"+string.Join(" ", binDict.Keys)
                    +"\n" + string.Join(" ", binDict.Values)+"\n");
            }
        } 

        //This function replace variables' names by their values
        public void setValuesOfElementsInRPN()
        {
            for (int i = 0; i < binDict.Count; i++)
            {
                string key = binDict.ElementAt(i).Key;
                string s = string.Join(" ", stackRPN).Replace(key, binDict.ElementAt(i).Value.ToString());
                stackRPN = s.Split(' ').ToList();
            }
        }

        //This function updates values returned by correspondant keys in binDict
        public void setElementsInDictionary(string vals)
        {
            string[] values = vals.Trim().Split(' ');
            for (int i = 0; i < binDict.Count; i++)
            {
                string key = binDict.ElementAt(i).Key;
                binDict.Remove(key);
                if (!values[i].Equals("0") && !values[i].Equals("1"))
                  throw new ArgumentException();
                binDict.Add(key, Int32.Parse(values[i].Trim()));
            }
            
        }
    
        //This function makes RPN formula of given expr and adds names of variables to binDict with same value(-1)
        public void setDictionaryAndRPNByFormula(string expr)
        {
            
            //+ is conjunction, * is disjunction, ^ is negation
            string OPERATORS = "+*^";
            List<char> stack = expr.ToCharArray().ToList();
            List<string> stackOperations = new List<string>();
            expr.Substring(1, expr.Length - 2);
            for (int i = 0; i < stack.Count; i++)
            {
                if (stack[i].Equals('('))
                {
                    stackOperations.Add(stack[i].ToString());
                }
                else if (stack[i].Equals(')'))
                {
                    while (!stackOperations[stackOperations.Count - 1].Equals("("))
                    {
                        stackRPN.Add(stackOperations[stackOperations.Count - 1]);
                        stackOperations.RemoveAt(stackOperations.Count - 1);
                    }
                    stackOperations.RemoveAt(stackOperations.Count - 1);
                }
                else if (OPERATORS.Contains(stack[i]))
                {
                    while (stackOperations.Count != 0 &&
                        OPERATORS.Contains(stackOperations[stackOperations.Count - 1]) &&
                        getPriority(stack[i].ToString()) <= getPriority(stackOperations[stackOperations.Count - 1]))
                    {
                        stackRPN.Add(stackOperations[stackOperations.Count - 1]);
                        stackOperations.RemoveAt(stackOperations.Count - 1);
                    }
                    stackOperations.Add(stack[i].ToString());
                }
                else 
                {
                    stackRPN.Add(stack[i].ToString());
                    //Console.WriteLine("Enter value for the variable: "+stack[i]);
                    //val = Int32.Parse(Console.ReadLine());
                    binDict.Add(stack[i].ToString(), -1);
                }
            }
            stackOperations.Reverse();
            stackRPN.AddRange(stackOperations);
            formulaRPN = stackRPN;
        }

        //This function returns value calculated From RPN
        public string getResultFromRPN()
        {
            List<string> resultStack = new List<string>();
                for (int i = 0; i < stackRPN.Count; i++)
                {
                    int v1 = -1;
                    int v2 = -1;
                    switch (stackRPN[i])
                    {
                        case "+":
                            v1= Int32.Parse(resultStack[resultStack.Count-1]);
                            resultStack.RemoveAt(resultStack.Count - 1);
                            v2 = Int32.Parse(resultStack[resultStack.Count - 1]);
                            resultStack.RemoveAt(resultStack.Count - 1);
                            if (v1==v2)
                                if (v1==0)
                                    resultStack.Add("0");
                                else
                                    resultStack.Add("1");
                            else
                                resultStack.Add("1"); 

                            break;
                        case "*":
                            v1 = Int32.Parse(resultStack[resultStack.Count - 1]);
                            resultStack.RemoveAt(resultStack.Count - 1);
                            v2 = Int32.Parse(resultStack[resultStack.Count - 1]);
                            resultStack.RemoveAt(resultStack.Count - 1);
                            if (v1 == v2)
                                if (v1 == 0)
                                    resultStack.Add("0");
                                else
                                    resultStack.Add("1");
                            else
                                resultStack.Add("0");
                            break;
                        case "^":
                            //binDict.TryGetValue(resultStack[resultStack.Count - 1], out v1);
                            v1 = Int32.Parse(resultStack[resultStack.Count - 1]);
                            resultStack.RemoveAt(resultStack.Count - 1);
                            if (v1 == 0)
                                resultStack.Add("1");
                            else
                                resultStack.Add("0");
                            break;
                        default:
                            resultStack.Add(stackRPN[i]);
                            break;
                    }
                 }
            return resultStack[0];
        }

        private int getPriority(string s)
        {
            if (s.Equals("^"))
                return 3;
            if (s.Equals("*"))
                return 2;
            if (s.Equals("+"))
                return 1;
            if (s.Equals("(") || s.Equals(")"))
                return 0;
            return -1;
        }
        public Dictionary<string, string> getAllPossiblePairsOfValuesAndResults()
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            string binstring="";
            int count = (int)Math.Pow(2, binDict.Keys.Count);
            int value = 0;
            string[] vars = binDict.Keys.ToArray<string>();
            string formula= string.Join(" ", formulaRPN);
            Console.WriteLine("");
            for (int i = 0; i < count; i++)
            {
                formula = string.Join(" ", formulaRPN);
                binstring = "";
                value = i;
                do
                {
                    if (binstring.Equals(""))
                        binstring = (value % 2).ToString();
                    else
                        binstring = value % 2 + " " + binstring;
                    value /= 2;
                } while (value > 0);
                while (binstring.Length < vars.Length * 2 - 1)
                {
                    binstring = "0 " + binstring;
                }
                string[] values = binstring.Split(' ');
                for (int j = 0; j < vars.Length; j++)
                {
                    formula = formula.Replace(vars[j], values[j]);
                    
                }
                stackRPN = formula.Split(' ').ToList();
                if (!results.ContainsKey(binstring))
                    results.Add(binstring, getResultFromRPN());
            }
            stackRPN = formulaRPN;
            return results;
        }
    }
}
