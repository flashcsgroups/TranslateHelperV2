using Android;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using PortableCore.BL.Models;
using Android.Util;
using System;
using PortableCore.DL;
using Droid.Core.Helpers;

namespace TranslateHelper.Droid.Adapters
{
    public class BubbleAdapter : ArrayAdapter<BubbleItem>
    {
        private Activity context;
        private List<BubbleItem> bubbleList;
        private DisplayMetrics metrics;
        private int maxWidth = 0;

        public BubbleAdapter(Activity context, List<BubbleItem> bubbleList, DisplayMetrics metrics)
            : base(context, Resource.Layout.Bubble, bubbleList)
        {
            this.context = context;
            this.bubbleList = bubbleList;
            this.metrics = metrics;
            this.maxWidth = Convert.ToInt32(metrics.WidthPixels * 0.7);
            
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            ViewHolder holder = null;
            var item = this.bubbleList[position];
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.Bubble, parent, false)) as LinearLayout;

            if (view != null)
            {
                holder = view.Tag as ViewHolder;
            }

            if (holder == null)
            {
                holder = new ViewHolder();
                holder.Initialize(view);
                view.Tag = holder;
            }
            holder.robotMessage.SetMaxWidth(maxWidth);
            holder.userMessage.SetMaxWidth(maxWidth);
            if (!item.IsRobotResponse)
            {
                holder.userMessage.Text = item.TextFrom;
                holder.userFlagView.Visibility = ViewStates.Visible;
                holder.userMessage.Visibility = ViewStates.Visible;
                holder.userFlagView.SetImageResource(getImageResourceByName(item.LanguageFrom.NameImageResource));
                holder.favoritesStatePic.Visibility = ViewStates.Gone;            
                holder.robotFlagView.Visibility = ViewStates.Gone;
                holder.robotLayout.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Left);
            }
            else
            {
                holder.robotMessage.Text = item.TextTo;
                holder.transcriptionTextView.Text = item.Transcription;
                holder.defTextView.Text = item.Definition;
                if (string.IsNullOrEmpty(item.Definition)) holder.robotLayoutDefinition.Visibility = ViewStates.Gone;
                holder.robotFlagView.Visibility = ViewStates.Visible;
                holder.robotLayout.Visibility = ViewStates.Visible;
                holder.robotFlagView.SetImageResource(getImageResourceByName(item.LanguageTo.NameImageResource));
                holder.favoritesStatePic.Visibility = ViewStates.Visible;
                if (item.InFavorites) holder.favoritesStatePic.SetImageResource(Resource.Drawable.v5alreadyaddedtofav);
                holder.userFlagView.Visibility = ViewStates.Gone;
                holder.userMessage.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Right);
            }
            return view;
        }

        internal void MarkBubbleItemAsDeleted(int elementPositionIndex)
        {
            this.bubbleList[elementPositionIndex].TextFrom = "deleted!";
            //this.bubbleList.RemoveAt(elementPositionIndex);
            //this.bubbleList[elementPositionIndex].TextFrom = "";
            //this.bubbleList[elementPositionIndex].TextTo = "";
        }

        internal BubbleItem GetBubbleItemByIndex(int elementPositionIndex)
        {
            BubbleItem result = new BubbleItem();
            if (this.bubbleList.Count > elementPositionIndex) result = this.bubbleList[elementPositionIndex];
            return result;
        }

        private int getImageResourceByName(string imgName)
        {
            return AndroidResourceHelper.GetImageResource(context, imgName);
            //return context.Resources.GetIdentifier(imgName.ToLower(), "drawable", context.PackageName);
        }

        class ViewHolder : Java.Lang.Object
        {
            public ImageView userFlagView { get; private set; }
            public ImageView robotFlagView { get; private set; }
            public ImageView favoritesStatePic { get; private set; }
            public LinearLayout robotLayout { get; private set; }
            public LinearLayout robotLayoutDefinition { get; private set; }
            public TextView userMessage { get; private set; }
            public TextView robotMessage { get; private set; }
            public TextView transcriptionTextView { get; private set; }
            public TextView defTextView { get; private set; }
            //public List<Language> languagesList {get; set;}

            internal void Initialize(LinearLayout viewElement)
            {
                this.userFlagView = viewElement.FindViewById<ImageView>(Resource.Id.userFlagView);
                this.robotFlagView = viewElement.FindViewById<ImageView>(Resource.Id.robotFlagView);
                this.robotLayout = viewElement.FindViewById<LinearLayout>(Resource.Id.LayoutRobotView);
                this.robotLayoutDefinition = viewElement.FindViewById<LinearLayout>(Resource.Id.LayoutDefinitionRobot);
                this.userMessage = viewElement.FindViewById<TextView>(Resource.Id.UserMessage);
                this.robotMessage = viewElement.FindViewById<TextView>(Resource.Id.RobotMessage);
                this.transcriptionTextView = viewElement.FindViewById<TextView>(Resource.Id.TranscriptionTextView);
                this.defTextView = viewElement.FindViewById<TextView>(Resource.Id.DefinitionTextView);
                this.favoritesStatePic = viewElement.FindViewById<ImageView>(Resource.Id.FavoritesStatePic);
                
            }
        }

    }
}
