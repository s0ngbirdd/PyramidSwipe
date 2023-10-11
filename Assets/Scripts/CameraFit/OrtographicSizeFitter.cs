using UnityEngine;

namespace CameraFit
{
    public class OrtographicSizeFitter : MonoBehaviour
    {
        [SerializeField] private CameraFitType _cameraFitType;
        [SerializeField] private SpriteRenderer _objectSpriteRenderer;
        [SerializeField] private int _spriteRendererNumber = 1;

        private UnityEngine.Camera _camera;
        private float _ortographicSize;
        private float _screenRatio;
        private float _targetRatio;
        private float _differenceInSize;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            if (_cameraFitType == CameraFitType.WidthFit)
            {
                FitCameraByWidth();
            }
            else if (_cameraFitType == CameraFitType.HeightFit)
            {
                FitCameraByHeight();
            }
            else if (_cameraFitType == CameraFitType.PerfectFit)
            {
                PerfectCameraFit();
            }
        }

        private void FitCameraByWidth()
        {
            _ortographicSize = _objectSpriteRenderer.size.x * Screen.height * _spriteRendererNumber / Screen.width * 0.5f;
            _camera.orthographicSize = _ortographicSize;
        }
        
        private void FitCameraByHeight()
        {
            _ortographicSize = _objectSpriteRenderer.bounds.size.y / 2;
            _camera.orthographicSize = _ortographicSize;
        }
        
        private void PerfectCameraFit()
        {
            _screenRatio = (float)Screen.width / (float)Screen.height;
            _targetRatio = _objectSpriteRenderer.bounds.size.x * _spriteRendererNumber / _objectSpriteRenderer.bounds.size.y;

            if (_screenRatio >= _targetRatio)
            {
                _camera.orthographicSize = _objectSpriteRenderer.bounds.size.y / 2;
            }
            else
            {
                _differenceInSize = _targetRatio / _screenRatio;
                _camera.orthographicSize = _objectSpriteRenderer.bounds.size.y / 2 * _differenceInSize;
            }
        }
    }
}
