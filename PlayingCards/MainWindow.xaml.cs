using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using CardLib;
namespace PlayingCards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            
            //var position = new Point(15, 15);
            //for (var i = 0; i < 4; i++)
            //{
            //    var suit = (CardLib.Suit)i;
            //    position.Y = 15;
            //    for (int rank = 1; rank < 14; rank++)
            //    {
            //        position.Y += 30;
            //        var card = new CardControl(new CardLib.Card((Suit)suit, (Rank)rank));
            //        card.VerticalAlignment = VerticalAlignment.Top;
            //        card.HorizontalAlignment = HorizontalAlignment.Left;
            //        card.Margin = new Thickness(position.X, position.Y, 0, 0);
            //        contentGrid.Children.Add(card);
            //    }
            //    position.X += 112;
            //}
            //var suit1 = (CardLib.Suit)3;
            //position.Y = 15;
            //var card1 = new CardControl(new CardLib.Card((Suit)suit1, (Rank)14));
            //card1.VerticalAlignment = VerticalAlignment.Top;
            //card1.HorizontalAlignment = HorizontalAlignment.Left;
            //card1.Margin = new Thickness(position.X, position.Y, 0, 0);
            //contentGrid.Children.Add(card1);
            //suit1 = (CardLib.Suit)1;
            //position.Y += 30;
            //card1 = new CardControl(new CardLib.Card((Suit)suit1, (Rank)15));
            //card1.VerticalAlignment = VerticalAlignment.Top;
            //card1.HorizontalAlignment = HorizontalAlignment.Left;
            //card1.Margin = new Thickness(position.X, position.Y, 0, 0);
            //contentGrid.Children.Add(card1);
        }

        private void CommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Close)
                e.CanExecute = true;
            if (e.Command == ApplicationCommands.Save)
                e.CanExecute = false;
            if (e.Command == GameViewModel.StartGameCommand)
                e.CanExecute = true;
            e.Handled = true;
        }

        private void CommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Close)
                this.Close();
            if (e.Command == GameViewModel.StartGameCommand)
            {
                model = new GameViewModel();
                var options = GameOptions.Create();
                options.Save();
                model.StartNewGame();
                DataContext = model;
                PlayButton.Visibility = Visibility.Visible;
                PassButton.Visibility = Visibility.Visible;
            }
            e.Handled = true;
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new about();
            dialog.ShowDialog();
        }

        private void Option_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new options();
            dialog.ShowDialog();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (model.CurrentPlayer.Index==model.CurrentAvailableCardPlayer)
                model.CurrentPlayer.play(null);
            else
                model.CurrentPlayer.play(model.Combination);
            //System.Media.SoundPlayer sp = new System.Media.SoundPlayer(".\\Audios\\background.wmv");//@"background.mp3"
            //sp.Play();
        }

        private void PassButton_Click(object sender, RoutedEventArgs e)
        {
            model.CurrentPlayer.pass(model.CurrentAvailableCardPlayer);
        }
    }
}
