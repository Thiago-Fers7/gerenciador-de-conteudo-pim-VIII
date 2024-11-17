using System.Globalization;

namespace Gerenciador_de_Conteudo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
