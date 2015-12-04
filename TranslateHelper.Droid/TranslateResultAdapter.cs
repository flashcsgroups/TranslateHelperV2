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
using TranslateHelper.Core.BL.Contracts;

namespace TranslateHelper.Droid
{

    public class TranslateResultAdapter : BaseAdapter<TranslateResult>
    {
        protected Activity context = null;
        protected IList<TranslateResult> listTranslateResult = new List<TranslateResult>();

        public TranslateResultAdapter(Activity context, IList<TranslateResult> listTranslateResult)
            : base()
        {
            this.context = context;
            this.listTranslateResult = listTranslateResult;
        }

        public override TranslateResult this[int position]
        {
            get { return listTranslateResult[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return listTranslateResult.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for this position
            var item = listTranslateResult[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultItem, parent, false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var translatedTextView = view.FindViewById<TextView>(Resource.Id.TranslatedTextView);
            var transcriptionTextView = view.FindViewById<TextView>(Resource.Id.TranscriptionTextView);
            var posTextView = view.FindViewById<TextView>(Resource.Id.PosTextView);
            var indexTextView = view.FindViewById<TextView>(Resource.Id.IndexTextView);
            var favStatePic = view.FindViewById<ImageView>(Resource.Id.FavoritesStatePic);

            //Assign this item's values to the various subviews
            translatedTextView.SetText(listTranslateResult[position].TranslatedText, TextView.BufferType.Normal);
            transcriptionTextView.SetText(listTranslateResult[position].Ts, TextView.BufferType.Normal);
            posTextView.SetText(listTranslateResult[position].Pos, TextView.BufferType.Normal);
            indexTextView.SetText((position + 1).ToString(), TextView.BufferType.Normal);
            if(item.FavoritesId != 0)
                favStatePic.SetImageResource(Resource.Drawable.rigth2);
            else
                favStatePic.SetImageResource(Resource.Drawable.book);

            return view;
        }
    }
}