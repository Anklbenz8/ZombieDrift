using System;
using UnityEngine;

[Serializable]
public class CarParticles :IPauseSensitive {
    [SerializeField] private ParticleSystem[] _wheelsSmoke;
    [SerializeField] private ParticleSystem[] _crashParticles;

    private bool _isWheelSmokeEnabled, _isCrashSmokeEnabled; 
    public bool isWheelSmokeEnabled {
        set {
	        _isWheelSmokeEnabled = value;
            foreach (var smoke in _wheelsSmoke)
                if (value)
                    smoke.Play();
                else
                    smoke.Stop();
        }
    }
    public bool isCrashSmokeEnabled {
        set {
	        _isCrashSmokeEnabled = value;
            foreach (var crash in _crashParticles)
                if (value)
                    crash.Play();
                else
                    crash.Stop();
        }
    }
    public void SetPause(bool isPaused) {
	    foreach (var crash in _crashParticles)
		    if (isPaused)
			    crash.Pause(true);
		    else
			    isCrashSmokeEnabled = _isCrashSmokeEnabled;
	    
	    foreach (var smoke in _wheelsSmoke)
		    if (isPaused)
			    smoke.Pause(true);
		    else
			    isWheelSmokeEnabled = _isWheelSmokeEnabled;
    }
}