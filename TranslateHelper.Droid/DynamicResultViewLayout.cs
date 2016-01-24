using System.Collections.Generic;

using Android.App;
using Android.Widget;
using PortableCore.BL.Contracts;
using Droid.Core.Helpers;
using PortableCore.BL.Managers;
using PortableCore.DAL;

//�� �������:
//https://forums.xamarin.com/discussion/6029/how-to-create-ui-elements-dynamically

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
            lvVariants.Adapter = new TranslateResultViewVariantAdapter(context, definitions);
            lvVariants.ItemClick += LvVariants_ItemClick;
        }

        private void LvVariants_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            ListView lvVariants = (ListView)sender;
            var item = lvVariants.GetItemAtPosition(e.Position).Cast<TranslateResultViewVariantAdapter.LineOfTranslateResult>();
            addToFavorites(item);
        }

        //ToDo:����� �� ������ ���� � ���� �������������!
        private void addToFavorites(TranslateResultViewVariantAdapter.LineOfTranslateResult item)
        {
            FavoritesManager favoritesManager = new FavoritesManager(SqlLiteInstance.DB);
            favoritesManager.AddWordToFavorites(item.TranslateVariant.TranslatedExpressionId);
            Toast.MakeText(this.context, "������� �������� � ���������", ToastLength.Short).Show();
        }
    }
}