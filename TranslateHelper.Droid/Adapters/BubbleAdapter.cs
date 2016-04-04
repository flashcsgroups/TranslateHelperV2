using Android;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Android.Text;
using PortableCore.BL.Models;

namespace TranslateHelper.Droid.Adapters
{
    public class BubbleAdapter : ArrayAdapter<BubbleItem>
    {

        private Activity _context;
        private List<BubbleItem> _bubbleList;

        public BubbleAdapter(Activity context, List<BubbleItem> bubbleList)
            : base(context, Resource.Layout.Bubble, bubbleList)
        {
            this._context = context;
            this._bubbleList = bubbleList;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var item = this._bubbleList[position];
            var view = (convertView ??
                   this._context.LayoutInflater.Inflate(
                   Resource.Layout.Bubble,
                   parent,
                   false)) as LinearLayout;

            ImageView userView = view.FindViewById<ImageView>(Resource.Id.list_bubble_userView);
            ImageView robotView = view.FindViewById<ImageView>(Resource.Id.list_bubble_robotView);
            TextView message = view.FindViewById<TextView>(Resource.Id.list_bubble_message);
            message.Text = item.Text;
            if (item.IsTheDeviceUser == false)
            {
                view.SetGravity(GravityFlags.Left);
                userView.Visibility = ViewStates.Visible;
                robotView.Visibility = ViewStates.Gone;
                message.SetBackgroundResource(Resource.Drawable.BubbleChatUserSelector);
            }
            else
            {
                view.SetGravity(GravityFlags.Right);
                userView.Visibility = ViewStates.Gone;
                robotView.Visibility = ViewStates.Visible;
                message.SetBackgroundResource(Resource.Drawable.BubbleChatRobotSelector);
            }

            return view;
        }
    }
}
