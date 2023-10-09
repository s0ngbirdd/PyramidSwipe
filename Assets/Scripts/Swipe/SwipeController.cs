using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SwipeController : MonoBehaviour
{
    //public static event Action OnResetRandomPyramid;
    
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private Transform[] _pyramidTransforms;
    [SerializeField] private float _tweenDuration = 0.25f;
    [SerializeField] private Image _arrowImage;
    [SerializeField] private CanvasGroup _arrowCanvasGroup;
    //[SerializeField] private TextMeshProUGUI _debugTMP;
    
    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;

    private int _swipeDirection;

    private List<Transform> _pyramidChildTransforms = new List<Transform>();

    private Camera _camera;
    private Transform _randomPyramidTransform;
    private int _randomPyramidTransformIndex;
    private int _randomPyramidTransformIndexTemp;
    private GameObject _currentBlock;
    private Tween _moveTween;
    private Tween _fadeTween;

    private void Start()
    {
        _camera = Camera.main;

        //ResetRandomPyramid();
        
        _swipeDirection = 1;
        _randomPyramidTransformIndex = _pyramidTransforms.Length - 1;
        _randomPyramidTransform = Instantiate(_pyramidTransforms[_randomPyramidTransformIndex]);

        foreach (Transform child in _randomPyramidTransform)
        {
            _pyramidChildTransforms.Add(child);
        }
        
        _arrowImage.rectTransform.anchoredPosition = new Vector2(Screen.width * 0.42f * _swipeDirection, _arrowImage.rectTransform.anchoredPosition.y);
        _arrowImage.rectTransform.localScale = new Vector3(_swipeDirection, 1, 1);
        
        _fadeTween = _arrowCanvasGroup.DOFade(0, _tweenDuration * 3).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        _moveTween.Kill();
        _fadeTween.Kill();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !_moveTween.IsActive())
        {
            _startTouchPosition = Input.GetTouch(0).position;
            
            if (_pyramidChildTransforms.Count > 0)
            {
                _currentBlock = SpawnBlock();
            }
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && _currentBlock != null && !_moveTween.IsActive())
        {
            _endTouchPosition = Input.GetTouch(0).position;
            
            // Move from right to LEFT
            if (_endTouchPosition.x < _startTouchPosition.x)
            {
                if (_pyramidChildTransforms.Count > 0)
                {
                    if (_swipeDirection == 1)
                    {
                        MoveBlock();
                    }
                    else
                    {
                        Debug.Log("LOSE");
                        RestartGame();
                    }
                }
            }
            // Move from left to RIGHT
            else if (_endTouchPosition.x > _startTouchPosition.x)
            {
                if (_pyramidChildTransforms.Count > 0)
                {
                    if (_swipeDirection == -1)
                    {
                        MoveBlock();
                    }
                    else
                    {
                        Debug.Log("LOSE");
                        RestartGame();
                    }
                }
            }
        }
    }

    private GameObject SpawnBlock()
    {
        GameObject block = Instantiate(_blockPrefab);
        block.transform.localScale = _pyramidChildTransforms[0].localScale;
        block.transform.position = new Vector2(_camera.orthographicSize * 2 * _swipeDirection, _pyramidChildTransforms[0].position.y);

        return block;
    }

    private void MoveBlock()
    {
        _moveTween = _currentBlock.transform.DOMoveX(_pyramidChildTransforms[0].position.x, _tweenDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            _currentBlock.transform.SetParent(_randomPyramidTransform);
            _swipeDirection *= -1;
            _pyramidChildTransforms.RemoveAt(0);
            _arrowImage.rectTransform.anchoredPosition = new Vector2(Screen.width * 0.42f * _swipeDirection, _arrowImage.rectTransform.anchoredPosition.y);
            _arrowImage.rectTransform.localScale = new Vector3(_swipeDirection, 1, 1);
            _currentBlock = null;

            if (_pyramidChildTransforms.Count <= 0)
            {
                Debug.Log("WIN");
                ResetRandomPyramid();
                //RestartGame();
            }
        });
    }
    
    private void ResetRandomPyramid()
    {
        if (_randomPyramidTransform != null)
        {
            _randomPyramidTransformIndexTemp = _randomPyramidTransformIndex;
            Destroy(_randomPyramidTransform.gameObject);
        }
        
        _swipeDirection = 1;

        do
        {
            _randomPyramidTransformIndex = Random.Range(0, _pyramidTransforms.Length);
        }
        while (_randomPyramidTransformIndex == _randomPyramidTransformIndexTemp);

        _randomPyramidTransform = Instantiate(_pyramidTransforms[_randomPyramidTransformIndex]);

        foreach (Transform child in _randomPyramidTransform)
        {
            _pyramidChildTransforms.Add(child);
        }
        
        _arrowImage.rectTransform.anchoredPosition = new Vector2(Screen.width * 0.42f * _swipeDirection, _arrowImage.rectTransform.anchoredPosition.y);
        _arrowImage.rectTransform.localScale = new Vector3(_swipeDirection, 1, 1);
        
        //OnResetRandomPyramid?.Invoke();
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
}
