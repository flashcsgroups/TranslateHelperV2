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
using TranslateHelper.Core;

namespace TranslateHelper.Droid
{

    public class FavoritesAdapter : BaseAdapter<Favorites>
    {
        protected Activity context = null;
        protected IList<Favorites> listFavorites = new List<Favorites>();

        public FavoritesAdapter(Activity context, IList<Favorites> listFavorites)
            : base()
        {
            this.context = context;
            this.listFavorites = listFavorites;
        }

        public override Favorites this[int position]
        {
            get { return listFavorites[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return listFavorites.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = listFavorites[position];
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.FavoritesItem, parent, false)) as LinearLayout;

            var translatedTextView = view.FindViewById<TextView>(Resource.Id.TranslatedTextView1);
            var sourceTextView = view.FindViewById<TextView>(Resource.Id.SourceTextView1);

            translatedTextView.SetText(listFavorites[position].TranslatedExpressionID.ToString(), TextView.BufferType.Normal);
            sourceTextView.SetText("test", TextView.BufferType.Normal);
            /*var posTextView = view.FindViewById<TextView>(Resource.Id.PosTextView);
            var indexTextView = view.FindViewById<TextView>(Resource.Id.IndexTextView);
            var favStatePic = view.FindViewById<ImageView>(Resource.Id.FavoritesStatePic);

            translatedTextView.SetText(listTranslateResult[position].TranslatedText, TextView.BufferType.Normal);
            transcriptionTextView.SetText(listTranslateResult[position].Ts, TextView.BufferType.Normal);
            posTextView.SetText(listTranslateResult[position].Pos, TextView.BufferType.Normal);
            indexTextView.SetText((position + 1).ToString(), TextView.BufferType.Normal);
            if(item.FavoritesId != 0)
                favStatePic.SetImageResource(Resource.Drawable.rigth2);
            else
                favStatePic.SetImageResource(Resource.Drawable.book);*/

            return view;
        }
    }
}