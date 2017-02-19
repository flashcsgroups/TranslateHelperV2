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

    public class FavoritesAdapter : BaseAdapter, ISectionIndexer
	{
		Dictionary<string, IAdapter> sections = new Dictionary<string, IAdapter> ();
		ArrayAdapter<string> headers;
		const int TypeSectionHeader = 0;
        Java.Lang.Object[] sectionObjects;
        private Context context;

        public FavoritesAdapter (Context context)
		{
			this.context = context;
			headers = new ArrayAdapter<string> (context, Resource.Layout.FavoritesSectionListHeader);
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

        internal void ListItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            /*ListView lvVariants = (ListView)sender;
            var objItem = GetItem(e.Position);
            FavoritesManager favoritesManager = new FavoritesManager(SqlLiteInstance.DB);
            favoritesManager.DeleteWord(objItem.Cast<FavoritesItem>().FavoritesId);
            var sourceTextView = e.View.FindViewById<TextView>(Resource.Id.SourceTextView);
            sourceTextView.SetTextAppearance(context, Resource.Style.FavoritesDeletedItemTextView);
            var translatedTextView = e.View.FindViewById<TextView>(Resource.Id.TranslatedTextView);
            translatedTextView.SetTextAppearance(context, Resource.Style.FavoritesDeletedItemTextView);
            var transcriptionTextView = e.View.FindViewById<TextView>(Resource.Id.TranscriptionTextView);
            transcriptionTextView.SetTextAppearance(context, Resource.Style.FavoritesDeletedItemTextView);
            var posTextView = e.View.FindViewById<TextView>(Resource.Id.PosTextView);
            posTextView.SetTextAppearance(context, Resource.Style.FavoritesDeletedItemTextView);            
            Toast.MakeText(context, Resource.String.msg_string_deleted_from_fav, ToastLength.Short).Show();*/
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
					FavoriteItem itemResult = obj.Cast<FavoriteItem> ();

					var sourceTextView = itemView.FindViewById<TextView> (Resource.Id.SourceTextView);
                    string originalText = itemResult.OriginalText!=null ? itemResult.OriginalText.ToString() : "not found";
                    sourceTextView.SetText (originalText, TextView.BufferType.Normal);
					var translatedTextView = itemView.FindViewById<TextView> (Resource.Id.TranslatedTextView);
                    string translatedText = itemResult.TranslatedText != null ? itemResult.TranslatedText.ToString() : "not found";
                    translatedTextView.SetText (translatedText, TextView.BufferType.Normal);
					//var transcriptionTextView = itemView.FindViewById<TextView> (Resource.Id.TranscriptionTextView);
					//transcriptionTextView.SetText (string.IsNullOrEmpty (itemResult.OriginalTranscription) ? "" : "[" + itemResult.OriginalTranscription + "]", TextView.BufferType.Normal);

					//var posTextView = itemView.FindViewById<TextView> (Resource.Id.PosTextView);
					//posTextView.SetText (DefinitionTypesManager.GetRusNameForEnum (itemResult.DefinitionType), TextView.BufferType.Normal);
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