using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;

namespace TranslateHelper.Droid
{

    public class TranslateResultViewVariantAdapter : BaseAdapter<TranslateResultVariant>
    {
        protected Activity context = null;
        protected IList<TranslateResultVariant> listVariantsView = new List<TranslateResultVariant>();

        public TranslateResultViewVariantAdapter(Activity context, IList<TranslateResultVariant> listVariants)
            : base()
        {
            this.context = context;
            this.listVariantsView = listVariants;
        }

        public override TranslateResultVariant this[int position]
        {
            get { return listVariantsView[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return listVariantsView.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for this position
            var item = listVariantsView[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultVariant, parent, false)) as LinearLayout;
            //var view = convertView;
            // Find references to each subview in the list item's view
            //var translatedTextView = view.FindViewById<TextView>(Resource.Id.TranslatedTextView);
            var indexTextView = view.FindViewById<TextView>(Resource.Id.IndexTextView);
            var translatedTextTextView = view.FindViewById<TextView>(Resource.Id.TranslatedTextTextView);
            var synTextView = view.FindViewById<TextView>(Resource.Id.SynonymsTextView);
            //var examplesLinkTextView = view.FindViewById<TextView>(Resource.Id.ExamplesLinkTextView);
            //var favStatePic = view.FindViewById<ImageView>(Resource.Id.FavoritesStatePic);

            //Assign this item's values to the various subviews
            indexTextView.SetText((position + 1).ToString(), TextView.BufferType.Normal);
            translatedTextTextView.SetText(listVariantsView[position].Text, TextView.BufferType.Normal);
            synTextView.SetText("синонимы", TextView.BufferType.Normal);
            /*if (item.FavoritesId != 0)
                favStatePic.SetImageResource(Resource.Drawable.v3alreadyaddedtofav);
            else
                favStatePic.SetImageResource(Resource.Drawable.v3addtofavorites);*/

            return view;
        }
    }
}