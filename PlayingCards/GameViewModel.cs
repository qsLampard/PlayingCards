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
            CreateGameDeck();
            CreatePlayers();
            InitializeGame();
            GameStarted = true;
        }

        private void InitializeGame()
        {
            AssignCurrentPlayer(0);
            CurrentAvailableCard = new Cards();
        }

        private void AssignCurrentPlayer(int index)
        {
            CurrentPlayer = Players[index];
            if (!Players.Any(x => x.State == PlayerState.Winner))
                Players.ForEach(x => x.State = (x == Players[index] ? PlayerState.Active :
        PlayerState.Inactive));
        }

        private void InitializePlayer(Player player)
        {
            player.DrawNewHand(GameDeck);
            player.OnCardDiscarded += player_OnCardDiscarded;
            player.OnPlayerHasWon += player_OnPlayerHasWon;
            player.OnCardPlayed += player_OnCardPlayed;
            Players.Add(player);
        }

        

        private void CreateGameDeck()
        {
            GameDeck = new CardLib.Deck();
            GameDeck.Shuffle();
        }

        private void CreatePlayers()
        {
            Players.Clear();
            InitializePlayer(new Player { Index = 0, PlayerName = _gameOptions.PlayerName });
            InitializePlayer(new ComputerPlayer { Index = 1, Skill = _gameOptions.ComputerSkill });
            InitializePlayer(new ComputerPlayer { Index = 2, Skill = _gameOptions.ComputerSkill });
        }

        void player_OnPlayerHasWon(object sender, PlayerEventArgs e)
        {
            Players.ForEach(x => x.State = (x == e.Player ? PlayerState.Winner : PlayerState.Loser));
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
            AssignCurrentPlayer(nextIndex);
        }

    }
}
