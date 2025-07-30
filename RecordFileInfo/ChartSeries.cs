using System.Collections.Generic;
using System.Drawing;

namespace RecordFileUtil
{
    public class ChartSeries
    {
        // Fields
        public Color LineColor = Color.Black;
        public List<IXYNode> Nodes = null;

        // Methods
        public static ChartSeries NewRedSeries()
        {
            return new ChartSeries { LineColor = Color.Red };
        }
    }
}