using System;
using System.Collections.Generic;
using ChzzAPI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class MarbleGameManager : MonoBehaviour
{
    public static MarbleGameManager Instance { get; private set; }

    [SerializeField] private ChzzkUnity chzzkUnity;
    
    [SerializeField] private TimeManager timeManager;

    public UnityEvent<List<Marble>> OnGameStart = new UnityEvent<List<Marble>>();
    public UnityEvent<Marble> OnGameEnd = new UnityEvent<Marble>();
    public UnityEvent OnGameForceEnd = new UnityEvent();

    public UnityEvent OnAggregationStart = new UnityEvent();
    public UnityEvent OnAggregationEnd = new UnityEvent();
    
    private DonationManager donationManager = new DonationManager();
    
    [SerializeField]
    private string GAME_AGGREGATION_COMMAND = "[구슬]";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        chzzkUnity.onOpen.AddListener(OnOpen);
        chzzkUnity.onClose.AddListener(OnClose);
        chzzkUnity.onMessage.AddListener(OnMessage);
        chzzkUnity.onDonation.AddListener(OnDonation);
        
        OnMarbleChanged.AddListener(marbleManager.UpdateAllMarblesPosition);
    }

    #region Game Section
    
    public enum EGameState
    {
        Idle = 0,
        Aggregation = 1,
        RacingStart = 2,
        Racing = 3,
        RacingEnd = 4,
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
            switch (eGameState)
            {
                case EGameState.Idle:
                default:
                {
                    break;
                }
                case EGameState.Aggregation:
                {
                    break;
                }
                case EGameState.RacingStart:
                {
                    InRacingMarbles?.Clear();
                    InRacingMarbles = new(marbleManager.Marbles);
        
                    marbleManager.SetAllMarbleSimulate(true);
        
                    OnGameStart?.Invoke(new (marbleManager.Marbles));
                    break;
                }
                case EGameState.Racing:
                {
                    break;
                }
                case EGameState.RacingEnd:
                {
                    break;
                }
            }
            OnGameStateChanged?.Invoke(eGameState);
        }
    } 
    [HideInInspector]
    public UnityEvent<EGameState> OnGameStateChanged = new UnityEvent<EGameState>();
    
    [SerializeField]
    private EGameState eGameState = EGameState.Idle;
    public bool IsRacing => eGameState == EGameState.Racing;
    public bool IsRacingStartAble => !IsRacing && eGameState != EGameState.Aggregation && marbleManager.MarbleCount > 1;
    private List<Marble> InRacingMarbles;
    
    public void StartRace()
    {
        if (IsRacing)
        {
            return;
        }
        GameState = EGameState.RacingStart;

        GameState = EGameState.Racing;
        Debug.Log("레이스 시작!");
    }
    
    public void ForceStopRace()
    {
        if (!IsRacing)
        {
            return;
        }
        
        marbleManager.ResetMarblesPosition();
        marbleManager.SetAllMarbleSimulate(IsRacing);
        Debug.Log("레이스 강제 종료!");
        OnGameForceEnd?.Invoke();
    }    
    public void EnterEndPoint(Marble marble)
    {
        if (!IsRacing)
        {
            return;
        }
        
        marbleManager.RemoveMarble(marble);
        InRacingMarbles.Remove(marble);
            
        if (InRacingMarbles.Count == 1)
        {
            
            Marble winner = InRacingMarbles[0];
            OnGameEnd?.Invoke(winner);
            marbleManager.AddAllMarblesByMarbleData();
            
            // Race End
            // 승자 연출
            // 승자 Popup 닫았을 때 호출 할 수 있도록 OnClick Close 에 바인드 할 것
            
            Debug.Log("Winner : " + winner.MarbleData.MarbleName);
        }
    }
    #endregion

    #region Chzzk API Section
    private void OnOpen()
    {
        Debug.Log("OnOpen");
        UnityMainThreadDispatcher.Instance.Enqueue(
            () =>
            {
                GameState = EGameState.Aggregation;
                OnAggregationStart?.Invoke();
            });
    }

    private void OnClose()
    {
        Debug.Log("OnClose");
        UnityMainThreadDispatcher.Instance.Enqueue(
            () =>
            {
                GameState = EGameState.Idle;
                OnAggregationEnd?.Invoke();
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
                donationManager.AddErrorDonation(new DonationData(true, profile.nickname, 1000, msg));
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
                donationManager.AddErrorDonation(new DonationData(donation.isAnonymous, profile.nickname, donation.payAmount, msg));
                Debug.LogWarning($"큰따옴표가 없습니다. : {msg}");
                return;
            }
        });
    }
    
    private bool IsAggregation(string msg)
    {
        return !string.IsNullOrEmpty(msg) && msg.Contains(GAME_AGGREGATION_COMMAND);
    }
    
    public void StartAggregation(string channelID)
    {
#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
        chzzkUnity.Connect(channelID);
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
    }
    public void StopAggregation()
    {
        chzzkUnity.StopListening();
    }
    #endregion

    #region Marble Section

    [SerializeField] private MarbleManager marbleManager;
    
    public UnityEvent<Marble> OnMarbleAdd => marbleManager.OnMarbleAdded;
    public UnityEvent<Marble> OnMarblePreRemove => marbleManager.OnMarblePreRemove;
    public UnityEvent OnMarbleChanged = new UnityEvent();
    
    public void AddMarble(string marbleName, int marbleCount)
    {
        marbleManager.AddMarbleData(marbleName, "수동", false, marbleCount * 1000, "수동");
        OnMarbleChanged?.Invoke();
    }

    public void RemoveMarble(string marbleName, int marbleCount)
    {
        marbleManager.RemoveMarble(marbleName, marbleCount, IsRacing);
        OnMarbleChanged?.Invoke();
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
    #endregion
}