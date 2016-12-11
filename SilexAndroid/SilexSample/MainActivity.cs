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



		/*
		private void CallSimulasi(){
			builder = new AlertDialog.Builder (this);
			builder.SetTitle ("Pilih Test");
			//
			View view_layout = LayoutInflater.From (this).Inflate (Resource.Layout.simulasi_proses, null);

			Spinner test = view_layout.FindViewById<Spinner> (Resource.Id.test);
			Spinner tahun = view_layout.FindViewById<Spinner> (Resource.Id.tahun);
			Spinner jenis = view_layout.FindViewById<Spinner> (Resource.Id.jenis);

			MenuAdapter spinnerAdapter = new MenuAdapter (this, Android.Resource.Layout.SimpleSpinnerItem, this.list_label.ToArray ());
			spinnerAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			test.Adapter = spinnerAdapter;

			test.ItemSelected += (o, e) => {
				this.list_tahun.Clear();
				if(e.Position>0){
					this.list_tahun=this.list_master.Where(lm=>lm.Label==list_label[e.Position]).Select(lm=>lm.Tahun.ToString()).Distinct().ToList();
				}

				MenuAdapter tahunAdapter = new MenuAdapter (this, Android.Resource.Layout.SimpleSpinnerItem, this.list_tahun.ToArray ());
				tahunAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
				tahun.Adapter = tahunAdapter;
			};

			tahun.ItemSelected += (o, e) => {
				this.list_jenis.Clear();
				this.list_ljenis.Clear();
				//if(e.Position>0){
				this.list_jenis =this.list_master.Where(lm=>lm.Label==list_label[test.SelectedItemPosition] && lm.Tahun.ToString()==list_tahun[e.Position]).Select(lm=>lm.Jenis).ToList();
				this.list_ljenis =this.list_master.Where(lm=>lm.Label==list_label[test.SelectedItemPosition] && lm.Tahun.ToString()==list_tahun[e.Position]).Select(lm=>lm.LJenis).ToList();
				//}

				MenuAdapter jenisAdapter = new MenuAdapter (this, Android.Resource.Layout.SimpleSpinnerItem, this.list_ljenis.ToArray ());
				jenisAdapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
				jenis.Adapter = jenisAdapter;
			};

			//
			builder.SetView (view_layout);
			builder.SetPositiveButton ("Ok", (os, es) => {
				if(test.SelectedItemPosition>0)
				{
					TestMaster id_test= new TestMaster();

					if(this.list_jenis.Count>0){
						id_test=this.list_master.Where(t=>t.Label==list_label[test.SelectedItemPosition] && t.Tahun.ToString()==list_tahun[tahun.SelectedItemPosition] && t.Jenis==list_jenis[jenis.SelectedItemPosition]).Single();
					}
					else{
						id_test=this.list_master.Where(t=>t.Label==list_label[test.SelectedItemPosition] && t.Tahun.ToString()==list_tahun[tahun.SelectedItemPosition]).Single();
					}

					if(this.is_simulasi==1){
						Intent myIntent = new Intent (this, typeof(SimulasiActivity));
						myIntent.PutExtra("id_test",id_test.Id);
						StartActivity (myIntent);
					}
					else{
						Intent myIntent = new Intent (this, typeof(BacaActivity));
						myIntent.PutExtra("id_test",id_test.Id);
						StartActivity (myIntent);
					}
				}

				((Dialog)os).Dismiss ();
			});

			builder.SetNegativeButton ("Cancel", (os, es) => {
				((Dialog)os).Dismiss();
			});
			builder.Show();

		}
		*/
	}
}


