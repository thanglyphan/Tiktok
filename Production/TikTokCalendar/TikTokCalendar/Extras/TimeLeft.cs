using System;

namespace TikTokCalendar
{
	public class TimeLeft
	{
		private Random rng = new Random();

		public string GetTimeLeft(DateTime item)
		{
			var timeLeft = "";
			if (item > DateTime.Now)
			{
				if (item < DateTime.Now.AddYears(2))
				{
					if (item < DateTime.Now.AddYears(1))
					{
						if (item < DateTime.Now.AddMonths(1))
						{
							if (item < DateTime.Now.AddDays(14))
							{
								if (item < DateTime.Now.AddDays(1))
								{
									if (item < DateTime.Now.AddHours(1))
									{
										if (item < DateTime.Now.AddMinutes(1))
										{
											timeLeft = Resources.LocalizedText.XSecondsAway + " " + (int) Math.Round((item - DateTime.Now).TotalSeconds, 4);
										}
										else
										{
											var mins = (int) Math.Floor((item - DateTime.Now).TotalMinutes);
											if (mins > 1)
											{
												timeLeft = "" + mins + " " + Resources.LocalizedText.XMinutesAway;
											}
											else
											{
												timeLeft = Resources.LocalizedText.OneMinuteAway;
											}
										}
									}
									else
									{
										var hours = (int) Math.Floor((item - DateTime.Now).TotalHours);
										if (hours > 1)
										{
											timeLeft = "" + hours + " " + Resources.LocalizedText.XHoursAway;
										}
										else
										{
											timeLeft = Resources.LocalizedText.OneHourAway;
										}
									}
								}
								else
								{
									var days = (int) Math.Ceiling((item - DateTime.Now).TotalDays);
									if (days > 1)
									{
										timeLeft = "" + days + " " + Resources.LocalizedText.XDaysAway;
									}
									else
									{
										timeLeft = Resources.LocalizedText.OneDayAway;
									}
									if (days == 14)
									{
										timeLeft = Resources.LocalizedText.ToWeeksAway;
									}
								}
							}
							else
							{
								var weeks = (int) Math.Floor((item - DateTime.Now).TotalDays/7);
								timeLeft = Resources.LocalizedText.OverX + " " + weeks + " " + Resources.LocalizedText.XWeeksAway;
							}
						}
						else
						{
							var months = (item.Year - DateTime.Now.Year)*12 + item.Month - DateTime.Now.Month;
							if (months > 1)
							{
								timeLeft = Resources.LocalizedText.OverX + " " + months + " " + Resources.LocalizedText.XMonthsAway;
							}
							else
							{
								timeLeft = Resources.LocalizedText.OverAMonthAway;
							}
						}
					}
					else
					{
						timeLeft = Resources.LocalizedText.OverAYearAway;
					}
				}
				else
				{
					timeLeft = Resources.LocalizedText.ManyYearsAway;
				}
			}
			else
			{
				timeLeft = "";
			}
			return timeLeft;
		}
	}
}