
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

namespace TranslateHelper.Droid
{
    [Activity(Label = "Избранное", Theme = "@style/MyTheme")]
    public class FavoritesActivity : Activity
	{
        static string[] COUNTRIES = new string[] { "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "British Virgin Islands", "Brunei", "Bulgaria", "Burkina Faso", "Burundi", "Cote d'Ivoire", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Cook Islands", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Democratic Republic of the Congo", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Faeroe Islands", "Falkland Islands", "Fiji", "Finland", "Former Yugoslav Republic of Macedonia", "France", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Island and McDonald Islands", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macau", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "North Korea", "Northern Marianas", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn Islands", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russia", "Rwanda", "Sqo Tome and Principe", "Saint Helena", "Saint Kitts and Nevis", "Saint Lucia", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Saudi Arabia", "Senegal", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "South Korea", "Spain", "Sri Lanka", "Sudan", "Suriname", "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "The Bahamas", "The Gambia", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Virgin Islands", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Wallis and Futuna", "Western Sahara", "Yemen", "Yugoslavia", "Zambia", "Zimbabwe" };
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.Favorites);

            // Create your application here
            /*TranslatedExpressionManager expr = new TranslatedExpressionManager();
			var translatedItems = expr.GetTranslatedItems ();
			var ListFavoritesStrings = new List<string>();
			foreach (TranslatedExpression item in translatedItems) {
				ListFavoritesStrings.Add (item.TranslatedText);
			}
			ListView lv = FindViewById<ListView>(Resource.Id.listFavoritesListView);
			lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListFavoritesStrings.ToArray());*/
            /*var listView = FindViewById<ListView>(Resource.Id.listFavoritesListView);
            FavoritesManager favManager = new FavoritesManager();
            var items = favManager.GetItems();
            listView.Adapter = new FavoritesAdapter(this, items);*/
            var listView = FindViewById<ListView>(Resource.Id.listFavoritesListView);

            listView.FastScrollEnabled = true;

            var data = new TranslateResultIndexedCollection<TranslateResult>();
            foreach (var c in COUNTRIES) data.Add(new TranslateResult() { OriginalText=c});
            var sortedContacts = data.GetSortedData();
            listView.Adapter = CreateAdapter(sortedContacts);


        }

        FavoritesAdapter CreateAdapter<T>(Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
        {
            var adapter = new FavoritesAdapter(this);
            foreach (var e in sortedObjects.OrderBy(de => de.Key))
            {
                var section = e.Value;
                var label = e.Key + "(" + section.Count.ToString() + ")";
                adapter.AddSection(label, new ArrayAdapter<T>(this, Resource.Layout.FavoritesSectionListItem, section));
            }
            return adapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_FavoritesScreen, menu);

            /*IMenuItem menuItem = menu.FindItem(Resource.Id.menu_delete_task);
            menuItem.SetTitle(task.ID == 0 ? "Cancel" : "Delete");
            */
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                /*case Resource.Id.menu_save_task:
                    Save();
                    return true;

                case Resource.Id.menu_delete_task:
                    CancelDelete();
                    return true;
                    */
                default:
                    Finish();
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}

