using System;
using UnityEngine;

[Serializable]
public class WheelTrails  {
   [SerializeField] private TrailRenderer[] _wheelsTrails;

   public bool isTrailsEmitting {
      set {
         foreach (var trail in _wheelsTrails) {
            trail.emitting = value;
         }
      }
   }
}
