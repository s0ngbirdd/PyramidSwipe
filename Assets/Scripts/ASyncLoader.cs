using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ASyncLoader : MonoBehaviour
{
    //public static event Action OnLoadScene;

    /*[Header("Loading Screen")]
    [SerializeField] private GameObject _menuObject;
    [SerializeField] private GameObject _loadingScreenObject;*/
    [SerializeField] private Slider _loadingSlider;
    [SerializeField] private string _loadSceneName;

    /*[Header("Load Scene Buttons")]
    [SerializeField] private List<Button> _loadButtons;*/

    //private GameScoreController _gameScoreController;

    /*private void OnEnable()
    {
        foreach (Button button in _loadButtons)
        {
            button.onClick.AddListener(LoadLevel);
        }
    }

    private void OnDisable()
    {
        foreach (Button button in _loadButtons)
        {
            button.onClick.RemoveListener(LoadLevel);
        }
    }*/

    /*private void Start()
    {
        _gameScoreController = FindObjectOfType<GameScoreController>();
    }*/

    private void Start()
    {
        LoadLevel();
    }

    private void LoadLevel()
    {
        //AudioManager.Instance.PlayOneShot("Click");
    
        /*if (_gameScoreController != null && _gameScoreController.Score > SaveLoadSystem.SaveLoadSystem.Instance.LoadGame())
        {
            _gameScoreController.SaveScore();
        }*/
    
        //OnLoadScene?.Invoke();

        //_menuObject.SetActive(false);
        //_loadingScreenObject.SetActive(true);

        StartCoroutine(LoadLevelASync());
        Time.timeScale = 1;
    }

    private IEnumerator LoadLevelASync()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_loadSceneName);

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            _loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
