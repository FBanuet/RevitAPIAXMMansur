using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmia
{
    class _99BotltlesofBeer
    {
        static string singleWord = "bottle";
        static string pluralWord = "bottles";
        static string protoText = "{0} {2} of beer on the wall, {0} {2} of beer\n" +
            "Take one down and pass it around, {1} {3} of beer on the wall\n\n";
        static string End = "No more bottles of beer on the wall , no more bottles of on the wall \n" +
            "Go to the store and buy some more, 99 bottles of beer on the wall";

        static void MainUI(string[] argso)
        {
            for (int i = 99; i > 0; i--)
            {
                Print(i);
            }
            Console.WriteLine(End);

        }

        static void Print(int num)
        {
            Console.WriteLine(protoText,
                                num,
                                (num - 1),
                                (num) == 1 ? singleWord : pluralWord,
                                (num - 1) == 1 ? singleWord : pluralWord);
        }
    }
}
