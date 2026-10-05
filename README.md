TesteTarget



Projeto em C# (.NET 8) com a resolução de 3 exercícios propostos.



Exercícios



Exercício 1 — Cálculo de Comissão de Vendas

Lê um arquivo JSON com registros de vendas de um time de vendas e calcula a comissão

de cada vendedor seguindo a regra:

\- Vendas abaixo de R$ 100,00 → sem comissão

\- Vendas entre R$ 100,00 e R$ 499,99 → 1% de comissão

\- Vendas a partir de R$ 500,00 → 5% de comissão



Arquivo JSON: `vendas.json`

Classe: `Exercicio1.cs`



\---



Exercício 2 — Controle de Estoque

Permite lançar movimentações de entrada e saída de mercadorias em um depósito.

Cada movimentação possui:

\- Número identificador único (ID sequencial)

\- Descrição do tipo da movimentação

\- Tipo (Entrada ou Saída)

\- Quantidade



Ao final de cada movimentação, o programa exibe o estoque final do produto movimentado.



Arquivo JSON: `estoque.json`

Classe:\*\* `Exercicio2.cs`



\---



Exercício 3 — Cálculo de Juros por Atraso

A partir de um valor e de uma data de vencimento, calcula os juros na data de hoje,

considerando multa de \*\*2,5% ao dia\*\* sobre o valor original (juros simples).



Classe: `Exercicio3.cs`



\---



Como executar



Como o projeto contém os três exercícios no mesmo repositório, cada um possui o seu

próprio método `Main`. Para escolher qual exercício rodar, altere a propriedade

`<StartupObject>` no arquivo `TesteTarget.csproj`:



```xml

<StartupObject>TesteTarget.Exercicio1</StartupObject>   <!-- Para rodar o Exercício 1 -->

<StartupObject>TesteTarget.Exercicio2</StartupObject>   <!-- Para rodar o Exercício 2 -->

<StartupObject>TesteTarget.Exercicio3</StartupObject>   <!-- Para rodar o Exercício 3 -->

