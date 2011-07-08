using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNet
{
    public class Genome
    {
        private List<double> dna;

        public Genome(int numberOfGenes)
        {
            this.dna = new List<double>(numberOfGenes);
            for (int i = 0; i < numberOfGenes; i++)
            {
                dna.Add(NeuralNet.NextDouble());
            }
        }

        public Genome(List<double> dna)
        {
            this.dna = dna;
        }

        public List<double> Dna
        {
            get { return this.dna; }
        }

        public double Fitness { get; set; }
    }
}
