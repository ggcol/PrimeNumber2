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
        public virtual long NTH { get; set; }

        /// <summary>
        /// check per primalità numero
        /// </summary>
        /// <param name="l">il numero di cui testare la primalità</param>
        /// <returns></returns>
        public bool IsPrime(long l)
        {
            if (l < 2) return false;

            for (long count = 2; count < l; count++)
            {
                if (l % count == 0) return false;

            }
            return true;
        }

        /// <summary>
        /// continua calcolo numeri primi da ultimo su db
        /// </summary>
        public void ContinuosCalc()
        {

            long flag = RetrieveLastPrimeCalc();
            long NTHflag = RetrieveLastNTHPrimeCalc();

            if (flag < 2) flag = 2;
            if (NTHflag <= 0) NTHflag = 1;

            NumbersContext c = new NumbersContext();

            for (flag = flag; flag <= long.MaxValue; flag++)
            {
                if (IsPrime(flag) == true)
                {
                    Number p = new Number();
                    p.IDN = flag;
                    p.NTH = NTHflag;
                    c.Add(p);
                    c.SaveChanges();
                    NTHflag++;
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
                    .OrderBy(p => p.IDN)
                    .Select(p => p.IDN)
                    .LastOrDefault();

            }

            return lastRetrieved;

        }

        public long RetrieveLastNTHPrimeCalc()
        {
            long lastRetrived;

            using (NumbersContext cont = new NumbersContext())
            {
                lastRetrived = cont.Numbers
                    .OrderBy(p => p.NTH)
                    .Select(p => p.NTH)
                    .LastOrDefault();
            }

            return lastRetrived;
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

        public string RetrieveNthPrime(long userNth)
        {
            var nth = "";

            using (NumbersContext cont = new NumbersContext())
            {
                nth = cont.Numbers
                    .Where(p => p.NTH == userNth)
                    .Select(p => p.IDN.ToString())
                    .FirstOrDefault();
            }

            return nth;
        }


    }

}
