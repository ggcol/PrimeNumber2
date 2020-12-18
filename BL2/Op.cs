using DAL;
using Microsoft.Data.SqlClient;
using PrimeNumber2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace BL
{
    public class Op
    {
        static PrimeNumber p = new PrimeNumber();
        static DbGnome gnome = new DbGnome();
        static Semaphore sem;
        private long flag;

        private void InitFlag()
        {

            if (gnome.RetrieveLastPrimeCalc() == null)
            {
                this.flag = 2;
            }
            else
            {
                this.flag = (gnome.RetrieveLastPrimeCalc().IDN+1);
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

            //inizializza flag e segna punto da cui ripartire
            InitFlag();
            //il semaforo ha un posto "attivo" per un massimo di due totali
            //edit (update ram) tre posti per un massimo di tre
            sem = new Semaphore(3, 3);
            List<PrimeNumber> container = new List<PrimeNumber>();

            do
            {
                Thread[] th = new Thread[3];

                //faccio partire tre thread
                for (int i = 0; i < th.Length; i++)
                {
                    //segment = porzione di numeri da analizzare
                    long segment = this.flag + 100000;

                    //inizializza thread per
                    th[i] = new Thread(new ThreadStart(() =>
                    {
                        //fa entrare un thread alla volta
                        sem.WaitOne();
                        //controlla numeri da proflag fino a flag+2000 
                        for (long ind = this.flag; ind <= segment; ind++)
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
                    this.flag += 100000;
                }

                //ordina lista
                SortPrimeList(container);

                try
                {
                    //cerca di persistere
                    await gnome.PersistPrimeAsync(container).ConfigureAwait(false);
                    container.Clear();
                    //incrementa di segment*3 (ogni thread controlla 2000n)
                    this.flag += 300000;
                }
                catch (Win32Exception win32_ex)
                {
                    //ci riprova
                    await gnome.PersistPrimeAsync(container);
                    container.Clear();
                    this.flag += 300000;
                } 
                catch (SqlException s_ex)
                {
                    //ci riprova
                    await gnome.PersistPrimeAsync(container);
                    container.Clear();
                    this.flag += 300000;
                }

            } while (this.flag < long.MaxValue);
        }

        private void SortPrimeList(List<PrimeNumber> list)
        {
            list.Sort((a, b) => a.IDN.CompareTo(b.IDN));
        }
    }


}
