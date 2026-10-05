using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TesteTarget
{
    
    public class Venda
    {
        [JsonPropertyName("vendedor")]
        public string Vendedor { get; set; } = string.Empty;

        [JsonPropertyName("valor")]
        public decimal Valor { get; set; }
    }

    public class VendasData
    {
        [JsonPropertyName("vendas")]
        public List<Venda> Vendas { get; set; } = new List<Venda>();
    }

    
    public class ResumoVendedor
    {
        public string Nome { get; set; } = string.Empty;
        public decimal TotalVendas { get; set; }
        public decimal TotalComissao { get; set; }
    }

    internal class Exercicio1
    {
        
        static decimal CalcularComissao(decimal valor)
        {
            if (valor < 100m) return 0m;
            if (valor < 500m) return valor * 0.01m;
            return valor * 0.05m;
        }

        static void Main()
        {
            string caminho = "vendas.json";

            
            if (!File.Exists(caminho))
            {
                Console.WriteLine($"Erro: O arquivo '{caminho}' não foi encontrado.");
                return;
            }

            try
            {
                string json = File.ReadAllText(caminho);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                VendasData? dados = JsonSerializer.Deserialize<VendasData>(json, options);

                if (dados?.Vendas == null)
                {
                    Console.WriteLine("Erro: Não foi possível ler os dados do JSON.");
                    return;
                }

                
                var resumo = new Dictionary<string, ResumoVendedor>();

                foreach (var venda in dados.Vendas)
                {
                    
                    if (!resumo.TryGetValue(venda.Vendedor, out var info))
                    {
                        info = new ResumoVendedor { Nome = venda.Vendedor };
                        resumo[venda.Vendedor] = info;
                    }

                    info.TotalVendas += venda.Valor;
                    info.TotalComissao += CalcularComissao(venda.Valor);
                }

                
                Console.WriteLine($"{"Vendedor",-20} {"Total de Vendas",15} {"Comissão",12}");
                Console.WriteLine(new string('-', 52));

                foreach (var vendedor in resumo.Values.OrderBy(v => v.Nome))
                {
                    
                    Console.WriteLine(
                        $"{vendedor.Nome,-20} " +
                        $"R$ {vendedor.TotalVendas,13:F2} " +
                        $"R$ {vendedor.TotalComissao,10:F2}"
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
           
        }
    }
}