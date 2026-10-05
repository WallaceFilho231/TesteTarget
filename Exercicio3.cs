using System;
using System.Globalization;

namespace TesteTarget
{
    public class Exercicio3
    {       
        private const decimal TaxaJurosPorDia = 0.025m; 

        public static void Main(string[] args)
        {
            Console.WriteLine("===== CÁLCULO DE JUROS POR ATRASO =====");
            Console.WriteLine("(Multa de 2,5% ao dia sobre o valor original)");
            Console.WriteLine();
           
            Console.Write("Informe o valor (ex.: 1500,00): ");
            string entradaValor = Console.ReadLine() ?? string.Empty;

            if (!decimal.TryParse(entradaValor, NumberStyles.Number,
                                  new CultureInfo("pt-BR"), out decimal valor))
            {
                Console.WriteLine("Valor inválido. Use algo como 1500,00.");
                return;
            }

            if (valor <= 0)
            {
                Console.WriteLine("O valor deve ser maior que zero.");
                return;
            }
   
            Console.Write("Informe a data de vencimento (dd/MM/aaaa): ");
            string entradaData = Console.ReadLine() ?? string.Empty;

            if (!DateTime.TryParseExact(entradaData, "dd/MM/yyyy",
                                        CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out DateTime vencimento))
            {
                Console.WriteLine("Data inválida. Use o formato dd/MM/aaaa (ex.: 10/10/2026).");
                return;
            }
            
            DateTime hoje = DateTime.Today;
            
            if (vencimento.Date >= hoje)
            {
                Console.WriteLine();
                Console.WriteLine(">> O título ainda não está vencido. Não há juros a pagar.");
                Console.WriteLine($"   Data de vencimento: {vencimento:dd/MM/yyyy}");
                Console.WriteLine($"   Data de hoje......: {hoje:dd/MM/yyyy}");
                Console.WriteLine($"   Valor a pagar.....: R$ {valor.ToString("N2", new CultureInfo("pt-BR"))}");
                return;
            }
            int diasAtraso = (hoje - vencimento.Date).Days;

            // Aqui estou calculando como Juros Simples. Para calcular com Juros Compostos, basta alterar a fórmula para:
            // decimal juros = valor * ((decimal)Math.Pow(1 + (double)TaxaJurosPorDia, diasAtraso) - 1);

            decimal juros = valor * TaxaJurosPorDia * diasAtraso;
            decimal total = valor + juros;
         
            var br = new CultureInfo("pt-BR");
            Console.WriteLine();
            Console.WriteLine("===== RESULTADO =====");
            Console.WriteLine($"  Valor original........: R$ {valor.ToString("N2", br)}");
            Console.WriteLine($"  Data de vencimento....: {vencimento:dd/MM/yyyy}");
            Console.WriteLine($"  Data de hoje..........: {hoje:dd/MM/yyyy}");
            Console.WriteLine($"  Dias em atraso........: {diasAtraso}");
            Console.WriteLine($"  Taxa de juros.........: {TaxaJurosPorDia * 100:N2}% ao dia"); 
            Console.WriteLine($"  JUROS.................: R$ {juros.ToString("N2", br)}");
            Console.WriteLine($"  TOTAL A PAGAR.........: R$ {total.ToString("N2", br)}");
        }
    }
}