﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model
{
    public class Password
    {
        public string Salt { get; set; }
        public string HashedPassword { get; set; }
    }
}
