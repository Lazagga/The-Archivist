// Assets/Editor/ArchivistSceneBuilder.cs
// Tools > The Archivist > 씬 자동 생성  을 실행하면
// Assets/Scenes/Main.unity + Assets/Prefabs/ 세 프리팹을 생성하고
// 모든 컴포넌트 참조를 자동으로 연결합니다.
// SO 에셋(PatientData, EpilogueDatabase) 및 이미지만 수동으로 연결하세요.

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public static class ArchivistSceneBuilder
{
    const string SCENE_PATH = "Assets/Scenes/Main.unity";
    const string PREFAB_DIR = "Assets/Prefabs";

    // ════════════════════════════════════════════════════════════
    // 진입점
    // ════════════════════════════════════════════════════════════

    [MenuItem("Tools/The Archivist/씬 자동 생성")]
    public static void Run()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

        EnsureFolder("Assets/Scenes");
        EnsureFolder(PREFAB_DIR);
        EnsureFolder("Assets/Editor");

        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // ── 1. 카메라 / EventSystem ──────────────────────────────
        BuildCamera();
        BuildEventSystem();

        // ── 2. 매니저 ───────────────────────────────────────────
        var managersGO = new GameObject("Managers");
        var gm = managersGO.AddComponent<GameManager>();
        var et = managersGO.AddComponent<EndingTracker>();

        // ── 3. 루트 캔버스 ──────────────────────────────────────
        var canvasGO = BuildRootCanvas();
        var canvasTr = canvasGO.transform;

        // ── 4. UI 레이어 생성 ──────────────────────────────────
        var fadeCG     = BuildFadeCanvas(canvasTr);
        var titleData  = BuildTitleScreen(canvasTr);
        var vnData     = BuildVNScreen(canvasTr);
        var puzzleData = BuildPuzzleScreen(canvasTr);

        // ── 5. 프리팹 생성 ─────────────────────────────────────
        var bubblePrefab = BuildPrefab_ChatBubble();
        var choicePrefab = BuildPrefab_ChoiceButton();
        var piecePrefab  = BuildPrefab_MemoryPiece();

        // ── 6. 참조 연결: GameManager ──────────────────────────
        gm.fadeCanvas    = fadeCG;
        gm.titleManager  = titleData.manager;
        gm.vnManager     = vnData.manager;
        gm.puzzleManager = puzzleData.manager;
        gm.endingTracker = et;
        EditorUtility.SetDirty(gm);

        // ── 7. 참조 연결: TitleManager ─────────────────────────
        titleData.manager.backgroundImage = titleData.bg;
        titleData.manager.startButton     = titleData.btn;
        EditorUtility.SetDirty(titleData.manager);

        // ── 8. 참조 연결: VNManager ────────────────────────────
        vnData.manager.backgroundImage = vnData.bg;
        vnData.manager.characterImage  = vnData.character;
        vnData.manager.speakerNameText = vnData.speakerName;
        vnData.manager.dialogueText    = vnData.dialogue;
        vnData.manager.dialogueBox     = vnData.dialogueBox;
        vnData.manager.nextButton      = vnData.nextBtn;
        EditorUtility.SetDirty(vnData.manager);

        // ── 9. 참조 연결: PuzzleManager ────────────────────────
        puzzleData.manager.grid                  = puzzleData.gridComp;
        puzzleData.manager.chatManager           = puzzleData.chatMgr;
        puzzleData.manager.completeButton        = puzzleData.completeBtn;
        puzzleData.manager.completedImageDisplay = puzzleData.completedImg;
        puzzleData.manager.completedImageGroup   = puzzleData.completedCG;
        EditorUtility.SetDirty(puzzleData.manager);

        // ── 10. 참조 연결: PuzzleGrid ──────────────────────────
        puzzleData.gridComp.piecePrefab = piecePrefab;
        EditorUtility.SetDirty(puzzleData.gridComp);

        // ── 11. 참조 연결: ChatManager ─────────────────────────
        var chat = puzzleData.chatMgr;
        chat.scrollRect         = puzzleData.scrollRect;
        chat.chatContent        = puzzleData.chatContent;
        chat.choiceArea         = puzzleData.choiceArea;
        chat.emptyLabel         = puzzleData.emptyLabel;
        chat.chatBubblePrefab   = bubblePrefab;
        chat.choiceButtonPrefab = choicePrefab;
        chat.processPanel       = puzzleData.processPanel;
        chat.optionALabel       = puzzleData.optALabel;
        chat.optionADesc        = puzzleData.optADesc;
        chat.optionBLabel       = puzzleData.optBLabel;
        chat.optionBDesc        = puzzleData.optBDesc;
        chat.optionAButton      = puzzleData.optABtn;
        chat.optionBButton      = puzzleData.optBBtn;
        EditorUtility.SetDirty(chat);

        // ── 12. 씬 저장 ────────────────────────────────────────
        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene(), SCENE_PATH);
        AssetDatabase.Refresh();

        Debug.Log("[The Archivist] 씬 생성 완료 → " + SCENE_PATH);
        EditorUtility.DisplayDialog(
            "씬 생성 완료",
            "Assets/Scenes/Main.unity 저장됨\n\n" +
            "남은 수동 작업:\n" +
            "• GameManager → patients[] 에 PatientData SO 3개 연결\n" +
            "• GameManager → epilogueDatabase 에 EpilogueDatabase SO 연결\n" +
            "• TitleManager → morningSprite / eveningSprite 연결\n" +
            "• VNManager → defaultBackground 연결",
            "확인");
    }

    // ════════════════════════════════════════════════════════════
    // FadeCanvas
    // ════════════════════════════════════════════════════════════

    static CanvasGroup BuildFadeCanvas(Transform parent)
    {
        var go  = MakeUI("FadeCanvas", parent);
        var img = go.AddComponent<Image>();
        img.color = Color.black;
        var cg = go.AddComponent<CanvasGroup>();
        cg.blocksRaycasts = false;
        SetFull(go);
        go.SetActive(false);
        return cg;
    }

    // ════════════════════════════════════════════════════════════
    // TitleScreen
    // ════════════════════════════════════════════════════════════

    struct TitleData { public TitleManager manager; public Image bg; public Button btn; }

    static TitleData BuildTitleScreen(Transform parent)
    {
        var root = MakeUI("TitleScreen", parent);
        var tm   = root.AddComponent<TitleManager>();
        SetFull(root);

        // 배경
        var bgGO = MakeUI("Background", root.transform);
        var bg   = bgGO.AddComponent<Image>();
        bg.color = new Color(0.1f, 0.1f, 0.15f, 1f);
        SetFull(bgGO);

        // 타이틀 텍스트 (플레이스홀더)
        var titleTextGO  = MakeUI("TitleText", root.transform);
        var titleTmp     = titleTextGO.AddComponent<TextMeshProUGUI>();
        titleTmp.text    = "THE ARCHIVIST";
        titleTmp.fontSize = 72;
        titleTmp.alignment = TextAlignmentOptions.Center;
        titleTmp.color   = Color.white;
        SetAnchor(titleTextGO, 0.2f, 0.55f, 0.8f, 0.75f);

        // 시작 버튼
        var btnGO  = MakeUI("StartButton", root.transform);
        var btnImg = btnGO.AddComponent<Image>();
        btnImg.color = new Color(0.88f, 0.88f, 0.88f, 1f);
        var startBtn = btnGO.AddComponent<Button>();
        SetAnchor(btnGO, 0.35f, 0.3f, 0.65f, 0.42f);

        var btnTextGO = MakeUI("Text", btnGO.transform);
        var btnTmp    = btnTextGO.AddComponent<TextMeshProUGUI>();
        btnTmp.text   = "시작";
        btnTmp.fontSize = 32;
        btnTmp.alignment = TextAlignmentOptions.Center;
        btnTmp.color  = Color.black;
        SetFull(btnTextGO);

        root.SetActive(false);
        return new TitleData { manager = tm, bg = bg, btn = startBtn };
    }

    // ════════════════════════════════════════════════════════════
    // VNScreen
    // ════════════════════════════════════════════════════════════

    struct VNData
    {
        public VNManager manager;
        public Image bg, character;
        public TextMeshProUGUI speakerName, dialogue;
        public GameObject dialogueBox;
        public Button nextBtn;
    }

    static VNData BuildVNScreen(Transform parent)
    {
        var root = MakeUI("VNScreen", parent);
        var vm   = root.AddComponent<VNManager>();
        SetFull(root);

        // 배경
        var bgGO = MakeUI("Background", root.transform);
        var bg   = bgGO.AddComponent<Image>();
        bg.color = new Color(0.05f, 0.05f, 0.1f, 1f);
        SetFull(bgGO);

        // 캐릭터 이미지
        var charGO  = MakeUI("Character", root.transform);
        var charImg = charGO.AddComponent<Image>();
        charImg.color = Color.white;
        charImg.preserveAspect = true;
        charGO.SetActive(false);
        SetAnchor(charGO, 0.15f, 0.2f, 0.85f, 0.95f);

        // 다이얼로그 박스 (하단 28%)
        var boxGO  = MakeUI("DialogueBox", root.transform);
        var boxImg = boxGO.AddComponent<Image>();
        boxImg.color = new Color(0f, 0f, 0f, 0.78f);
        var boxRT = boxGO.GetComponent<RectTransform>();
        boxRT.anchorMin = new Vector2(0f, 0f);
        boxRT.anchorMax = new Vector2(1f, 0.28f);
        boxRT.offsetMin = new Vector2(20f, 12f);
        boxRT.offsetMax = new Vector2(-20f, -12f);

        // 화자 이름
        var nameGO  = MakeUI("SpeakerName", boxGO.transform);
        var nameTmp = nameGO.AddComponent<TextMeshProUGUI>();
        nameTmp.text      = "";
        nameTmp.fontSize  = 24;
        nameTmp.fontStyle = FontStyles.Bold;
        nameTmp.color     = new Color(0.95f, 0.95f, 0.75f, 1f);
        SetAnchorOffset(nameGO, 0f, 0.65f, 0.6f, 1f, 16f, 0f, 0f, -6f);

        // 대사 텍스트
        var diagGO  = MakeUI("DialogueText", boxGO.transform);
        var diagTmp = diagGO.AddComponent<TextMeshProUGUI>();
        diagTmp.text             = "";
        diagTmp.fontSize         = 22;
        diagTmp.color            = Color.white;
        diagTmp.enableWordWrapping = true;
        SetAnchorOffset(diagGO, 0f, 0f, 0.86f, 0.65f, 16f, 8f, -8f, -4f);

        // 다음 버튼
        var nextGO  = MakeUI("NextButton", boxGO.transform);
        var nextImg = nextGO.AddComponent<Image>();
        nextImg.color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
        var nextBtn = nextGO.AddComponent<Button>();
        SetAnchorOffset(nextGO, 0.86f, 0f, 1f, 1f, 4f, 8f, -8f, -8f);

        var nextTextGO = MakeUI("Text", nextGO.transform);
        var nextTmp    = nextTextGO.AddComponent<TextMeshProUGUI>();
        nextTmp.text      = "▶";
        nextTmp.fontSize  = 30;
        nextTmp.alignment = TextAlignmentOptions.Center;
        nextTmp.color     = Color.white;
        SetFull(nextTextGO);

        root.SetActive(false);
        return new VNData
        {
            manager = vm, bg = bg, character = charImg,
            speakerName = nameTmp, dialogue = diagTmp,
            dialogueBox = boxGO, nextBtn = nextBtn
        };
    }

    // ════════════════════════════════════════════════════════════
    // PuzzleScreen
    // ════════════════════════════════════════════════════════════

    struct PuzzleData
    {
        public PuzzleManager  manager;
        public PuzzleGrid     gridComp;
        public ChatManager    chatMgr;
        public Button         completeBtn;
        public Image          completedImg;
        public CanvasGroup    completedCG;
        public ScrollRect     scrollRect;
        public Transform      chatContent, choiceArea;
        public TextMeshProUGUI emptyLabel, optALabel, optADesc, optBLabel, optBDesc;
        public Button         optABtn, optBBtn;
        public GameObject     processPanel;
    }

    static PuzzleData BuildPuzzleScreen(Transform parent)
    {
        var root = MakeUI("PuzzleScreen", parent);
        var pm   = root.AddComponent<PuzzleManager>();
        SetFull(root);
        root.SetActive(false);

        // ── 왼쪽: 퍼즐 영역 (0 ~ 60%) ─────────────────────────
        var puzzleAreaGO  = MakeUI("PuzzleArea", root.transform);
        var puzzleAreaImg = puzzleAreaGO.AddComponent<Image>();
        puzzleAreaImg.color = new Color(0.06f, 0.06f, 0.08f, 1f);
        SetAnchor(puzzleAreaGO, 0f, 0f, 0.6f, 1f);

        // 퍼즐 그리드 (중앙에 배치)
        var gridGO     = MakeUI("PuzzleGrid", puzzleAreaGO.transform);
        var gridComp   = gridGO.AddComponent<PuzzleGrid>();
        var gridLayout = gridGO.AddComponent<GridLayoutGroup>();
        gridLayout.cellSize        = new Vector2(160f, 160f);
        gridLayout.spacing         = new Vector2(3f, 3f);
        gridLayout.startCorner     = GridLayoutGroup.Corner.UpperLeft;
        gridLayout.startAxis       = GridLayoutGroup.Axis.Horizontal;
        gridLayout.childAlignment  = TextAnchor.MiddleCenter;
        SetAnchorOffset(gridGO, 0.05f, 0.1f, 0.95f, 0.9f, 0, 0, 0, 0);

        // 완료 버튼 (퍼즐 영역 상단 오른쪽)
        var completeBtnGO  = MakeUI("CompleteButton", puzzleAreaGO.transform);
        var completeBtnImg = completeBtnGO.AddComponent<Image>();
        completeBtnImg.color = new Color(0.35f, 0.75f, 0.4f, 1f);
        var completeBtn    = completeBtnGO.AddComponent<Button>();
        SetAnchor(completeBtnGO, 0.55f, 0.9f, 0.98f, 0.99f);
        var completeBtnTextGO = MakeUI("Text", completeBtnGO.transform);
        var completeBtnTmp    = completeBtnTextGO.AddComponent<TextMeshProUGUI>();
        completeBtnTmp.text      = "치료 완료";
        completeBtnTmp.fontSize  = 26;
        completeBtnTmp.alignment = TextAlignmentOptions.Center;
        completeBtnTmp.color     = Color.white;
        SetFull(completeBtnTextGO);
        completeBtnGO.SetActive(false);

        // 완성 이미지 오버레이 (퍼즐 영역 위에 표시)
        var completedOverlayGO = MakeUI("CompletedImageOverlay", root.transform);
        var completedCG        = completedOverlayGO.AddComponent<CanvasGroup>();
        completedCG.alpha      = 0f;
        SetAnchor(completedOverlayGO, 0f, 0f, 0.6f, 1f);
        completedOverlayGO.SetActive(false);

        var completedImgGO  = MakeUI("CompletedImage", completedOverlayGO.transform);
        var completedImg    = completedImgGO.AddComponent<Image>();
        completedImg.color  = Color.white;
        completedImg.preserveAspect = true;
        SetFull(completedImgGO);

        // ── 오른쪽: 채팅 패널 (60% ~ 100%) ────────────────────
        var chatPanelGO  = MakeUI("ChatPanel", root.transform);
        var chatMgr      = chatPanelGO.AddComponent<ChatManager>();
        var chatBgImg    = chatPanelGO.AddComponent<Image>();
        chatBgImg.color  = new Color(0.04f, 0.04f, 0.06f, 1f);
        SetAnchor(chatPanelGO, 0.6f, 0f, 1f, 1f);

        // 빈 상태 안내 텍스트
        var emptyLabelGO = MakeUI("EmptyLabel", chatPanelGO.transform);
        var emptyLabel   = emptyLabelGO.AddComponent<TextMeshProUGUI>();
        emptyLabel.text      = "기억 조각을 선택하세요";
        emptyLabel.fontSize  = 22;
        emptyLabel.alignment = TextAlignmentOptions.Center;
        emptyLabel.color     = new Color(0.5f, 0.5f, 0.5f, 1f);
        SetAnchor(emptyLabelGO, 0f, 0.4f, 1f, 0.6f);

        // 채팅 스크롤뷰 (상단 68%)
        var scrollGO   = MakeUI("ScrollView", chatPanelGO.transform);
        var scrollRect = scrollGO.AddComponent<ScrollRect>();
        scrollRect.horizontal = false;
        scrollRect.scrollSensitivity = 30f;
        SetAnchorOffset(scrollGO, 0f, 0.32f, 1f, 1f, 0, 0, 0, 0);

        // Viewport
        var viewportGO = MakeUI("Viewport", scrollGO.transform);
        viewportGO.AddComponent<Image>().color = Color.clear;
        viewportGO.AddComponent<Mask>().showMaskGraphic = false;
        SetFull(viewportGO);
        scrollRect.viewport = viewportGO.GetComponent<RectTransform>();

        // Content
        var contentGO  = MakeUI("Content", viewportGO.transform);
        var contentVLG = contentGO.AddComponent<VerticalLayoutGroup>();
        contentVLG.childControlWidth      = true;
        contentVLG.childControlHeight     = true;
        contentVLG.childForceExpandWidth  = true;
        contentVLG.childForceExpandHeight = false;
        contentVLG.spacing = 6f;
        contentVLG.padding = new RectOffset(8, 8, 8, 8);
        var contentCSF = contentGO.AddComponent<ContentSizeFitter>();
        contentCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        var contentRT = contentGO.GetComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0f, 1f);
        contentRT.anchorMax = new Vector2(1f, 1f);
        contentRT.pivot     = new Vector2(0.5f, 1f);
        contentRT.offsetMin = contentRT.offsetMax = Vector2.zero;
        scrollRect.content  = contentRT;

        // 선택지 영역 (ProcessPanel 위, 스크롤뷰 아래 → 32%~32% 사이)
        var choiceAreaGO  = MakeUI("ChoiceArea", chatPanelGO.transform);
        var choiceVLG     = choiceAreaGO.AddComponent<VerticalLayoutGroup>();
        choiceVLG.childControlWidth      = true;
        choiceVLG.childControlHeight     = false;
        choiceVLG.childForceExpandWidth  = true;
        choiceVLG.childForceExpandHeight = false;
        choiceVLG.spacing = 4f;
        choiceVLG.padding = new RectOffset(8, 8, 4, 4);
        SetAnchor(choiceAreaGO, 0f, 0.2f, 1f, 0.32f);

        // 처리 패널 (하단 20%)
        var processPanelGO  = MakeUI("ProcessPanel", chatPanelGO.transform);
        var processPanelImg = processPanelGO.AddComponent<Image>();
        processPanelImg.color = new Color(0.1f, 0.1f, 0.15f, 1f);
        SetAnchor(processPanelGO, 0f, 0f, 1f, 0.2f);

        // OptionA (왼쪽 절반)
        var optAGO  = MakeUI("OptionA", processPanelGO.transform);
        var optAImg = optAGO.AddComponent<Image>();
        optAImg.color = new Color(0.28f, 0.48f, 0.78f, 1f);
        var optABtn = optAGO.AddComponent<Button>();
        SetAnchorOffset(optAGO, 0f, 0f, 0.5f, 1f, 6f, 6f, -3f, -6f);

        var optALabelGO = MakeUI("Label", optAGO.transform);
        var optALabel   = optALabelGO.AddComponent<TextMeshProUGUI>();
        optALabel.text      = "Option A";
        optALabel.fontSize  = 20;
        optALabel.fontStyle = FontStyles.Bold;
        optALabel.alignment = TextAlignmentOptions.Center;
        optALabel.color     = Color.white;
        SetAnchor(optALabelGO, 0f, 0.5f, 1f, 1f);

        var optADescGO = MakeUI("Desc", optAGO.transform);
        var optADesc   = optADescGO.AddComponent<TextMeshProUGUI>();
        optADesc.text      = "";
        optADesc.fontSize  = 15;
        optADesc.alignment = TextAlignmentOptions.Center;
        optADesc.color     = new Color(0.88f, 0.88f, 0.88f, 1f);
        optADesc.enableWordWrapping = true;
        SetAnchorOffset(optADescGO, 0f, 0f, 1f, 0.5f, 4f, 0f, -4f, 0f);

        // OptionB (오른쪽 절반)
        var optBGO  = MakeUI("OptionB", processPanelGO.transform);
        var optBImg = optBGO.AddComponent<Image>();
        optBImg.color = new Color(0.72f, 0.35f, 0.35f, 1f);
        var optBBtn = optBGO.AddComponent<Button>();
        SetAnchorOffset(optBGO, 0.5f, 0f, 1f, 1f, 3f, 6f, -6f, -6f);

        var optBLabelGO = MakeUI("Label", optBGO.transform);
        var optBLabel   = optBLabelGO.AddComponent<TextMeshProUGUI>();
        optBLabel.text      = "Option B";
        optBLabel.fontSize  = 20;
        optBLabel.fontStyle = FontStyles.Bold;
        optBLabel.alignment = TextAlignmentOptions.Center;
        optBLabel.color     = Color.white;
        SetAnchor(optBLabelGO, 0f, 0.5f, 1f, 1f);

        var optBDescGO = MakeUI("Desc", optBGO.transform);
        var optBDesc   = optBDescGO.AddComponent<TextMeshProUGUI>();
        optBDesc.text      = "";
        optBDesc.fontSize  = 15;
        optBDesc.alignment = TextAlignmentOptions.Center;
        optBDesc.color     = new Color(0.88f, 0.88f, 0.88f, 1f);
        optBDesc.enableWordWrapping = true;
        SetAnchorOffset(optBDescGO, 0f, 0f, 1f, 0.5f, 4f, 0f, -4f, 0f);

        processPanelGO.SetActive(false);

        return new PuzzleData
        {
            manager      = pm,
            gridComp     = gridComp,
            chatMgr      = chatMgr,
            completeBtn  = completeBtn,
            completedImg = completedImg,
            completedCG  = completedCG,
            scrollRect   = scrollRect,
            chatContent  = contentGO.transform,
            choiceArea   = choiceAreaGO.transform,
            emptyLabel   = emptyLabel,
            optALabel    = optALabel,
            optADesc     = optADesc,
            optBLabel    = optBLabel,
            optBDesc     = optBDesc,
            optABtn      = optABtn,
            optBBtn      = optBBtn,
            processPanel = processPanelGO
        };
    }

    // ════════════════════════════════════════════════════════════
    // 프리팹: ChatBubble
    // ════════════════════════════════════════════════════════════

    static GameObject BuildPrefab_ChatBubble()
    {
        const string path = PREFAB_DIR + "/ChatBubble.prefab";
        var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        // 루트: HorizontalLayoutGroup (정렬 방향은 ChatBubble.Setup에서 런타임 변경)
        var root = new GameObject("ChatBubble");
        root.AddComponent<RectTransform>();
        var hlg = root.AddComponent<HorizontalLayoutGroup>();
        hlg.childAlignment      = TextAnchor.MiddleLeft;
        hlg.padding             = new RectOffset(8, 80, 4, 4);
        hlg.childControlWidth   = true;
        hlg.childControlHeight  = true;
        hlg.childForceExpandWidth  = true;
        hlg.childForceExpandHeight = false;
        var rootCSF = root.AddComponent<ContentSizeFitter>();
        rootCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // 말풍선 배경
        var bubbleGO  = new GameObject("Bubble");
        bubbleGO.transform.SetParent(root.transform, false);
        bubbleGO.AddComponent<RectTransform>();
        var bubbleImg = bubbleGO.AddComponent<Image>();
        bubbleImg.color = new Color(0.93f, 0.93f, 0.93f, 1f);
        var bubbleLE = bubbleGO.AddComponent<LayoutElement>();
        bubbleLE.flexibleWidth = 1f;
        var bubbleVLG = bubbleGO.AddComponent<VerticalLayoutGroup>();
        bubbleVLG.padding            = new RectOffset(12, 12, 8, 8);
        bubbleVLG.childControlWidth  = true;
        bubbleVLG.childControlHeight = true;
        var bubbleCSF = bubbleGO.AddComponent<ContentSizeFitter>();
        bubbleCSF.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // 메시지 텍스트
        var textGO = new GameObject("MessageText");
        textGO.transform.SetParent(bubbleGO.transform, false);
        textGO.AddComponent<RectTransform>();
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text               = "메시지";
        tmp.fontSize           = 18;
        tmp.color              = Color.black;
        tmp.enableWordWrapping = true;

        // ChatBubble 컴포넌트 & 참조 연결
        var chatBubble                = root.AddComponent<ChatBubble>();
        chatBubble.messageText        = tmp;
        chatBubble.bubbleBackground   = bubbleImg;
        chatBubble.bubbleRect         = bubbleGO.GetComponent<RectTransform>();

        var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
        Object.DestroyImmediate(root);
        return prefab;
    }

    // ════════════════════════════════════════════════════════════
    // 프리팹: ChoiceButton
    // ════════════════════════════════════════════════════════════

    static GameObject BuildPrefab_ChoiceButton()
    {
        const string path = PREFAB_DIR + "/ChoiceButton.prefab";
        var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        var root    = new GameObject("ChoiceButton");
        root.AddComponent<RectTransform>();
        var img     = root.AddComponent<Image>();
        img.color   = new Color(0.82f, 0.82f, 0.82f, 1f);
        var btn     = root.AddComponent<Button>();
        var le      = root.AddComponent<LayoutElement>();
        le.minHeight = 48f;
        var csf     = root.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.MinSize;

        var textGO = new GameObject("LabelText");
        textGO.transform.SetParent(root.transform, false);
        var rt = textGO.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = rt.offsetMax = Vector2.zero;
        var tmp = textGO.AddComponent<TextMeshProUGUI>();
        tmp.text      = "선택지";
        tmp.fontSize  = 18;
        tmp.color     = Color.black;
        tmp.alignment = TextAlignmentOptions.MidlineLeft;
        tmp.margin    = new Vector4(14f, 0f, 14f, 0f);

        var dcb       = root.AddComponent<DialogueChoiceButton>();
        dcb.labelText = tmp;
        dcb.button    = btn;

        var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
        Object.DestroyImmediate(root);
        return prefab;
    }

    // ════════════════════════════════════════════════════════════
    // 프리팹: MemoryPiece
    // ════════════════════════════════════════════════════════════

    static GameObject BuildPrefab_MemoryPiece()
    {
        const string path = PREFAB_DIR + "/MemoryPiece.prefab";
        var existing = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (existing != null) return existing;

        var root = new GameObject("MemoryPiece");
        root.AddComponent<RectTransform>();
        root.AddComponent<RawImage>();
        root.AddComponent<MemoryPiece>();

        var prefab = PrefabUtility.SaveAsPrefabAsset(root, path);
        Object.DestroyImmediate(root);
        return prefab;
    }

    // ════════════════════════════════════════════════════════════
    // 씬 공통 오브젝트
    // ════════════════════════════════════════════════════════════

    static GameObject BuildRootCanvas()
    {
        var go     = new GameObject("Canvas");
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode          = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution  = new Vector2(1920f, 1080f);
        scaler.screenMatchMode      = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight   = 0.5f;

        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    static void BuildCamera()
    {
        var go  = new GameObject("Main Camera");
        go.tag  = "MainCamera";
        var cam = go.AddComponent<Camera>();
        cam.clearFlags      = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.orthographic    = true;
        go.AddComponent<AudioListener>();
    }

    static void BuildEventSystem()
    {
        var go = new GameObject("EventSystem");
        go.AddComponent<EventSystem>();
        go.AddComponent<StandaloneInputModule>();
    }

    // ════════════════════════════════════════════════════════════
    // RectTransform 헬퍼
    // ════════════════════════════════════════════════════════════

    static GameObject MakeUI(string name, Transform parent)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    static void SetFull(GameObject go)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static void SetAnchor(GameObject go, float xMin, float yMin, float xMax, float yMax)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(xMin, yMin);
        rt.anchorMax = new Vector2(xMax, yMax);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    /// <summary>offset: left, bottom, right, top (offsetMax의 x,y는 right,top의 음수)</summary>
    static void SetAnchorOffset(GameObject go,
        float xMin, float yMin, float xMax, float yMax,
        float left, float bottom, float right, float top)
    {
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(xMin, yMin);
        rt.anchorMax = new Vector2(xMax, yMax);
        rt.offsetMin = new Vector2(left, bottom);
        rt.offsetMax = new Vector2(right, top);
    }

    // ════════════════════════════════════════════════════════════
    // 폴더 헬퍼
    // ════════════════════════════════════════════════════════════

    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        var parts   = path.Split('/');
        var current = parts[0];
        for (int i = 1; i < parts.Length; i++)
        {
            var next = current + "/" + parts[i];
            if (!AssetDatabase.IsValidFolder(next))
                AssetDatabase.CreateFolder(current, parts[i]);
            current = next;
        }
    }
}
