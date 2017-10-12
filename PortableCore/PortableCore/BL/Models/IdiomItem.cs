using PortableCore.BL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Models
{
    public class IdiomItem : IEquatable<IdiomItem>, IHasLabel, IComparable<IdiomItem>
    {
        public string TextFrom;
        public string TextTo;
        public string ExampleTextFrom;
        public string ExampleTextTo;
        public string CategoryTextFrom;
        public int Id;
        //public int ReadMark;

        public string Label
        {
            get
            {
                string label = string.Empty;
                if (!string.IsNullOrEmpty(TextFrom))
                {
                    label = CategoryTextFrom;
                }
                return label;
            }
        }
        
        public IdiomItem()
        {
        }

        public bool Equals(IdiomItem other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            if (object.ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(IdiomItem other)
        {
            return TextFrom.CompareTo(other.TextFrom);
        }
    }
}
