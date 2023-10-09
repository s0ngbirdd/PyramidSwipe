using TMPro;
using UnityEngine;

public class MainMenuScoreController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _score = 0;

    private void Start()
    {
        _score = SaveLoadSystem.Instance.LoadGame();
        _scoreText.text = _score.ToString();
    }
}
