using System;
using System.ComponentModel.DataAnnotations;

namespace sELedit.CORE.Extencion
{
	public class EventDataChanged : EventArgs
	{
		[Required(ErrorMessage = "ListIndex is required")]
		public int ListIndex { get; set; }

		[Required(ErrorMessage = "ElementIndex is required")]
		public int ElementIndex { get; set; }

		[Required(ErrorMessage = "FieldIndex is required")]
		public int FieldIndex { get; set; }

		public string Value { get; set; }

		public string ListName { get; set; }
		public string ElementName { get; set; }
		public string FieldName { get; set; }


		public void Validate()
		{
			var context = new ValidationContext(this, null, null);
			var results = new System.Collections.Generic.List<ValidationResult>();

			if (!Validator.TryValidateObject(this, context, results, true))
			{
				foreach (var error in results)
				{
					Console.WriteLine(error.ErrorMessage);
				}

				throw new ValidationException("Some required fields are missing.");
			}
		}
	}
}
