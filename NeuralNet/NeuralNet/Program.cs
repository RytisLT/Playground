
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NeuralNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();            
            program.Run();
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        private void ExecuteCode(int times)
        {
            Thread.Sleep(200);            
            for (int i = 0; i < times; i++)
            {
                Thread.Sleep(5);
            }
            Thread.Sleep((int) (50 + NeuralNet.NextDouble() * 100));
        }

        public void Run()
        {
            this.TeachXor();
            //TeachSum();            
        }

        private void TeachXor()
        {
            var population = new List<NetHolder>();
            var genes = new List<Genome>();
            var populationCount = 100;
            var numberOfGenerations = 5000;
            var trainingSet = 1000;
            var initialFitness = 0;
            for (int i = 0; i < populationCount; i++)
            {
                var neuralNet = new NeuralNet(2, 1, 2, 6);
                var genome = new Genome(neuralNet.GetTotalInputs());
                genome.Fitness = initialFitness; // Set initial fitness
                var holder = new NetHolder() {Genome = genome, NeuralNet = neuralNet};
                this.SetWeights(holder);
                population.Add(holder);
                genes.Add(genome);
            }


            var algorithm = new GeneticAlgorithm(genes, 0.1, 0.7, 0.3, (int) (populationCount * 0.1));
            var random = new Random();
            var inputHolders = new List<InputHolder>();
            var maxValue = 1;
            trainingSet = 4;
            inputHolders.Add(new InputHolder() {X = 1, Y = 1});
            inputHolders.Add(new InputHolder() {X = 1, Y = 0});
            inputHolders.Add(new InputHolder() {X = 0, Y = 1});
            inputHolders.Add(new InputHolder() {X = 0, Y = 0});


            for (int i = 0; i < numberOfGenerations; i++)
            {
                Console.WriteLine("Generation {0}", i);
                int generationCorrect = 0;
                double maxError = double.MinValue;
                double minError = double.MaxValue;
                double averageError = 0;
                int errorIteration = 0;

                double minFitness = double.MaxValue;
                double maxFitness = double.MinValue;
                double averageFitness = 0;
                int fitnessIteration = 0;
                foreach (var netHolder in population)
                {


                    netHolder.Genome.Fitness = initialFitness;
                    int totalCorrect = 0;
                    foreach (var input in inputHolders)
                    {
                        var list =
                            netHolder.NeuralNet.Update(new List<double>() {input.X, input.Y});
                        var output = list[0];

                        var expectedOutput = (int) input.X ^ (int) input.Y;
                        var error = Math.Abs(output - expectedOutput);

                        averageError = (averageError * errorIteration + error) /
                                       ++errorIteration;

                        if (netHolder.MaxError > error)
                        {
                            netHolder.MaxError = error;
                        }
                        lock (this)
                        {
                            if (error > maxError)
                            {
                                maxError = error;
                            }
                            if (error < minError)
                            {
                                minError = error;
                            }
                        }                       
                         var bonus = 100.0;
                         var coef = 1.0;
                         if (error > 0)
                         {
                             coef = (maxValue - error) / (maxValue);
                         }
                         else
                         {
                             totalCorrect++;
                         }

                         bonus = bonus * coef;

                         double fitnessPenalty = bonus / trainingSet;

                        netHolder.Genome.Fitness += fitnessPenalty;
                        /*   Console.WriteLine("{0} -> output: {1}, expected: {2}, error: {3}", j, output, expectedOutput,
Math.Round(error, 2));*/
                    }

                    //netHolder.Genome.Fitness += totalCorrect;
                    generationCorrect += totalCorrect;

                    //Console.WriteLine("{0} -> Fitness: {1}", j, population[j].Genome.Fitness);
                    lock (this)
                    {
                        if (netHolder.Genome.Fitness > maxFitness)
                        {
                            maxFitness = netHolder.Genome.Fitness;
                        }
                        if (netHolder.Genome.Fitness < minFitness)
                        {
                            minFitness = netHolder.Genome.Fitness;
                        }
                        averageFitness = (averageFitness * fitnessIteration +
                                          netHolder.Genome.Fitness) /
                                         ++fitnessIteration;
                    }
                }


                if (generationCorrect / populationCount == trainingSet)
                {
                    Console.WriteLine("Found solution");
                    for (int j = 0; j < 1000; j++)
                    {
                        int x = random.Next(1);
                        int y = random.Next(1);
                        var z = population[0].NeuralNet.Update(new List<double>() {x, y})[0];
                        if ((x ^ y) != z)
                        {
                        }
                    }
                }

                Console.WriteLine("Fitness: min: {0} max: {1}, avg: {2}", Math.Round(minFitness, 2),
                                  Math.Round(maxFitness, 2), Math.Round(averageFitness, 2));
                Console.WriteLine("Error: min: {0} max: {1}, avg: {2}, corr: {3}", Math.Round(minError, 2),
                                  Math.Round(maxError, 2), Math.Round(averageError, 2), generationCorrect);
                var newPopulation = algorithm.Epoch();
                for (int j = 0; j < newPopulation.Count; j++)
                {
                    population[j].Genome = newPopulation[j];
                }
                foreach (var netHolder in population)
                {
                    this.SetWeights(netHolder);
                }
            }

            NetHolder best = population[0];
            foreach (var netHolder in population)
            {
                if (netHolder.Genome.Fitness > best.Genome.Fitness)
                {
                    best = netHolder;
                }
            }

            best.NeuralNet.Update(0, 0);
        }

        private void TeachSum()
        {
            var population = new List<NetHolder>();
            var genes = new List<Genome>();
            var populationCount = 100;
            var numberOfGenerations = 200;
            var trainingSet = 100;
            var initialFitness = 0;            
            for (int i = 0; i < populationCount; i++)
            {
                var neuralNet = new NeuralNet(2, 1, 3, 6);                
                var genome = new Genome(neuralNet.GetTotalInputs());
                genome.Fitness = initialFitness; // Set initial fitness
                var holder = new NetHolder(){Genome = genome, NeuralNet = neuralNet};
                this.SetWeights(holder);
                population.Add(holder);
                genes.Add(genome);
            }

            NetHolder bestEver = new NetHolder()
                                     {
                                         Genome = new Genome(new List<double>()) {Fitness = double.MinValue},
                                         NeuralNet = new NeuralNet(2, 1, 3, 6)
                                     };

            var algorithm = new GeneticAlgorithm(genes, 0.1, 0.7, 0.3, (int) (populationCount * 0.1));
            var random = new Random();
            var inputHolders = new List<InputHolder>();
            var maxNumber = 5;
            var maxValue = maxNumber * 2;



            inputHolders.Add(new InputHolder() {X = 0, Y = 1});            
            inputHolders.Add(new InputHolder() {X = 5, Y = 5});
            

            trainingSet = inputHolders.Count;
            
            for (int i = 0; i < numberOfGenerations; i++)
            {
                Console.WriteLine("Generation {0}", i);
                int generationCorrect = 0;
                double maxError = double.MinValue;
                double minError = double.MaxValue;
                double averageError = 0;
                int errorIteration = 0;
                
                double minFitness = double.MaxValue;
                double maxFitness = double.MinValue;
                double averageFitness = 0;
                int fitnessIteration = 0;
                foreach (var netHolder in population)
                {

                    averageError = TrainNetwork(initialFitness, inputHolders, maxValue, netHolder,
                                                averageError, errorIteration, ref maxError, ref minError,
                                                ref generationCorrect, ref maxFitness, ref minFitness,
                                                ref averageFitness, fitnessIteration);
                }                

                foreach (var holder in population)
                {
                    if (holder.MaxError < maxError)
                    {
                        var errorBonus = (maxError - holder.MaxError) * 10;
                        holder.Genome.Fitness += errorBonus;
                    }
                    if (holder.Genome.Fitness > bestEver.Genome.Fitness)
                    {
                        bestEver.Genome = holder.Genome;
                        bestEver.Genome.Fitness = holder.Genome.Fitness;
                        bestEver.MaxError = holder.MaxError;
                        this.SetWeights(bestEver);
                    }
                }

                Console.WriteLine("Fitness: min: {0} max: {1}, avg: {2}", Math.Round(minFitness, 2),
                                  Math.Round(maxFitness, 2), Math.Round(averageFitness, 2));
                Console.WriteLine("Error: min: {0} max: {1}, avg: {2}, corr: {3}", Math.Round(minError, 2),
                                  Math.Round(maxError, 2),  Math.Round(averageError, 2), generationCorrect);
                Console.WriteLine("Best: fitness {0}, max error {1}%", Math.Round(bestEver.Genome.Fitness, 2),
                                  Math.Round(bestEver.MaxError / maxValue * 100, 2));
                                
                var newPopulation = algorithm.Epoch();
                for (int j = 0; j < newPopulation.Count; j++)
                {
                    population[j].Genome = newPopulation[j];
                    population[j].MaxError = 0;
                }
                foreach (var netHolder in population)
                {
                    this.SetWeights(netHolder);
                }                
            }

            Console.WriteLine("Completed. Testing best");
            for (int i = 0; i < 10; i++)
            {
                double avg = 0d;
                int iter = 0;
                double maxError = 0;
                double minError =0;
                int generationCorrect=0;
                double maxFitness=0;
                double minFitness=0;
                double averageFitness=0;
                int fitnessIteration=0;
                this.TrainNetwork(initialFitness, inputHolders, maxValue, bestEver, avg, iter, ref maxError, ref minError,
                                  ref generationCorrect, ref maxFitness, ref minFitness, ref averageFitness,
                                  fitnessIteration);
                var x = random.Next(maxNumber);
                var y = random.Next(maxNumber);
                var z = bestEver.NeuralNet.Update(new List<double> {x, y})[0] * maxValue;
                Console.WriteLine("{0} + {1} = {2}", x, y, Math.Round(z, 2));
            }
        }

        private double TrainNetwork(int initialFitness, List<InputHolder> inputHolders, int maxValue, NetHolder netHolder, double averageError, int errorIteration, ref double maxError, ref double minError, ref int generationCorrect, ref double maxFitness, ref double minFitness, ref double averageFitness, int fitnessIteration)
        {
            netHolder.Genome.Fitness = initialFitness;
            int totalCorrect = 0;
            foreach (var input in inputHolders)
            {
                var list = netHolder.NeuralNet.Update(new List<double>() {input.X, input.Y});
                var output = list[0] * maxValue;
                var expectedOutput = input.X + input.Y;
                var error = Math.Abs(output - expectedOutput);

                averageError = (averageError * errorIteration + error) / ++errorIteration;

                if (netHolder.MaxError < error)
                {
                    netHolder.MaxError = error;
                }
                lock (this)
                {
                    if (error > maxError)
                    {
                        maxError = error;
                    }
                    if (error < minError)
                    {
                        minError = error;
                    }
                }
                var bonus = 100.0;
                var coef = 1.0;
                if (error > 1)
                {
                    coef = (maxValue - error) / (maxValue);
                }
                else
                {                
                    totalCorrect++;
                }
                
                bonus = bonus * Math.Pow(coef, 3);

                double fitnessPenalty = bonus / inputHolders.Count;

                netHolder.Genome.Fitness += fitnessPenalty;
                /*   Console.WriteLine("{0} -> output: {1}, expected: {2}, error: {3}", j, output, expectedOutput,
         Math.Round(error, 2));*/
            }

            netHolder.Genome.Fitness += totalCorrect;
            generationCorrect += totalCorrect;

            //Console.WriteLine("{0} -> Fitness: {1}", j, population[j].Genome.Fitness);
            lock (this)
            {
                if (netHolder.Genome.Fitness > maxFitness)
                {
                    maxFitness = netHolder.Genome.Fitness;
                }
                if (netHolder.Genome.Fitness < minFitness)
                {
                    minFitness = netHolder.Genome.Fitness;
                }
                averageFitness = (averageFitness * fitnessIteration + netHolder.Genome.Fitness) /
                                 ++fitnessIteration;
            }
            return averageError;
        }

        private void SetWeights(NetHolder holder)
        {
            holder.NeuralNet.SetInputWeights(holder.Genome.Dna);
        }

        private void IncrementFitness(NetHolder holder, double fitness)
        {
            holder.Genome.Fitness += fitness;
        }
    }

    public class NetHolder
    {
        public NetHolder()
        {
            ExecutionTime = TimeSpan.MaxValue;
        }

        public Genome Genome { get; set; }
        
        public NeuralNet NeuralNet { get; set; }

        public TimeSpan ExecutionTime { get; set; }

        public double MaxError { get; set; }
    }

    public class InputHolder
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}
