using System;
using System.Collections.Generic;

using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Views;

namespace SilexSample
{
	[Activity (Label = "SilexSample", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		AlertDialog.Builder builder;
		private ListView my_list;
		private TextView title;
		private ImageButton btn_add;
		private ImageButton btn_refresh;

		private LoadTask asyncTask;  
		private Handler mHandler = new Handler();
		Runnable mUpdateTimeTask;

		List<FoodMenu> datas = new List<FoodMenu> ();
		private FoodAdapter adapter;
		private int current_position = 0;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Main);

			my_list = FindViewById<ListView> (Resource.Id.my_list);
			title = FindViewById<TextView> (Resource.Id.title);

			adapter = new FoodAdapter (this, this.datas);
			my_list.Adapter = adapter;
			ListEvent ();
			SetActionBar();

			mUpdateTimeTask = new Runnable(Run);
			mHandler.RemoveCallbacks(mUpdateTimeTask);
			mHandler.Post(mUpdateTimeTask);
		}

		private void RefreshData()
		{
			mUpdateTimeTask = new Runnable(Run);
			mHandler.RemoveCallbacks(mUpdateTimeTask);
			mHandler.Post(mUpdateTimeTask);
		}

		private void SetActionBar()
		{
			Context context = ActionBar.ThemedContext;

			ActionBar.DisplayOptions = ActionBarDisplayOptions.ShowCustom;
			ActionBar.SetCustomView(Resource.Layout.bar_menu);

			btn_add = FindViewById<ImageButton>(Resource.Id.add);
			this.btn_add.Click += (o, e) =>
			{
				builder = new AlertDialog.Builder(this);
				builder.SetTitle("Add Data");

				View view_layout = LayoutInflater.From(this).Inflate(Resource.Layout.form_food, null);
				EditText name = view_layout.FindViewById<EditText>(Resource.Id.name);
				EditText price = view_layout.FindViewById<EditText>(Resource.Id.price);

				builder.SetView(view_layout);
				builder.SetPositiveButton("Ok", (os, es) =>
				{
					FoodMenu temp = new FoodMenu(name.Text, double.Parse(price.Text));
					FoodLoader.InsertData(temp);
					mHandler.RemoveCallbacks(mUpdateTimeTask);
					mHandler.Post(mUpdateTimeTask);
					((Dialog)os).Dismiss();
				});

				builder.SetNegativeButton("Cancel", (os, es) =>
				{
					((Dialog)os).Dismiss();
				});
				builder.Show();
			};

			btn_refresh = FindViewById<ImageButton>(Resource.Id.refresh);
			btn_refresh.Click += (o, e) =>
			{
				RefreshData();
			};
		}

		private void ListEvent(){
			my_list.ItemClick += (o, e) => {
				this.current_position=e.Position;
				builder = new AlertDialog.Builder (this);
				builder.SetMessage("Select an action");
				builder.SetNegativeButton ("Delete", DeleteClicked);
				builder.SetPositiveButton("Update",UpdateClicked);
				builder.Show();
			};
		}

		private void DeleteClicked (object sender, DialogClickEventArgs e)
		{
			FoodLoader.DeleteData (datas [current_position].Id);
			mHandler.RemoveCallbacks(mUpdateTimeTask);
			mHandler.Post(mUpdateTimeTask);
			((Dialog)sender).Dismiss ();
		}
			
		private void UpdateClicked (object sender, DialogClickEventArgs e)
		{
			((Dialog)sender).Dismiss ();

			builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Update Data");
			//
			View view_layout = LayoutInflater.From (this).Inflate (Resource.Layout.form_food, null);

			EditText name = view_layout.FindViewById<EditText> (Resource.Id.name);
			EditText price = view_layout.FindViewById<EditText> (Resource.Id.price);

			name.Text = datas [current_position].Name;
			price.Text = datas [current_position].Price.ToString ();

			//
			builder.SetView (view_layout);
			builder.SetPositiveButton ("Ok", (os, es) => {
				FoodMenu temp=new FoodMenu(name.Text,double.Parse(price.Text));
				FoodLoader.UpdateData(temp,datas[current_position].Id);
				mHandler.RemoveCallbacks(mUpdateTimeTask);
				mHandler.Post(mUpdateTimeTask);
				((Dialog)os).Dismiss ();
			});

			builder.SetNegativeButton ("Cancel", (os, es) => {
				((Dialog)os).Dismiss();
			});
			builder.Show();
		}

		public void Run(){
			if (asyncTask != null)
			if (asyncTask.GetStatus() == AsyncTask.Status.Running)
				asyncTask.Cancel(true);   		

			asyncTask = (LoadTask)new LoadTask (this).Execute ();
		}

		private class MyResult {
			public Boolean success;
			public Exception exception;
		}

		private class LoadTask : AsyncTask< Java.Lang.Void , Java.Lang.Void , MyResult>{
			MainActivity parent;
			public LoadTask(MainActivity parent){
				this.parent=parent;
			}

			protected override void OnPreExecute() {
				parent.title.Text = "Loading...";
				base.OnPreExecute();
			}

			protected override void OnPostExecute(MyResult result){
				if (result.exception != null && result.success == false)
					Toast.MakeText (parent, result.exception.Message, ToastLength.Long).Show ();

				parent.title.Text = "List of Food";
				base.OnPostExecute (result);
			}

			protected override MyResult RunInBackground(params Java.Lang.Void[] @params){			
				MyResult result = new MyResult();
				result.exception = null;
				result.success = true;

				try {
					parent.RunOnUiThread(() => {
						parent.LoadData();
					});
				}
				catch (Exception e) {
					result.exception = e;
					result.success = false;
				}
				return result;
			}
		}

		public void LoadData(){
			try {
				this.datas=FoodLoader.LoadData();
				adapter = new FoodAdapter (this, this.datas);
				my_list.Adapter = adapter;
			} 
			catch (Exception) {
				Toast.MakeText (this, "Something error on the server or your connection", ToastLength.Long).Show ();
			}
		}
	}
}


