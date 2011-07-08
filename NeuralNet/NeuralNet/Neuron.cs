using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNet
{
    public class Neuron
    {
        private readonly List<NeuronInput> inputs = new List<NeuronInput>();
        public const double Bias = -1.0;

        public double Threshold { get; set; }

        public Neuron(int numberOfInputs)
        {
            for (int i = 0; i < numberOfInputs; i++)
            {
                inputs.Add(new NeuronInput() { Weight = NeuralNet.NextDouble() - 0.5 });
            }
            Threshold = NeuralNet.NextDouble() - 0.5;
        }

        public double GetActivation()
        {
            double activation = 0;
            foreach (var input in this.inputs)
            {
                activation += input.Activation;
            }
            return activation;
        }

        public bool IsActivated()
        {
            return GetActivation() > Threshold;
        }

        public int NumberOfInputs
        {
            get
            {
                return inputs.Count;
            }
        }

        public List<NeuronInput> Inputs
        {
            get { return this.inputs; }
        }
    }
}
