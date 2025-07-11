using System.Collections.Generic;
using ChzzAPI;
using UnityEngine;
using UnityEngine.Assertions;

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

    private bool isRacing = false;
    
    private void Awake()
    {
        isRacing = false;
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
        
    }

    private void OnClose()
    {
        
    }

    private void OnMessage(Profile profile, string msg)
    {
        // for test
    }

    private void OnDonation(Profile profile, string msg, DonationExtras donation)
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
    }

    private void EnterEndPoint(Marble marble)
    {
        if (!isRacing)
        {
            return;
        }
        
        marbleManager.RemoveMarble(marble);

        if (marbleManager.MarbleCount == 1)
        {
            // Race End
            // 승자 연출
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
        if (isRacing)
        {
            return;
        }
        
        isRacing = true;
    }
}
