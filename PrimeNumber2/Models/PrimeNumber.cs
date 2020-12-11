using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using PrimeNumber2;
using System.Threading.Tasks;
using System.Threading;


namespace PrimeNumber2.Models
{
    public class PrimeNumber
    {
        public virtual long IDN { get; set; }
        public virtual long NTH { get; set; }
        long flag;
        static Semaphore sem;


        private void InitProflag()
        {

            if (RetrieveLastPrimeCalc() == null)
            {
                this.flag = 2;
            }
            else
            {
                this.flag = RetrieveLastPrimeCalc().IDN;
            }
        }


        /// <summary>
        /// check per primalità numero
        /// </summary>
        /// <param name="l">il numero di cui testare la primalità</param>
        /// <returns></returns>
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

        /// <summary>
        /// continua calcolo numeri primi da ultimo su db
        /// </summary>
        public async void ContinuosCalc()
        {

            //inizializza proflag e segna punto da cui ripartire
            InitProflag();
            //il semaforo ha un posto "attivo" per un massimo di due totali
            sem = new Semaphore(1, 2);
            List<PrimeNumber> container = new List<PrimeNumber>();

            do
            {
                //3 thread
                Thread[] th = new Thread[3];

                //faccio partire tre thread
                for (int i = 0; i < th.Length; i++)
                {
                    //segment = porzione di numeri da analizzare
                    long segment = this.flag + 2000;

                    //inizializza thread per
                    th[i] = new Thread(new ThreadStart(() =>
                    {
                        //fa entrare un thread alla volta
                        sem.WaitOne();
                        //controlla numeri da proflag fino a proflag +1000 
                        for (long i = this.flag; i <= segment; i++)
                        {
                            if (IsPrime(i) == true)
                            {
                                PrimeNumber p = new PrimeNumber();
                                p.IDN = i;
                                //se è primo lo sbatte in lista
                                container.Add(p);

                            }

                        }
                        //libera un posto
                        sem.Release(1);
                    }
                    ));
                    //thread partono
                    th[i].Start();
                    //attende gli altri prima di continuare
                    th[i].Join();
                    this.flag += 2000;
                }

                //ordina lista
                SortPrimeList(container);
                
                try
                {
                    //cerca di persistere
                    await PersistPrimeAsync(container);
                    container.Clear();
                    //incrementa di segment*3 (ogni thread controlla 2000n)
                    this.flag += 6000;
                } catch (TimeoutException ex)
                {
                    //ci riprova
                    await PersistPrimeAsync(container);
                    container.Clear();
                    this.flag += 6000;
                    
                } 

            } while (this.flag < long.MaxValue);
        }

        private void SortPrimeList(List<PrimeNumber> list)
        {
            list.Sort((a, b) => a.IDN.CompareTo(b.IDN));
        }

        //private void CalcAndSave()
        //{

        //    //Object locker = new Object();
        //    //PrimeNumbersContext c = new PrimeNumbersContext();
        //    List<PrimeNumber> container = new List<PrimeNumber>();
        //    //lock (locker)
        //    //{

        //    Object locker = new Object();


        //    //lock (new object())
        //    //{

        //    //one thread currentrly into, max of 3

        //    sem.WaitOne();






        //    sem.Release();



        //    //}
        //}

        private async Task PersistPrimeAsync(List<PrimeNumber> list)
        {
            PrimeNumberContext c = new PrimeNumberContext();
            await c.BulkInsertAsync(list);
            await c.BulkSaveChangesAsync();
        }


        /// <summary>
        /// Recupera ultimo numero primo calcolato su DB
        /// </summary>
        /// <returns></returns>
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


    }

}
