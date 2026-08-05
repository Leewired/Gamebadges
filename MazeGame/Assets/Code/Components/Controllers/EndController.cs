using UnityEngine;
using MazeGame.Core;
using TMPro;

public class EndController : BaseController
{

    public GameObject m_gameEnd = null;
    private TextMeshProUGUI m_mazeText = null;

    public override void Start()
    {
        base.Start();
        this.m_gameEnd = GameObject.Find("GameEnd").gameObject;
        m_mazeText = this.m_gameEnd.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Hide()
    {
        m_gameEnd.SetActive(false);
    }

    public override void Show()
    {
        m_gameEnd.SetActive(true);
    }

    public void ShowGameEnd()
    {
        this.Hide(); //hide others
        m_gameEnd.SetActive(true); //show this specific one
    }

    public void SetEndText(string text)
    {
        m_mazeText.text = text;
    }

}
