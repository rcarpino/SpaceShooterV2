using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Text _lowAmmoText;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
        _scoreText.text = "Score: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }
    private void Update()
    {
        
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmoCount(int ammoCount, int maxAmmoCount)
    {
        _ammoText.text = "Ammo: " + ammoCount.ToString() + " / " + maxAmmoCount.ToString();
        if(ammoCount <= 5)
        {
            _lowAmmoText.gameObject.SetActive(true);
            StartCoroutine(LowAmmoFlickerRoutine());
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];
        
        if(currentLives == 0)
        {
            GameOverSequence();     
        }
        
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator LowAmmoFlickerRoutine()
    {
        while (true)
        {
            _lowAmmoText.text = "LOW AMMO";
            yield return new WaitForSeconds(0.5f);
            _lowAmmoText.text = "";
            yield return new WaitForSeconds(0.5f);

        }
    }


    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {

            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
        
    }

}
