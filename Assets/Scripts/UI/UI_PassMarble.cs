using TMPro;
using UnityEngine;

public class UI_PassMarble : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleRankText;
    [SerializeField]
    private TextMeshProUGUI marbleNameText;

    public void UpdateUI(int marbleRank, string marbleName)
    {
        marbleRankText.SetText(marbleRank.ToString());
        marbleNameText.SetText(marbleName);
    }
}
