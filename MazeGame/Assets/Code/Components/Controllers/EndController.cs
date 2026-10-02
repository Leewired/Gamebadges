using UnityEngine;
using MazeGame.Core;
using TMPro;

public class EndController : BaseController
{

    public GameObject m_gameOver = null;
    private TextMeshProUGUI m_gameOverText = null;
    public GameObject m_victory = null;
    private TextMeshProUGUI m_victoryText = null;

    public override void Start()
    {
        base.Start();
        this.m_gameOver = GameObject.Find("GameOver").gameObject;
        this.m_victory = GameObject.Find("Victory").gameObject;
        m_gameOverText = this.m_gameOver.GetComponentInChildren<TextMeshProUGUI>();
        m_victoryText = this.m_victory.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Hide()
    {
        m_gameOver.SetActive(false);
        m_victory.SetActive(false);
    }

    public void ShowVictory()
    {
        this.Hide();
        m_victory.SetActive(true);

    }

    public void ShowGameOver()
    {
        this.Hide();
        m_gameOver.SetActive(true);
    }

    public void SetGameOverText(string text)
    {
        m_gameOverText.text = text;
    }

    public void SetVictoryText(string text)
    {
        m_victoryText.text = text;
    }
}
