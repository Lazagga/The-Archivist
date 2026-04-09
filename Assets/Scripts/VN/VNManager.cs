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

    [Header("기본 배경")]
    public Sprite defaultBackground; // 환자 전환 시 초기화용 (null이면 유지)

    [Header("타이핑 속도")]
    public float typingSpeed = 0.03f;

    bool waitingForNext = false;
    bool skipTyping = false;
    bool isTyping = false;

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

        // 배경 초기화 (defaultBackground가 지정된 경우)
        if (defaultBackground != null)
            backgroundImage.sprite = defaultBackground;
        characterImage.gameObject.SetActive(false);

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

            // 타이핑 중 스킵 클릭은 다음 줄로 넘기는 역할도 겸함
            if (!skipTyping)
            {
                waitingForNext = false;
                yield return new WaitUntil(() => waitingForNext);
            }
        }
    }

    IEnumerator TypeLine(string text)
    {
        dialogueText.text = "";
        skipTyping = false;
        isTyping = true;

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogueText.text = text;
                break;
            }
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void OnNextClicked()
    {
        if (isTyping)
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
