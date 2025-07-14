using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UI_RacingResult : MonoBehaviour
{
    [Header("Game Manager")]
    [SerializeField]
    private GameManager gameManager;
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI winnerNameText;
    [SerializeField] 
    private TextMeshProUGUI winnerMarbleNameText;

    [SerializeField] 
    private Button closeButton;

    private const string ANONYMOUS_NAME = "익명";
    private void Awake()
    {
        Assert.IsNotNull(gameManager, "Game manager is null.");
        
        gameManager.OnGameStateChanged.AddListener(OnGameStateChanged);
        gameManager.OnGameEnded.AddListener(OnGameEnded);
        OnGameStateChanged(gameManager.GameState);
        
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private void OnClickCloseButton()
    {
        SetActiveUI(false);
    }

    private void OnGameStateChanged(GameManager.EGameState eGameState)
    {
        switch (eGameState)
        {
            case GameManager.EGameState.Idle:
            case GameManager.EGameState.Aggregation:
            case GameManager.EGameState.Racing:
            default:
            {
                SetActiveUI(false);
                break;
            }
            case GameManager.EGameState.RacingEnd:
            {
                
                break;
            }
        }
    }
    private void OnGameEnded(Marble marble)
    {
        SetActiveUI(true);
        DonationData donationData = marble.DonationData;
        string winnerName = donationData.IsAnonymous ? ANONYMOUS_NAME : donationData.Donor;
        string winnerMarbleName = marble.MarbleData.MarbleName;
        winnerNameText.SetText(winnerName);
        winnerMarbleNameText.SetText(winnerMarbleName);
    }

    private void SetActiveUI(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
