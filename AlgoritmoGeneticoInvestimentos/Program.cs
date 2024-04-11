partial class Program
{
    static Random random = new Random();
    static int tamanhoPopulacao = 100;
    static int geracaoLimite = 100;
    static int capacidadeMochila = 50;

    static List<Item> items = new List<Item>()
    {
        new Item("Item1", 10, 60),
        new Item("Item2", 20, 100),
        new Item("Item3", 30, 120),
        new Item("Item4", 40, 150),
        new Item("Item5", 15, 70)
    };

    static void Main(string[] args)
    {
        List<Cromossomo> populacao = IniciarPopulacao();

        for (int geracao = 0; geracao < geracaoLimite; geracao++)
        {

            foreach (Cromossomo cromossomo in populacao)
            {
                Validar(cromossomo);
            }

            Cromossomo melhorIndividuo = populacao.OrderByDescending(c => c.Compatibilidade).First();
            Console.WriteLine($"Geração {geracao + 1}: Mais compatível = {melhorIndividuo.Compatibilidade}");
        }
    }

    // Inicialização da população
    static List<Cromossomo> IniciarPopulacao()
    {
        List<Cromossomo> population = new List<Cromossomo>();

        for (int i = 0; i < tamanhoPopulacao; i++)
        {
            Cromossomo chromosome = new Cromossomo();
            chromosome.Genes = IniciarGenes();
            population.Add(chromosome);
        }

        return population;
    }

    // Inicialização dos genes de um cromossomo
    static List<bool> IniciarGenes()
    {
        List<bool> genes = new List<bool>();

        foreach (var item in items)
        {
            genes.Add(random.NextDouble() < 0.5); 
        }

        return genes;
    }

    // Avaliação 
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

        // Diminuir a compatibilidade se o peso pasasra do limite
        if (pesoTotal > capacidadeMochila)
        {
            cromossomo.Compatibilidade = 0;
        }
        else
        {
            cromossomo.Compatibilidade = pesoTotal <= capacidadeMochila ? valorTotal : 0;
            
        }
    }

}
