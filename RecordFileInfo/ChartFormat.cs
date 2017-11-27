using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordFileUtil
{
    public class ChartFormat
    {
        public String Xname;
        public String Yname;
        public double Xinterval;
        public double Yinterval;
        public double Xmin;
        public double Ymin;
        public double Xmax;
        public double Ymax;
        public int Xtype;//0-Int32,1-Double
        public int Ytype;
        public Boolean Xreverse = false;
        public Boolean Yreverse = true;
    }
}
