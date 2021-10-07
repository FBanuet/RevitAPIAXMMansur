using System;

namespace Algoritmia
{
    class BubbleAlgo
    {
        static int[] numbers = new int[] { 10, 2, 6, 5, 3, 2, 5, 7, 8 };
        static void Main(string[] args)
        {
            Console.WriteLine("Antes de Ordenar");
            Show();
            int extIteracion = 0;
            int intIteracion = 0;
            bool flag = true;

            for(int i = 0; i<numbers.Length && flag; i++)
            {
                flag = false;
                extIteracion++;

                for(int j = 0; j< numbers.Length -i -1; j++)
                {
                    intIteracion++;
                    if(numbers[j] > numbers[j+1])
                    {
                        flag = true;
                        int aux = numbers[j];
                        numbers[j] = numbers[j + 1];
                        numbers[j + 1] = aux;
                    }
                }

                Show();
            }

            Console.WriteLine("Numero de iteraciones externas : " + extIteracion);
            Console.WriteLine("Numero de iteraciones internas : " + intIteracion);
            Console.WriteLine("Despues de Ordenar");
            Show();
        }

        public static void Show()
        {
            foreach(var number in numbers)
            {
                Console.WriteLine(number + ",");

            }
            Console.WriteLine("\n");
        }
    }
}
