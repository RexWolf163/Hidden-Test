using System;
using AppScripts.LevelSystem.ZHandler;
using UnityEngine;
using Vortex.Unity.AppSystem.System.TimeSystem;
using Vortex.Unity.UI.Attributes;
using Vortex.Unity.UI.StateSwitcher;
using Vortex.Unity.UI.UIComponents;
using Zenject;

namespace AppScripts.LevelSystem
{
    public class LevelTimerHandler : MonoBehaviour
    {
        private enum TimerState
        {
            Off,
            Run,
            Complete,
            Pause
        }

        [SerializeField, StateSwitcher(typeof(TimerState))]
        private UIStateSwitcher uiStateSwitcher;

        [SerializeField] private UIComponent text;

        [Inject] private IMakeFail failHandler;

        private DateTime _timerTarget;

        private void OnEnable()
        {
            LevelController.OnLevelStarted += OnLevelStarted;
            LevelController.OnWin += StopTimer;
        }

        private void OnDisable()
        {
            LevelController.OnLevelStarted -= OnLevelStarted;
            LevelController.OnWin += StopTimer;
            StopTimer();
        }

        private void StopTimer() => TimeController.RemoveCall(this);

        private void OnLevelStarted()
        {
            var level = LevelController.GetCurrentLevel();
            if (level.Timer == 0)
            {
                uiStateSwitcher.Set(TimerState.Off);
                return;
            }

            uiStateSwitcher.Set(TimerState.Run);

            _timerTarget = TimeController.Date.AddSeconds(level.Timer);

            var span = _timerTarget - TimeController.Date;
            text.SetText(span.ToString(@"mm\:ss"));

            TimeController.Call(Refresh, 0.2f, this);
        }

        private void Refresh()
        {
            if (TimeController.Date >= _timerTarget)
            {
                text.SetText(TimeSpan.Zero.ToString(@"mm\:ss"));
                uiStateSwitcher.Set(TimerState.Complete);
                failHandler.Fail();
                return;
            }

            uiStateSwitcher.Set(TimerState.Run);

            var span = _timerTarget - TimeController.Date;
            text.SetText(span.ToString(@"mm\:ss"));
            TimeController.Call(Refresh, 0.2f, this);
        }
    }
}