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
using PortableCore.BL.Managers;

namespace TranslateHelper.Droid.Adapters
{
	public class FunDirectionsAdapter : ArrayAdapter<StoryWithTranslateItem>
	{

		private Activity context;
		private List<StoryWithTranslateItem> directionsStoryList;
        private List<Tuple<string, int>> flagImageIdsList = new List<Tuple<string, int>>();

        public FunDirectionsAdapter(Activity context, List<StoryWithTranslateItem> directionsStoryList)
			: base (context, Resource.Layout.FunDirectionsAllListItem, directionsStoryList)
		{
			this.context = context;
			this.directionsStoryList = directionsStoryList;

        }

		public override View GetView (int position, View convertView, ViewGroup parent)
        {

            var item = this.directionsStoryList[position];
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.FunDirectionsAllListItem, parent, false)) as LinearLayout;

            ImageView langFromView = view.FindViewById<ImageView>(Resource.Id.langFromImageView);
            int flagLangFromResourceId = getFlagResourceId(item.LanguageFrom.NameImageResource.ToLower());
            langFromView.SetImageResource(flagLangFromResourceId);

            ImageView langToView = view.FindViewById<ImageView>(Resource.Id.langToImageView);
            int flagLangToResourceId = getFlagResourceId(item.LanguageTo.NameImageResource.ToLower());
            langToView.SetImageResource(flagLangToResourceId);

            TextView captionTextView = view.FindViewById<TextView>(Resource.Id.captionItemTextView);
            captionTextView.Text = string.Format("{0} - {1}", item.LanguageFrom.NameLocal, item.LanguageTo.NameLocal);

            return view;
        }

        private int getFlagResourceId(string resourceName)
        {
            var cacheImgItem = flagImageIdsList.Find(i => i.Item1 == resourceName);
            int flagResourceId = 0;
            if (cacheImgItem != null)
            {
                flagResourceId = cacheImgItem.Item2;
            }
            else
            {
                //ToDo:Попробовать вынести в отдельный класс кеша. По непонятной причине в Xamarin не портирован класс Hashtable.
                flagResourceId = context.Resources.GetIdentifier(resourceName, "drawable", context.PackageName);
                flagImageIdsList.Add(new Tuple<string, int>(resourceName, flagResourceId));
            }

            return flagResourceId;
        }

        internal StoryWithTranslateItem GetStoryItem(int position)
        {
            return this.directionsStoryList[position];
        }
    }
}
