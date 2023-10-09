using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static event Action OnLoadScene;
    
    [SerializeField] private Button _loadSceneButton;
    [SerializeField] private string _sceneName;

    private void OnEnable()
    {
        _loadSceneButton.onClick.AddListener(LoadScene);
    }

    private void OnDisable()
    {
        _loadSceneButton.onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        OnLoadScene?.Invoke();
        
        SceneManager.LoadScene(_sceneName);
    }
}
