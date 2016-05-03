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

            var item = this.bubbleList[position];
            var view = (convertView ??
                   this.context.LayoutInflater.Inflate(
                   Resource.Layout.Bubble,
                   parent,
                   false)) as LinearLayout;

            ImageView userView = view.FindViewById<ImageView>(Resource.Id.list_bubble_userView);
            ImageView robotView = view.FindViewById<ImageView>(Resource.Id.list_bubble_robotView);
            LinearLayout robotLayout = view.FindViewById<LinearLayout>(Resource.Id.LayoutRobotView);
            TextView userMessage = view.FindViewById<TextView>(Resource.Id.UserMessage);
            TextView robotMessage = view.FindViewById<TextView>(Resource.Id.RobotMessage);
            TextView transcriptionTextView = view.FindViewById<TextView>(Resource.Id.TranscriptionTextView);
            TextView defTextView = view.FindViewById<TextView>(Resource.Id.DefinitionTextView);
            robotMessage.SetMaxWidth(maxWidth);
            userMessage.SetMaxWidth(maxWidth);
            if (!item.IsRobotResponse)
            {
                userMessage.Text = item.TextTo;
                userView.Visibility = ViewStates.Visible;
                userMessage.Visibility = ViewStates.Visible;
                robotView.Visibility = ViewStates.Gone;
                robotLayout.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Left);
            }
            else
            {
                robotMessage.Text = item.TextFrom;
                transcriptionTextView.Text = item.Transcription;
                defTextView.Text = item.Definition;
                robotView.Visibility = ViewStates.Visible;
                robotLayout.Visibility = ViewStates.Visible;
                userView.Visibility = ViewStates.Gone;
                userMessage.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Right);
            }
            //userMessage.Text = item.Text;
            /*if (!item.IsRobotResponse)
            {
                userMessage.Text = item.TextTo;
                robotMessage.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Left);
                userView.Visibility = ViewStates.Visible;
                robotView.Visibility = ViewStates.Gone;
                userMessage.SetBackgroundResource(Resource.Drawable.BubbleChatUserSelector);
                userMessage.SetTextColor(Android.Graphics.Color.White);
            }
            else
            {
                userMessage.Text = item.TextFrom;
                //userMessage.Visibility = ViewStates.Gone;
                view.SetGravity(GravityFlags.Right);
                userView.Visibility = ViewStates.Gone;
                robotView.Visibility = ViewStates.Visible;
                robotMessage.SetBackgroundResource(Resource.Drawable.BubbleChatRobotSelector);
                //message.SetTextColor(Android.Graphics.Color.Black);
            }*/

            return view;
        }
    }
}
