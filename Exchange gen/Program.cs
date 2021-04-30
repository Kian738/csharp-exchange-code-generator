using System;
using System.Diagnostics;
using System.Threading;
using RestSharp;

namespace Exchange_gen
{
	class Program
	{
		static readonly string clientToken = "YjA3MGYyMDcyOWY4NDY5M2I1ZDYyMWM5MDRmYzViYzI6SEdAWEUmVEdDeEVKc2dUIyZfcDJdPWFSbyN+Pj0+K2M2UGhSKXpYUA==",
		switchToken = "NTIyOWRjZDNhYzM4NDUyMDhiNDk2NjQ5MDkyZjI1MWI6ZTNiZDJkM2UtYmY4Yy00ODU3LTllN2QtZjNkOTQ3ZDIyMGM3";
        //iosToken = "MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=";

        static void Main()
		{
            Console.WriteLine("Generating exchange code...");
			Console.WriteLine(GenExchange());
			Quit(true);
		}

		static void Quit(bool msg)
        {
			if (msg) Console.WriteLine("Type CONFIRM to exit");
			if (Console.ReadLine().ToUpper() == "CONFIRM") Environment.Exit(0);
			else Quit(false);
        }

		static string GenExchange()
        {
			RestClient restClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token");
			RestRequest restRequest_0 = new RestRequest((Method)1);
			restRequest_0.AddHeader("Authorization", $"Basic {clientToken}");
			restRequest_0.AddHeader("Content-Type", "application/x-www-form-urlencoded");
			restRequest_0.AddParameter("grant_type", "client_credentials");
			RestRequest restRequest_1 = restRequest_0;
			string[] array_0 = restClient.Execute(restRequest_1).Content.Split(new char[]
			{
				':'
			}, 26);
			string DeviceToken;
			try
			{
				DeviceToken = array_0[1].ToString().Split(new char[]
				{
					','
				}, 2)[0].ToString().Split(new char[]
				{
					'"'
				}, 2)[1].ToString().Split(new char[]
				{
					'"'
				}, 2)[0].ToString();
			}
			catch
			{
				Console.WriteLine("Okay its patched.");
				DeviceToken = "error";
			}
			restClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/deviceAuthorization");
			RestRequest restRequest_2 = new RestRequest((Method)1);
			restRequest_2.AddHeader("Authorization", "Bearer " + DeviceToken);
			restRequest_2.AddHeader("Content-Type", "application/x-www-form-urlencoded");
			RestRequest restRequest_3 = restRequest_2;
			string[] array_1 = restClient.Execute(restRequest_3).Content.Split(new char[]
			{
				','
			}, 8);
			string[] array_2 = array_1[3].ToString().Split(new char[]
			{
				'"'
			}, 4)[3].ToString().Split(new char[]
			{
				'"'
			}, 2);
			string[] array_3 = array_1[1].ToString().Split(new char[]
			{
				'"'
			}, 4)[3].ToString().Split(new char[]
			{
				'"'
			}, 2);
			Process.Start(array_2[0]);
			string content;
			for (; ; )
			{
				restClient = new RestClient("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token");
				RestRequest restRequest_4 = new RestRequest((Method)1);
				restRequest_4.AddHeader("Authorization", $"Basic {switchToken}");
				restRequest_4.AddHeader("Content-Type", "application/x-www-form-urlencoded");
				restRequest_4.AddParameter("grant_type", "device_code");
				restRequest_4.AddParameter("device_code", array_3[0].ToString());
				RestRequest restRequest_5 = restRequest_4;
				content = restClient.Execute(restRequest_5).Content;
				if (content.Contains("access_token"))
					break;
				content.Contains("errors.com.epicgames.not_found");
				Thread.Sleep(150);
			}
			string[] array_4 = content.Split(new char[]
			{
				':'
			}, 26);
			string token = array_4[1].ToString().Split(new char[]
			{
				','
			}, 2)[0].ToString().Split(new char[]
			{
				'"'
			}, 2)[1].ToString().Split(new char[]
			{
				'"'
			}, 2)[0].ToString() + "," + array_4[16].ToString().Split(new char[]
			{
				','
			}, 2)[0];
			string[] token_array = token.Split(new char[]
			{
				','
			}, 2);
			if (token.Contains("error"))
				return "Error";
			restClient = new RestClient("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/exchange");
			RestRequest restRequest_6 = new RestRequest(0);
			restRequest_6.AddHeader("Authorization", "bearer " + token_array[0]);
			RestRequest restRequest_7 = restRequest_6;
			string exchange = restClient.Execute(restRequest_7).Content.Split(new char[]
			{
				','
			}, 4)[1].ToString().Split(new char[]
			{
				','
			}, 2)[0].ToString().Split(new char[]
			{
				'"'
			}, 2)[1].ToString().Split(new char[]
			{
				'"'
			}, 2)[1].ToString().Split(new char[]
			{
				'"'
			}, 2)[1].ToString().Split(new char[]
			{
				'"'
			}, 2)[0].ToString();
			Console.Clear();
			return $"Exchange code:\n{exchange}";
		}
	}
}