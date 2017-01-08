using Android;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using PortableCore.BL.Models;
using Android.Util;
using System;
using PortableCore.DL;
using Android.Content.Res;

namespace TranslateHelper.Droid.Adapters
{
    public class DirectionsRecentAdapter : ArrayAdapter<DirectionsRecentItem>
    {

        private Activity context;
        private List<DirectionsRecentItem> directionsList;
        private List<Tuple<string, int>> flagImageIdsList = new List<Tuple<string, int>>();

        public DirectionsRecentAdapter(Activity context, List<DirectionsRecentItem> directionsList)
            : base(context, Resource.Layout.DirectionsRecentListItem, directionsList)
        {
            this.context = context;
            this.directionsList = directionsList;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var item = this.directionsList[position];
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.DirectionsRecentListItem, parent, false)) as LinearLayout;

            ImageView destLangImageView = view.FindViewById<ImageView>(Resource.Id.destLangImageView);
            TextView destLangTextView = view.FindViewById<TextView>(Resource.Id.destLangTextView);
            TextView destLangCountMsgTextView = view.FindViewById<TextView>(Resource.Id.destLangCountMsgTextView);
            destLangTextView.Text = string.Format("{0}-{1}", item.LangTo, item.LangFrom);
            destLangCountMsgTextView.Text = string.Format("({0})", item.CountOfAllMessages.ToString());
            var flagResourceId = context.Resources.GetIdentifier(item.LangToFlagImageResourcePath.ToLower(), "drawable", context.PackageName);
            destLangImageView.SetImageResource(flagResourceId);
            return view;
        }

        internal DirectionsRecentItem GetLanguageItem(int position)
        {
            return this.directionsList[position];
        }
    }
}
