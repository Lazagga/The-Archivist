using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MemoryPiece : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public MemoryPieceData data;

    RawImage rawImage;
    PieceProcessState processState = PieceProcessState.Unprocessed;

    // 이 조각이 표시할 UV 영역 (전체 이미지 기준 0~1)
    Rect uvRect;

    // 기본 텍스처 / 대체 텍스처 (아이 전용)
    Texture2D baseTexture;
    Texture2D altTexture;

    // 선택 강조 오버레이
    Image highlightOverlay;

    public PieceProcessState ProcessState => processState;
    public bool IsProcessed => processState != PieceProcessState.Unprocessed;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        if (rawImage == null) rawImage = gameObject.AddComponent<RawImage>();

        // 강조 오버레이 생성
        var overlayGo = new GameObject("Highlight");
        overlayGo.transform.SetParent(transform, false);
        highlightOverlay = overlayGo.AddComponent<Image>();
        highlightOverlay.color = new Color(1f, 1f, 0.6f, 0.3f);
        highlightOverlay.raycastTarget = false;

        var rt = overlayGo.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        highlightOverlay.gameObject.SetActive(false);
    }

    /// <summary>
    /// PuzzleGrid에서 호출. 전체 이미지에서 해당 조각의 UV Rect를 계산해서 설정.
    /// </summary>
    public void Init(MemoryPieceData pieceData, Texture2D texture, Texture2D alt,
                     Vector2Int gridSize)
    {
        data = pieceData;
        baseTexture = texture;
        altTexture = alt;

        rawImage.texture = baseTexture;

        // UV Rect 계산: gridPosition 기준
        // RawImage.uvRect는 (u, v, width, height) — 좌하단 기준
        float w = 1f / gridSize.x;
        float h = 1f / gridSize.y;
        // Unity UV는 좌하단이 (0,0)이므로 row를 뒤집어야 위→아래로 읽힘
        int col = pieceData.gridPosition.x;
        int row = (gridSize.y - 1) - pieceData.gridPosition.y;

        uvRect = new Rect(col * w, row * h, w, h);
        rawImage.uvRect = uvRect;
        rawImage.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsProcessed)
            PuzzleManager.Instance.OnPieceClicked(this);
    }

    public void SetSelected(bool selected)
    {
        highlightOverlay.gameObject.SetActive(selected);
    }

    // OptionA: 덜어낸다 / 남긴다 / 드러낸다
    public void ApplyOptionA()
    {
        processState = PieceProcessState.OptionA;

        if (data.pieceId.StartsWith("child"))
        {
            // 아이: altTexture의 같은 UV 영역으로 교체 (진실 이미지)
            if (altTexture != null)
            {
                rawImage.texture = altTexture;
                rawImage.uvRect = uvRect;
                rawImage.color = Color.white;
            }
        }
        else
        {
            // 군인/노인: 흑백 효과 (색상을 회색으로)
            rawImage.color = new Color(0.45f, 0.45f, 0.45f, 1f);
        }

        SetSelected(false);
    }

    // OptionB: 끌어안는다 / 제거한다 / 그대로 둔다
    public void ApplyOptionB()
    {
        processState = PieceProcessState.OptionB;

        if (data.pieceId.StartsWith("elder"))
        {
            // 노인: 제거 → 투명하게
            rawImage.color = new Color(1f, 1f, 1f, 0.15f);
        }
        else
        {
            // 군인: 끌어안음 → 원본 유지, 약간 어둡게
            rawImage.color = new Color(0.7f, 0.7f, 0.8f, 1f);
        }

        SetSelected(false);
    }

    // 연결 조각 페이드 효과
    public void ApplyFadeEffect()
    {
        if (!IsProcessed)
            rawImage.color = new Color(0.65f, 0.65f, 0.65f, 0.75f);
    }
}
