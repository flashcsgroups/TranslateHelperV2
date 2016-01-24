using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;

namespace TranslateHelper.Droid
{
    public class FavoritesItem : IHasLabel, IComparable<FavoritesItem>
    {
        private string originalText = string.Empty;
        public string OriginalText
        {
            get
            {
                return originalText;
            }

            set
            {
                originalText = value;
            }
        }
        private string translatedText;
        public string TranslatedText
        {
            get
            {
                return translatedText;
            }

            set
            {
                translatedText = value;
            }
        }

        private int translatedExpressionId;
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

        private int favoritesId;
        public int FavoritesId
        {
            get
            {
                return favoritesId;
            }

            set
            {
                favoritesId = value;
            }
        }

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


        public int CompareTo(FavoritesItem other)
        {
            return OriginalText.CompareTo(other.OriginalText);
        }
    }
}