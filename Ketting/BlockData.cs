﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ketting
{
    public interface BlockData
    {
        public bool Verify(Block block);

        public string ToString();
       
    }
}
