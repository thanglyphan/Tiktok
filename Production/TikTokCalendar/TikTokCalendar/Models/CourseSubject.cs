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

		public CourseSubject(int id, int semester, int courseId, int subjectId, List<Course> courses, List<Subject> subjects)
		{
			ID = id;
			Semester = semester;
			foreach (var course in courses)
			{
				if (course.ID == courseId)
				{
					Course = course;
					break;
				}
			}
			foreach (var subject in subjects)
			{
				if (subject.ID == subjectId)
				{
					Subject = subject;
					break;
				}
			}
		}
	}
}