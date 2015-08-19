using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.ObjectModel
{
    public class UnitTest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Storage { get; set; }
        public Execution Execution { get; set; }
        public TestMethod TestMethod { get; set; }
    }

}
