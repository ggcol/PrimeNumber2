using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PrimeNumber2;
using System.Threading.Tasks;

namespace PrimeNumber2.Models
{
    public class Number
    {
        public virtual long IDN { get; set; }
        public virtual Guid Position { get; set; }
     
        public bool IsPrime(long l)
        {
            if (l < 2) return false;

            for (long count = 2; count < l; count++)
            {
                if (l % count == 0) return false;

            }
            return true;
        }

        public void ContinuosCalc()
        {

            long flag = RetrieveLastPrimeCalc();

            if (flag < 2) flag = 2;
            
            NumbersContext c = new NumbersContext();

            for (flag = flag; flag <= long.MaxValue; flag++)
            {
                if (IsPrime(flag) == true)
                {
                    Number p = new Number();
                    p.IDN = flag;
                    c.Add(p);
                    c.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Recupera ultimo numero primo calcolato su DB
        /// </summary>
        /// <returns></returns>
        public long RetrieveLastPrimeCalc()
        {

            long lastRetrieved;

            using (NumbersContext cont = new NumbersContext())
            {
                lastRetrieved = cont.Numbers
                    .OrderByDescending(p => p.IDN)
                    .Select(p => p.IDN)
                    .LastOrDefault();
                    
            }

            return lastRetrieved;

        }

        
        public List<string> ListOfPrime(long lowerBound, long upperBound)
        {
            //check per popolazione DB
            long max = RetrieveLastPrimeCalc();
            List<string> exit = new List<string>();
            List<string> stringexit = new List<string>();

            if (upperBound > max)
            {
                exit.Add($"This number is too high. Try with n < {max}!");

            }
            else if (lowerBound < 2)
            {
                exit.Add("No Prime Number < 2 for definition.");

            }
            else if (lowerBound == upperBound)
            {
                if (IsPrime(lowerBound))
                {
                    exit.Add($"Lucky shoot. {lowerBound} is a prime number!");
                }
                else
                {
                    exit.Add("You should enter a wider range");
                }

            }
            else
            {
                using (var context = new NumbersContext())
                {
                    exit = context.Numbers
                        .Where(p => p.IDN >= lowerBound && p.IDN <= upperBound)
                        .Select(p => p.IDN.ToString())
                        .ToList();
                }

            }

            return exit;

        }


    }

}
