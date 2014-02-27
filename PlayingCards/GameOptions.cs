using CardLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;

namespace PlayingCards
{
    [Serializable]
    public class GameOptions : INotifyPropertyChanged
    {
        private String _PlayerName = "";
        private ComputerSkillLevel _computerSkill = ComputerSkillLevel.Easy;
        private ObservableCollection<string> _playerNames = new ObservableCollection<string>();
        private ObservableCollection<profile> _userProfile = new ObservableCollection<profile>();
        private bool _EasyChecked = true;
        private bool _HardChecked = false;

        public GameOptions()
        {
        }

        public static RoutedCommand OptionsCommand = new RoutedCommand("Show Options", typeof(GameOptions), new InputGestureCollection(new List<InputGesture> { new KeyGesture(Key.O, ModifierKeys.Control) }));

        public ObservableCollection<profile> userProfile
        {
            get
            {
                return _userProfile;
            }
            set
            {
                _userProfile = value;
                OnPropertyChanged("userProfile");
            }
        }

        public ObservableCollection<string> playerNames
        {
            get
            {
                return _playerNames;
            }
            set
            {
                _playerNames = value;
                OnPropertyChanged("playerNames");
            }
        }

        public void AddPlayer(string playerName)
        {
            if (_playerNames.Contains(playerName))
                return;
            _playerNames.Add(playerName);
            _userProfile.Add(new profile(playerName, 0, 0, 0));
            OnPropertyChanged("playerNames");
        }

        public String PlayerName
        {
            get { return _PlayerName; }
            set
            {
                _PlayerName = value;
                OnPropertyChanged("PlayerName");
            }
        }

        public ComputerSkillLevel ComputerSkill
        {
            get { return _computerSkill; }
            set
            {
                _computerSkill = value;
                OnPropertyChanged("ComputerSkill");
            }
        }

        public bool EasyChecked
        {
            get { return _EasyChecked; }
            set
            {
                _EasyChecked = value;
                OnPropertyChanged("EasyChecked");
            }
        }

        public bool HardChecked
        {
            get { return _HardChecked; }
            set
            {
                _HardChecked = value;
                OnPropertyChanged("HardChecked");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            using (var stream = File.Open("GameOptions.xml", FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(GameOptions));
                serializer.Serialize(stream, this);
            }
        }

        public static GameOptions Create()
        {
            if (File.Exists("GameOptions.xml"))
            {
                using (var stream = File.OpenRead("GameOptions.xml"))
                {
                    var serializer = new XmlSerializer(typeof(GameOptions));
                    return serializer.Deserialize(stream) as GameOptions;
                }
            }
            else
                return new GameOptions();
        }
    }

   

    

    [Serializable]
    public class profile
    {
        public String name;
        public int score;
        public int win;
        public int total;
        profile()//Remember that serialized classes must have default (i.e. parameterless) constructors.
        {
            
        }
        
        public profile(String n,int s,int w,int t)
        {
            name = n;
            score = s;
            win = w;
            total = t;
        }
        
    }
}
