using System;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Serialization;
using CardLib;
namespace PlayingCards
{
    /// <summary>
    /// Interaction logic for options.xaml
    /// </summary>

    public partial class options : Window
    {
        public GameOptions _gameOptions;
        public options()
        {
            _gameOptions = GameOptions.Create();
            DataContext = _gameOptions;
            InitializeComponent();
        }

        private void easyAIRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _gameOptions.ComputerSkill = ComputerSkillLevel.Easy;
            _gameOptions.EasyChecked = true;  //different
            _gameOptions.HardChecked = false;
        }

        private void hardAIRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            _gameOptions.ComputerSkill = ComputerSkillLevel.Hard;
            _gameOptions.HardChecked = true;
            _gameOptions.EasyChecked = false;
        }

        private void timeAllowedTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            newPlayerTextBox.SelectAll();
        }

        private void timeAllowedTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var control = sender as TextBox;
            if (control == null)
                return;
            Keyboard.Focus(control);
            e.Handled = true;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (_gameOptions.PlayerName.Equals(""))
            {
                MessageBox.Show("Please choose or create a user", "Alert");
                return;
            }

            using (var stream = File.Open("GameOptions.xml", FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(GameOptions));
                serializer.Serialize(stream, _gameOptions);
            }
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            _gameOptions = null;
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(newPlayerTextBox.Text))
                _gameOptions.AddPlayer(newPlayerTextBox.Text);
            newPlayerTextBox.Text = string.Empty;
        }

        
    }
}
