using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Contracts
{
    public enum DefinitionTypesEnum :int
    {
        unknown = 0, noun, verb, adjective, adverb, participle,
        predicative,
        particle,
        numeral,
        pronoun
    }
}
