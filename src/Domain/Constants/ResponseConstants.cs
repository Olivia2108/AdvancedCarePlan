using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
	public class ResponseConstants
	{
		public const string InvalidId = "Invalid Id provided";
		public const string ModelStateInvalid = "Model State is Invalid";
		public const string NotFound = "No record was found";
		public const string Found = "Records returned";
		public const string Saved = "Record saved successfully";
		public const string IsExist = "Patient with this username already exist";
		public const string EndDateInvalid = "Actual End Date must be greater than Actual Start Date";
		public const string NotSaved = "Record was not saved, pls try again later";
		public const string Updated = "Record was updated successfully";
		public const string NotUpdated = "Record was not updated, pls try again later";
		public const string Deleted = "Record was deleted successfully";
		public const string NotDeleted = "Record was not deleted, pls provide a valid ID";
	}
}
