using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetworkTest
{
    public class Chromosome
    {
        private const int ChromosomeLength = 300;
        private const float CrossoverRate = 0.7f;
        private const int GeneLength = 4;
        private const float MutationRate = 0.101f;
        private static readonly Dictionary<string, string> GeneCode = new Dictionary<string, string>();
        private static readonly Random Random = new Random();
        private readonly string encodedChromosome = string.Empty;
        private double fitness = 0.0;

        static Chromosome()
        {
            GeneCode.Add("0", "0000");
            GeneCode.Add("1", "0001");
            GeneCode.Add("2", "0010");
            GeneCode.Add("3", "0011");
            GeneCode.Add("4", "0100");
            GeneCode.Add("5", "0101");
            GeneCode.Add("6", "0110");
            GeneCode.Add("7", "0111");
            GeneCode.Add("8", "1000");
            GeneCode.Add("9", "1001");
            GeneCode.Add("+", "1010");
            GeneCode.Add("-", "1011");
            GeneCode.Add("*", "1100");
            GeneCode.Add("/", "1101");
        }

        public Chromosome()
        {
            var chromosome = new StringBuilder();
            for (int i = 0; i < ChromosomeLength; i++)
            {
                if (this.GetRandomNumber() > 0.5)
                {
                    chromosome.Append('1');
                }
                else
                {
                    chromosome.Append('0');
                }
            }

            this.encodedChromosome = chromosome.ToString();
        }

        public Chromosome(string encodedChromosome)
        {
            if (encodedChromosome.Length != ChromosomeLength)
            {
                throw new ApplicationException("Invalid chromosome mutation occured");
            }

            this.encodedChromosome = encodedChromosome;
        }

        public string EncodedChromosome
        {
            get { return this.encodedChromosome; }
        }

        public double Fitness
        {
            get { return this.fitness; }
        }

        public Chromosome Mutate(Chromosome chromosome)
        {
            int chromosomeLength = this.EncodedChromosome.Length;
            if (chromosome.EncodedChromosome.Length != chromosomeLength)
            {
                throw new ApplicationException("Chromosomes can't make babies");
            }

            string childChromosome = this.EncodedChromosome;
            int crossoverBeginIndex = Random.Next(chromosomeLength);
            var array = childChromosome.ToCharArray();

            if (this.GetRandomNumber() < CrossoverRate)
            {
                for (int i = crossoverBeginIndex; i < chromosomeLength; ++i)
                {
                    array[i] = chromosome.EncodedChromosome[i];
                }
            }

            for (int i = crossoverBeginIndex; i < chromosomeLength; ++i)
            {
                if (this.GetRandomNumber() < MutationRate)
                {
                    array[i] = array[i] == '0' ? '1' : '0';
                }
            }

            var child = new Chromosome(new string(array));
            return child;
        }

        private double GetRandomNumber()
        {
            return Random.NextDouble();
        }

        public string Decode(string chromosome)
        {
            if (chromosome.Length % GeneLength != 0)
            {
                throw new ApplicationException("Invalid chromosome");
            }

            var decodedChromosome = new StringBuilder();
            int numberOfGenes = chromosome.Length / GeneLength;
            bool wasNumber = false;
            for (int i = 0; i < numberOfGenes; i+= 4)
            {
                string gene = chromosome.Substring(i, GeneLength);
                string decodedGene = this.DecodeGene(gene);
                if (!string.IsNullOrEmpty(decodedGene))
                {
                    bool isNumber = this.IsNumber(decodedGene);
                    if (wasNumber ^ isNumber)
                    {
                        decodedChromosome.Append(decodedGene);
                        wasNumber = isNumber;
                    }
                }
            }

            string decoded = decodedChromosome.ToString();
            if (!this.IsNumber(decoded[decoded.Length - 1].ToString()))
            {
                decoded = decoded.Substring(0, decoded.Length - 1);
            }
            return decoded;
        }

        public int Value()
        {
            string decodedChromosome = this.Decode(this.encodedChromosome);
            int? result = this.ComputeValue(decodedChromosome);
            Console.WriteLine("{0}={1}", decodedChromosome, result);
            return (int) result;
        }

        private int? ComputeValue(string decodedChromosome)
        {
            int? result = null;
            char? @operator = null;
            for (int i = 0; i < decodedChromosome.Length; i++)
            {
                int number;
                if (int.TryParse(decodedChromosome[i].ToString(), out number))
                {
                    if (result == null)
                    {
                        result = decodedChromosome[i];
                    }
                    else
                    {
                        switch (@operator)
                        {
                            case '+':
                                result += decodedChromosome[i];
                                break;
                            case '-':
                                result -= decodedChromosome[i];
                                break;
                            case '*':
                                result *= decodedChromosome[i];
                                break;
                            case '/':
                                result /= decodedChromosome[i];
                                break;
                        }
                    }
                }
                else
                {
                    @operator = decodedChromosome[i];
                }
            }
            if (result == null)
            {
                result = 0;
            }
            return result;
        }

        private bool IsNumber(string gene)
        {
            int result;
            return int.TryParse(gene, out result);
        }

        public void ComputeFitness(int target)
        {
            int value = this.Value();
            if (value == target)
            {
                this.fitness = 999;
            }
            else
            {
                this.fitness = 1 / ((double)target - value);   
            }            
        }


        private string DecodeGene(string gene)
        {
            string result = string.Empty;
            foreach (var code in GeneCode)
            {
                if (code.Value == gene)
                {
                    result = code.Key;
                }
            }

            return result;
        }
    }
}