using Android;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using PortableCore.BL.Models;
using Android.Util;
using System;

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
                holder.userMessage.Text = item.TextTo;
                holder.userView.Visibility = ViewStates.Visible;
                holder.userMessage.Visibility = ViewStates.Visible;
                holder.robotView.Visibility = ViewStates.Gone;
                holder.robotLayout.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Left);
            }
            else
            {
                holder.robotMessage.Text = item.TextFrom;
                holder.transcriptionTextView.Text = item.Transcription;
                holder.defTextView.Text = item.Definition;
                if (string.IsNullOrEmpty(item.Definition)) holder.robotLayoutDefinition.Visibility = ViewStates.Gone;
                holder.robotView.Visibility = ViewStates.Visible;
                holder.robotLayout.Visibility = ViewStates.Visible;
                holder.userView.Visibility = ViewStates.Gone;
                holder.userMessage.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Right);
            }
            return view;
        }

        class ViewHolder : Java.Lang.Object
        {
            public ImageView userView { get; private set; }
            public ImageView robotView { get; private set; }
            public LinearLayout robotLayout { get; private set; }
            public LinearLayout robotLayoutDefinition { get; private set; }
            public TextView userMessage { get; private set; }
            public TextView robotMessage { get; private set; }
            public TextView transcriptionTextView { get; private set; }
            public TextView defTextView { get; private set; }

            internal void Initialize(LinearLayout viewElement)
            {
                this.userView = viewElement.FindViewById<ImageView>(Resource.Id.list_bubble_userView);
                this.robotView = viewElement.FindViewById<ImageView>(Resource.Id.list_bubble_robotView);
                this.robotLayout = viewElement.FindViewById<LinearLayout>(Resource.Id.LayoutRobotView);
                this.robotLayoutDefinition = viewElement.FindViewById<LinearLayout>(Resource.Id.LayoutDefinitionRobot);
                this.userMessage = viewElement.FindViewById<TextView>(Resource.Id.UserMessage);
                this.robotMessage = viewElement.FindViewById<TextView>(Resource.Id.RobotMessage);
                this.transcriptionTextView = viewElement.FindViewById<TextView>(Resource.Id.TranscriptionTextView);
                this.defTextView = viewElement.FindViewById<TextView>(Resource.Id.DefinitionTextView);
            }
        }
    }
}
