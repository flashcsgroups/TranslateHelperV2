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
	public class DirectionsAllAdapter : ArrayAdapter<DirectionsAllItem>
	{

		private Activity context;
		private List<DirectionsAllItem> directionsList;
		private DisplayMetrics metrics;
		//private int maxWidth = 0;

		public DirectionsAllAdapter (Activity context, List<DirectionsAllItem> directionsList, DisplayMetrics metrics)
			: base (context, Resource.Layout.DirectionsAllListItem, directionsList)
		{
			this.context = context;
			this.directionsList = directionsList;
			this.metrics = metrics;
			//this.maxWidth = Convert.ToInt32 (metrics.WidthPixels * 0.7);
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{

			var item = this.directionsList [position];
			var view = (convertView ??
			           this.context.LayoutInflater.Inflate (
				           Resource.Layout.DirectionsAllListItem,
				           parent,
				           false)) as LinearLayout;

			//ImageView userView = view.FindViewById<ImageView> (Resource.Id.list_bubble_userView);
			//ImageView robotView = view.FindViewById<ImageView> (Resource.Id.list_bubble_robotView);
			//LinearLayout robotMessage = view.FindViewById<LinearLayout> (Resource.Id.list_bubble_robotMessage);
			TextView destLangTextView = view.FindViewById<TextView> (Resource.Id.destLangTextView);
			//robotMessage.SetMaxWidth(maxWidth);
			//userMessage.SetMaxWidth (maxWidth);
			//userMessage.Text = item.Text;
			/*if (item.IsTheDeviceUser == false) {
				robotMessage.Visibility = ViewStates.Gone;
				view.SetGravity (GravityFlags.Left);
				userView.Visibility = ViewStates.Visible;
				robotView.Visibility = ViewStates.Gone;
				userMessage.SetBackgroundResource (Resource.Drawable.BubbleChatUserSelector);
				userMessage.SetTextColor (Android.Graphics.Color.White);
			} else {
				userMessage.Visibility = ViewStates.Gone;
				view.SetGravity (GravityFlags.Right);
				userView.Visibility = ViewStates.Gone;
				robotView.Visibility = ViewStates.Visible;
				robotMessage.SetBackgroundResource (Resource.Drawable.BubbleChatRobotSelector);
			}*/

			return view;
		}
	}
}
