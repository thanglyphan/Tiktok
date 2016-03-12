using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Extras;

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

		public void SetAndParse(string id, string courseId, string subjectId, string semester, List<Course> courses, List<Subject> subjects)
		{
			// Parse the paramteres
			ID = Utils.ParsePositiveInt(id);
			int courseID = Utils.ParsePositiveInt(courseId);
			int subjectID = Utils.ParsePositiveInt(subjectId);
			Semester = Utils.ParsePositiveInt(semester);


			// Find the course and subject that matches the IDs
			if (Semester <= 2)
			{
				Course = courses[((int)SchoolCourses.BacheloriIT) - 1];
				courseID = Course.ID;
			}
			else
			{
				foreach (var course in courses)
				{
					if (course.ID == courseID)
					{
						Course = course;
						break;
					}
				}
			}
			foreach (var subject in subjects)
			{
				if (subject.ID == subjectID)
				{
					Subject = subject;
					break;
				}
			}
			CourseID = Course.ID;
			SubjectID = Subject.ID;
		}

		public static int GetClassYearFromSemester(int semester)
		{
			int year = 0;
			int partOfYear = (semester % 2 == 0) ? 0 : 1; // 0 if before august, 1 after august
			year = (semester / 2) + partOfYear;
			return year;
		}
	}
}