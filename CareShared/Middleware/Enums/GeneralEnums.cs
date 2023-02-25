using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareShared.Middleware.Enums
{
	public class GeneralEnums
	{
		public enum DbInfo
		{
			NoIdFound = -4,
			ErrorThrown = -5,
		}
 
		public enum AuditType
		{
			None = -1,
			Create = 0,
			Update = 1,
			Delete = 2,

		}
		public enum Titles
		{
			Sir,
			Ma,
			Mr,
			Mrs,
			Ms,
			Miss,
			Madam,
			Master,
			Dr,
			Prof,

		}
	}
}
