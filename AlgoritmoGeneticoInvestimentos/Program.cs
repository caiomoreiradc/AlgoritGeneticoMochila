partial class Program
{
    static Random random = new Random();
    static int tamanhoPopulacao = 10;
    static int geracaoLimite = 30;
    static int capacidadeMochila = 300;
    static double taxaMutacao = 0.01;
    static int elitismoContagem = 2;

    static List<Item> items = new List<Item>()
    {
        new Item("Agenda", 40, 20),
        new Item("Notebook", 200, 30),
        new Item("Penal", 20, 25),
        new Item("Caderno", 40, 15),
        new Item("Régua", 15, 10)
    };

    static void Main(string[] args)
    {
        List<Cromossomo> populacao = IniciarPopulacao();

        // Cabeçalho 
        MostrarCabecalho();

        for (int geracao = 0; geracao < geracaoLimite; geracao++)
        {
            foreach (Cromossomo cromossomo in populacao)
            {
                Validar(cromossomo);
            }

            List<Cromossomo> novaPopulacao = new List<Cromossomo>();

            // elitismo
            List<Cromossomo> melhoresIndividuos = populacao
                .OrderByDescending(c => c.Compatibilidade)
                .Take(elitismoContagem)
                .ToList();

            novaPopulacao.AddRange(melhoresIndividuos);

            // cruzamento aleatório
            while (novaPopulacao.Count < tamanhoPopulacao)
            {
                int indice1 = random.Next(tamanhoPopulacao);
                int indice2 = random.Next(tamanhoPopulacao);

                while (indice2 == indice1) //verificar os indices
                {
                    indice2 = random.Next(tamanhoPopulacao);
                }

                Cromossomo pai1 = populacao[indice1];
                Cromossomo pai2 = populacao[indice2];

                List<Cromossomo> filhos = Cruzamento(pai1, pai2);

                // Adicionar na população
                foreach (var filho in filhos)
                {
                    if (novaPopulacao.Count < tamanhoPopulacao)
                    {
                        novaPopulacao.Add(filho);
                    }
                }
            }

            populacao = novaPopulacao;

            Cromossomo melhorIndividuo = populacao.OrderByDescending(c => c.Compatibilidade).First();
            string itensEscolhidos = ObterItensEscolhidos(melhorIndividuo);

            if (melhorIndividuo.Compatibilidade == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0,-10} {1,-10} {2}", "|" + (geracao + 1) + "|", "|" + "Excedeu" + "|", "|" + itensEscolhidos + "|");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("{0,-10} {1,-10} {2}", "|" + (geracao + 1) + "|", "|" + melhorIndividuo.Compatibilidade + "|", "|" + itensEscolhidos + "|");
                Console.ResetColor();
            }
        }
    }

    private static void MostrarCabecalho()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("------------------------------------------------------------------------------------------------------");
        Console.WriteLine("{0,-10} {1,-10} {2}", "|Geração|", "|Valor|", "|Itens|");
        Console.WriteLine("------------------------------------------------------------------------------------------------------");
        Console.ResetColor();
    }

    static List<Cromossomo> IniciarPopulacao()
    {
        List<Cromossomo> population = new List<Cromossomo>(); //lista da população

        for (int i = 0; i < tamanhoPopulacao; i++)
        {
            Cromossomo chromosome = new Cromossomo();
            chromosome.Genes = IniciarGenes();
            population.Add(chromosome);
        }

        return population;
    }

    static List<bool> IniciarGenes()
    {
        List<bool> genes = new List<bool>();

        foreach (var item in items)
        {
            genes.Add(random.NextDouble() < 0.5);
        }

        return genes;
    }

    static void Validar(Cromossomo cromossomo)
    {
        int pesoTotal = 0;
        int valorTotal = 0;

        for (int i = 0; i < items.Count; i++)
        {
            if (cromossomo.Genes[i])
            {
                pesoTotal += items[i].Peso;
                valorTotal += items[i].Valor;
            }
        }

        // calcular os cromossomos que passam da cappacidade da mochila
        if (pesoTotal > capacidadeMochila)
        {
            cromossomo.Compatibilidade = 0;
        }
        else
        {
            cromossomo.Compatibilidade = valorTotal;
        }
    }

    static List<Cromossomo> Cruzamento(Cromossomo pai1, Cromossomo pai2)
    {
        int pontoCorte = random.Next(pai1.Genes.Count);

        List<bool> genesFilho1 = new List<bool>();
        List<bool> genesFilho2 = new List<bool>();

        for (int i = 0; i < pai1.Genes.Count; i++)
        {
            if (i < pontoCorte)
            {
                genesFilho1.Add(pai1.Genes[i]);
                genesFilho2.Add(pai2.Genes[i]);
            }
            else
            {
                genesFilho1.Add(pai2.Genes[i]);
                genesFilho2.Add(pai1.Genes[i]);
            }
        }

        Cromossomo filho1 = new Cromossomo { Genes = genesFilho1 };
        Cromossomo filho2 = new Cromossomo { Genes = genesFilho2 };

        return new List<Cromossomo> { filho1, filho2 };
    }

    static string ObterItensEscolhidos(Cromossomo cromossomo)
    {
        List<string> itensEscolhidos = new List<string>();
        for (int i = 0; i < cromossomo.Genes.Count; i++)
        {
            if (cromossomo.Genes[i])
            {
                itensEscolhidos.Add(items[i].Nome + " (Valor: " + items[i].Valor + ")");
            }
        }
        return string.Join(", ", itensEscolhidos);
    }
}
