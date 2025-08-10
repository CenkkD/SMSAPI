using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMSAPI.Domain.Entities.Common;

namespace SMSAPI.Domain.Entities
{
	public class Product_Order:BaseEntity
	{
		public string ProductId { get; set; }
		public Product Product { get; set; }
		public string OrderId { get; set; }
		public Order Order { get; set; }
       
    }
}
