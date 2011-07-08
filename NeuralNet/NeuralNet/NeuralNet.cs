using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NeuralNet
{
    public class NeuralNet
    {
        public int NumberOfInputs { get; set; }
        public int NumberOfOutputs { get; set; }
        public int NumberOfHiddenLayers { get; set; }
        public int NeuronsPerHiddenLayer { get; set; }
        private readonly List<NeuronLayer> layers = new List<NeuronLayer>();
        private static double ActivationResponse = 1.0;
        private static RNGCryptoServiceProvider randomProvider = new RNGCryptoServiceProvider();        
        private static MersenneTwister random = new MersenneTwister((uint) DateTime.Now.Ticks);
        
        public static double NextDouble()
        {
            /*var bytes = new byte[8];
            randomProvider.GetBytes(bytes);
            var value = BitConverter.ToDouble(bytes, 0);
            if (value > 1)
            {
                value = 1 / value;
            }
            var result = Math.Round(value, 10);*/
            var result = random.NextDouble();
            return result;
        }

        public static double RandomClamped()
        {
            var result = NextDouble() - NextDouble();
            return result;
        }

        public NeuralNet(int numberOfInputs, int numberOfOutputs, int numberOfHiddenLayers, int neuronsPerHiddenLayer)
        {
            this.NumberOfInputs = numberOfInputs;
            this.NumberOfOutputs = numberOfOutputs;
            this.NumberOfHiddenLayers = numberOfHiddenLayers;
            this.NeuronsPerHiddenLayer = neuronsPerHiddenLayer;

            layers.Add(new NeuronLayer(NeuronsPerHiddenLayer, NumberOfInputs));

            for (int i = 0; i < numberOfHiddenLayers; i++)
            {
                layers.Add(new NeuronLayer(NeuronsPerHiddenLayer, NeuronsPerHiddenLayer));
            }

            layers.Add(new NeuronLayer(numberOfOutputs, NeuronsPerHiddenLayer));
        }

        public double Sigmoid(double netInput, double response)
        {
            var result = (1.0 / (1 + Math.Exp(-netInput)));
            //var result = (1.0 / (1.0 + Math.Exp(-netInput / response)));
            return result;
        }


        public List<double> Update(params double[] parameters)
        {
            return this.Update(new List<double>(parameters));
        }

        public List<double> Update(List<double> inputs)
        {
            var outputs = new List<double>();
            if (inputs.Count != NumberOfInputs)
            {
                throw new ArgumentException("Invalid number of inputs", "inputs");
            }

            for (int i = 0; i < this.layers.Count; i++)
            {
                if (i > 0)
                {
                    inputs = outputs;
                }

                outputs = new List<double>();

                var layer = this.layers[i];
                for (int j = 0; j < layer.NumberOfNeurons; j++)
                {
                    double netInput = 0;
                    int numOfInputs = layer.Neurons[j].NumberOfInputs;
                    for (int k = 0; k < numOfInputs; k++)
                    {
                        layer.Neurons[j].Inputs[k].Value = inputs[k];
                        netInput += layer.Neurons[j].Inputs[k].Activation;
                    }
                    netInput += layer.Neurons[j].Threshold * Neuron.Bias;
                    outputs.Add(this.Sigmoid(netInput, NeuralNet.ActivationResponse));
                }
            }
            return outputs;
        }

        public int GetTotalInputs()
        {
            int inputs = 0;
            for (int i = 0; i < layers.Count; i++)
            {
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                {
                    inputs += layers[i].Neurons[j].Inputs.Count;
                }
            }
            return inputs;
        }

        public void SetInputWeights(List<double> weights)
        {
            int totalInputs = this.GetTotalInputs();
            if (totalInputs != weights.Count)
            {
                throw new ApplicationException(string.Format("Invalid weights count. Expected {0} got {1}", totalInputs,
                                                             weights.Count));
            }
            int n = 0;
            for (int i = 0; i < layers.Count; i++)
            {
                for (int j = 0; j < layers[i].Neurons.Count; j++)
                {
                    for (int k = 0; k < layers[i].Neurons[j].Inputs.Count; k++)
                    {
                        layers[i].Neurons[j].Inputs[k].Weight = weights[n++];
                    }
                }
            }
        }
    }
}
