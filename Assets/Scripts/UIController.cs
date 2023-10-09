using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private float _startTime = 10;
    [SerializeField] private TextMeshProUGUI _scoreTMP;
    [SerializeField] private TextMeshProUGUI _timeTMP;

    private int _startScore;

    private void OnEnable()
    {
        SwipeController.OnBuildBlock += UpdateScore;
        SwipeController.OnBuildPyramid += UpdateTime;
        SwipeController.OnWrongSwipe += RestartGame;

        SceneLoader.OnLoadScene += CheckScoreForSave;
    }

    private void OnDisable()
    {
        SwipeController.OnBuildBlock -= UpdateScore;
        SwipeController.OnBuildPyramid -= UpdateTime;
        SwipeController.OnWrongSwipe -= RestartGame;
        
        SceneLoader.OnLoadScene -= CheckScoreForSave;
    }

    private void Update()
    {
        UpdateTime(-Time.deltaTime);

        if (_startTime <= 0)
        {
            RestartGame();
        }
    }
    
    private void UpdateScore(int score)
    {
        StartCoroutine(UpdateScoreCoroutine(score));
    }

    private IEnumerator UpdateScoreCoroutine(int score)
    {
        for (int i = 0; i < score; i++)
        {
            _startScore++;
            _scoreTMP.text = _startScore.ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateTime(float time)
    {
        if (Mathf.Sign(time) < 0)
        {
            _startTime += time;
            _timeTMP.text = Mathf.Round(_startTime).ToString();
        }
        else
        {
            StartCoroutine(UpdateTimeCoroutine(time));
        }
    }
    
    private IEnumerator UpdateTimeCoroutine(float time)
    {
        for (int i = 0; i < (int)time; i++)
        {
            _startTime++;
            _timeTMP.text = Mathf.Round(_startTime).ToString();

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void RestartGame()
    {
        CheckScoreForSave();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void CheckScoreForSave()
    {
        if (_startScore > SaveLoadSystem.Instance.LoadGame())
        {
            SaveLoadSystem.Instance.SaveGame(_startScore);
        }
    }

    private void OnApplicationQuit()
    {
        CheckScoreForSave();
    }
}
