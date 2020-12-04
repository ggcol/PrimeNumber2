using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PrimeNumber2;

namespace PrimeNumber2.Models
{
    public class PrimeNumber
    {
        //[Key]
        public virtual long IDN { get; set; }
    
    public bool IsPrime(long u)
        {
            if (u >= 2 && u <= 7 && u / u == 1) return true;
            if (u > 2 && u % 2 == 0 || u % 3 == 0 || u % 5 == 0 || u % 7 == 0) return false;
            if (u > 2 && u / 1 == u && u / u == 1) return true;

            return false;
        }
    
    public List<string> ListOfPrime(long lowerBound, long upperBound)
        {
            long max = 10000;
            List<PrimeNumber> exit= new List<PrimeNumber>();
            List<string> stringexit = new List<string>();

            if (upperBound > max)
            {
                stringexit.Add("This number is too high.");
                //do nth
            } else if (lowerBound < 2)
            {
                stringexit.Add("No Prime Number < 2 for definition.");
                //do nth
            } else
            {
                using (var context = new NumbersContext())
                {
                    exit = context.PrimeNumbers
                        .Where(p => p.IDN >= lowerBound && p.IDN <= upperBound)
                        .ToList();
                }

                foreach (PrimeNumber pn in exit)
                {
                    stringexit.Add(pn.IDN.ToString());
                }
                return stringexit;

            }

            return stringexit;

        }

        public new string ToString()
        {
            return this.IDN.ToString();
        }

    
    }

}
