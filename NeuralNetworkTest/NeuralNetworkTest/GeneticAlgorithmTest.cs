using System;
using System.Globalization;
using System.Text;

namespace NeuralNetworkTest
{
    using System.Collections.Generic;

    public class GeneticAlgorithmTest
    {
        private const int PopulationSize = 1000;
        private Chromosome[] generation;
        private Random random = new Random();

        public GeneticAlgorithmTest()
        {

            bool found = false;
            int target = 1547881;
            generation = new Chromosome[PopulationSize];
            for (int i = 0; i < PopulationSize; i++)
            {
                generation[i] = new Chromosome();
                generation[i].ComputeFitness(target);
            }

            while (!found)
            {
                double totalFitness = 0.0f;
                foreach (var chromosome in generation)
                {
                    totalFitness += chromosome.Fitness;
                    if (chromosome.Fitness == 999)
                    {
                        Console.WriteLine("------------------");
                        Console.WriteLine("Result is {0}={1}", chromosome.Decode(chromosome.EncodedChromosome), target);
                        Console.WriteLine("------------------");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();

                        found = true;
                        break;
                    }
                }
                Console.WriteLine("Total fitness {0}", totalFitness);
                Chromosome[] newPopulation = new Chromosome[PopulationSize];
                int newSize = 0;
                do
                {
                    Chromosome parent1 = this.Roulette(totalFitness, this.generation);
                    Chromosome parent2 = this.Roulette(totalFitness, this.generation);
                    Chromosome child = parent1.Mutate(parent2);
                    child.ComputeFitness(target);
                    newPopulation[newSize++] = child;
                } while (newSize < PopulationSize);
                generation = newPopulation;
            }
        }        

        private Chromosome Roulette(double totalFittness, IEnumerable<Chromosome> population)
        {
            double slice = totalFittness * this.random.NextDouble();
            double fitnessSoFar = 0.0;
            Chromosome result = null;
            foreach (var chromosome in population)
            {
                fitnessSoFar += chromosome.Fitness;
                if (fitnessSoFar > slice)
                {
                    result = chromosome;
                    break;
                }
            }

            return result;
        }
    }
}