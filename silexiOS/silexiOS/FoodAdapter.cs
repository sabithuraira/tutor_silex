using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

namespace silexiOS
{
	public class FoodAdapter : UITableViewSource
	{
		private MainController parent;
		private List<FoodMenu> datas = new List<FoodMenu>();
		private string cellIdentifier = "CellData";

		public FoodAdapter(MainController parent, List<FoodMenu> datas)
		{
			this.parent = parent;
			this.datas = datas;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return this.datas.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			parent.EditPosition = indexPath.Row;
			parent.PerformSegue("editSegue", parent);
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(cellIdentifier);

			if (cell == null)
			{
				cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
			}

			cell.TextLabel.Text = this.datas[indexPath.Row].Name;
			cell.DetailTextLabel.Text = this.datas[indexPath.Row].Price.ToString();
			return cell;
		}
	}
}
