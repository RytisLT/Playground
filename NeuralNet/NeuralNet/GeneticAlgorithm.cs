using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNet
{
    public class GeneticAlgorithm
    {        
        private List<Genome> population;
        private Genome fitest;
        private double mutationRate;
        private double crossoverRate;
        private int generation;
        private double maxPerturbation;
        private int numberOfCopiesElite;

        public double TotalFitness { get; set; }
        public double BestFitness { get; set; }
        public double AverageFitness { get; set; }
        public double WorstFitness { get; set; }

        public GeneticAlgorithm(List<Genome> population, double mutationRate, double crossoverRate, double maxPerturbation, int numberOfCopiesElite)
        {
            this.population = population;
            this.mutationRate = mutationRate;
            this.crossoverRate = crossoverRate;
            this.maxPerturbation = maxPerturbation;
            this.numberOfCopiesElite = numberOfCopiesElite;            
        }

        public List<Genome> Epoch()
        {
            this.ResetFitness();
            population.Sort(Compare);
            Calculate();
            var newPopulation = new List<Genome>();

            int populationSize = population.Count;
            for (int i = 0; i < numberOfCopiesElite; i++)
            {
                population.Add(population[populationSize - 1 - i]);
            }

            population.Sort(Compare);

            while (newPopulation.Count < populationSize)
            {
                var mum = GetGenomeRoulette(TotalFitness);
                var dad = GetGenomeRoulette(TotalFitness);
                Genome baby1;
                Genome baby2;
                this.Crossover(mum, dad, out baby1, out baby2);
                this.Mutate(baby1);
                this.Mutate(baby2);
                newPopulation.Add(baby1);
                newPopulation.Add(baby2);
            }
            population = newPopulation;
            return population;
        }

        private Genome GetGenomeRoulette(double totalFitness)
        {
            double slice = NeuralNet.NextDouble() * totalFitness;

            Genome theChosenOne = null;
            double fitnessSoFar = 0d;

            for (int i = 0; i < population.Count; i++)
            {
                fitnessSoFar += population[i].Fitness;
                if (fitnessSoFar > slice)
                {
                    theChosenOne = population[i];
                    break;
                }
            }
            if (theChosenOne == null)
            {
                throw new ApplicationException("Shouldn't be null");
            }
            return theChosenOne;
        }

        private void Calculate()
        {
            var highest = new Genome(null){Fitness = BestFitness};
            var lowest = new Genome(null){Fitness = WorstFitness};
            for (int i = 0; i < population.Count; i++)
            {
                var genome = this.population[i];
                if (genome.Fitness > highest.Fitness)
                {
                    highest = genome;
                    BestFitness = highest.Fitness;
                }

                if (genome.Fitness < lowest.Fitness)
                {
                    lowest = genome;
                    WorstFitness = lowest.Fitness;
                }
                TotalFitness += genome.Fitness;
            }
            AverageFitness = TotalFitness / population.Count;
            if (TotalFitness == 0)
            {
                throw new ApplicationException("TotalFitness cannot be 0");
            }
        }

        private int Compare(Genome lhs, Genome rhs)
        {
            var result = (lhs.Fitness*10000) - (rhs.Fitness*10000);
            return (int) result;
        }

        private void ResetFitness()
        {
            TotalFitness = 0;
            BestFitness = double.MinValue;
            WorstFitness = double.MaxValue;
            AverageFitness = 0;
        }

        private void Crossover(Genome mum, Genome dad, out Genome baby1, out Genome baby2)
        {
            if (NeuralNet.NextDouble() > crossoverRate || mum ==dad)            
            {
                baby1 = new Genome(mum.Dna);
                baby2 = new Genome(dad.Dna);
            }
            else
            {
                var dnaLength = mum.Dna.Count;
                int crossoverPoint =  (int) (NeuralNet.NextDouble() * dnaLength);
                baby1 = new Genome(new List<double>());
                baby2 = new Genome(new List<double>());

                for (int i = 0; i < crossoverPoint; i++)
                {
                    baby1.Dna.Add(mum.Dna[i]);
                    baby2.Dna.Add(dad.Dna[i]);
                }
                for (int i = crossoverPoint; i < dnaLength; i++)
                {
                    baby1.Dna.Add(dad.Dna[i]);
                    baby2.Dna.Add(mum.Dna[i]);
                }
            }
        }

        private void Mutate(Genome genome)
        {
            for (int i = 0; i < genome.Dna.Count; i++)
            {
                if (NeuralNet.NextDouble() < mutationRate)
                {
                    genome.Dna[i] += NeuralNet.RandomClamped() * maxPerturbation;
                }
            }
        }

        
    }
}
