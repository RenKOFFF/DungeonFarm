using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

    public class WorldTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        public static WorldTimer Instance { get; set; }
        public WorldTime CurrentTime { get; set; }

        private const int RealSecondsInOneWorldMinute = 1;

        private readonly UnityEvent _onDayChanged = new();
        private DateTime _lastWorldSecondInRealTime;

        public static void AddOnDayChangedHandler(UnityAction onDayChanged)
        {
            Instance._onDayChanged.AddListener(onDayChanged);
        }

        private void CalculateWorldTime()
        {
            var realTimePassedFromLastWorldSecond = DateTime.Now - _lastWorldSecondInRealTime;
            var passedSeconds = realTimePassedFromLastWorldSecond.Seconds;

            if (passedSeconds < RealSecondsInOneWorldMinute)
                return;

            _lastWorldSecondInRealTime = DateTime.Now;

            if (CurrentTime.AddMinutes(passedSeconds))
                _onDayChanged.Invoke();

            Save();
        }

        private void Save()
        {
            GameDataController.Save(CurrentTime, DataCategory.Time, gameObject.name);
        }

        private void Load()
        {
            CurrentTime = GameDataController.LoadWithInitializationIfEmpty<WorldTime>(DataCategory.Time, gameObject.name);
        }

        private void Awake()
        {
            Instance = this;
            _lastWorldSecondInRealTime = DateTime.Now;
            Load();
        }

        private void Update()
        {
            CalculateWorldTime();
            timerText.text = $"Day: {CurrentTime.Days}, \nTime: {CurrentTime.Time:HH:mm}";
        }
    }
}
