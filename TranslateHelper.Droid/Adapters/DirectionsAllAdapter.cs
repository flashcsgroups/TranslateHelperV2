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
	public class DirectionsAllAdapter : ArrayAdapter<Language>
	{

		private Activity context;
		private List<Language> directionsList;
        private List<Tuple<string, int>> flagImageIdsList = new List<Tuple<string, int>>();

        public DirectionsAllAdapter (Activity context, List<Language> directionsList)
			: base (context, Resource.Layout.DirectionsAllListItem, directionsList)
		{
			this.context = context;
			this.directionsList = directionsList;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			var item = this.directionsList [position];
			var view = (convertView ?? this.context.LayoutInflater.Inflate (Resource.Layout.DirectionsAllListItem, parent, false)) as LinearLayout;

			ImageView userView = view.FindViewById<ImageView> (Resource.Id.destLangImageView);
            var cacheImgItem = flagImageIdsList.Find(i => i.Item1 == item.NameImageResource.ToLower());
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
            destLangTextView.Text = item.NameLocal;

            return view;
		}
	}
}
