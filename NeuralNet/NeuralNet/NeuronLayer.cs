using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNet
{
    public class NeuronLayer
    {
        private readonly List<Neuron> neurons = new List<Neuron>();

        public NeuronLayer(int numberOfNeurons, int numberOfInputs)
        {
            this.NumberOfNeurons = numberOfNeurons;
            for (int i = 0; i < NumberOfNeurons; i++)
            {
                this.neurons.Add(new Neuron(numberOfInputs));
            }
        }

        public int NumberOfNeurons { get; private set; }

        public List<Neuron> Neurons
        {
            get { return this.neurons; }
        }
    }
}
