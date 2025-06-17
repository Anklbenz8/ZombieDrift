using System;
using UnityEngine;

public class Timer {
    public event Action TickEvent, AlarmEvent;
    public TimeSpan current => TimeSpan.FromSeconds(_totalSeconds);
    public float currentSeconds => _totalSeconds;
    public bool isRunning => _isRunning;

    private float _totalSeconds, _alarmSeconds;
    private bool _isRunning, _isAlarmed, _isRepeatEnabled;

    public void StartWithRepeatAlarm(float alarmSeconds) {
        _isRepeatEnabled = true;
        StartWithAlarm(alarmSeconds);
    }

    public void StartWithAlarm(float alarmSeconds) {
        SetAlarm(alarmSeconds);
        Start();
    }

    public void Start() {
        ResetElapsedTime();
        Run();
    }

    public void Stop() {
        _isRunning = false;
        _isAlarmed = false;
        _isRepeatEnabled = false;
        ResetElapsedTime();
    }

    public void Pause() =>
        _isRunning = false;

// Run method only for unpause. To Start timer Use Start, StartWithAlarm and StartWithRepeatAlarm methods
    public void Run() =>
        _isRunning = true;

//For update timer call Tick() method from external Update()
    public void Tick() {
        if (!_isRunning)
            return;

        _totalSeconds += Time.deltaTime;

        TickNotify();
        var isAlarmHappened = _isAlarmed && _totalSeconds >= _alarmSeconds;

        if (!isAlarmHappened)
            return;

        AlarmNotify();
        if (_isRepeatEnabled)
            ResetElapsedTime();
        else
            AlarmReset();
    }

    public void ResetElapsedTime() =>
        _totalSeconds = 0;

    private void SetAlarm(float alarmSeconds) {
        _isAlarmed = true;
        _alarmSeconds = alarmSeconds;
    }

    private void AlarmReset() {
        _isAlarmed = false;
        Pause();
    }

    private void AlarmNotify() =>
        AlarmEvent?.Invoke();

    private void TickNotify() =>
        TickEvent?.Invoke();
}