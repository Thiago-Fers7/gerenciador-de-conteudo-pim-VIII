using System;
using System.Globalization;

namespace Gerenciador_de_Conteudo.Utils
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Inverte o valor booleano
            return value is bool boolean ? !boolean : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Normalmente não é necessário implementar o ConvertBack para este caso
            throw new NotImplementedException();
        }
    }
}