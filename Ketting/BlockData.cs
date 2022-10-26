using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ketting
{
    public interface BlockData
    {
        public bool Verify();

    }
}
