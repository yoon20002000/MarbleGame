using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChzzSetting : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField]
    private GameManager gameManager;
    
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
        gameManager.OnGameStateChanged.AddListener(OnGameStateChanged);
        dataAggregationToggle.onValueChanged.AddListener(OnDataAggregationToggleValueChanged);
        OnGameStateChanged(gameManager.GameState);
    }

    private void OnGameStateChanged(GameManager.EGameState eState)
    {
        SetGameStateText(eState);
        SetToggleText(eState);
    }

    private void OnDataAggregationToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            gameManager.StopAggregation();
        }
        else
        {
            if (string.IsNullOrEmpty(channelIDInputField.text))
            {
                return;
            }
            gameManager.StartAggregation(channelIDInputField.text);
        }
    }

    private void SetGameStateText(GameManager.EGameState eState)
    {
        string gameState;
        switch (eState)
        {
            case GameManager.EGameState.Idle:
            case GameManager.EGameState.Racing:
            case GameManager.EGameState.RacingEnd:
            default:
            {
                gameState = "집계 중단 됨";
                break;
            }
            case GameManager.EGameState.Aggregation:
            {
                gameState = "집계 중";
                break;
            }
        }
        gameStateText.SetText(gameState);
    }

    private void SetToggleText(GameManager.EGameState eState)
    {
        string toggleText;
        switch (eState)
        {
            case GameManager.EGameState.Idle:
            {
                toggleText = "집계 시작";
                break;
            }
            case GameManager.EGameState.Racing:
            case GameManager.EGameState.RacingEnd:
            default:
            {
                toggleText = "-";
                break;
            }
            case GameManager.EGameState.Aggregation:
            {
                toggleText = "집계 중단";
                break;
            }
        }
        dataAggregationToggleText.SetText(toggleText);
    }
}
