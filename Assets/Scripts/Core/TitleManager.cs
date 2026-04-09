using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("UI 참조")]
    public Image backgroundImage;
    public Button startButton;      // 첫 시작 / 다시 하기 버튼

    [Header("배경 이미지")]
    public Sprite morningSprite;    // 게임 시작 전: 아침 진료실
    public Sprite eveningSprite;    // 엔딩 이후: 저녁 진료실

    bool buttonPressed = false;

    void Awake()
    {
        startButton.onClick.AddListener(() => buttonPressed = true);
        gameObject.SetActive(false);
    }

    // GameManager에서 호출
    public void Show(bool isEvening)
    {
        buttonPressed = false;
        backgroundImage.sprite = isEvening ? eveningSprite : morningSprite;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator WaitForStart()
    {
        yield return new WaitUntil(() => buttonPressed);
    }
}
