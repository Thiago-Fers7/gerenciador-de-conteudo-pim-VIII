using Gerenciador_de_Conteudo.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;


namespace Gerenciador_de_Conteudo.Views
{
    public partial class GestaoConteudosPage : ContentPage, INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;

        public ObservableCollection<Criador> Criadores { get; } = new ObservableCollection<Criador>();
        public string NovoCriadorNome { get; set; } = string.Empty;
        public string NovoConteudoNome { get; set; } = string.Empty;
        public string NovoConteudoTipo { get; set; } = string.Empty;
        public ObservableCollection<Conteudo> Conteudos { get; } = new ObservableCollection<Conteudo>();
        public Criador? _criadorSelecionado;
        public bool IsCriadorSelecionado => CriadorSelecionado != null;

        public ICommand AdicionarNovoCriadorCommand { get; }
        public ICommand AdicionarConteudoCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public GestaoConteudosPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();

            AdicionarNovoCriadorCommand = new Command(async () => await AdicionarNovoCriador());
            AdicionarConteudoCommand = new Command(async () => await AdicionarConteudo());

            BindingContext = this;

            // Chame o método para buscar os criadores ao carregar a página
            _ = BuscarCriadores();
        }

        public Criador? CriadorSelecionado
        {
            get => _criadorSelecionado;
            set
            {
                _criadorSelecionado = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsCriadorSelecionado));
                Console.WriteLine(JsonSerializer.Serialize(_criadorSelecionado));
                if (_criadorSelecionado != null)
                {
                    // Chame o método para buscar os conteúdos quando um criador for selecionado
                    _ = BuscarConteudos();
                }
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task BuscarCriadores()
        {
            var response = await _httpClient.GetFromJsonAsync<Criador[]>("http://localhost:5067/api/criadores");

            if (response != null)
            {
                Criadores.Clear();

                foreach (var criador in response)
                {
                    Criadores.Add(criador);
                }
            }
        }

        private async Task AdicionarNovoCriador()
        {
            var criador = new Criador { Nome = NovoCriadorNome };
            Console.WriteLine(criador);

            try
            {
                if (string.IsNullOrWhiteSpace(criador.Nome))
                {
                    await DisplayAlert("Erro", "Nome do criador não pode ser vazio", "OK");
                    return;
                }

                var response = await _httpClient.PostAsJsonAsync("http://localhost:5067/api/criadores", criador);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sucesso", "Criador adicionado com sucesso", "OK");

                    NovoCriadorNome = string.Empty;

                    await BuscarCriadores();
                }
                else
                {
                    await DisplayAlert("Erro", "Erro ao adicionar criador", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task AdicionarConteudo()
        {
            // Adicionar conteúdo (rota: POST /api/conteudos

            if (CriadorSelecionado == null)
            {
                await DisplayAlert("Erro", "Selecione um criador", "OK");
                return;
            }

            var conteudo = new Conteudo
            {
                Titulo = NovoConteudoNome,
                Tipo = NovoConteudoTipo,
                CriadorID = CriadorSelecionado.Id
            };

            try
            {
                if (string.IsNullOrWhiteSpace(conteudo.Titulo) || string.IsNullOrWhiteSpace(conteudo.Tipo))
                {
                    await DisplayAlert("Erro", "Nome e tipo do conteúdo não podem ser vazios", "OK");
                    return;
                }

                var response = await _httpClient.PostAsJsonAsync("http://localhost:5067/api/conteudos", conteudo);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Sucesso", "Conteúdo adicionado com sucesso", "OK");

                    NovoConteudoNome = string.Empty;
                    NovoConteudoTipo = string.Empty;

                    _ = BuscarConteudos();
                }
                else
                {
                    await DisplayAlert("Erro", "Erro ao adicionar conteúdo", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async Task BuscarConteudos()
        {
            if (CriadorSelecionado == null)
            {

                return;
            }

            var response = await _httpClient.GetFromJsonAsync<Conteudo[]>($"http://localhost:5067/api/conteudos/conteudos-do-criador/{CriadorSelecionado.Id}");

            if (response != null)
            {
                Conteudos.Clear();

                foreach (var conteudo in response)
                {
                    Conteudos.Add(conteudo);
                }
            }
        }
    }
}