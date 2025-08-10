using Newtonsoft.Json;
using SMSAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSAPI.Application.Dtos
{
	public class ProductsGetDto:BaseEntity
	{
		public string? BrandName { get; set; }
		public string? ModelName { get; set; }
		public string? ModelYear { get; set; }
		public int Stock { get; set; }
		[JsonProperty(PropertyName = "$Price")]
		public long Price { get; set; }
		public string? HorsePower { get; set; }
		public string? Kw { get; set; }
		public string? Torque { get; set; }
		public string? TurboType { get; set; }
		public string? TractionSystem { get; set; }
		public string? Transmission { get; set; }
		public string? TopSpeed { get; set; }
		public string? Zero2Hundread { get; set; }
		public string? Hundread2TwoHundread { get; set; }
		public string? FuelType { get; set; }
		public string? FuelEffiency { get; set; }
		public string? ChassisMaterial { get; set; }
		public string? CargoSpace { get; set; }
		public string? Doors { get; set; }
		public string? PassengerCapacity { get; set; }
		public string? BuildQuality { get; set; }
		public string? Length { get; set; }
		public string? Width { get; set; }
		public string? Height { get; set; }
	}
}
