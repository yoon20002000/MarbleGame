using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_MainLayout : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [Header("UI")] 
    [Header("RaceState Change Button")]
    [SerializeField]
    private Toggle raceStateChangeButton;
    [SerializeField]
    private TextMeshProUGUI toggleDescription;
    
    private void Awake()
    {
        Assert.IsNotNull(gameManager, "Game manager is null.");
        
        gameManager.OnGameStateChanged.AddListener(OnGameStateChanged);
        raceStateChangeButton.onValueChanged.AddListener(OnRacingStateValueChanged);
    }

    private void OnRacingStateValueChanged(bool isOn)
    {
        if (isOn == true)
        {
            gameManager.StartRace();    
        }
        else
        {
            gameManager.StopRace();    
        }
    }
    
    private void OnGameStateChanged(GameManager.EGameState eGameState)
    {
        SetRaceButtonText(eGameState);
        SetRaceButton(eGameState);
    }

    private void SetRaceButtonText(GameManager.EGameState eGameState)
    {
        string raceButtonText ;

        if (gameManager.IsRacingStartAble)
        {
            raceButtonText = "레이스 시작!";
        }
        else
        {
            switch (eGameState)
            {
                case GameManager.EGameState.Idle:
                {
                    raceButtonText = "레이스 시작!";
                    break;
                }
                case GameManager.EGameState.Aggregation:
                {
                    raceButtonText = "레이스 준비 중...";
                    break;
                }
                case GameManager.EGameState.Racing:
                case GameManager.EGameState.RacingEnd:
                default:
                {
                    raceButtonText = "";
                    break;
                }
            }
        }

        toggleDescription.SetText(raceButtonText);
    }
    private void SetRaceButton(GameManager.EGameState eGameState)
    {
        bool isToggleActive = eGameState is GameManager.EGameState.Idle or GameManager.EGameState.Aggregation;
        raceStateChangeButton.gameObject.SetActive(isToggleActive);
        
        bool isInteractable = gameManager.IsRacingStartAble;
        raceStateChangeButton.interactable = isInteractable;
    }
}
