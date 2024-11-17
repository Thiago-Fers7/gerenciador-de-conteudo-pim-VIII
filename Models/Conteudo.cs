namespace Gerenciador_de_Conteudo.Models
{
    public class Conteudo
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = String.Empty;
        public string Tipo { get; set; } = String.Empty;
        public int CriadorID { get; set; }
        public int PlaylistsCount { get; set; }
    }
}
