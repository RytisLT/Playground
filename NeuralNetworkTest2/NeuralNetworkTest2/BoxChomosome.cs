using System;
using System.Collections.Generic;

namespace NeuralNetworkTest2
{
    public class BoxChomosome : Chromosome<BoxGene>
    {
        public BoxChomosome(int numberOfGenes) : base(numberOfGenes)
        {
        }

        public BoxChomosome(int numberOfGenes, bool[] genome)
            : base(numberOfGenes)
        {
            this.SetGenome(genome);
        }

        public new List<Point2D> Decode()
        {
            var result = new List<Point2D>();
            foreach (var gene in this.Genes)
            {
                result.Add(new Point2D(gene.X, gene.Y));
            }

            return result;
        }

        public bool[] GetGenome()
        {
            var genome = new bool[this.Genes.Length * BoxGene.GeneLength];
            int n = 0;
            for (int i = 0; i < this.Genes.Length; i++)
            {
                for (int j = 0; j < this.Genes[i].Bits.Length; j++)
                {
                    genome[n++] = this.Genes[i].Bits[j];
                }
            }

            return genome;
        }

        public void SetGenome(bool[] genome)
        {
            int n = 0;
            for (int i = 0; i < this.Genes.Length; i++)
            {
                for (int j = 0; j < this.Genes[i].Bits.Length; j++)
                {
                    this.Genes[i].Bits[j] = genome[n++];
                }
            }
        }

        public void Crossover(BoxChomosome dad, out BoxChomosome baby1, out BoxChomosome baby2)
        {
            if (Algorithm.Random.NextDouble() > Algorithm.CrossoverRate || this == dad)
            {
                baby1 = this;
                baby2 = dad;
            }
            else
            {
                var corssoverPoint = (int)(dad.Genes.Length * BoxGene.GeneLength * Algorithm.CrossoverRate);
                var dadGenome = this.GetGenome();
                var momGenome = dad.GetGenome();
                var baby1Genome = new bool[dadGenome.Length];
                var baby2Genome = new bool[dadGenome.Length];
                Array.Copy(dadGenome, baby1Genome, corssoverPoint);
                Array.Copy(momGenome, baby2Genome, corssoverPoint);

                var length = dadGenome.Length - corssoverPoint;
                Array.Copy(dadGenome, corssoverPoint, baby2Genome, corssoverPoint, length);
                Array.Copy(momGenome, corssoverPoint, baby1Genome, corssoverPoint, length);                

                this.Mutate(baby1Genome);
                this.Mutate(baby2Genome);

                baby1 = new BoxChomosome(this.Genes.Length, baby1Genome);
                baby2 = new BoxChomosome(this.Genes.Length, baby2Genome);
            }
        }

        private void Mutate(bool[] genome)
        {
            for (int i = 0; i < genome.Length; i++)
            {
                var mutate = Algorithm.Random.NextDouble() < Algorithm.MutationRate;
                if (mutate)
                {
                    genome[i] = !genome[i];
                }
            }
        }
    }
}