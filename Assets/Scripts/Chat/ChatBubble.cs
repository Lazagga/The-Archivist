using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BubbleSide { Patient, Player }

public class ChatBubble : MonoBehaviour
{
    [Header("UI 참조")]
    public TextMeshProUGUI messageText;
    public Image bubbleBackground;
    public RectTransform bubbleRect;

    [Header("색상")]
    public Color patientColor = new Color(0.93f, 0.93f, 0.93f);
    public Color playerColor  = new Color(0.78f, 0.93f, 0.68f);

    public void Setup(string message, BubbleSide side)
    {
        messageText.text = message;
        bubbleBackground.color = side == BubbleSide.Patient ? patientColor : playerColor;

        var layout = GetComponent<HorizontalLayoutGroup>();
        if (layout == null) return;

        // 플레이어 말풍선은 오른쪽 정렬
        if (side == BubbleSide.Player)
        {
            layout.childAlignment = TextAnchor.MiddleRight;
            // 패딩 조정 - 왼쪽에 공간 주기
            layout.padding.left = 80;
            layout.padding.right = 8;
        }
        else
        {
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.padding.left = 8;
            layout.padding.right = 80;
        }
    }
}
