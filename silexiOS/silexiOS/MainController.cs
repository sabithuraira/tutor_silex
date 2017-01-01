using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace silexiOS
{
    public partial class MainController : UIViewController
    {
		public int EditPosition = -1;

		List<FoodMenu> datas = new List<FoodMenu>();

        public MainController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			InitializeView();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.InitializeBar();
		}

		private void InitializeBar()
		{
			List<UIBarButtonItem> rightButtons = new List<UIBarButtonItem>();

			rightButtons.Add(new UIBarButtonItem(
				UIBarButtonSystemItem.Add,
				(sender, e) => { PerformSegue("editSegue", this); }
			));

			rightButtons.Add(new UIBarButtonItem(
				UIBarButtonSystemItem.Refresh,
				(sender, e) => { this.InitializeView(); }
			));

			NavigationItem.RightBarButtonItems = rightButtons.ToArray();
		}

		private void InitializeView()
		{
			this.EditPosition = -1;
			txtTitle.Text = "Loading...";

			this.datas = FoodLoader.LoadData();
			TabelFood.Source = new FoodAdapter(this, this.datas);
			TabelFood.ReloadData();

			txtTitle.Text = "List Food";
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			if (segue.Identifier == "editSegue")
			{
				var vc = segue.DestinationViewController as FormController;
;
				if (EditPosition >= 0)
				{
					FoodMenu temp = this.datas[EditPosition];
					vc.SetEdit(temp.Id, temp.Name, temp.Price);
				}
			}
		}
    }
}