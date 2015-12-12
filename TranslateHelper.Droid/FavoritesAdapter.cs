using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TranslateHelper.Core;
using TranslateHelper.Core.BL.Contracts;
using TranslateHelper.Core.Helpers;

namespace TranslateHelper.Droid
{

    public class FavoritesAdapter : BaseAdapter, ISectionIndexer
    {
        //protected Activity context = null;
        //protected IList<Favorites> listFavorites = new List<Favorites>();
        Dictionary<string, IAdapter> sections = new Dictionary<string, IAdapter>();
        ArrayAdapter<string> headers;
        const int TypeSectionHeader = 0;

        public FavoritesAdapter(Context context)            
        {
            //this.context = context;
            //this.listFavorites = listFavorites;
			headers = new ArrayAdapter<string>(context, Resource.Layout.FavoritesSectionListHeader);
        }

        public void AddSection(string section, IAdapter adapter)
        {
            headers.Add(section);
            sections.Add(section, adapter);
        }

        public override Java.Lang.Object GetItem(int position)
        {
            int op = position;
            foreach (var section in sections.Keys)
            {
                var adapter = sections[section];
                int size = adapter.Count + 1;
                if (position == 0)
                    return section;
                if (position < size)
                    return adapter.GetItem(position - 1);
                position -= size;
            }
            return null;
        }

        public override int Count
        {
            get
            {
                return sections.Values.Sum(adapter => adapter.Count + 1);
            }
        }

        public override int ViewTypeCount
        {
            get
            {
                return 1 + sections.Values.Sum(adapter => adapter.ViewTypeCount);
            }
        }

        public override int GetItemViewType(int position)
        {
            int type = 1;
            foreach (var section in sections.Keys)
            {
                var adapter = sections[section];
                int size = adapter.Count + 1;

                // check if position inside this section
                if (position == 0)
                    return TypeSectionHeader;
                if (position < size)
                    return type + adapter.GetItemViewType(position - 1);

                // otherwise jump into next section
                position -= size;
                type += adapter.ViewTypeCount;
            }
            return -1;
        }

        public override bool AreAllItemsEnabled()
        {
            return false;
        }

        public override bool IsEnabled(int position)
        {
            return (GetItemViewType(position) != TypeSectionHeader);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            int sectionnum = 0;
            foreach (var section in sections.Keys)
            {
                var adapter = sections[section];
                int size = adapter.Count + 1;

                // check if position inside this section
                if (position == 0)
                    return headers.GetView(sectionnum, convertView, parent);
                if (position < size)
                {
                    View itemView = adapter.GetView(position - 1, convertView, parent);
                    Java.Lang.Object obj = adapter.GetItem(position - 1);
					TranslateResult itemResult = obj.Cast<TranslateResult>();

					var sourceTextView = itemView.FindViewById<TextView>(Resource.Id.SourceTextView);
					sourceTextView.SetText(itemResult.OriginalText.ToString(), TextView.BufferType.Normal);
					/*var translatedTextView = itemView.FindViewById<TextView>(Resource.Id.TranslatedTextView);
					translatedTextView.SetText(itemResult.TranslatedText.ToString(), TextView.BufferType.Normal);*/
					/*var transcriptionTextView = itemView.FindViewById<TextView>(Resource.Id.TranscriptionTextView);
					var posTextView = itemView.FindViewById<TextView>(Resource.Id.PosTextView);
					sourceTextView.SetText(itemResult.OriginalText.ToString(), TextView.BufferType.Normal);
					translatedTextView.SetText(itemResult.TranslatedText.ToString(), TextView.BufferType.Normal);
					transcriptionTextView.SetText(itemResult.Ts.ToString(), TextView.BufferType.Normal);
					posTextView.SetText(itemResult.Pos.ToString(), TextView.BufferType.Normal);*/
                    return itemView;
                }

                // otherwise jump into next section
                position -= size;
                sectionnum++;
            }
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        // -- ISectionIndexer --
        public int GetPositionForSection(int section)
        {
            int sectionnum = 0;
            int position = 0;
            foreach (var s in sections.Keys)
            {
                var adapter = sections[s];
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

        public int GetSectionForPosition(int position)
        {
            return 1;
        }
        Java.Lang.Object[] sectionObjects;
        public Java.Lang.Object[] GetSections()
        {
            if (sectionObjects == null)
            {
                var keys = sections.Keys.ToArray();
                sectionObjects = new Java.Lang.Object[keys.Length];
                for (int i = 0; i < keys.Length; i++)
                {
                    sectionObjects[i] = new Java.Lang.String(keys[i]);
                }
            }
            return sectionObjects;
        }
        /*public override Favorites this[int position]
        {
            get { return listFavorites[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return listFavorites.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = listFavorites[position];
            var view = (convertView ?? this.context.LayoutInflater.Inflate(Resource.Layout.FavoritesItem, parent, false)) as LinearLayout;

            var translatedTextView = view.FindViewById<TextView>(Resource.Id.TranslatedTextView1);
            var sourceTextView = view.FindViewById<TextView>(Resource.Id.SourceTextView1);

            translatedTextView.SetText(listFavorites[position].TranslatedExpressionID.ToString(), TextView.BufferType.Normal);
            sourceTextView.SetText("test", TextView.BufferType.Normal);

            return view;
        }*/
    }
}