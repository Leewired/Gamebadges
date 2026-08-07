using UnityEngine;
using MazeGame.Core;
using TMPro;

public class PauseController : BaseController
{

    public GameObject m_gamePause = null;
    private TextMeshProUGUI m_mazeText = null;

    public override void Start()
    {
        base.Start();
        this.m_gamePause = GameObject.Find("GamePause").gameObject;
        m_mazeText = this.m_gamePause.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Hide()
    {
        m_gamePause.SetActive(false);
    }

    public override void Show()
    {
        m_gamePause.SetActive(true);
    }

    public void ShowGamePause()
    {
        this.Hide(); //hide others
        m_gamePause.SetActive(true); //show this specific one
    }

    public void SetPauseText(string text)
    {
        m_mazeText.text = text;
    }

}
