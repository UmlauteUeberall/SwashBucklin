using NDream.AirConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Canvas CanvasLobby;
    public Canvas CanvasHighScore;
    public Canvas CanvasHUD;

    public Text TextTimer;
    public Text TextKillFeed;

    public Text TextLobby1;
    public Text TextLobby2;

    private UIStage stage = UIStage.Lobby;

	void Awake ()
    {
        SetStage(UIStage.Lobby);
    }

    public void SetStage(UIStage _stage)
    {
        switch(_stage)
        {
            case UIStage.Lobby:
                CanvasLobby.transform.position = new Vector2(Screen.width / 2 , Screen.height / 2);
                CanvasHUD.transform.position = new Vector2(4000.0f, 3000.0f); ;
                CanvasHighScore.transform.position = new Vector2(4000.0f, 3000.0f);
                break;
            case UIStage.Highscore:
                CanvasHighScore.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
                CanvasHUD.transform.position = new Vector2(4000.0f, 3000.0f);
                CanvasLobby.transform.position = new Vector2(4000.0f, 3000.0f);
                break;
            case UIStage.HUD:
                CanvasHUD.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
                CanvasHighScore.transform.position = new Vector2(4000.0f, 3000.0f);
                CanvasLobby.transform.position = new Vector2(4000.0f, 3000.0f);
                break;
        }
    }

    public void SetTimerDisplay(float _timeRemaining)
    {
        if(_timeRemaining >= 60.0f)
        {
            TextTimer.text = "1:00";
        }
        else
        {
            if(_timeRemaining >= 10.0f)
            {
                TextTimer.text = "0:" + (System.Convert.ToInt32(_timeRemaining)).ToString();
            }
            else
            {
                TextTimer.text = "0:0" + (System.Convert.ToInt32(_timeRemaining)).ToString();
            }
        }
    }

    public void AddKillToKillfeed(string _killer, string _victim)
    {
        TextKillFeed.text += _killer + " killed " + _victim + "\n";
    }

    public void UpdateLobbyScreen(int _connectedCount, int _requiredCount)
    {
        TextLobby1.text = "You need at least " + _requiredCount.ToString() + " Players!";
        TextLobby2.text = "Currently there are " + _connectedCount.ToString() + " Players connected!";
    }
}

public enum UIStage
{
    HUD,
    Lobby,
    Highscore
}