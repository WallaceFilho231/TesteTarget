using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TesteTarget
{ 
    public class Produto
    {
        [JsonPropertyName("codigoProduto")]
        public int CodigoProduto { get; set; }

        [JsonPropertyName("descricaoProduto")]
        public string DescricaoProduto { get; set; } = string.Empty;

        [JsonPropertyName("estoque")]
        public int Estoque { get; set; }
    }

    public class EstoqueData
    {
        [JsonPropertyName("estoque")]
        public List<Produto> Estoque { get; set; } = new();
    }

    public enum TipoMovimentacao
    {
        Entrada,
        Saida
    }

    public class Movimentacao
    {
        public int Id { get; set; }
        public int CodigoProduto { get; set; }
        public TipoMovimentacao Tipo { get; set; }
        public int Quantidade { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataHora { get; set; } = DateTime.Now;
    }
    
    public class EstoqueService
    {
        private readonly List<Produto> _produtos;
        private readonly List<Movimentacao> _movimentacoes = new();
        private int _proximoId = 1;

        public EstoqueService(List<Produto> produtos) => _produtos = produtos;

        public IReadOnlyList<Produto> Produtos => _produtos;
        public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

        public Produto? BuscarProduto(int codigo) =>
            _produtos.FirstOrDefault(p => p.CodigoProduto == codigo);

        public Movimentacao Registrar(int codigoProduto, TipoMovimentacao tipo,
                                      int quantidade, string descricao)
        {
            var produto = BuscarProduto(codigoProduto)
                ?? throw new InvalidOperationException($"Produto com código {codigoProduto} não encontrado.");

            if (quantidade <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            if (tipo == TipoMovimentacao.Saida && quantidade > produto.Estoque)
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {produto.Estoque}, solicitado: {quantidade}.");

            produto.Estoque += tipo == TipoMovimentacao.Entrada ? quantidade : -quantidade;

            var mov = new Movimentacao
            {
                Id = _proximoId++,
                CodigoProduto = codigoProduto,
                Tipo = tipo,
                Quantidade = quantidade,
                Descricao = descricao
            };

            _movimentacoes.Add(mov);
            return mov;
        }
    }
   
    public class Exercicio2
    {
        private static EstoqueService _service = null!;

     
        public static void Main(string[] args)
        {
            const string caminho = "estoque.json"; // Nome atualizado

            if (!File.Exists(caminho))
            {
                Console.WriteLine($"Arquivo '{caminho}' não encontrado. Certifique-se de que ele está na pasta de saída (bin/Debug/...).");
                return;
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dados = JsonSerializer.Deserialize<EstoqueData>(File.ReadAllText(caminho), options);

                if (dados?.Estoque == null)
                {
                    Console.WriteLine("Não foi possível ler os produtos do JSON.");
                    return;
                }

                _service = new EstoqueService(dados.Estoque);

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("===== CONTROLE DE ESTOQUE =====");
                    Console.WriteLine("1 - Registrar movimentação");
                    Console.WriteLine("2 - Consultar estoque");
                    Console.WriteLine("3 - Listar movimentações");
                    Console.WriteLine("0 - Sair");
                    Console.Write("Opção: ");

                    switch (Console.ReadLine())
                    {
                        case "1": RegistrarMovimentacao(); break;
                        case "2": ConsultarEstoque(); break;
                        case "3": ListarMovimentacoes(); break;
                        case "0": return;
                        default: Console.WriteLine("Opção inválida."); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
            }
        }

        static void RegistrarMovimentacao()
        {
            Console.WriteLine();
            Console.WriteLine("--- Produtos disponíveis ---");
            foreach (var p in _service.Produtos)
                Console.WriteLine($"{p.CodigoProduto} - {p.DescricaoProduto} (estoque: {p.Estoque})");

            Console.Write("Código do produto: ");
            if (!int.TryParse(Console.ReadLine(), out int codigo))
            {
                Console.WriteLine("Código inválido.");
                return;
            }

            Console.Write("Tipo (E = Entrada, S = Saída): ");
            var tipoInput = Console.ReadLine()?.Trim().ToUpper();
            if (tipoInput != "E" && tipoInput != "S")
            {
                Console.WriteLine("Tipo inválido.");
                return;
            }
            var tipo = tipoInput == "E" ? TipoMovimentacao.Entrada : TipoMovimentacao.Saida;

            Console.Write("Quantidade: ");
            if (!int.TryParse(Console.ReadLine(), out int qtd))
            {
                Console.WriteLine("Quantidade inválida.");
                return;
            }

            Console.Write("Descrição da movimentação: ");
            var descricao = Console.ReadLine() ?? string.Empty;

            try
            {
                var mov = _service.Registrar(codigo, tipo, qtd, descricao);
                var produto = _service.BuscarProduto(codigo)!;

                Console.WriteLine();
                Console.WriteLine(">> Movimentação registrada com sucesso!");
                Console.WriteLine($"  ID.................: {mov.Id}");
                Console.WriteLine($"  Produto............: {produto.DescricaoProduto}");
                Console.WriteLine($"  Tipo...............: {mov.Tipo}");
                Console.WriteLine($"  Quantidade Atualizada.........: {mov.Quantidade}");
                Console.WriteLine($"  Descrição..........: {mov.Descricao}");
                Console.WriteLine($"  Data/Hora..........: {mov.DataHora:dd/MM/yyyy HH:mm:ss}");
                Console.WriteLine($"  ESTOQUE FINAL......: {produto.Estoque}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        static void ConsultarEstoque()
        {
            Console.WriteLine();
            Console.WriteLine($"{"Código",-8} {"Produto",-32} {"Estoque",8}");
            Console.WriteLine(new string('-', 50));
            foreach (var p in _service.Produtos)
                Console.WriteLine($"{p.CodigoProduto,-8} {p.DescricaoProduto,-32} {p.Estoque,8}");
        }

        static void ListarMovimentacoes()
        {
            if (_service.Movimentacoes.Count == 0)
            {
                Console.WriteLine("Nenhuma movimentação registrada.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"Data/Hora",-20} {"Tipo",-8} {"Cód.",-6} {"Qtd",-6} Descrição");
            Console.WriteLine(new string('-', 95));
            foreach (var m in _service.Movimentacoes)
                Console.WriteLine($"{m.Id,-5} {m.DataHora:dd/MM/yyyy HH:mm:ss} {m.Tipo,-8} " +
                                  $"{m.CodigoProduto,-6} {m.Quantidade,-6} {m.Descricao}");
        }
    }
}