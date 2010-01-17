using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetworkTest2
{
    public abstract class Chromosome<TGene> where TGene : Gene, new()
    {
        private readonly TGene[] genes;


        public Chromosome(int numberOfGenes)
        {
            this.genes = new TGene[numberOfGenes];
            for (int i = 0; i < numberOfGenes; i++)
            {
                this.genes[i] = new TGene();
            }
        }


        protected TGene[] Genes
        {
            get { return this.genes; }
        }

        /// <summary>
        /// Gets or sets Fitness.
        /// </summary>
        public double Fitness { get; set; }

        public int[] Decode()
        {
            var value = new int[this.genes.Length];
            for (int i = 0; i < this.genes.Length; i++)
            {
                value[i] = this.genes[i].Decode();
            }

            return value;
        }        
    }
}
