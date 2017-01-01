using Foundation;
using System;
using System.Collections.Generic;
using UIKit;

namespace silexiOS
{
    public partial class FormController : UIViewController
    {
		private int EditId = 0;
		private string EditName;
		private double EditPrice;

        public FormController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.InitializeBar();
			this.InitializeData();
		}

		private void InitializeBar()
		{
			List<UIBarButtonItem> rightButtons = new List<UIBarButtonItem>();

			rightButtons.Add(new UIBarButtonItem(
				UIBarButtonSystemItem.Save,
				(sender, e) => {
					FoodMenu temp = new FoodMenu(txtName.Text, double.Parse(txtPrice.Text));
					if (this.EditId == 0)
						FoodLoader.InsertData(temp);
					else
						FoodLoader.UpdateData(temp, EditId);
					NavigationController.PopViewController(true);
				}
			));

			if (this.EditId > 0)
			{
				rightButtons.Add(new UIBarButtonItem(
					UIBarButtonSystemItem.Trash,
					(sender, e) =>
					{
						FoodLoader.DeleteData(this.EditId);
						NavigationController.PopViewController(true);
					}
				));
			}

			NavigationItem.RightBarButtonItems = rightButtons.ToArray();
		}

		private void InitializeData()
		{
			if (this.EditId > 0)
			{
				this.txtName.Text = EditName;
				this.txtPrice.Text = EditPrice.ToString();
			}
		}

		public void SetEdit(int edit_id, string name,double price)
		{
			this.EditId = edit_id;
			this.EditName = name;
			this.EditPrice = price;
		}
    }
}