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
        UpdateRaceButton(isOn);
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
        raceStateChangeButton.interactable = gameManager.IsRacingStartAble; 
    }

    private void UpdateRaceButton(bool isOn)
    {
        toggleDescription.text = isOn == false ? "레이스 시작" : "레이스 중단";
    }
}
