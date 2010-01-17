using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetworkTest
{
    public class Gene
    {
        private static readonly Random random = new Random();
        private readonly bool[] bits;
            
        public Gene(int geneLength)
        {
            this.bits = new bool[geneLength];
            for (int i = 0; i < geneLength; i++)
            {
                this.bits[i] = random.Next(0, 1) == 1;
            }
        }

        public int Decode()
        {
            int value = 0;
            int multiplier = 1;
            for (int i = 0; i < this.bits.Length; i++)
            {
                value += this.bits[i] ? multiplier : 0;
                multiplier *= 2;
            }

            return value;
        }
    }
}
