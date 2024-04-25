using System;
using System.Collections.Generic;

partial class Program
{
    static Random random = new Random();
    static int tamanhoPopulacao = 10;
    static int geracaoLimite = 1;
    static int capacidadeMochila = 300;

    static List<Item> items = new List<Item>()
    {
        new Item("Caderno", 40, 150),
        new Item("Notebook", 200, 300),
        new Item("Penal", 10, 200),
        new Item("Caderno2", 40, 150),
        new Item("Caderno3", 40, 150)
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


            for (int i = 0; i < 4; i++)
            {
                int indice1 = random.Next(tamanhoPopulacao);
                int indice2 = random.Next(tamanhoPopulacao);
                while (indice2 == indice1) 
                {
                    indice2 = random.Next(tamanhoPopulacao);
                }

                Cromossomo pai1 = populacao[indice1];
                Cromossomo pai2 = populacao[indice2];

                Cruzamento(pai1, pai2);
            }
        }
    }

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

        if (pesoTotal > capacidadeMochila)
        {
            cromossomo.Compatibilidade = 0;
        }
        else
        {
            cromossomo.Compatibilidade = pesoTotal <= capacidadeMochila ? valorTotal : 0;
        }
    }

    static void Cruzamento(Cromossomo pai1, Cromossomo pai2)
    {
        int pontoCorte = random.Next(pai1.Genes.Count); 

        for (int i = pontoCorte; i < pai1.Genes.Count; i++)
        {
            bool temp = pai1.Genes[i];
            pai1.Genes[i] = pai2.Genes[i];
            pai2.Genes[i] = temp;
        }
    }

}
