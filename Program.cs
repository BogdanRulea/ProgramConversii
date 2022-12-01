
using System.Runtime.CompilerServices;

namespace Conversii
{
    internal class Program
    {
        private static void checkBaseFormat(int baseNumber, string number)
        {
            number = number.ToLower(); //make number case insensitive
            if (number.StartsWith('.'))
            {
                throw new Exception("Invalid number format in base " + baseNumber);

            }
            if (number.Count(f => f == '.') > 1)
            {
                throw new Exception("Invalid number format in base " + baseNumber);
            }
            if (number.EndsWith('.'))
            {
                throw new Exception("Invalid number format in base " + baseNumber);

            }
            if (baseNumber <= 10)
            {
                for (int i = 0; i<number.Length; i++)
                {
                    bool ok = false;
                    if (number[i]>='0' && int.Parse(number[i].ToString()) < baseNumber ||  number[i] == '.')
                    {
                        ok = true;
                    }

                    if(ok == false)
                    {
                        throw new Exception("Invalid number format in base " + baseNumber);
                    }
                }
            } else
            {
                for (int i = 0; i < number.Length; i++)
                {
                    bool ok = false;
                    if ((number[i] >= '0' && number[i] <= '9') 
                        || number[i] == '.' 
                        || (number[i] >= 'a' && int.Parse((number[i]-'a').ToString()) < baseNumber-10))
                    {
                        ok = true;
                    }

                    if (ok == false)
                    {
                        throw new Exception("Invalid number format in base " + baseNumber);
                    }
                }
            }
        }

        private static string formatPeriodicNumber(string number)
        {
            List<int> resturi = new List<int>();
            for(int i = 0;i < number.Length; i++)
            {
                if (!resturi.Contains(int.Parse(number[i].ToString())))
                {
                    resturi.Add(int.Parse(number[i].ToString()));
                } else
                {
                    break;
                }
            }
            
            return String.Join("", resturi);
        }
        private static string convertToBase10(int baseNumber, string number)
        {
            int numberConversion = 0;
            double fractPartConversion = 0.0;
            string numberInBase10 = "";
            if(baseNumber == 10)
            {
                return number;
            }
            else if(baseNumber < 10)
            {
                string[] baseArray = number.Split('.');

                string parteIntreaga = baseArray[0];
                int parteIntreagaLength = parteIntreaga.Length;
                for(int i = 0; i<parteIntreagaLength; i++)
                {
                    numberConversion += (int)((parteIntreaga[i] - '0') * Math.Pow(baseNumber, parteIntreagaLength - i - 1));
                }
                numberInBase10 = numberConversion.ToString();
                if (baseArray.Length == 2)
                {
                    string parteFract = baseArray[1];
                    int parteFractLength = parteFract.Length;
                    numberInBase10 += ".";
                    numberConversion = 0;
                    for (int i = 0; i < parteFractLength; i++)
                    {
                        fractPartConversion += (double)((parteFract[i] - '0') * Math.Pow(baseNumber,- i - 1));
                    }

                    numberInBase10 += formatPeriodicNumber(fractPartConversion.ToString().Split('.')[1]);
                }
            }
            else
            {
                string[] baseArray = number.ToLower().Split('.');

                string parteIntreaga = baseArray[0];
                int parteIntreagaLength = parteIntreaga.Length;
                for (int i = 0; i < parteIntreagaLength; i++)
                {
                    if (parteIntreaga[i] >= 'a' && parteIntreaga[i] <= 'f')
                    {
                        numberConversion += (int)((parteIntreaga[i] - 'a' + 10) * Math.Pow(baseNumber, parteIntreagaLength - i - 1));
                    }
                    else
                    {
                        numberConversion += (int)((parteIntreaga[i] - '0') * Math.Pow(baseNumber, parteIntreagaLength - i - 1));
                    }
                }
                numberInBase10 = numberConversion.ToString();
                if (baseArray.Length == 2)
                {
                    string parteFract = baseArray[1];
                    int parteFractLength = parteFract.Length;
                    numberInBase10 +=  ".";
                    for (int i = 0; i < parteFractLength; i++)
                    {
                        
                        if(parteFract[i] >= 'a' && parteFract[i] <= 'f')
                        {
                            fractPartConversion += (double)((parteFract[i] - 'a' + 10) * Math.Pow(baseNumber, -i - 1));
                        }
                        else
                        {
                            fractPartConversion += (double)((parteFract[i] - '0') * Math.Pow(baseNumber, -i - 1));
                        }
                    }
                    numberInBase10 += formatPeriodicNumber(fractPartConversion.ToString().Split('.')[1]);
                }
            }
            return numberInBase10;
        }

        private static string convertFromBase10ToAnyBase(int baseNumber, string number)
        {
            string[] numberSplit = number.Split('.');
            int parteIntreaga = 0;
            double parteFract = 0.0;
            string conversie = "";
            if (baseNumber == 10)
            {
                return number;
            }
            if (baseNumber < 10)
            {
                parteIntreaga = int.Parse(numberSplit[0]);
                string conversieParteInt = "";
                while (parteIntreaga > 0)
                {
                    conversieParteInt = parteIntreaga % baseNumber + conversieParteInt;
                    parteIntreaga /= baseNumber;
                }
                conversie += conversieParteInt == "" ? "0" : conversieParteInt ;
                if (numberSplit.Length == 2)
                {
                    conversie += '.';
                    parteFract = double.Parse("0." + numberSplit[1]);
                    string aux = "";
                    List<double> produse = new List<double>();
                    double produs = 0.0;
                    bool periodic = false;
                    while (parteFract != (int)parteFract)
                    {
                        produs = Math.Round(parteFract * baseNumber,6);
                        if (produse.Contains(produs - (int)produs))
                        {
                            periodic = true;
                            break;
                        }
                        else
                        {
                            produse.Add(produs - (int)produs);
                        }
                        aux += (int)produs;
                        string[] fractSplit = produs.ToString().Split('.');
                        parteFract = double.Parse("0." + (fractSplit.Length == 2 ? fractSplit[1] : "0"));
                    }

                    if (periodic == true)
                    {
                       int index = produse.IndexOf(produs - (int)produs);
                       aux = aux.Substring(0, index) + '(' + aux.Substring(index, aux.Length - index) + ')';
                    }

                    conversie += aux;
                }
            }
            else
            {
                parteIntreaga = int.Parse(numberSplit[0]);
                string conversieParteInt = "";
                while (parteIntreaga > 0)
                {
                    int aux = parteIntreaga % baseNumber;
                    conversieParteInt = (aux < 10 ? aux.ToString() : Char.ConvertFromUtf32('a' + aux - 10)) + conversieParteInt;
                    parteIntreaga /= baseNumber;
                }
                conversie += conversieParteInt == "" ? "0" : conversieParteInt;
                if (numberSplit.Length == 2)
                {
                    conversie += '.';
                    parteFract = double.Parse("0." + numberSplit[1]);
                    string aux = "";
                    List<double> produse = new List<double>();
                    double produs = 0.0;
                    bool periodic = false;
                    while (parteFract != (int)parteFract)
                    {
                        produs = Math.Round(parteFract * baseNumber,6);
                        if (produse.Contains(produs - (int)produs))
                        {
                            periodic = true;
                            break;
                        }
                        else
                        {
                            produse.Add(produs - (int)produs);
                        }
                        aux += produs < 10 ? produs.ToString().Split('.')[0] : Char.ConvertFromUtf32('a' + int.Parse((produs).ToString().Split('.')[0]) - 10);
                        string[] fractSplit = produs.ToString().Split('.');
                        parteFract = double.Parse("0." + (fractSplit.Length == 2? fractSplit[1] : "0"));
                    }

                    if (periodic == true)
                    {
                        int index = produse.IndexOf(produs - (int)produs);
                        aux = aux.Substring(0, index) + '(' + aux.Substring(index, aux.Length-index) + ')';
                    }

                    conversie += aux;
                }
            }
            return conversie;
        }

        private static void Main(string[] args)
        {
            
            bool flag = true;
            while (flag)
            {
                try
                {
                    Console.WriteLine("Scrie prima baza: ");
                    int base1 = int.Parse(Console.ReadLine());
                    Console.WriteLine("Scrie a doua baza: ");
                    int base2 = int.Parse(Console.ReadLine());
                    Console.WriteLine("Scrie numarul in baza {0}: ", base1);
                    string number = Console.ReadLine();

                    checkBaseFormat(base1, number);
                    Console.WriteLine("Conversia din baza {0} in baza {1} a numarului {2} este: {3}", base1, base2, number, convertFromBase10ToAnyBase(base2, convertToBase10(base1, number)));

                    string optiune = "";
                    Console.WriteLine("Incheie programul?(x)");
                    optiune = Console.ReadLine();

                    switch (optiune.Trim().ToLower())
                    {
                        case "x":
                            flag = false;
                            break;
                        default:
                            flag = true;
                            break;
                    }
              
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            
        }
    }
}
