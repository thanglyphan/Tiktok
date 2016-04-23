using System.ComponentModel.DataAnnotations;

// User table
// TODO Refactor to use the new "login" system or delete and just load from cookies

namespace TikTokCalendar.Models
{
	public class Account
	{
		public int ID { get; set; }


		//public string Name { get; set; }
		//public string CourseID { get; set; }

		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		public int CourseID { get; set; }
		public int SemesterID { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }

		[Display(Name = "Remember on this computer")]
		public bool RememberMe { get; set; }

		public virtual Course Course { get; set; }
		//		cn.Open();
		//		cmd.Parameters.Add(new SqlParameter("@p",SqlDbType.NVarChar)).Value = TikTokCalendar.Encoder.SHA1.Encode(_password);
		//		cmd.Parameters.Add(new SqlParameter("@u",SqlDbType.NVarChar)).Value = _username;
		//		var cmd = new SqlCommand(_sql,cn);
		//			   @"WHERE [Username] = @u AND [Password] = @p";
		//		string _sql = @"SELECT [Username] FROM [dbo].[Account] " +
		//	using (var cn = new SqlConnection("Server = tcp:r8ky8qlxxp.database.windows.net,1433; Database = tiktokcal_db; User ID = TikTokAdmin@r8ky8qlxxp; Password =Gruppe15Login; Trusted_Connection = False; Encrypt = True;Connection Timeout=30;")) {
		//{

		//public bool IsValid(string _username,string _password)
		//		var reader = cmd.ExecuteReader();
		//		if (reader.HasRows) {
		//			reader.Dispose();
		//			cmd.Dispose();
		//			return true;
		//		}
		//		else {
		//			reader.Dispose();
		//			cmd.Dispose();
		//			return false;
		//		}
		//	}
		//}
	}
}