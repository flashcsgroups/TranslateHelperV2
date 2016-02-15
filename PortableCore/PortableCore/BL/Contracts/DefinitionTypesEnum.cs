using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Contracts
{
    public enum DefinitionTypesEnum :int
    {
        unknown = 0,
        translater,//элемент определяет что слово получено не из словаря, а из сервиса перевода, там нет типов речи
        noun, verb, adjective, adverb, participle,
        predicative,
        particle,
        numeral,
        pronoun,
        preposition,
        conjunction,
        interjection,
        parenthetic
    }
}
