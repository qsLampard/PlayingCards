using CardLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlayingCards
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get { return _currentPlayer; }
            set
            {
                _currentPlayer = value;
                OnPropertyChanged("CurrentPlayer");
            }
        }

        private List<Player> _players;
        public List<Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                OnPropertyChanged("Players");
            }
        }

        private Cards _availableCard;
        public Cards CurrentAvailableCard
        {
            get { return _availableCard; }
            set
            {
                _availableCard = value;
                OnPropertyChanged("CurrentAvailableCard");
                
            }
        }

        private int _availableCardPlayer;
        public int CurrentAvailableCardPlayer
        {
            get { return _availableCardPlayer; }
            set
            {
                _availableCardPlayer = value;
                OnPropertyChanged("CurrentAvailableCardPlayer");

            }
        }

        private kindsOfCombination _combination;
        public kindsOfCombination Combination
        {
            get { return _combination; }
            set
            {
                _combination = value;
                OnPropertyChanged("CombinationChanged");
            }
        }

        private Deck _deck;
        public Deck GameDeck
        {
            get { return _deck; }
            set
            {
                _deck = value;
                OnPropertyChanged("GameDeck");
            }
        }

        private bool _gameStarted;
        public bool GameStarted
        {
            get { return _gameStarted; }
            set
            {
                _gameStarted = value;
                if (_gameStarted)
                    ComputerPlayer.ResetPlayerNames();
                OnPropertyChanged("GameStarted");
            }
        }

        

        private GameOptions _gameOptions;

        public static RoutedCommand StartGameCommand = new RoutedCommand("Start New Game", typeof(GameViewModel), new InputGestureCollection(new List<InputGesture> { new KeyGesture(Key.N, ModifierKeys.Control) }));
        public static RoutedCommand ShowAboutCommand = new RoutedCommand("Show About Dialog", typeof(GameViewModel));

        public GameViewModel()
        {
            _players = new List<Player>();
            this._gameOptions = GameOptions.Create();
            
        }

        public void StartNewGame()
        {
            Random rand = new Random();
            int LandlordIndex = rand.Next(3);
            CreateGameDeck();
            CreatePlayers(LandlordIndex);
            InitializeGame(LandlordIndex);
            GameStarted = true;
            
        }

        private void InitializeGame(int LandlordIndex)
        {

            AssignCurrentPlayer(LandlordIndex);
            CurrentAvailableCard = new Cards();
        }

        private void AssignCurrentPlayer(int index)
        {
            CurrentPlayer = Players[index];
            if (!Players.Any(x => x.State == PlayerState.Winner))
            {
                foreach(Player p in Players)
                {
                    if (p == Players[index])
                        p.State = PlayerState.Active;
                    else if (p.State == PlayerState.Pass)
                        p.State = PlayerState.Pass;
                    else
                        p.State = PlayerState.Inactive;
                }
            }
                
        }

        private void InitializePlayer(Player player)
        {
            player.DrawNewHand(GameDeck);
            //player.OnCardDiscarded += player_OnCardDiscarded;
            player.OnPlayerHasWon += player_OnPlayerHasWon;
            player.OnCardPlayed += player_OnCardPlayed;
            Players.Add(player);
        }

        

        private void CreateGameDeck()
        {
            GameDeck = new CardLib.Deck();
            GameDeck.Shuffle();
        }

        private void CreatePlayers(int LandlordIndex)
        {
            Players.Clear();
            if (LandlordIndex == 0)
                InitializePlayer(new Player { Index = 0, Landlord = true, PlayerName = _gameOptions.PlayerName });
            else
                InitializePlayer(new Player { Index = 0, Landlord = false, PlayerName = _gameOptions.PlayerName });
            if (LandlordIndex == 1)
                InitializePlayer(new ComputerPlayer { Index = 1, Landlord = true, Skill = _gameOptions.ComputerSkill });
            else
                InitializePlayer(new ComputerPlayer { Index = 1, Landlord = false, Skill = _gameOptions.ComputerSkill });
            if (LandlordIndex == 2)
                InitializePlayer(new ComputerPlayer { Index = 2, Landlord = true, Skill = _gameOptions.ComputerSkill });
            else
                InitializePlayer(new ComputerPlayer { Index = 2, Landlord = false, Skill = _gameOptions.ComputerSkill });
            //InitializePlayer(new ComputerPlayer { Index = 1, Skill = _gameOptions.ComputerSkill });
            //InitializePlayer(new ComputerPlayer { Index = 2, Skill = _gameOptions.ComputerSkill });
        }

        void player_OnPlayerHasWon(object sender, PlayerEventArgs e)
        {
            Players.ForEach(x => x.State = (x.Landlord == e.Player.Landlord ? PlayerState.Winner : PlayerState.Loser));
        }

        void player_OnCardDiscarded(object sender, CardEventArgs e)
        {
            /*CurrentAvailableCard = e.Card;

            var nextIndex = CurrentPlayer.Index + 1 >= 3 ? 0 : CurrentPlayer.Index + 1;

            if (GameDeck.CardsInDeck == 0)
            {
                var cardsInPlay = new List<Card>();
                foreach (var player in Players)
                    cardsInPlay.AddRange(player.GetCards());
                cardsInPlay.Add(CurrentAvailableCard);
                GameDeck.ReshuffleDiscarded(cardsInPlay);
            }
            AssignCurrentPlayer(nextIndex);*/
        }

        private void player_OnCardPlayed(object sender, CardEventArgs e)
        {
            var nextIndex = CurrentPlayer.Index + 1 >= 3 ? 0 : CurrentPlayer.Index + 1;
            CurrentAvailableCard = new Cards();
            var cardsInPlay = new List<Card>();
            for (int i = 0; i < e.Cards.Count; i++)
                CurrentAvailableCard.Add(e.Cards[i]);
            if (e.Cards.Count > 0)
            {
                CurrentAvailableCardPlayer = e.index;
                Combination = e.CurrentCombination;
            }
            AssignCurrentPlayer(nextIndex);
        }

    }
}
