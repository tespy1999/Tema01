using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema01
{
    public class Firma
    {
       

        static void Main(string[] args)
        {
            int ore=100;
            double tarif_ore=10.50;
            Console.WriteLine("Citim numarul de ore:");
            ore=Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Citim tariful pentru ora:");
            tarif_ore = Convert.ToDouble(Console.ReadLine());


            var salariu = ore * tarif_ore;
            Console.WriteLine(salariu);
            if(salariu>3000)
            {
                Console.WriteLine("Salariu este mare!");
            }else
            {
                Console.WriteLine("Ați lucrat prea puține ore pentru a avea un salariumare!");
            }
            Console.ReadKey();
        }
   
    }
}
