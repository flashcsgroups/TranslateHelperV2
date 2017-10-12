using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Widget;
using Droid.Core.Helpers;
using PortableCore.BL.Managers;
using PortableCore.DAL;
using PortableCore.BL.Models;

namespace TranslateHelper.Droid.Adapters
{

    public class IdiomsAdapter : BaseAdapter, ISectionIndexer
	{
		Dictionary<string, IAdapter> sections = new Dictionary<string, IAdapter> ();
		ArrayAdapter<string> headers;
		const int TypeSectionHeader = 0;
        Java.Lang.Object[] sectionObjects;
        private Context context;

        public IdiomsAdapter(Context context)
		{
			this.context = context;
			headers = new ArrayAdapter<string> (context, Resource.Layout.IdiomsCategoryListHeader);
		}

		public void AddSection (string section, IAdapter adapter)
		{
			headers.Add (section);
			sections.Add (section, adapter);
		}

		public override Java.Lang.Object GetItem (int position)
		{
			int op = position;
			foreach (var section in sections.Keys) {
				var adapter = sections [section];
				int size = adapter.Count + 1;
				if (position == 0)
					return section;
				if (position < size)
					return adapter.GetItem (position - 1);
				position -= size;
			}
			return null;
		}

        public override int Count {
			get {
				return sections.Values.Sum (adapter => adapter.Count + 1);
			}
		}

		public override int ViewTypeCount {
			get {
				return 1 + sections.Values.Sum (adapter => adapter.ViewTypeCount);
			}
		}

		public override int GetItemViewType (int position)
		{
			int type = 1;
			foreach (var section in sections.Keys) {
				var adapter = sections [section];
				int size = adapter.Count + 1;

				// check if position inside this section
				if (position == 0)
					return TypeSectionHeader;
				if (position < size)
					return type + adapter.GetItemViewType (position - 1);

				// otherwise jump into next section
				position -= size;
				type += adapter.ViewTypeCount;
			}
			return -1;
		}

		public override bool AreAllItemsEnabled ()
		{
			return false;
		}

		public override bool IsEnabled (int position)
		{
			return (GetItemViewType (position) != TypeSectionHeader);
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			int sectionnum = 0;
			foreach (var section in sections.Keys) {
				var adapter = sections [section];
				int size = adapter.Count + 1;

				// check if position inside this section
				if (position == 0)
					return headers.GetView (sectionnum, convertView, parent);
				if (position < size) {
					View itemView = adapter.GetView (position - 1, convertView, parent);
					Java.Lang.Object obj = adapter.GetItem (position - 1);
                    IdiomItem item = obj.Cast<IdiomItem>();
                    var sourceTextView = itemView.FindViewById<TextView>(Resource.Id.IdiomsTextFromTextView);
                    sourceTextView.SetText(item.TextFrom, TextView.BufferType.Normal);
                    var translatedTextView = itemView.FindViewById<TextView>(Resource.Id.IdiomsTextToTextView);
                    translatedTextView.SetText(item.TextTo, TextView.BufferType.Normal);
                    var tvExampleTextFrom = itemView.FindViewById<TextView>(Resource.Id.tvExampleTextFrom);
                    string exampleTextFrom = string.IsNullOrEmpty(item.ExampleTextFrom) ? string.Empty : string.Format("- {0}", item.ExampleTextFrom);
                    tvExampleTextFrom.SetText(exampleTextFrom, TextView.BufferType.Normal);

                    string exampleTextTo = string.IsNullOrEmpty(item.ExampleTextTo) ? string.Empty : string.Format("- {0}", item.ExampleTextTo);
                    var tvExampleTextTo = itemView.FindViewById<TextView>(Resource.Id.tvExampleTextTo);
                    tvExampleTextTo.SetText(exampleTextTo, TextView.BufferType.Normal);
                    return itemView;
                }

				// otherwise jump into next section
				position -= size;
				sectionnum++;
			}
			return null;
		}

        public override long GetItemId (int position)
		{
			return position;
		}

		// -- ISectionIndexer --
		public int GetPositionForSection (int section)
		{
			int sectionnum = 0;
			int position = 0;
			foreach (var s in sections.Keys) {
				var adapter = sections [s];
				int size = adapter.Count + 1;

				// check if position inside this section
				if (section == sectionnum)
					return position;

				position += size;
				// otherwise jump into next section
				sectionnum++;
			}
			return -1;
		}

		public int GetSectionForPosition (int position)
		{
			return 1;
		}

        public Java.Lang.Object[] GetSections ()
		{
			if (sectionObjects == null) {
				var keys = sections.Keys.ToArray ();
				sectionObjects = new Java.Lang.Object[keys.Length];
				for (int i = 0; i < keys.Length; i++) {
					sectionObjects [i] = new Java.Lang.String (keys [i]);
				}
			}
			return sectionObjects;
		}
	}
}