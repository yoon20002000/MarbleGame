using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChzzSetting : MonoBehaviour
{
    // [Header("Game Manager")]
    // [SerializeField]
    // private GameManager gameManager;
    
    [Header("UI")]
    [Header("ChannelID")]
    [SerializeField]
    private TMP_InputField channelIDInputField;
    
    [Header("Channel State")]
    [SerializeField]
    private TextMeshProUGUI gameStateText;
    [SerializeField]
    private Toggle dataAggregationToggle;
    [SerializeField]
    private TextMeshProUGUI dataAggregationToggleText;

    private void Awake()
    {
        MarbleGameManager.Instance.OnGameStateChanged.AddListener(OnGameStateChanged);
        dataAggregationToggle.onValueChanged.AddListener(OnDataAggregationToggleValueChanged);
        OnGameStateChanged(MarbleGameManager.Instance.GameState);
    }

    private void OnDestroy()
    {
        MarbleGameManager.Instance.OnGameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(MarbleGameManager.EGameState eState)
    {
        SetGameStateText(eState);
        SetToggleText(eState);
    }

    private void OnDataAggregationToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            MarbleGameManager.Instance.StopAggregation();
        }
        else
        {
            if (string.IsNullOrEmpty(channelIDInputField.text))
            {
                return;
            }
            MarbleGameManager.Instance.StartAggregation(channelIDInputField.text);
        }
    }

    private void SetGameStateText(MarbleGameManager.EGameState eState)
    {
        string gameState;
        switch (eState)
        {
            case MarbleGameManager.EGameState.Idle:
            case MarbleGameManager.EGameState.Racing:
            case MarbleGameManager.EGameState.RacingEnd:
            default:
            {
                gameState = "집계 중단 됨";
                break;
            }
            case MarbleGameManager.EGameState.Aggregation:
            {
                gameState = "집계 중";
                break;
            }
        }
        gameStateText.SetText(gameState);
    }

    private void SetToggleText(MarbleGameManager.EGameState eState)
    {
        string toggleText;
        switch (eState)
        {
            case MarbleGameManager.EGameState.Idle:
            {
                toggleText = "집계 시작";
                break;
            }
            case MarbleGameManager.EGameState.Racing:
            case MarbleGameManager.EGameState.RacingEnd:
            default:
            {
                toggleText = "-";
                break;
            }
            case MarbleGameManager.EGameState.Aggregation:
            {
                toggleText = "집계 중단";
                break;
            }
        }
        dataAggregationToggleText.SetText(toggleText);
    }
}
