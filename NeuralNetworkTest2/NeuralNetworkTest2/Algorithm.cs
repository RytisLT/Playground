using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NeuralNetworkTest2
{
    public class Algorithm
    {        
        public const double MutationRate = 0.15;
        public const double CrossoverRate = 0.7;
        private const int Population = 1000;
    
        private readonly List<Box> cargo = new List<Box>();
        private List<BoxChomosome> population;
        private int width = 60;
        private int length = 120;
        private BoxChomosome best;
        
        public static Random Random = new Random();        
        
        private readonly Comparison<BoxChomosome> comparison;
        

        public Algorithm()
        {            
            this.comparison = this.Compare;
        }

        public List<Box> Cargo
        {
            get { return this.cargo; }
        }

        private int Compare(BoxChomosome x, BoxChomosome y)
        {
            return (int)(x.Fitness - y.Fitness);
        }

        public BoxChomosome Best
        {
            get { return this.best; }
        }

        public void AddBox (Box box)
        {
            cargo.Add(box);
        }

        public void Run()
        {
            this.population = new List<BoxChomosome>();
            for (int i = 0; i < Population; i++)
            {
                this.population.Add(new BoxChomosome(this.cargo.Count));
            }   
        }

        public void Epoch()
        {
            double totalFitness = 0;
            BoxChomosome populationBest = null;
            foreach (var chromosome in this.population)
            {
                this.CalculateFitness(chromosome);
                totalFitness += chromosome.Fitness;
                if (populationBest == null || populationBest.Fitness < chromosome.Fitness)
                {
                    populationBest = chromosome;
                }
            }

            var avarageFitness = totalFitness / this.population.Count;

            Console.WriteLine("Avarage fitness {0}", avarageFitness);
            Console.WriteLine("Population best fitness {0}", populationBest.Fitness);

            if (this.best == null || this.best.Fitness < populationBest.Fitness)
            {
                this.best = populationBest;
            }
            if (this.best != null)
            {            
                Console.WriteLine("Best fitness {0}", this.best.Fitness);
                Console.WriteLine("------");
                var decode = this.Best.Decode();
                foreach (var point in decode)
                {
                    Console.WriteLine("X{0} Y{1}", point.X, point.Y);
                }
            }

            var oldPopulation = this.population;

            oldPopulation.Sort(this.comparison);
            this.population = new List<BoxChomosome>();
            while (this.population.Count < Population)
            {                            
                BoxChomosome mum = this.GetChromosome(oldPopulation, totalFitness);
                BoxChomosome dad = this.GetChromosome(oldPopulation, totalFitness);
                BoxChomosome baby1;
                BoxChomosome baby2;
                mum.Crossover(dad, out baby1, out baby2);
                this.population.Add(baby1);
                this.population.Add(baby2);
            }

            Debug.Assert(this.population.Count == Population, "Invalid population length");
        }

        private BoxChomosome GetChromosome(List<BoxChomosome> population, double fitness)
        {
            double slice = Random.NextDouble() * fitness;
            BoxChomosome result = null;
            double fitnessSoFar = 0;
            for (int i = 0; i < population.Count; i++)
            {
                var chomosome = population[i];
                fitnessSoFar += chomosome.Fitness;
                if (fitnessSoFar > slice)
                {
                    result = chomosome;
                    break;
                }
            }

            Debug.Assert(result != null, "Shouldn't be null");
            return result;
        }

        private void CalculateFitness(BoxChomosome chromosome)
        {
            var decode = chromosome.Decode();
            double fitness = 0;
            for (int i = 0; i < this.cargo.Count; i++)
            {
                var box = this.cargo[i];
                box.X = decode[i].X;
                box.Y = decode[i].Y;
            }


            List<Box> nonOverlaping = new List<Box>();
            foreach (var box in cargo)
            {
                if ( box.Width + box.X > this.width ||
                    box.Height + box.Y > this.length)
                {                    
                        fitness = 0;                    
                }
                else
                {
                    fitness += 10;
                }

                if (!this.Overlaps(box))
                {
                    nonOverlaping.Add(box);
                    fitness += 10;
                }
                else 
                {
                    fitness = 0;
                }

            }
                        
            if (nonOverlaping.Count > 1)
            {
                var reference = nonOverlaping[0];
                double distance = 0;
                double coordinatesSum = 0;
                for (int i = 1; i < nonOverlaping.Count; i++)
                {
                    distance = reference.Center.Distance(nonOverlaping[i].Center);
                    coordinatesSum += nonOverlaping[i].X + nonOverlaping[i].Y;
                }
                if (distance == 0)
                {
                    distance = 100;
                }
                if (coordinatesSum == 0)
                {
                    coordinatesSum = 100;
                }
                fitness += 100/distance;
                fitness += 100 / coordinatesSum;
            }
            if (fitness < 0)
            {
                fitness = 0;
            }
            chromosome.Fitness = fitness;
        }

        private bool Overlaps(Box box)
        {
            bool result = false;
            foreach (var cargoBox in this.cargo)
            {
                if (cargoBox != box)
                {
                    result = box.Overlaps(cargoBox);
                }
            }

            return result;
        }     
    }
}
