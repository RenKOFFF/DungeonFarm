using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

        public static WorldTimer Instance { get; private set; }

        public WorldTime CurrentTime { get; set; }

        private int TimeMultiplier { get; set; } = 1;
        private DateTime LastWorldSecondInRealTime { get; set; }

        private const int RealSecondsInOneWorldMinute = 1;
        private const int TimeMultiplierForUsers = 10;
        private const int TimeMultiplierForDevelopers = 50;

        private static readonly UnityEvent OnDayChangedEvent = new();

        public static void AddOnDayChangedHandler(UnityAction onDayChanged)
        {
            OnDayChangedEvent.AddListener(onDayChanged);
        }

        public static void RemoveOnDayChangedHandler(UnityAction onDayChanged)
        {
            OnDayChangedEvent.RemoveListener(onDayChanged);
        }

        public void SkipOneDay()
        {
            CurrentTime.Days++;
            CurrentTime.SetMorningTime();
            OnDayChanged();
            Save();
        }

        private void CalculateWorldTime()
        {
            var realTimePassedFromLastWorldSecond = DateTime.Now - LastWorldSecondInRealTime;
            var passedSeconds = realTimePassedFromLastWorldSecond.TotalSeconds * TimeMultiplier;

            if (passedSeconds < RealSecondsInOneWorldMinute)
                return;

            LastWorldSecondInRealTime = DateTime.Now;

            if (CurrentTime.AddMinutes((int) passedSeconds))
                OnDayChanged();

            Save();
        }

        private void OnDayChanged()
        {
            //TODO temp solve
            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainScene"))
                return;

            OnDayChangedEvent.Invoke();
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

            LastWorldSecondInRealTime = DateTime.Now;
            TimeMultiplier = TimeMultiplierForUsers;

#if DEBUG
            TimeMultiplier = TimeMultiplierForDevelopers;
#endif

            Load();
        }

        private void Update()
        {
            CalculateWorldTime();
            timerText.text = $"{CurrentTime.Time:HH:mm}";
        }

        private void OnDestroy()
        {
            OnDayChangedEvent.RemoveAllListeners();
        }
    }
}
