using Ketting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KettingTests
{
    public class TestData : BlockData
    {

        public override string ToString()
        {
            return "text";
        }

        public bool Verify(Block block)
        {
            return true;
        }
    }
}
