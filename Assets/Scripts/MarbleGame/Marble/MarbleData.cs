using System;
using UnityEngine;

[Serializable]
public class MarbleData
{
    public int MarbleID { get; private set; }
    public string MarbleName { get; private set; }
    public DonationData DonationData { get; private set; }
    public Color MarbleColor { get; private set; }

    public MarbleData(int marbleID, string marbleName, Color marbleColor, DonationData donationData )
    {
        MarbleID = marbleID;
        MarbleName = marbleName;
        MarbleColor = marbleColor;
        DonationData = donationData;
    }
}
