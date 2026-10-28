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

        public Individuo(){}

        public double fitness {get;set;}
        public double x;
        public double y;

    // hardcodado o genotipo do slide pra teste
        public int[] Gene { get; set; } = new int[]
        {
            0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1,
            1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 1, 1
     
        };
        
        public int Dec()
        {
            var xBin = Gene[..TAM_COORD];
            var yBin = Gene[TAM_COORD..];
            
            var xDec = BinArrayParaReal(xBin);
            var yDec = BinArrayParaReal(yBin);

            x = xDec * ((DOM_MAX - DOM_MIN) / (Math.Pow(2, TAM_COORD) - 1)) + DOM_MIN;
            y = yDec * ((DOM_MAX - DOM_MIN) / (Math.Pow(2, TAM_COORD) - 1)) + DOM_MIN;

            return 0;
        }

        public double BinArrayParaReal(int[] binArray)
        {
            double valor = 0;

            for(int i = binArray.Length, y = 0; i > 0; i--, y++)
            {
                Console.WriteLine(binArray[i - 1]);
                var ATUAL = binArray[i - 1];
                valor += (binArray[y] * Math.Pow(2, i - 1));
            }
            return valor;
        }

    };



 
    class Program
    {
        static void Main(string[] args)
        {
            Individuo ind = new Individuo();
            ind.Dec();
            Console.WriteLine(ind.x);
            Console.WriteLine(ind.y);
        }
    }
}
