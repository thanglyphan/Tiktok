using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

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
                                                timeLeft = "Teller ned: " + (int)(item - DateTime.Now).TotalSeconds;
                                            }
                                            else
                                            {
                                                int mins = (int)(item - DateTime.Now).TotalMinutes;
                                                timeLeft = "" + mins;
                                                if (mins > 1)
                                                {
                                                    timeLeft += " minutter til";
                                                }
                                                else
                                                {
                                                    timeLeft += " minutt til";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            int hours = (int)(item - DateTime.Now).TotalHours;
                                            timeLeft = "" + hours;
                                            if (hours > 1)
                                            {
                                                timeLeft += " timer til";
                                            }
                                            else
                                            {
                                                timeLeft += " time til";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int days = (int)(item - DateTime.Now).TotalDays;
                                        timeLeft = "" + days;
                                        if (days > 1)
                                        {
                                            timeLeft += " dager til";
                                        }
                                        else
                                        {
                                            timeLeft += " dag til";
                                        }

                                    }
                                }
                                else
                                {
                                    int weeks = (int)(item - DateTime.Now).TotalDays / 7;
                                    timeLeft = "" + weeks + " uker til";
                                }
                            }
                            else
                            {
                                int months = ((item.Year - DateTime.Now.Year) * 12 + item.Month - DateTime.Now.Month);
                                timeLeft = "" + months;
                                if (months > 1)
                                {
                                    timeLeft += " måneder til";
                                }
                                else
                                {
                                    timeLeft += " måned til";
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
                    switch (rng.Next(6))
                    {
                        case 0:
                            timeLeft = "Don't judge yourself by your past. You don't live there anymore.";
                            break;
                        case 1:
                            timeLeft = "You can't start the next chapter of your life if you keep re-reading the last one.";
                            break;
                        case 2:
                            timeLeft = "When your past calls don't answer. It has nothing new to say";
                            break;
                        case 3:
                            timeLeft = "Leave the past where it belongs. Don't look back when you know you shouldn't.";
                            break;
                        case 4:
                            timeLeft = "The best thing about the past is that it shows you what not to bring into the future.";
                            break;
                        case 5:
                            timeLeft = "Oh yes, the past can hurt. But you can either run from it or, learn from it.";
                            break;
                    }

                }
            return timeLeft;
        }
    }
}