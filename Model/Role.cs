using FinancniInformacniSystemBanky.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.Model
{
    public class Role : ILookupEntry
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
