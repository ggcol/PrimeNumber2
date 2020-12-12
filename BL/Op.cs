using System;
using System.Collections.Generic;
using System.Threading;

namespace BL
{
    public class Op
    {
        static PrimeNumber p = new PrimeNumber();
        static DbGnome gnome = new DbGnome();
        static Semaphore sem;

        private void InitProflag()
        {

            if (gnome.RetrieveLastPrimeCalc() == null)
            {
                p.Flag = 2;
            }
            else
            {
                p.Flag = gnome.RetrieveLastPrimeCalc().IDN;
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
                    long segment = p.Flag + 2000;

                    //inizializza thread per
                    th[i] = new Thread(new ThreadStart(() =>
                    {
                        //fa entrare un thread alla volta
                        sem.WaitOne();
                        //controlla numeri da proflag fino a flag+2000 
                        for (long ind = p.Flag; ind <= segment; ind++)
                        {
                            if (IsPrime(ind) == true)
                            {
                                PrimeNumber p = new PrimeNumber();
                                p.IDN = ind;
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
                    p.Flag += 2000;
                }

                //ordina lista
                SortPrimeList(container);

                try
                {
                    //cerca di persistere
                    await gnome.PersistPrimeAsync(container);
                    container.Clear();
                    //incrementa di segment*3 (ogni thread controlla 2000n)
                    p.Flag += 6000;
                }
                catch (TimeoutException ex)
                {
                    //ci riprova
                    await gnome.PersistPrimeAsync(container);
                    container.Clear();
                    p.Flag += 6000;

                }

            } while (p.Flag < long.MaxValue);
        }

        private void SortPrimeList(List<PrimeNumber> list)
        {
            list.Sort((a, b) => a.IDN.CompareTo(b.IDN));
        }
    }


}
