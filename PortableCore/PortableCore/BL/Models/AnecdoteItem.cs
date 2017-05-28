using PortableCore.BL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Models
{
    public class AnecdoteItem : IEquatable<AnecdoteItem>, IHasLabel, IComparable<AnecdoteItem>
    {
        public string OriginalText;
        public string TranslatedText;
        public int AnecdoteId;
        public int ReadMark;

        public string Label
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(OriginalText))
                    label = OriginalText[0].ToString();
                return label;
            }
        }
        
        public AnecdoteItem()
        {
        }

        public bool Equals(AnecdoteItem other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            if (object.ReferenceEquals(this, other)) return true;
            return AnecdoteId.Equals(other.AnecdoteId);
        }

        public override int GetHashCode()
        {
            return AnecdoteId.GetHashCode();
        }

        public int CompareTo(AnecdoteItem other)
        {
            return OriginalText.CompareTo(other.OriginalText);
        }
    }
}
