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
            //check per popolazione DB
            long max = 735839;
            List<string> exit= new List<string>();
            List<string> stringexit = new List<string>();

            if (upperBound > max)
            {
                exit.Add("This number is too high.");
                
            } else if (lowerBound < 2)
            {
                exit.Add("No Prime Number < 2 for definition.");
                
            } else if (lowerBound == upperBound) {
                if (IsPrime(lowerBound)) 
                {
                    exit.Add($"Lucky shoot. {lowerBound} is a prime number!");
                } else
                {
                    exit.Add("You should enter a wider range");
                }

            } else
            {
                using (var context = new NumbersContext())
                {
                    exit = context.PrimeNumbers
                        .Where(p => p.IDN >= lowerBound && p.IDN <= upperBound)
                        .Select(p => p.IDN.ToString())
                        .ToList();
                }

            }

            return exit;

        }

    
    }

}
