using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HW01_09_2023Casino.Model
{
    class Game
    {
        static Semaphore sem = new Semaphore(5, 5);
        Thread thread;
        public Player player;
        MainWindow main;
        static public List<Player> listPlayers = new List<Player>();
        
        static public int countPeopleOnDay = 0;
        static public int HelpCountPeopleOnDay = 0;
        public static int countPlayerChose = 0;


        public Game(MainWindow main, Player player)
        {
            this.main = main;
            this.player = player;
            thread = new Thread(StartPlay);
            thread.IsBackground = true;
            thread.Start();
        }

        public void StartPlay()
        {

            sem.WaitOne();
            listPlayers.Add(player);
            HelpCountPeopleOnDay++;
            main.SetAll(player);
            Thread.Sleep(1000);

            bool UserCouse = false;
            while (player.CanPlay)
            {
                if (!UserCouse)
                {
                    player.ChoseNumber = Generate.GenerateNumber(0, 37);
                    main.SetChoseNumb(player);
                    UserCouse = true;
                    countPlayerChose++;
                }

                Thread.Sleep(3000);

                if (countPlayerChose == 5)
                {
                    main.SetNumbCircle(ref listPlayers);
                    UserCouse = false;
                }
            }
            if (HelpCountPeopleOnDay == countPeopleOnDay)
            {
                main.FinishDay();
            }
            sem.Release();

        }
    }
}
