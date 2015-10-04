
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

namespace TranslateHelper.Droid
{
	[Activity (Label = "Dictionary")]			
	public class DictionaryActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			base.ActionBar.Hide ();
			SetContentView(Resource.Layout.Dictionary);

			// Create your application here
			/*ListView listEmployeeListView = FindViewById<ListView>(Resource.Id.listEmployeesListView);
			listEmployeeListView.ItemClick += delegate
			{
				//SetContentView (Resource.Layout.EditProject);
				StartActivity(typeof(EditEmployeeActivity));
			};*/
			//UpdateListResults ();
			EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
			editSourceText.TextChanged += delegate {
				UpdateListResults ();
			};
		}

		void UpdateListResults()
		{
			/*var ExistsEmployees = ScrumHelper.BL.Managers.EmployeeManager.GetItems();
		if (ExistsEmployees.Count < 1)
		{
			CreateDefaultData();
			ExistsEmployees = ScrumHelper.BL.Managers.EmployeeManager.GetItems();
		}*/
			var ListResultStrings = new List<string>();
			ListResultStrings.Add("Сделка");
			ListResultStrings.Add("Транзакция");
			ListResultStrings.Add("Урегулирование спора");
			ListResultStrings.Add("Ведение");
			ListView lv = FindViewById<ListView>(Resource.Id.listResultListView);
			lv.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ListResultStrings.ToArray());

		}

	}
}

