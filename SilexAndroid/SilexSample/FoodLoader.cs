using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace SilexSample
{
	public class FoodLoader
	{
		private static readonly string serverLink = "http://192.168.1.103/medical/web/index2.php/";

		public FoodLoader ()
		{}

		public static List<FoodMenu> LoadData()
		{
			string link = serverLink + "daftar";
			List<FoodMenu> datas = new List<FoodMenu> ();
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
				request.Method="GET";
				request.ContentType = "application/json";

				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) {
					Stream receiveStream = response.GetResponseStream();
					StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
					string outputString = readStream.ReadToEnd();

					response.Close();
					readStream.Close();

					outputString = outputString.Trim().Replace("// ", "");
					var resultArray = JArray.Parse(outputString);

					for (int i=0; i < resultArray.Count; i++) {
						var jstock = (JObject)resultArray[i];

						FoodMenu temp_data=new FoodMenu(
							int.Parse((string)jstock["id"]),
							(string)jstock["name"],
							double.Parse((string)jstock["price"])
						);

						datas.Add(temp_data);
					}
				}
			}
			catch (Exception e) {
				throw;
			}
			return datas;
		}

		public static String InsertData(FoodMenu data)
		{
			string link = serverLink + "insert";
			String result = "";
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);

				StringBuilder postData = new StringBuilder();
				postData.Append("name="+data.Name+"&");
				postData.Append("price="+data.Price.ToString());
				var data_encode = Encoding.ASCII.GetBytes(postData.ToString());

				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = data_encode.Length;

				using (var stream = request.GetRequestStream())
				{
					stream.Write(data_encode, 0, data_encode.Length);
				}

				var response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) {
					Stream receiveStream = response.GetResponseStream();
					StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
					result = readStream.ReadToEnd();

					response.Close();
					readStream.Close();
				}
			}
			catch (Exception) {
				throw;
			}
			return result;
		}

		public static String UpdateData(FoodMenu data, int id)
		{
			string link = serverLink + "update/" + id.ToString ();
			String result = "";
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);

				StringBuilder postData = new StringBuilder();
				postData.Append("name="+data.Name+"&");
				postData.Append("price="+data.Price.ToString());
				var data_encode = Encoding.ASCII.GetBytes(postData.ToString());

				request.Method = "PUT";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = data_encode.Length;

				using (var stream = request.GetRequestStream())
				{
					stream.Write(data_encode, 0, data_encode.Length);
				}

				var response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) {
					Stream receiveStream = response.GetResponseStream();
					StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
					result = readStream.ReadToEnd();

					response.Close();
					readStream.Close();
				}
			}
			catch (Exception) {
				throw;
			}
			return result;
		}

		public static String DeleteData(int id)
		{
			string link = serverLink + "delete/" + id.ToString ();
			String result = "";
			try {
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
				request.Method = "DELETE";
				var response = (HttpWebResponse)request.GetResponse();

				if (response.StatusCode == HttpStatusCode.OK) {
					Stream receiveStream = response.GetResponseStream();
					StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
					result = readStream.ReadToEnd();

					response.Close();
					readStream.Close();
				}
			}
			catch (Exception) {
				throw;
			}
			return result;
		}
	}
}