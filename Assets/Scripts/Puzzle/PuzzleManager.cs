using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager Instance { get; private set; }

    [Header("참조")]
    public PuzzleGrid grid;
    public ChatManager chatManager;
    public Button completeButton;

    [Header("완성 그림 표시")]
    public Image completedImageDisplay;
    public CanvasGroup completedImageGroup;

    MemoryPiece selectedPiece;
    PatientData currentPatient;

    public bool IsComplete { get; private set; }
    public PuzzleGrid Grid => grid;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        completeButton.onClick.AddListener(OnCompleteClicked);
        completeButton.gameObject.SetActive(false);
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public void LoadPatient(PatientData patient)
    {
        IsComplete = false;
        currentPatient = patient;

        // 알츠하이머 전용: 프레임 제한 설정
        grid.maxPieces = patient.maxPiecesInFrame;

        grid.Build(patient);
        chatManager.Init(patient);

        completeButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    // ── 조각 클릭 ──────────────────────────────
    public void OnPieceClicked(MemoryPiece piece)
    {
        if (selectedPiece == piece) return;

        // 이전 선택 해제
        if (selectedPiece != null)
            selectedPiece.SetSelected(false);

        selectedPiece = piece;
        piece.SetSelected(true);

        chatManager.OnPieceSelected(piece);
    }

    // ── 처리 적용 ──────────────────────────────
    public void ApplyProcess(MemoryPiece piece, PieceProcessState state)
    {
        if (state == PieceProcessState.OptionA)
        {
            piece.ApplyOptionA();
            // 연결된 조각 페이드
            if (piece.data.connectedPieceIds != null && piece.data.connectedPieceIds.Length > 0)
                grid.FadeConnectedPieces(piece.data.connectedPieceIds);
        }
        else
        {
            piece.ApplyOptionB();
        }

        piece.SetSelected(false);
        selectedPiece = null;
        CheckCompletion();
    }

    // ── 완료 체크 ──────────────────────────────
    public void CheckCompletion()
    {
        if (grid.AllProcessed())
            completeButton.gameObject.SetActive(true);
    }

    void OnCompleteClicked()
    {
        StartCoroutine(ShowCompletedImage());
    }

    IEnumerator ShowCompletedImage()
    {
        completeButton.gameObject.SetActive(false);

        // 완성 이미지 페이드인 (있다면)
        if (completedImageDisplay != null && completedImageGroup != null)
        {
            completedImageGroup.gameObject.SetActive(true);
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime;
                completedImageGroup.alpha = t;
                yield return null;
            }
            yield return new WaitForSeconds(2.5f);

            t = 1f;
            while (t > 0f)
            {
                t -= Time.deltaTime;
                completedImageGroup.alpha = t;
                yield return null;
            }
            completedImageGroup.gameObject.SetActive(false);
        }

        IsComplete = true;
    }
}
