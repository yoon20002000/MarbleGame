using System.Collections.Generic;
using ChzzAPI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private ChzzkUnity chzzkUnity;
    
    [SerializeField]
    private MarbleManager marbleManager;
    
    [SerializeField]
    private DonationManager donationManager;

    private const string GAME_AGGREGATION_COMMAND = "[구슬]";
    
    private List<DonationData> errorDonations = new List<DonationData>(100);

    public bool IsRacing => eGameState == EGameState.Racing;
    public bool IsRacingStartAble => eGameState == EGameState.Idle && marbleManager.MarbleCount > 1;
    public enum EGameState
    {
        Idle = 0,
        Aggregation = 1,
        Racing = 2,
        RacingEnd = 3,
    }

    public EGameState GameState
    {
        get
        {
            return eGameState;
        }
        
        private set
        {
            eGameState = value;
            OnGameStateChanged?.Invoke(eGameState);
        }
    } 

    private EGameState eGameState = EGameState.Idle;
    [HideInInspector]
    public UnityEvent<EGameState> OnGameStateChanged = new UnityEvent<EGameState>();
    [HideInInspector]
    public UnityEvent<Marble> OnGameEnded = new UnityEvent<Marble>();
    
    private void Awake()
    {
        if (marbleManager == null)
        {
            marbleManager = FindFirstObjectByType<MarbleManager>();
            Assert.IsNotNull(marbleManager, "marbleManager not found");
        }

        if (donationManager == null)
        {
            donationManager = FindFirstObjectByType<DonationManager>();
            Assert.IsNotNull(donationManager, "donationManager not found");
        }

        chzzkUnity.onOpen.AddListener(OnOpen);
        chzzkUnity.onClose.AddListener(OnClose);
        chzzkUnity.onMessage.AddListener(OnMessage);
        chzzkUnity.onDonation.AddListener(OnDonation);
    }

    private void OnOpen()
    {
        Debug.Log("OnOpen");
        UnityMainThreadDispatcher.Instance.Enqueue(
            () =>
            {
                GameState = EGameState.Aggregation;
            });
    }

    private void OnClose()
    {
        Debug.Log("OnClose");
        UnityMainThreadDispatcher.Instance.Enqueue(
            () =>
            {
                GameState = EGameState.Idle;
            });
    }

    private void OnMessage(Profile profile, string msg)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            if (!IsAggregation(msg))
            {
                return;
            }
        
            int firstQuote = msg.IndexOf('"');
            int secondQuote = msg.IndexOf('"', firstQuote + 1);

            if (firstQuote != -1 && secondQuote != -1)
            {
                // 도네이션 자
                string donor = profile.nickname;
                // 구슬 이름
                string marbleName = msg.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
            
                marbleManager.AddMarbleData(marbleName, donor, true, 1000, msg);
                Debug.Log($"구슬 추가 : 구슬 이름 {marbleName}, 생성자 {donor}");
            }
            else
            {
                AddErrorDonation(new DonationData(true, profile.nickname, 1000, msg));
                Debug.LogWarning($"큰따옴표가 없습니다. : {msg}");
                return;
            }
        });
    }

    private void OnDonation(Profile profile, string msg, DonationExtras donation)
    {
        UnityMainThreadDispatcher.Instance.Enqueue(() =>
        {
            if (!IsAggregation(msg))
            {
                return;
            }
        
            int firstQuote = msg.IndexOf('"');
            int secondQuote = msg.IndexOf('"', firstQuote + 1);

            if (firstQuote != -1 && secondQuote != -1)
            {
                // 도네이션 자
                string donor = profile.nickname;
                // 도네 금액
                int donationAmount = donation.payAmount;
                // 익명 여부
                bool isAnonymous = donation.isAnonymous;
                // 구슬 이름
                string marbleName = msg.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
            
                marbleManager.AddMarbleData(marbleName, donor, isAnonymous, donationAmount, msg);
            }
            else
            {
                AddErrorDonation(new DonationData(donation.isAnonymous, profile.nickname, donation.payAmount, msg));
                Debug.LogWarning($"큰따옴표가 없습니다. : {msg}");
                return;
            }
        });
    }
    
    public void EnterEndPoint(Marble marble)
    {
        if (!IsRacing)
        {
            return;
        }
        
        marbleManager.RemoveMarble(marble);

        if (marbleManager.MarbleCount == 1)
        {
            GameState = EGameState.RacingEnd;
            
            OnGameEnded?.Invoke(marble);
            // Race End
            // 승자 연출
            // 게임 종료 시 MarbleDatas를 이용해서 다시 재 생성하는 로직을
            // 승자 Popup 닫았을 때 호출 할 수 있도록 OnClick Close 에 바인드 할 것
            
            Debug.Log("Winner : " + marble.MarbleData.MarbleName);
        }
    }

    private bool IsAggregation(string msg)
    {
        return !string.IsNullOrEmpty(msg) && msg.Contains(GAME_AGGREGATION_COMMAND);
    }

    private void AddErrorDonation(DonationData donationData)
    {
        errorDonations.Add(donationData);
    }

    private void RemoveErrorDonation(DonationData donationData)
    {
        errorDonations.Remove(donationData);
    }
    private void ResetErrorDonations()
    {
        errorDonations.Clear();
    }

    public void StartRace()
    {
        if (IsRacing)
        {
            return;
        }
        GameState = EGameState.Racing;
        marbleManager.SetAllMarbleSimulate(IsRacing);
        
        Debug.Log("레이스 시작!");
    }

    public void StopRace()
    {
        if (!IsRacing)
        {
            return;
        }
        
        GameState = EGameState.Idle;
        marbleManager.ResetMarblesPosition();
        marbleManager.SetAllMarbleSimulate(IsRacing);
        Debug.Log("레이스 강제 종료!");
    }

    public void AddMarble(string marbleName, int marbleCount)
    {
        marbleManager.AddMarbleData(marbleName, "수동", false, marbleCount * 1000, "수동");
    }

    public void RemoveMarble(string marbleName, int marbleCount)
    {
        marbleManager.RemoveMarble(marbleName, marbleCount, IsRacing);
    }

    public void RemoveMarble(GameObject marbleGameObject)
    {
        Marble marble = marbleGameObject.GetComponent<Marble>();
        if (marble == null)
        {
            return;
        }
        marbleManager.RemoveMarble(marble);
    }

    public void RemoveAllMarbles()
    {
        marbleManager.RemoveAllMarbles();
        marbleManager.RemoveAllMarblesData();
    }

    public void StartAggregation(string channelID)
    {
        chzzkUnity.Connect(channelID);
    }

    public void StopAggregation()
    {
        chzzkUnity.StopListening();
    }
}
