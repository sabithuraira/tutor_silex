using System;

namespace SilexSample
{
	public class FoodMenu
	{
		private int id;
		private string name;
		private double price;

		public FoodMenu (string name, double price)
		{
			this.name = name;
			this.price = price;
		}

		public FoodMenu (int id, string name, double price)
		{
			this.id = id;
			this.name = name;
			this.price = price;
		}

		public int Id{get{ return this.id; }}

		public string Name{get{ return this.name; }}
		public double Price{get{ return this.price; }}
	}
}

