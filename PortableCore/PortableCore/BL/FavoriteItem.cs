using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL
{
    public class FavoriteItem : IEquatable<FavoriteItem>
    {
        private int favoriteId = 0;
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
        }

        public FavoriteItem()
        {
        }

        public bool Equals(FavoriteItem other)
        {
            if (object.ReferenceEquals(other, null)) return false;
            if (object.ReferenceEquals(this, other)) return true;
            return sourceExprId.Equals(other.sourceExprId);
        }

        public override int GetHashCode()
        {

            int hashFav = favoriteId.GetHashCode();

            int hashTransExpr = translatedExpressionId.GetHashCode();

            int hashSourceDef = sourceDefinitionId.GetHashCode();

            int hashSourceExpr = sourceExprId.GetHashCode();


            //return hashFav ^ hashTransExpr ^ hashSourceDef ^ hashSourceExpr;
            return hashSourceExpr;
        }
    }
}
