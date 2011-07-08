using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNet
{
    public class NeuronInput
    {
        public double Weight { get; set; }

        public double Value { get; set; }

        public double Activation
        {
            get
            {
                return this.Value * this.Weight;
            }
        }
    }
}
