using System;
using System.Collections.Generic;
using UnityEngine;

public class DonationManager : MonoBehaviour
{
    private List<DonationData> donations = new List<DonationData>();

    private void Awake()
    {
        donations.Capacity = 100;
    }

    public void AddDonation(DonationData data)
    {
        donations.Add(data);
    }

    public void RemoveDonation(DonationData data)
    {
        donations.Remove(data);
    }
}
