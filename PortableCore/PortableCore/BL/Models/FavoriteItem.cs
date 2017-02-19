using PortableCore.BL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Models
{
    public class FavoriteItem : IEquatable<FavoriteItem>, IHasLabel, IComparable<FavoriteItem>
    {

        /*private int favoriteId = 0;
private int translatedExpressionId = 0;
private int sourceDefinitionId = 0;
private int sourceExprId = 0;

public int FavoriteId
{
get
{
return favoriteId;
}

set
{
favoriteId = value;
}
}

public int TranslatedExpressionId
{
get
{
return translatedExpressionId;
}

set
{
translatedExpressionId = value;
}
}

public int SourceDefinitionId
{
get
{
return sourceDefinitionId;
}

set
{
sourceDefinitionId = value;
}
}

public int SourceExprId
{
get
{
return sourceExprId;
}

set
{
sourceExprId = value;
}
}*/
        public string OriginalText;
        public string TranslatedText;
        public string Transcription;
        public int ChatHistoryId;

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
        
        public FavoriteItem()
        {
        }

        public bool Equals(FavoriteItem other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            if (object.ReferenceEquals(this, other)) return true;
            return ChatHistoryId.Equals(other.ChatHistoryId);
        }

        public override int GetHashCode()
        {
            //return hashFav ^ hashTransExpr ^ hashSourceDef ^ hashSourceExpr;
            return ChatHistoryId.GetHashCode();
        }

        public int CompareTo(FavoriteItem other)
        {
            return OriginalText.CompareTo(other.OriginalText);
        }
    }
}
