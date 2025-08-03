using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_MainLayout : MonoBehaviour
{
    // [SerializeField]
    // private GameManager gameManager;

    [Header("UI")] 
    [Header("RaceState Change Button")]
    [SerializeField]
    private Toggle raceStateChangeButton;
    [SerializeField]
    private TextMeshProUGUI toggleDescription;
    
    private void Awake()
    {
        MarbleGameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        raceStateChangeButton.onValueChanged.AddListener(OnRacingStateValueChanged);
        OnGameStateChanged(MarbleGameManager.Instance.GameState);
    }

    private void OnDestroy()
    {
        MarbleGameManager.Instance.OnGameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnRacingStateValueChanged(bool isOn)
    {
        if (isOn == true)
        {
            MarbleGameManager.Instance.StartRace();    
        }
        else
        {
            MarbleGameManager.Instance.ForceStopRace();    
        }
    }
    
    private void OnGameStateChanged(MarbleGameManager.EGameState eGameState)
    {
        SetRaceButtonText(eGameState);
        SetRaceButton(eGameState);
    }

    private void SetRaceButtonText(MarbleGameManager.EGameState eGameState)
    {
        string raceButtonText ;

        if (MarbleGameManager.Instance.IsRacingStartAble)
        {
            raceButtonText = "레이스 시작!";
        }
        else
        {
            switch (eGameState)
            {
                case MarbleGameManager.EGameState.Idle:
                case MarbleGameManager.EGameState.Aggregation:
                {
                    raceButtonText = "레이스\n준비 중...";
                    break;
                }
                case MarbleGameManager.EGameState.Racing:
                case MarbleGameManager.EGameState.RacingEnd:
                default:
                {
                    raceButtonText = "";
                    break;
                }
            }
        }

        toggleDescription.SetText(raceButtonText);
    }
    private void SetRaceButton(MarbleGameManager.EGameState eGameState)
    {
        bool isToggleActive = eGameState is MarbleGameManager.EGameState.Idle or MarbleGameManager.EGameState.Aggregation;
        raceStateChangeButton.gameObject.SetActive(isToggleActive);
        
        bool isInteractable = MarbleGameManager.Instance.IsRacingStartAble;
        raceStateChangeButton.interactable = isInteractable;
    }
}
