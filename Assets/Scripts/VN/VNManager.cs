using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VNManager : MonoBehaviour
{
    [Header("UI 참조")]
    public Image backgroundImage;
    public Image characterImage;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public Button nextButton;

    [Header("타이핑 속도")]
    public float typingSpeed = 0.03f;

    bool waitingForNext = false;
    bool skipTyping = false;

    void Awake()
    {
        nextButton.onClick.AddListener(OnNextClicked);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    // GameManager에서 호출
    public IEnumerator PlayLines(VNLine[] lines)
    {
        if (lines == null || lines.Length == 0) yield break;

        gameObject.SetActive(true);

        foreach (var line in lines)
        {
            // 배경 교체
            if (line.backgroundSprite != null)
                backgroundImage.sprite = line.backgroundSprite;

            // 캐릭터 교체
            if (line.characterSprite != null)
            {
                characterImage.gameObject.SetActive(true);
                characterImage.sprite = line.characterSprite;
            }
            else
            {
                characterImage.gameObject.SetActive(false);
            }

            speakerNameText.text = line.speakerName;

            yield return TypeLine(line.line);

            // 다음 클릭 대기
            waitingForNext = false;
            yield return new WaitUntil(() => waitingForNext);
        }
    }

    IEnumerator TypeLine(string text)
    {
        dialogueText.text = "";
        skipTyping = false;

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogueText.text = text;
                yield break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void OnNextClicked()
    {
        if (!skipTyping && dialogueText.text.Length < dialogueText.text.Length)
        {
            // 타이핑 중이면 스킵
            skipTyping = true;
        }
        else
        {
            waitingForNext = true;
        }
    }
}
