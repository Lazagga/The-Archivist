using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public static ChatManager Instance { get; private set; }

    [Header("UI 참조")]
    public ScrollRect scrollRect;
    public Transform chatContent;       // ScrollRect의 Content
    public Transform choiceArea;        // 하단 선택지 영역
    public TextMeshProUGUI emptyLabel;  // 조각 미선택 시 안내 문구

    [Header("프리팹")]
    public GameObject chatBubblePrefab;
    public GameObject choiceButtonPrefab;
    public GameObject processPanelPrefab; // 처리 선택 패널

    [Header("처리 선택 패널 참조")]
    public GameObject processPanel;
    public TextMeshProUGUI optionALabel;
    public TextMeshProUGUI optionADesc;
    public TextMeshProUGUI optionBLabel;
    public TextMeshProUGUI optionBDesc;
    public Button optionAButton;
    public Button optionBButton;

    // 조각별 대화 내역 저장 (pieceId → 버블 리스트)
    readonly Dictionary<string, List<GameObject>> pieceHistories
        = new Dictionary<string, List<GameObject>>();

    // 조각별 현재 노드 저장 (pieceId → 마지막 활성 노드)
    readonly Dictionary<string, DialogueNodeData> pieceCurrentNodes
        = new Dictionary<string, DialogueNodeData>();

    // 현재 선택된 조각
    MemoryPiece currentPiece;
    DialogueNodeData currentNode;
    PatientData currentPatient;

    // ShowPatientLine 코루틴 핸들 (중복 실행 방지용)
    Coroutine patientLineCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Init(PatientData patient)
    {
        currentPatient = patient;
        pieceHistories.Clear();
        pieceCurrentNodes.Clear();
        ClearChat();
        ShowEmpty(true);
        processPanel.SetActive(false);
    }

    // ── 조각 선택 ──────────────────────────────
    public void OnPieceSelected(MemoryPiece piece)
    {
        // 이전 조각 대화 내역 숨기기
        if (currentPiece != null)
            SetHistoryVisible(currentPiece.data.pieceId, false);

        currentPiece = piece;
        ShowEmpty(false);

        string id = piece.data.pieceId;

        // 미처리 조각이면 항상 EndingTracker에 활성 조각 알림 (첫 선택/재선택 공통)
        if (!piece.IsProcessed)
            EndingTracker.Instance.BeginPiece(id);

        if (!pieceHistories.ContainsKey(id))
        {
            // 첫 선택: 새 대화 시작
            pieceHistories[id] = new List<GameObject>();

            currentNode = piece.data.rootNode;
            pieceCurrentNodes[id] = currentNode;
            if (patientLineCoroutine != null) StopCoroutine(patientLineCoroutine);
            patientLineCoroutine = StartCoroutine(ShowPatientLine(currentNode.patientLine, id, currentNode, currentPiece));
        }
        else
        {
            // 재선택: 내역 복원
            SetHistoryVisible(id, true);
            ScrollToBottom();

            // 처리된 조각이면 선택지 비움
            if (piece.IsProcessed)
                ClearChoices();
            else
            {
                pieceCurrentNodes.TryGetValue(id, out currentNode);
                ShowChoices(currentNode, id);
            }
        }
    }

    // ── 환자 대사 표시 ─────────────────────────
    IEnumerator ShowPatientLine(string line, string pieceId, DialogueNodeData node, MemoryPiece piece)
    {
        ClearChoices();
        yield return new WaitForSeconds(0.3f);

        AddBubble(line, BubbleSide.Patient, pieceId);
        ScrollToBottom();

        yield return new WaitForSeconds(0.5f);

        if (node != null && !piece.IsProcessed)
            ShowChoices(node, pieceId);

        patientLineCoroutine = null;
    }

    // ── 선택지 표시 ────────────────────────────
    void ShowChoices(DialogueNodeData node, string pieceId)
    {
        ClearChoices();
        if (node == null || node.choices == null) return;

        processPanel.SetActive(false);

        foreach (var choice in node.choices)
        {
            var go = Instantiate(choiceButtonPrefab, choiceArea);
            var btn = go.GetComponent<DialogueChoiceButton>();

            var capturedChoice = choice;
            btn.Setup(choice.choiceText, () => OnChoiceSelected(capturedChoice, pieceId));
        }
    }

    // ── 선택지 클릭 ────────────────────────────
    void OnChoiceSelected(DialogueChoice choice, string pieceId)
    {
        // 태그 기록
        EndingTracker.Instance.RecordTags(choice.tags ?? new DialogueTag[0]);

        // 플레이어 말풍선
        AddBubble(choice.choiceText, BubbleSide.Player, pieceId);
        ScrollToBottom();

        if (choice.isProcessTrigger)
        {
            // 처리 화면으로 전환
            ClearChoices();
            ShowProcessPanel();
            return;
        }

        if (choice.nextNode != null)
        {
            currentNode = choice.nextNode;
            pieceCurrentNodes[pieceId] = currentNode;
            if (patientLineCoroutine != null) StopCoroutine(patientLineCoroutine);
            patientLineCoroutine = StartCoroutine(ShowPatientLine(currentNode.patientLine, pieceId, currentNode, currentPiece));
        }
        else
        {
            // 대화 종료 — 처리 선택지만 남김
            ClearChoices();
            ShowProcessTriggerOnly(pieceId);
        }
    }

    // 대화 끝났을 때 처리 트리거 버튼만 표시
    void ShowProcessTriggerOnly(string pieceId)
    {
        var go = Instantiate(choiceButtonPrefab, choiceArea);
        var btn = go.GetComponent<DialogueChoiceButton>();
        btn.Setup("이 기억을 여기서 마무리하겠습니다.", () =>
        {
            AddBubble("이 기억을 여기서 마무리하겠습니다.", BubbleSide.Player, pieceId);
            ClearChoices();
            ShowProcessPanel();
        });
    }

    // ── 처리 패널 표시 ─────────────────────────
    void ShowProcessPanel()
    {
        if (currentPatient == null) return;

        optionALabel.text = currentPatient.optionALabel;
        optionBLabel.text = currentPatient.optionBLabel;
        optionADesc.text  = currentPiece?.data.optionAPreview ?? "";
        optionBDesc.text  = currentPiece?.data.optionBPreview ?? "";

        // 알츠하이머: 프레임이 꽉 찼으면 OptionA(남긴다) 비활성화
        bool canA = PuzzleManager.Instance.Grid.CanKeep();
        optionAButton.interactable = canA;

        optionAButton.onClick.RemoveAllListeners();
        optionBButton.onClick.RemoveAllListeners();
        optionAButton.onClick.AddListener(() => OnProcessSelected(PieceProcessState.OptionA));
        optionBButton.onClick.AddListener(() => OnProcessSelected(PieceProcessState.OptionB));

        processPanel.SetActive(true);
    }

    // ── 처리 결정 ──────────────────────────────
    void OnProcessSelected(PieceProcessState state)
    {
        processPanel.SetActive(false);
        EndingTracker.Instance.RecordProcess(state);
        PuzzleManager.Instance.ApplyProcess(currentPiece, state);

        // 처리 완료 메시지 버블
        string msg = state == PieceProcessState.OptionA
            ? currentPatient.optionALabel
            : currentPatient.optionBLabel;
        AddBubble($"[{msg}]", BubbleSide.Player, currentPiece.data.pieceId);
        ScrollToBottom();

        ClearChoices();

        // 모든 조각 처리 완료 체크
        PuzzleManager.Instance.CheckCompletion();
    }

    // ── 유틸 ───────────────────────────────────
    void AddBubble(string message, BubbleSide side, string pieceId)
    {
        var go = Instantiate(chatBubblePrefab, chatContent);
        var bubble = go.GetComponent<ChatBubble>();
        bubble.Setup(message, side);

        if (!pieceHistories.ContainsKey(pieceId))
            pieceHistories[pieceId] = new List<GameObject>();
        pieceHistories[pieceId].Add(go);
    }

    void SetHistoryVisible(string pieceId, bool visible)
    {
        if (!pieceHistories.TryGetValue(pieceId, out var list)) return;
        foreach (var go in list)
            if (go != null) go.SetActive(visible);
    }

    void ClearChoices()
    {
        foreach (Transform child in choiceArea)
            Destroy(child.gameObject);
    }

    void ClearChat()
    {
        foreach (Transform child in chatContent)
            Destroy(child.gameObject);
    }

    void ShowEmpty(bool show)
    {
        if (emptyLabel != null)
            emptyLabel.gameObject.SetActive(show);
    }

    void ScrollToBottom()
    {
        StartCoroutine(ScrollNextFrame());
    }

    IEnumerator ScrollNextFrame()
    {
        yield return null; // 레이아웃 업데이트 대기
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
