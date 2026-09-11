namespace BancoSENAIAPI.Models
{
    public class cliente
    {
        public int CodigoCliente { get; set; }

        public required string nomeCliente { get; set; }

        public required string Cpf {  get; set; }

        public  int NumeroAgencia { get; set; }

        public int SaldoTotal {  get; set; }

        public required string Sexo {  get; set; }
        public required string Endereco { get; set; }
        public required string cidade { get; set;}
        public required string estado {  get; set; }  
    }
}
