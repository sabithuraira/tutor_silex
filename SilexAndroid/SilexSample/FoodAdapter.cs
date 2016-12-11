using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Android.Util;

using Android.Support.V4.App;
using Android.Support.V4.View;

using Java.Text;
using Java.Util;

namespace SilexSample
{
	class FoodAdapter : BaseAdapter
	{
		private LayoutInflater mInflater;
		private List<FoodMenu> datas=new List<FoodMenu>();
		private Context context;

		public FoodAdapter(Context context, List<FoodMenu> datas) {
			mInflater = LayoutInflater.From(context);
			this.datas = datas;
			this.context = context;
		}

		public override int Count {
			get{ 
				return this.datas.Count; 
			}
		}

		public override Java.Lang.Object GetItem(int position) {
			return position;
		}

		public override long GetItemId(int position) {
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent) {
			ViewHolder holder = null;
			if (convertView == null || convertView.Tag == null) {
				convertView = mInflater.Inflate (Resource.Layout.row_food, null);

				holder = new ViewHolder ();
				holder.Name = convertView.FindViewById<TextView> (Resource.Id.name);
				holder.Price = convertView.FindViewById<TextView> (Resource.Id.price);

				convertView.Tag = holder;
			} else {
				holder = (ViewHolder)convertView.Tag;
			}

			holder.Name.Text = datas [position].Name;
			holder.Price.Text = datas [position].Price.ToString ();

			return convertView;
		}

		public class ViewHolder : Java.Lang.Object {
			public TextView Name;
			public TextView Price;
		}
	}
}