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
     
        public bool IsPrime2(long l)
        {
            if (l < 2) return false;

            for (long count = 2; count < l; count++)
            {
                if (l % count == 0) return false;

            }
            return true;
        }

        public void IsPrimeImpl()
        {
            NumbersContext c = new NumbersContext();
            for (long flag = 2; flag <= short.MaxValue; flag++)
            {
                if (IsPrime2(flag) == true)
                {
                    Number p = new Number();
                    p.IDN = flag;
                    c.SaveChanges();
                }
            }
        }

        //public async void Populate()
        //{
        //    Task t1 = new Task();
        //    await t1.IsPrimeImpl();


        //}

        public List<string> ListOfPrime(long lowerBound, long upperBound)
        {
            //check per popolazione DB
            long max = 735839;
            List<string> exit = new List<string>();
            List<string> stringexit = new List<string>();

            if (upperBound > max)
            {
                exit.Add("This number is too high.");

            }
            else if (lowerBound < 2)
            {
                exit.Add("No Prime Number < 2 for definition.");

            }
            else if (lowerBound == upperBound)
            {
                if (IsPrime2(lowerBound))
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
