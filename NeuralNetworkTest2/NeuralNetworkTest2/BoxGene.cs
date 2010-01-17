using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetworkTest2
{
    public class BoxGene : Gene
    {
        public const int GeneLength = 16;

        public BoxGene()
            : base(GeneLength)
        {            
        }

        public int X
        {
            get { return this.Decode(0, GeneLength / 2); }
        }

        public int Y
        {
            get
            {
                return this.Decode(GeneLength / 2, GeneLength);
            }
        }
    }
}
