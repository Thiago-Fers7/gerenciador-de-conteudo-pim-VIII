using System.Globalization;

namespace Gerenciador_de_Conteudo
{
    public class NullToBooleanConverterBase
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}