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

            if (Time.Day != 2)
                return false;

            Days++;
            Time = Time.AddDays(-1);
            return true;
        }

        public void SetMorningTime()
        {
            Time = new DateTime().AddHours(8);
        }
    }

    public class WorldTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text timerText;

        public static WorldTimer Instance { get; set; }
        public WorldTime CurrentTime { get; set; }

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

            if (passedSeconds < 1)
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

        private void Awake()
        {
            Instance = this;

            CurrentTime = GameDataController.Load<WorldTime>(DataCategory.Time, gameObject.name);
            _lastWorldSecondInRealTime = DateTime.Now;

            if (CurrentTime != null)
                return;

            CurrentTime = new WorldTime();
            Save();
        }

        private void Update()
        {
            CalculateWorldTime();
            timerText.text = $"День: {CurrentTime.Days}, Время: {CurrentTime.Time:HH:mm}";
        }
    }
}
