using SMSAPI.Domain.Entities.Common;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Domain.Entities
{
	public class Order:BaseEntity	{

		public string? Description {  get; set; }
		public Guid? CustomerId { get; set; }
		public string? CustomerName { get; set; }

		public ICollection<Product_Order> Product_Orders { get; set; }

	}
}
