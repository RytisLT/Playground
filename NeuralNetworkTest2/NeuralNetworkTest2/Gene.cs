using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetworkTest2
{
    public class Gene
    {
        protected static readonly Random random = new Random();
        protected readonly bool[] bits;
            
        public Gene(int geneLength)
        {
            this.bits = new bool[geneLength];
            for (int i = 0; i < geneLength; i++)
            {
                this.bits[i] = random.Next(0, 2) == 1;
            }
        }

        public bool[] Bits
        {
            get { return this.bits; }
        }

        public int Decode()
        {
            int value = this.Decode(0, this.bits.Length);
            return value;
        }

        protected int Decode(int startIndex, int endIndex)
        {
            int value = 0;
            int multiplier = 1;
            for (int i = startIndex; i < endIndex; i++)
            {
                value += this.bits[i] ? multiplier : 0;
                multiplier *= 2;
            }

            return value;
        }
    }
}