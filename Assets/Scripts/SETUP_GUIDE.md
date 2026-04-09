# The Archivist — Unity 씬 설정 가이드

## 스크립트 구조
```
Scripts/
  Core/
    GameManager.cs      - 전체 게임 흐름, 환자 순서, 페이드
    EndingTracker.cs    - 대화 태그/처리 선택 기록, 엔딩 계산
  Data/
    PatientData.cs      - 환자 ScriptableObject
    MemoryPieceData.cs  - 기억 조각 ScriptableObject
    DialogueNodeData.cs - 대화 노드 ScriptableObject
  VN/
    VNManager.cs        - VN 파트 UI 제어
  Puzzle/
    PuzzleManager.cs    - 퍼즐 파트 총괄
    PuzzleGrid.cs       - 조각 그리드 생성
    MemoryPiece.cs      - 개별 조각 클릭/상태
  Chat/
    ChatManager.cs      - 채팅 UI, 선택지, 처리 패널
    ChatBubble.cs       - 말풍선 컴포넌트
    DialogueChoiceButton.cs - 선택지 버튼
```

---

## 계층 구조 (Hierarchy)

```
GameManager (빈 오브젝트)
  - GameManager.cs
  - EndingTracker.cs

FadeCanvas (Canvas, CanvasGroup)
  - Panel (Image, 검은색, raycastTarget=false)

[VN Canvas]
  VNManager (VNManager.cs)
  Background (Image)
  Character (Image)
  DialogueBox (Panel)
    NameText (TextMeshProUGUI)
    DialogueText (TextMeshProUGUI)
    NextButton (Button)

[Puzzle Canvas]
  PuzzleManager (PuzzleManager.cs)
  
  LeftPanel (조각 영역)
    PuzzleGrid (PuzzleGrid.cs, GridLayoutGroup)
  
  RightPanel
    ChatManager (ChatManager.cs)
    EmptyLabel (TextMeshProUGUI) - "조각을 선택하면..."
    
    ChatScrollView (ScrollRect)
      Viewport
        ChatContent (VerticalLayoutGroup, ContentSizeFitter)
    
    ChoiceArea (VerticalLayoutGroup)
      [런타임에 선택지 버튼 생성됨]
    
    ProcessPanel (기본 비활성화)
      OptionAButton (Button)
        OptionALabel (TextMeshProUGUI)
        OptionADesc (TextMeshProUGUI)
      OptionBButton (Button)
        OptionBLabel (TextMeshProUGUI)
        OptionBDesc (TextMeshProUGUI)
    
    CompleteButton (Button) - 기본 비활성화
  
  CompletedImageDisplay (Image, CanvasGroup) - 기본 비활성화
```

---

## 프리팹 설정

### ChatBubblePrefab
- HorizontalLayoutGroup (padding 좌우 8)
- ChatBubble.cs
- 자식: BubbleBackground (Image)
  - 자식: MessageText (TextMeshProUGUI, word wrap=true)

### DialogueChoiceButtonPrefab  
- Button
- DialogueChoiceButton.cs
- 자식: LabelText (TextMeshProUGUI)

### MemoryPiecePrefab
- Image (200x200 권장)
- MemoryPiece.cs
- Button 또는 EventTrigger (IPointerClickHandler로 대체 가능)

---

## ScriptableObject 생성 방법

### 환자 데이터
Assets 우클릭 → Create → TheArchivist → PatientData
- patientName: "김준혁 상사" (군인)
- patientType: Soldier
- optionALabel: "덜어낸다"
- optionBLabel: "끌어안는다"
- maxPiecesInFrame: -1 (노인은 7 설정)

### 기억 조각 데이터
Assets 우클릭 → Create → TheArchivist → MemoryPieceData
- pieceId: "soldier_colleague_death" (규칙: {환자타입}_{카테고리}_{세부})
- gridPosition: (col, row) → (0,0)부터 시작
- connectedPieceIds: 연결된 조각 IDs 배열
- optionAPreview: "고통이 줄어듭니다\n동료 기억이 흐릿해집니다"
- optionBPreview: "기억이 온전히 남습니다\n고통도 함께 남습니다"

### pieceId 명명 규칙
- 군인: soldier_war_*, soldier_colleague_*, soldier_family_*, soldier_guilt_*
- 노인: elder_daughter_*, elder_wife_*, elder_career_*
- 아이: child_birthday_*, child_gift_*, child_voice_*

### 대화 노드
Assets 우클릭 → Create → TheArchivist → DialogueNode
- choices[2].isProcessOption = true (마지막 선택지는 항상 처리 트리거)
- choices[2].choiceText = "이 기억은 여기서 마무리하겠습니다."

---

## 엔딩 키

### 군인
- soldier_retrauma
- soldier_embrace_colleague
- soldier_release_colleague
- soldier_embrace_no_colleague
- soldier_release_no_colleague

### 노인
- elder_arbitrary
- elder_kept_daughter
- elder_kept_wife
- elder_kept_career
- elder_kept_misc

### 아이
- child_shock (대화 3회 미만 + 드러냄)
- child_overcome (드러냄 + KeyEmotion 태그)
- child_revealed_alone (드러냄 but 감정 지원 없음)
- child_kept_supported (그대로 둠 + 대화 충분)
- child_unchanged

### 주인공
- protagonist_exhausted
- protagonist_efficient
- protagonist_balanced
