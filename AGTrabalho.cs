using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Trabalho{


    public class Individuo{

        public const int TAM_CROMOSSOMO = 44;
        public const int TAM_COORD = 22;
        public const int DOM_MIN = -100;
        public const int DOM_MAX = 100;

        static readonly Random rand = new Random();

        public Individuo()
        {
            gerarGenes();
            DecodificacaoEAvaliacao();
        }

        public double fitness {get;set;}
        public double x;
        public double y;

    // hardcodado o genotipo do slide pra teste
        public int[] Gene { get; set; } = new int[TAM_CROMOSSOMO];
        

        public void gerarGenes()
        {
            for(int i = 0; i < this.Gene.Length; i++)
            {
                this.Gene[i] = rand.Next(2);
            }
        }

        public Individuo Clonar()
        {
            var clone = new Individuo();
            clone.Gene = (int[]) this.Gene.Clone();
            clone.x = this.x;
            clone.y = this.y;
            clone.fitness = this.fitness;

            return clone;
        }
        public void DecodificacaoEAvaliacao()
        {
            var xBin = Gene[..TAM_COORD];
            var yBin = Gene[TAM_COORD..];
            
            var xDec = BinArrayParaReal(xBin);
            var yDec = BinArrayParaReal(yBin);

            this.x = xDec * ((DOM_MAX - DOM_MIN) / (Math.Pow(2, TAM_COORD) - 1)) + DOM_MIN;
            this.y = yDec * ((DOM_MAX - DOM_MIN) / (Math.Pow(2, TAM_COORD) - 1)) + DOM_MIN;

            this.fitness = CalcularFuncaoF6(this.x, this.y);
        }

        public double BinArrayParaReal(int[] binArray)
        {
            double valor = 0;

            for(int i = binArray.Length, y = 0; i > 0; i--, y++)
            {
                valor += binArray[y] * Math.Pow(2, i - 1);
            }
            
            return valor;
        }

        public double CalcularFuncaoF6(double x, double y)
        {
            double numerador = Math.Pow(Math.Sin(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2))), 2) - 0.5;
            double denominador = Math.Pow(1.0 + 0.001 * (Math.Pow(x, 2) + Math.Pow(y, 2)) , 2);
            return 0.5 - (numerador / denominador);
        }

    };

 
    class Program
    {
        static readonly Random rand = new Random();
        static void Main(string[] args)
        {
            const int TOTAL_POP = 100;
            const int NUM_GERACOES = 40;


            Individuo[] populacao = new Individuo[TOTAL_POP];
         
            // Inicializando a primeira população
            for(int i = 0; i < populacao.Length; i++){
               populacao[i] = new Individuo(); 
            }   


            Console.WriteLine($"{"Geração",-8} | {"Melhor F6",-12} | {"Média F6",-12} | {"Melhor X",-14} | {"Melhor Y",-14}");
            Console.WriteLine(new string('-', 68));

            for(int geracao = 1; geracao <= NUM_GERACOES; geracao++)
            {
                List<Individuo> novaPopulacao = new List<Individuo>(TOTAL_POP);

                Individuo melhorAtual = ObterMelhorIndividuo(populacao);
                novaPopulacao.Add(melhorAtual.Clonar());

                while(novaPopulacao.Count < TOTAL_POP)
                {
                    Individuo pai1 = Roleta(populacao);
                    Individuo pai2 = Roleta(populacao);

                    var (filho1, filho2) = Crossover(pai1, pai2);

                    Mutacao(filho1);
                    Mutacao(filho2);

                    novaPopulacao.Add(filho1);
                    if(novaPopulacao.Count < TOTAL_POP)
                    {
                        novaPopulacao.Add(filho2);
                    }
                }
                
                populacao = novaPopulacao.ToArray();




                Individuo melhorDaRodada = ObterMelhorIndividuo(populacao);
                double mediaFitness = populacao.Average(ind => ind.fitness);
                
                Console.WriteLine($"{geracao,-8} | {melhorDaRodada.fitness,-12:F6} | {mediaFitness,-12:F6} | {melhorDaRodada.x,-14:F6} | {melhorDaRodada.y,-14:F6}");


            }

            Individuo campeao = ObterMelhorIndividuo(populacao);
            Console.WriteLine("\n=== MELHOR INDIVÍDUO ENCONTRADO ===");
            Console.WriteLine($"X: {campeao.x:F6}");
            Console.WriteLine($"Y: {campeao.y:F6}");
            Console.WriteLine($"Fitness F6: {campeao.fitness:F8} (Ótimo teórico = 1.0 em X=0, Y=0)");

        }



        static public Individuo? Roleta(Individuo[] populacao)
        {
            double somaAptidoes = 0;
            double somaAcumulada = 0;

            Individuo? paiEscolhido = null;
            
            foreach(Individuo ind in populacao)
            {
                somaAptidoes += ind.fitness;
            }
            
            double valorEscolhido = rand.NextDouble() * somaAptidoes;

            foreach(Individuo ind in populacao)
            {
                somaAcumulada += ind.fitness;
                if(somaAcumulada >= valorEscolhido)
                {
                    paiEscolhido = ind;
                    break;
                }
            }
            return paiEscolhido ?? populacao[^1];
        }


        static public (Individuo, Individuo) Crossover(Individuo pai1, Individuo pai2)
        {
            const double TAXA_CROSSOVER = 0.65;

            Individuo filho1 = pai1.Clonar();
            Individuo filho2 = pai2.Clonar();
            
            if(rand.NextDouble() <= TAXA_CROSSOVER)
            {
                int pontoDeCorte = rand.Next(1, Individuo.TAM_CROMOSSOMO);

                filho1.Gene = [.. pai1.Gene[..pontoDeCorte], .. pai2.Gene[pontoDeCorte..]];
                filho2.Gene = [.. pai2.Gene[..pontoDeCorte], .. pai1.Gene[pontoDeCorte..]];

                filho1.DecodificacaoEAvaliacao();
                filho2.DecodificacaoEAvaliacao();
            }


            return (filho1, filho2);
        }

        static public void Mutacao(Individuo individuo)
        {
            const double TAXA_MUTACAO = 0.008;

            for(int i = 0; i < Individuo.TAM_CROMOSSOMO; i++)
            {
                if(rand.NextDouble() < TAXA_MUTACAO)
                {
                    individuo.Gene[i] = 1 - individuo.Gene[i];
                }
            }
            individuo.DecodificacaoEAvaliacao();
        }

        static public Individuo ObterMelhorIndividuo(Individuo[] populacao)
        {
            Individuo melhor = populacao[0];
            foreach(var ind in populacao)
            {
                if(ind.fitness > melhor.fitness)
                {
                    melhor = ind;
                }
            }
            return melhor;
        }
    }
}
