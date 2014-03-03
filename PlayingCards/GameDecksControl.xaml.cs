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
    /// Interaction logic for GameDecksControl.xaml
    /// </summary>
    public partial class GameDecksControl : UserControl
    {
        public GameDecksControl()
        {
            InitializeComponent();
        }
        public bool GameStarted
        {
            get { return (bool)GetValue(GameStartedProperty); }
            set { SetValue(GameStartedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GameStarted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GameStartedProperty =
            DependencyProperty.Register("GameStarted", typeof(bool), typeof(GameDecksControl), new PropertyMetadata(false, new PropertyChangedCallback(OnGameStarted)));



        public Player CurrentPlayer
        {
            get { return (Player)GetValue(CurrentPlayerProperty); }
            set { SetValue(CurrentPlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPlayerProperty =
            DependencyProperty.Register("CurrentPlayer", typeof(Player), typeof(GameDecksControl), new PropertyMetadata(null, new PropertyChangedCallback(OnPlayerChanged)));



        public Deck Deck
        {
            get { return (Deck)GetValue(DeckProperty); }
            set { SetValue(DeckProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Deck.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeckProperty =
            DependencyProperty.Register("Deck", typeof(Deck), typeof(GameDecksControl), new PropertyMetadata(null, new PropertyChangedCallback(OnDeckChanged)));



        public Cards AvailableCard
        {
            get { return (Cards)GetValue(AvailableCardProperty); }
            set { SetValue(AvailableCardProperty, value); }
        }

        public kindsOfCombination Combination
        {
            get { return (kindsOfCombination)GetValue(CombinationProperty); }
            set { SetValue(CombinationProperty, value); }
        }

        public int AvailableCardPlayer
        {
            get { return (int)GetValue(AvailableCardPlayerProperty); }
            set { SetValue(AvailableCardPlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AvailableCard.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableCardProperty =
            DependencyProperty.Register("AvailableCard", typeof(Cards), typeof(GameDecksControl), new PropertyMetadata(null, new PropertyChangedCallback(OnAvailableCardChanged)));

        public static readonly DependencyProperty CombinationProperty =
            DependencyProperty.Register("Combination", typeof(kindsOfCombination), typeof(GameDecksControl), new PropertyMetadata(null, new PropertyChangedCallback(KindsOfCombinationChanged)));

        public static readonly DependencyProperty AvailableCardPlayerProperty =
            DependencyProperty.Register("AvailableCardPlayer", typeof(int), typeof(GameDecksControl));

        
        private static void OnGameStarted(DependencyObject source,
    DependencyPropertyChangedEventArgs e)
        {
            var control = source as GameDecksControl;
            control.DrawDecksInitial();
        }

        private static void OnPlayerChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var control = source as GameDecksControl;
            if (control.CurrentPlayer == null)
                return;

            //control.CurrentPlayer.OnCardDiscarded += control.CurrentPlayer_OnCardDiscarded;
            control.DrawDecks();
        }

        private void CurrentPlayer_OnCardDiscarded(object sender, CardEventArgs e)
        {
            AvailableCard = e.Cards;
            DrawDecks();
        }

        private static void OnDeckChanged(DependencyObject source,
    DependencyPropertyChangedEventArgs e)
        {
            var control = source as GameDecksControl;
            control.DrawDecksInitial();
        }

        private static void OnAvailableCardChanged(DependencyObject source,
    DependencyPropertyChangedEventArgs e)
        {
            var control = source as GameDecksControl;
            //control.DrawDecks();
        }

        private static void KindsOfCombinationChanged(DependencyObject source,
    DependencyPropertyChangedEventArgs e)
        {
            var control = source as GameDecksControl;
            control.DrawDecks();
        }

        private void DrawDecksInitial()
        {
            controlCanvas.Children.Clear();
        }

        private void DrawDecks()
        {
            if ((AvailableCard == null || AvailableCard.Count == 0) && (CurrentPlayer != null&&CurrentPlayer.Index != AvailableCardPlayer))   ////no change, no flush; clear when same user
                return;
            Console.WriteLine("Pass");
            controlCanvas.Children.Clear();
            if (CurrentPlayer == null || Deck == null || !GameStarted)
                return;

            List<CardControl> stackedCards = new List<CardControl>();//Deck.CardsInDeck
            for (int i = 0; i < Deck.CardsInDeck; i++)
                stackedCards.Add(new CardControl(Deck.GetCard(i))
                {
                    Margin =
                      new Thickness(150 + (i * 1.25), 25 - (i * 1.25), 0, 0),
                    IsFaceUp = false
                });

            if (stackedCards.Count > 0)
                stackedCards.Last().MouseDoubleClick += Deck_MouseDoubleClick;
            if (AvailableCard != null)
            {
                for (var i = 0; i < AvailableCard.Count; i++)
                {
                    var cardControl = new CardControl(AvailableCard[i]);
                    cardControl.Margin = new Thickness(i * 35, 35, 0, 0);
                    controlCanvas.Children.Add(cardControl);
                }
            }
            stackedCards.ForEach(x => controlCanvas.Children.Add(x));
        }

        void AvailalbleCard_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrentPlayer.State != PlayerState.Active)
                return;

            var control = sender as CardControl;
            CurrentPlayer.AddCard(control.Card);
            AvailableCard = null;
            DrawDecks();
        }

        void Deck_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CurrentPlayer.State != PlayerState.Active)
                return;

            CurrentPlayer.DrawCard(Deck);
            DrawDecks();
        }
    }
}
