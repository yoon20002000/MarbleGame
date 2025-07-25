using System;
using System.Collections.Generic;
using UnityEngine;

public class DonationManager
{
    private List<DonationData> donations = new List<DonationData>(100);
    private List<DonationData> errorDonations = new List<DonationData>(100);
    public DonationManager()
    {
        
    }

    public void AddDonation(DonationData data)
    {
        donations.Add(data);
    }

    public void RemoveDonation(DonationData data)
    {
        donations.Remove(data);
    }
    public void AddErrorDonation(DonationData donationData)
    {
        errorDonations.Add(donationData);
    }

    public void RemoveErrorDonation(DonationData donationData)
    {
        errorDonations.Remove(donationData);
    }
    
    public void ResetErrorDonations()
    {
        errorDonations.Clear();
    }
}
