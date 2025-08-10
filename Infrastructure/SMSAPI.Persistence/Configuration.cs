using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Persistence
{
	public class Configuration
	{
		static public string ConnectionString
		{
			get
			{

				ConfigurationManager configurationManager = new();

				configurationManager.AddJsonFile("appsettings.json");

				return configurationManager.GetConnectionString("StockDbConnectionString");

			}
		}

	}
}

