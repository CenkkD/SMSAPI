using Newtonsoft.Json;
using SMSAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Application.Dtos
{
	public class OrdersCreateDto
	{
		public string? CustomerId { get; set; }
		public string? ProductId {  get; set; }
		public string? Description { get; set; }
	}
}
