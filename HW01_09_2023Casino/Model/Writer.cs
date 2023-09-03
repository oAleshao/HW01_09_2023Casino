using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW01_09_2023Casino.Model
{
    public class Writer
    {
        static public int countDay = 1; 
        static public void Write(Player player)
        {
            try
            {
                StreamWriter writer = new StreamWriter("Result.txt", true);
                writer.WriteLine($"{player.Name} | Начальная сумма [{player.FirstBalance}] | Конечная сумма [{player.LastBalance}]");
                writer.Close();
            }
            catch (Exception ex) { }
        }

        static public void WriteFirstStr()
        {
            try
            {
                StreamWriter writer = new StreamWriter("Result.txt", true);
                writer.WriteLine($"\n\nDay {countDay}, date {DateTime.Now.ToShortDateString()}");
                writer.Close();
                countDay++;
            }
            catch (Exception ex) { }
        }

    }
}
