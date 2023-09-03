using HW01_09_2023Casino.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace HW01_09_2023Casino
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SynchronizationContext sync = null;
        static Semaphore sem = new Semaphore(1, 3);
        static public BindingList<Player> players;
        public bool[] checkBord = new bool[5];
        public List<Image> borders;
        public List<Label> labelsNames;
        public List<Label> labelsChoseNumb;


        public MainWindow()
        {
            InitializeComponent();
            sync = SynchronizationContext.Current;

            borders = new List<Image>() { Player1,  Player2, Player3, Player4, Player5 };
            labelsNames = new List<Label>() { NamePlayer1, NamePlayer2, NamePlayer3, NamePlayer4, NamePlayer5 };
            labelsChoseNumb = new List<Label>() { NumberOfPlayer1, NumberOfPlayer2, NumberOfPlayer3, NumberOfPlayer4, NumberOfPlayer5 };         
            DataGridViewPlayers.IsReadOnly = true;
        }



        // Методы для контекста синхронизации
        private void ItemSourceForList(List<Player> players)
        {
            sync.Send(s => DataGridViewPlayers.ItemsSource = null, null);
            sync.Send(s => DataGridViewPlayers.ItemsSource = players, null);
        }

        public void SetAll(Player player)
        {
            for (int i = 0; i < checkBord.Length; i++)
            {
                if (!checkBord[i])
                {
                    sync.Send(s => listBoxMessage.Items.Add($"{player.Name} присоиденился"), null);
                    sync.Send(s => borders[i].Source = BitmapFrame.Create(new Uri($"pack://application:,,,/Players/{Generate.GenerateNumber(1,8)}.png")), null);
                    sync.Send(s => labelsNames[i].Content = player.Name, null);
                    sync.Send(s => labelsChoseNumb[i].Content = string.Empty, null);
                    player.indxUser = i;
                    ItemSourceForList(Game.listPlayers);
                    checkBord[i] = true;
                    break;
                }
            }
        
        }

        public void SetChoseNumb(Player player)
        {
            sync.Send(s => labelsChoseNumb[player.indxUser].Content = player.ChoseNumber.ToString(), null);
        }

        public void SetNumbCircle(ref List<Player> players)
        {
            try
            {
                int numbWheel = 0;
                sync.Send(s => NumberOfCircle.Text = Generate.GenerateNumber(0, 36).ToString(), null);
                sync.Send(s => numbWheel = int.Parse(NumberOfCircle.Text), null);

                int i = 0;
                foreach (var player in players.ToList())
                {
                    if (player.ChoseNumber == numbWheel)
                    {
                        player.LastBalance *= 2;
                        ItemSourceForList(Game.listPlayers.ToList());
                       sync.Send(s => listBoxMessage.Items.Add($"{player.Name} выиграл | {player.LastBalance}"), null); ;
                    }
                    else
                    {
                        player.CanPlay = false;
                        players.Remove(player);
                        checkBord[i] = false;
                        Writer.Write(player);
                        Game.countPlayerChose--;
                        sync.Send(s => listBoxMessage.Items.Add($"{player.Name} покидает нас "), null);
                    }
                    if (i != 5)
                    {
                        i++;
                    }
                }
                Thread.Sleep(1000);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool temp = false;
        public void FinishDay()
        {
            sync.Send(s=> ButtonOpen.IsEnabled = true, null);
            if (!temp)
            {
                temp = true;
                MessageBox.Show("Все поситители ушли. Казино Закрывается!!!");
                Game.HelpCountPeopleOnDay = 0;
            }
        }
        
        

        // Запуск игры
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listBoxMessage.Items.Clear();
            DataGridViewPlayers.ItemsSource = null;
            ButtonOpen.IsEnabled = false;
            Task.Factory.StartNew(StartGame);
        }

        // Метод поток для запуска
        private void StartGame()
        {
            Writer.WriteFirstStr();
            Game.countPeopleOnDay = Generate.GenerateNumber(20, 101);
            for (int i = 1; i <= Game.countPeopleOnDay; i++)
            {
                Player player = new Player();
                player.Name = $"Игрок {i}";
                player.FirstBalance = Generate.GenerateNumber(1000, 2000);
                player.LastBalance = player.FirstBalance;
                player.CanPlay = true;
                Game game = new Game(this, player);
                Thread.Sleep(100);
            }
        }
    }
}
