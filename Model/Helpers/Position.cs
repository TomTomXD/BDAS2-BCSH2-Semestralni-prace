﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model.Helpers
{
    public class Position : ILookupEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
