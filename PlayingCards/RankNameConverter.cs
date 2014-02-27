using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
namespace PlayingCards
{
    [ValueConversion(typeof(CardLib.Rank), typeof(string))]
    public class RankNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int source = (int)value;
            if (source == 1 || source > 10)
            {
                switch (source)
                {
                    case 1:
                        return "Ace";
                    case 11:
                        return "Jack";
                    case 12:
                        return "Queen";
                    case 13:
                        return "King";
                    case 14:
                        return "Ace";
                    case 15:
                        return "2";
                    case 16:
                        return "joker";
                    case 17:
                        return "Joker";
                    default:
                        return DependencyProperty.UnsetValue;
                }
            }
            else
                return source.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
