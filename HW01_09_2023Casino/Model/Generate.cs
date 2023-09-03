using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW01_09_2023Casino.Model
{
    public class Generate
    {
        public static int GenerateNumber(int min, int max)
        {
            Random rand = new Random();
            int result = rand.Next(min, max);
            return result;
        }
    }
}
