using System.Collections.Generic;

using Android.App;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;

namespace TranslateHelper.Droid
{

    public class TranslateResultViewHeaderAdapter : BaseAdapter<TranslateResultDefinition>
    {
        protected Activity context = null;
        protected IList<TranslateResultDefinition> listTranslateResultView = new List<TranslateResultDefinition>();

        public TranslateResultViewHeaderAdapter(Activity context, IList<TranslateResultDefinition> listTranslateResult)
            : base()
        {
            this.context = context;
            this.listTranslateResultView = listTranslateResult;
        }

        public override TranslateResultDefinition this[int position]
        {
            get { return listTranslateResultView[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return listTranslateResultView.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for this position
            var item = listTranslateResultView[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.TranslateResultHeader, parent, false)) as LinearLayout;

            // Find references to each subview in the list item's view
            //var translatedTextView = view.FindViewById<TextView>(Resource.Id.TranslatedTextView);
            var originalTextTextView = view.FindViewById<TextView>(Resource.Id.OriginalTextTextView);
            var transcriptionTextView = view.FindViewById<TextView>(Resource.Id.OriginalTextTranscriptionTextView);
            var posTextView = view.FindViewById<TextView>(Resource.Id.PosTextView);
            var examplesLinkTextView = view.FindViewById<TextView>(Resource.Id.ExamplesLinkTextView);
            //var favStatePic = view.FindViewById<ImageView>(Resource.Id.FavoritesStatePic);

            //Assign this item's values to the various subviews
            originalTextTextView.SetText(listTranslateResultView[position].OriginalText, TextView.BufferType.Normal);
            transcriptionTextView.SetText(string.Format("[{0}]",listTranslateResultView[position].Transcription), TextView.BufferType.Normal);
            posTextView.SetText(listTranslateResultView[position].Pos.ToString(), TextView.BufferType.Normal);
            examplesLinkTextView.SetText("Примеры", TextView.BufferType.Normal);

            var listVariantView = view.FindViewById<ListView>(Resource.Id.listVariantListView);
            listVariantView.FastScrollEnabled = true;
            listVariantView.Adapter = new TranslateResultViewVariantAdapter(context, listTranslateResultView[position].TranslateVariants);

            /*indexTextView.SetText((position + 1).ToString(), TextView.BufferType.Normal);
            if (item.FavoritesId != 0)
                favStatePic.SetImageResource(Resource.Drawable.v3alreadyaddedtofav);
            else
                favStatePic.SetImageResource(Resource.Drawable.v3addtofavorites);*/

            return view;
        }
    }
}