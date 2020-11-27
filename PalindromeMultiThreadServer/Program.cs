using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalindromeMultiThreadServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int MaxThreads;
            bool ok = false;
            do
            {
                Console.WriteLine("Введите максимальное количество потоков ");
                ok = Int32.TryParse(Console.ReadLine(), out MaxThreads);
            } while (!ok);

            PalindromeServer.Connection(PalindromeServer.IP, PalindromeServer.Port, MaxThreads);
            

        }
    }
}
