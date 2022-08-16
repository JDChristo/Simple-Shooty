using System.Collections;
using System.Collections.Generic;
using SimpleShooty.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        GameManager.Instance.OnStateUpdate += EnablePanel;
        GameManager.Instance.UpdateState(GameState.MainMenu);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnStateUpdate -= EnablePanel;
    }
    public void EnablePanel(GameState status)
    {
        switch (status)
        {
            case GameState.MainMenu:
                startPanel.SetActive(true);
                losePanel.SetActive(false);
                winPanel.SetActive(false);
                break;

            case GameState.Start:
                startPanel.SetActive(false);
                losePanel.SetActive(false);
                winPanel.SetActive(false);
                break;

            case GameState.Won:
                startPanel.SetActive(false);
                losePanel.SetActive(false);
                winPanel.SetActive(true);
                break;

            case GameState.Lost:
                startPanel.SetActive(false);
                losePanel.SetActive(true);
                winPanel.SetActive(false);
                break;

            default:
                break;
        }
    }
    public void StartGame()
    {
        GameManager.Instance.UpdateState(GameState.Start);
    }
    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
