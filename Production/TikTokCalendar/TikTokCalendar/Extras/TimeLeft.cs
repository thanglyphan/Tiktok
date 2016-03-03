using System;

namespace TikTokCalendar
{
    public class TimeLeft
    {
        Random rng = new Random();
        public string GetTimeLeft(DateTime item)
        {
            string timeLeft = "";
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
                                            timeLeft = "Sekunder igjen: " + (int)Math.Round((item - DateTime.Now).TotalSeconds, 4);
                                        }
                                        else
                                        {
                                            int mins = (int)Math.Floor((item - DateTime.Now).TotalMinutes);
                                            if (mins > 1)
                                            {
                                                timeLeft = "" + mins + " minutter til";
                                            }
                                            else
                                            {
                                                timeLeft = "Ett minutt til";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int hours = (int)Math.Floor((item - DateTime.Now).TotalHours);
                                        if (hours > 1)
                                        {
                                            timeLeft = "" + hours + " timer til";
                                        }
                                        else
                                        {
                                            timeLeft = "Én time til";
                                        }
                                    }
                                }
                                else
                                {
                                    int days = (int)Math.Ceiling((item - DateTime.Now).TotalDays);
                                    if (days > 1)
                                    {
                                        timeLeft = "" + days + " dager til";
                                    }
                                    else
                                    {
                                        timeLeft = "Én dag til";
                                    }
                                    if (days == 14)
                                    {
                                        timeLeft = "To uker til";
                                    }
                                }
                            }
                            else
                            {
                                int weeks = (int)Math.Floor((item - DateTime.Now).TotalDays / 7);
                                timeLeft = "Over " + weeks + " uker til";
                            }
                        }
                        else
                        {
                            int months = ((item.Year - DateTime.Now.Year) * 12 + item.Month - DateTime.Now.Month);
                            if (months > 1)
                            {
                                timeLeft = "Over " + months + " måneder til";
                            }
                            else
                            {
                                timeLeft = "Over én måned til";
                            }
                        }
                    }
                    else
                    {
                        timeLeft = "Over et år til";
                    }
                }
                else
                {
                    timeLeft = "Mange år til";
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