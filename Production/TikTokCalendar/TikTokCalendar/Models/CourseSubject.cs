using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Models
{
	public class CourseSubject
	{
		public int ID { get; set; }
		public int CourseID { get; set; }
		public int SubjectID { get; set; }
		public int Semester { get; set; }

		public Course Course { get; set; }
		public Subject Subject { get; set; }
	}
}