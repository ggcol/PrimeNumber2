using PrimeNumber2;
using PrimeNumber2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class DbGnome
    {

        public async Task PersistPrimeAsync(List<PrimeNumber> list)
        {
            PrimeNumberContext c = new PrimeNumberContext();
            await c.BulkInsertAsync(list);
            await c.BulkSaveChangesAsync();
        }

        public PrimeNumber RetrieveLastPrimeCalc()
        {

            PrimeNumber lastRetrieved;

            using (PrimeNumberContext cont = new PrimeNumberContext())
            {
                lastRetrieved = cont.PrimeNumbers
                    .OrderBy(p => p.IDN)
                    .Select(p => p)
                    .LastOrDefault();
            }


            return lastRetrieved;

        }

        public string RetrieveNthPrime(long userNth)
        {
            var nth = "";

            using (PrimeNumberContext cont = new PrimeNumberContext())
            {
                nth = cont.PrimeNumbers
                    .Where(p => p.NTH == userNth)
                    .Select(p => p.IDN.ToString())
                    .FirstOrDefault();
            }

            return nth;
        }

        /// <summary>
        /// ritorna una lista di numeri primi come stringhe
        /// </summary>
        /// <param name="lowerBound">exp</param>
        /// <param name="upperBound">exp</param>
        /// <returns></returns>
        public List<string> ListOfPrime(long lowerBound, long upperBound)
        {
            //check per popolazione DB
            long max = RetrieveLastPrimeCalc().IDN;
            List<string> exit = new List<string>();


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
                using (var context = new PrimeNumberContext())
                {
                    exit = context.PrimeNumbers
                        .Where(p => p.IDN >= lowerBound && p.IDN <= upperBound)
                        .Select(p => p.IDN.ToString())
                        .ToList();
                }

            }

            return exit;

        }

        public bool IsPrime(long l)
        {
            //corner case
            if (l < 2) return false;
            if (l == 2) return true;
            //even n
            if (l % 2 == 0) return false;

            //limit can be = sqrt(n) which is much lower than n-1
            long limit = (long)Math.Floor(Math.Sqrt(l));

            //odd n
            for (long count = 3; count <= limit; count += 2)
            {
                if (l % count == 0) return false;

            }
            return true;
        }
    }
}
