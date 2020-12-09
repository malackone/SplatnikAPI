using Splatnik.Data.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Splatnik.Data.Database.DbModels
{
	public class Currency : BaseEntity
	{
		public string ShortName { get; set; }
		public string LongName { get; set; }
	}
}
