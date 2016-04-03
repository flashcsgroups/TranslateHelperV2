using System.Collections.Generic;
using Android.App;
using Android.Widget;
using PortableCore.BL.Contracts;
using TranslateHelper.Droid.Adapters;

namespace TranslateHelper.Droid
{
    public class DynamicResultViewLayout : LinearLayout
    {
        private Activity context;
        private List<TranslateResultDefinition> definitions;

        public DynamicResultViewLayout(Activity context, List<TranslateResultDefinition> definitions) : base(context)
        {
            this.definitions = definitions;
            this.context = context;
            this.Initialize();
        }

        private void Initialize()
        {
            this.Orientation = Orientation.Vertical;
            ListView lvVariants = new ListView(context);
            lvVariants.FastScrollEnabled = false;
            this.AddView(lvVariants);
            var adapter = new TranslateResultViewVariantAdapter(context, definitions);
            lvVariants.Adapter = adapter;
            lvVariants.ItemClick += adapter.ListItemClick;
        }
    }
}