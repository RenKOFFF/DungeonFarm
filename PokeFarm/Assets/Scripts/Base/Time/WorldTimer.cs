using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Base.Time
{
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
