using TMPro;
using UnityEngine;

public class UI_MarbleName : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI marbleNameText;

    public Marble TargetMarble { get; private set; }
    
    private RectTransform marbleNameRectTransform;

    private Camera mainCamera;
    private void Awake()
    {
        marbleNameRectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    public void SetMarble(Marble marble)
    {
        TargetMarble = marble;
        if (TargetMarble.MarbleData == null)
        {
            return;
        }
        string marbleName = TargetMarble.MarbleData.MarbleName;
        InitUI(marbleName, ColorUtil.RandomColor());
    }

    private void FixedUpdate()
    {
        if (!TargetMarble)
        {
            return;
        }

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(TargetMarble.transform.position);
        Vector3 screenPoint = mainCamera.ViewportToScreenPoint(viewportPoint);
        screenPoint.z = 0;
        marbleNameRectTransform.position = screenPoint;
    }

    private void InitUI(string marbleName, Color fontColor)
    {
        marbleNameText.SetText(marbleName);
        marbleNameText.color = fontColor;
    }
}
