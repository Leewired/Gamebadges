using UnityEngine;
using MazeGame.Core;
using TMPro;

public class IntroController : BaseController
{

    public GameObject m_mazeReady = null;
    private TextMeshProUGUI m_mazeText = null;

    public override void Start()
    {
        base.Start();
        this.m_mazeReady = GameObject.Find("MazeReady").gameObject;
        m_mazeText = this.m_mazeReady.GetComponentInChildren<TextMeshProUGUI>();
    }

    public override void Hide()
    {
        m_mazeReady.SetActive(false);
    }

    public override void Show()
    {
        m_mazeReady.SetActive(true);
    }

    public void ShowMazeReady()
    {
        this.Hide(); //hide others
        m_mazeReady.SetActive(true); //show this specific one
    }

    public void SetIntroText(string text)
    {
        m_mazeText.text = text;
    }

}
