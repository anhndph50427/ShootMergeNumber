using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI score;
    public TextMeshProUGUI scoreMenu;

    private int currentScore = 0;


    public AudioSource audioSource;
    public AudioClip[] soundClips;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(int index)
    {
        if (index >= 0 && index < soundClips.Length)
        {
            audioSource.PlayOneShot(soundClips[index]);
        }
        else
        {
            Debug.LogWarning("âm thanh không hợp lệ: " + index);
        }
    }
    public void Score(int scoreInGame)
    {
        currentScore += scoreInGame; 
        score.text = currentScore.ToString(); 
        scoreMenu.text = score.text;
    }
}
