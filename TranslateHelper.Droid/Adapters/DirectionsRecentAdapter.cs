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
	public class DirectionsRecentAdapter : ArrayAdapter<Tuple<Language, Language>>
	{

		private Activity context;
		private List<Tuple<Language, Language>> directionsList;
        private List<Tuple<string, int>> flagImageIdsList = new List<Tuple<string, int>>();

        public DirectionsRecentAdapter(Activity context, List<Tuple<Language, Language>> directionsList)
			: base (context, Resource.Layout.DirectionsRecentListItem, directionsList)
		{
			this.context = context;
			this.directionsList = directionsList;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			var item = this.directionsList [position];
			var view = (convertView ?? this.context.LayoutInflater.Inflate (Resource.Layout.DirectionsRecentListItem, parent, false)) as LinearLayout;

			ImageView sourceLangImageView = view.FindViewById<ImageView>(Resource.Id.sourceLangImageView);
            ImageView destLangImageView = view.FindViewById<ImageView>(Resource.Id.destLangImageView);
            TextView sourceLangTextView = view.FindViewById<TextView>(Resource.Id.sourceLangTextView);
            TextView destLangTextView = view.FindViewById<TextView>(Resource.Id.destLangTextView);
            int sourceFlagResourceId = context.Resources.GetIdentifier(item.Item1.NameImageResource.ToLower(), "drawable", context.PackageName);
            int destFlagResourceId = context.Resources.GetIdentifier(item.Item2.NameImageResource.ToLower(), "drawable", context.PackageName);
            sourceLangImageView.SetImageResource(sourceFlagResourceId);
            destLangImageView.SetImageResource(destFlagResourceId);
            sourceLangTextView.Text = item.Item1.NameLocal;
            destLangTextView.Text = item.Item2.NameLocal;
            /*var cacheImgItem = flagImageIdsList.Find(i => i.Item1 == item.NameImageResource.ToLower());
            int flagResourceId = 0;
            string imgName = item.NameImageResource.ToLower();
            if (cacheImgItem != null)
            {
                flagResourceId = cacheImgItem.Item2;
            } else
            {
                //ToDo:Попробовать вынести в отдельный класс кеша. По непонятной причине в Xamarin не портирован класс Hashtable.
                flagResourceId = context.Resources.GetIdentifier(imgName, "drawable", context.PackageName);
                flagImageIdsList.Add(new Tuple<string, int>(imgName, flagResourceId));
            }
            userView.SetImageResource(flagResourceId);
            TextView destLangTextView = view.FindViewById<TextView> (Resource.Id.destLangTextView);
            destLangTextView.Text = item.NameLocal;*/

            return view;
		}
	}
}
