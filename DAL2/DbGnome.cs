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

        public PrimeNumber RetrieveNthPrime(long userNth)
        {
            PrimeNumber nth = null;

            using (PrimeNumberContext cont = new PrimeNumberContext())
            {
                nth = cont.PrimeNumbers
                    .Where(p => p.NTH == userNth)
                    .Select(p => p)
                    .FirstOrDefault();
            }

            return nth;
        }

        /// <summary>
        /// ritorna una lista di numeri primi
        /// </summary>
        /// <param name="lowerBound">exp</param>
        /// <param name="upperBound">exp</param>
        /// <returns></returns>
        public List<PrimeNumber> ListOfPrimeMod(long lowerBound, long upperBound, out string msg)
        {
            //check per popolazione DB
            long max = RetrieveLastPrimeCalc().IDN;
            List<PrimeNumber> exit = new List<PrimeNumber>();


            if (upperBound > max)
            {
                msg = $"This number is too high. Try with n < { max}!";
                return exit;

            }
            else if (lowerBound < 2)
            {
                msg = "No Prime Number < 2 for definition.";
                return exit;

            }
            else
            {
                using (var context = new PrimeNumberContext())
                {
                    exit = context.PrimeNumbers
                        .Where(p => p.IDN >= lowerBound && p.IDN <= upperBound)
                        .Select(p => p)
                        .ToList();
                }
            }
            msg = null;
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
