using System;

namespace Base.Time
{
    public class WorldTime
    {
        // оставить публичные сеттеры, чтобы работало сохранение
        public DateTime Time { get; set; }
        public int Days { get; set; }

        private const int MorningTimeHours = 8;
        private const int NeedDaysToResetTime = 1;

        public WorldTime() { }

        public WorldTime(DateTime time, int days)
        {
            Time = time;
            Days = days;
        }

        /// <returns>true, если день изменился</returns>
        public bool AddMinutes(int minutes)
        {
            Time = Time.AddMinutes(minutes);

            if (Time.Day <= NeedDaysToResetTime)
                return false;

            Days++;
            Time = Time.AddDays(-NeedDaysToResetTime);
            return true;
        }

        public void SetMorningTime()
        {
            Time = new DateTime().AddHours(MorningTimeHours);
        }
    }
}