using System;

[Serializable]
public class DonationData
{
    public bool IsAnonymous { get; private set; }
    public string Donor { get; private set; }
    public int DonationAmount { get; private set; }
    
    public string Message { get; private set; }
    
    public DonationData(bool isAnonymous, string donor, int donationAmount, string message)
    {
        IsAnonymous = isAnonymous;
        Donor = donor;
        DonationAmount = donationAmount;
        Message = message;
    }
}
