using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace HW01_09_2023Casino.Model
{
    public class Player
    {
        public string Name { get; set; }
        public int FirstBalance { get; set; }
        public int LastBalance { get; set; }
        public int ChoseNumber { get; set; }
        public int indxUser { get; set; }
        public bool CanPlay { get; set; }


        public Player() { }

        public override string ToString()
        {
            return $"{Name} {FirstBalance}";
        }
    }
}
