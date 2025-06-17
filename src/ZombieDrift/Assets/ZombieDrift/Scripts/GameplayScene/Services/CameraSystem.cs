using System;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Gameplay {

   public class CameraSystem {

	   public float mainCameraOrthoSize {
		   set=> _mainCamera.Lens.OrthographicSize = value;
		   get => _mainCamera.Lens.OrthographicSize;
	   }
      public CinemachineCamera mainCamera {
         set {
            if (value == null)
               return;
            _mainCamera = value;
            _basicMultiChannelPerlin = (CinemachineBasicMultiChannelPerlin)_mainCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise);
         }
      }

      public CinemachineCamera zoomCamera {
         set {
            if (value == null)
               return;
            _zoomCamera = value;
         }
      }

      public bool isZoomed {
         get => _isZoomed;
         set {
            _mainCamera.enabled = !value;
            _zoomCamera.enabled = value;
         }
      }

      public Transform target {
         set {
            if (_mainCamera == null || _zoomCamera == null)
               throw new Exception($"Cinemachine camera is null, please set camera property in CameraSystem ");
            _mainCamera.Follow = value;
            _zoomCamera.Follow = value;
         }
      }

      private CinemachineBasicMultiChannelPerlin _basicMultiChannelPerlin;
      private CinemachineCamera _mainCamera, _zoomCamera;
      private bool _isZoomed;

      public async UniTask Shake(float amplitude, int shakeDurationMilliseconds) {
         _basicMultiChannelPerlin.AmplitudeGain = amplitude;
         await UniTask.Delay(shakeDurationMilliseconds);
         _basicMultiChannelPerlin.AmplitudeGain = 0;
      }
   }
}