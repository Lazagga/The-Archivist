# ScriptableObject 생성 가이드 — The Archivist

## 개요

Unity 에디터에서 수동으로 생성해야 하는 SO 목록:
- **DialogueNodeData** : 대화 노드 (~80개)
- **MemoryPieceData** : 기억 조각 (총 30개: 군인 9 + 노인 12 + 아동 9)
- **PatientData** : 환자 데이터 (3개)

---

## SO 생성 방법

Project 창 우클릭 → **Create → TheArchivist → [타입]**

---

## 폴더 구조 (Assets/ScriptableObjects/)

```
ScriptableObjects/
├── Patients/
│   ├── Patient_Soldier.asset
│   ├── Patient_Elder.asset
│   └── Patient_Child.asset
├── Pieces/
│   ├── Soldier/
│   │   ├── Piece_soldier_colleague_first.asset
│   │   ├── ...
│   ├── Elder/
│   │   ├── Piece_elder_daughter_face.asset
│   │   ├── ...
│   └── Child/
│       ├── Piece_child_birthday_cake.asset
│       ├── ...
└── Dialogues/
    ├── Soldier/
    │   ├── Node_soldier_colleague_first_root.asset
    │   ├── Node_soldier_colleague_first_A.asset
    │   ├── ...
    ├── Elder/
    └── Child/
```

---

## 노드 명명 규칙

| 노드 | SO 파일명 |
|------|-----------|
| rootNode | `Node_{pieceId}_root` |
| 선택A 이후 | `Node_{pieceId}_A` |
| 선택B 이후 | `Node_{pieceId}_B` |
| 선택A → A | `Node_{pieceId}_AA` |
| 선택A → B | `Node_{pieceId}_AB` |
| 선택B → A | `Node_{pieceId}_BA` |

---

## DialogueChoice 설정 규칙

각 노드의 `choices` 배열은 항상 크기 3.

- **isProcessTrigger = false** : 일반 대화 선택지. `nextNode`를 지정하거나 null(대화 종료 후 자동 처리 버튼 표시).
- **isProcessTrigger = true** : 선택 즉시 처리 패널로 전환. `nextNode` 불필요.

> **리프 노드 처리**: choices의 모든 항목이 처리 트리거인 경우 3개 모두 `isProcessTrigger=true`, 동일한 텍스트("이 기억은 여기서 마무리하겠습니다.") 사용.

---
---

# 환자 1 — 박성우 (군인, PTSD)

## PatientData: `Patient_Soldier`

| 필드 | 값 |
|------|----|
| patientName | 박성우 |
| patientType | Soldier |
| optionALabel | 덜어낸다 |
| optionBLabel | 끌어안는다 |
| gridSize | x=3, y=3 |
| maxPiecesInFrame | -1 (제한 없음) |
| memoryImage | [군인 기억 이미지 Texture2D] |
| memoryImageAlt | (사용 안 함, null 가능) |
| pieces | 아래 9개 MemoryPieceData 참조 |

---

## MemoryPieceData — 군인 9개

| SO 파일명 | pieceId | pieceName | gridPosition | connectedPieceIds | rootNode |
|-----------|---------|-----------|--------------|-------------------|---------|
| Piece_soldier_colleague_first | soldier_colleague_first | 처음 만난 날 | (0,0) | — | Node_soldier_colleague_first_root |
| Piece_soldier_colleague_training | soldier_colleague_training | 훈련소 시절 | (1,0) | — | Node_soldier_colleague_training_root |
| Piece_soldier_war_before | soldier_war_before | 파병 전날 밤 | (2,0) | — | Node_soldier_war_before_root |
| Piece_soldier_war_combat | soldier_war_combat | 전투 장면 | (0,1) | soldier_colleague_death | Node_soldier_war_combat_root |
| Piece_soldier_colleague_death | soldier_colleague_death | 이동현의 마지막 | (1,1) | soldier_colleague_first, soldier_colleague_training, soldier_war_before | Node_soldier_colleague_death_root |
| Piece_soldier_war_order | soldier_war_order | 철수 명령 | (2,1) | soldier_guilt_survive | Node_soldier_war_order_root |
| Piece_soldier_family_return | soldier_family_return | 귀환 | (0,2) | — | Node_soldier_family_return_root |
| Piece_soldier_family_son | soldier_family_son | 아들의 얼굴 | (1,2) | — | Node_soldier_family_son_root |
| Piece_soldier_guilt_survive | soldier_guilt_survive | 생존자의 죄책감 | (2,2) | — | Node_soldier_guilt_survive_root |

### 처리 미리보기 텍스트

| pieceId | optionAPreview (덜어낸다) | optionBPreview (끌어안는다) |
|---------|--------------------------|---------------------------|
| soldier_war_combat | "전투의 기억이 희미해집니다.\n이동현의 마지막 기억도 흐릿해집니다." | "그날의 기억이 남습니다.\n고통도 함께 남습니다." |
| soldier_colleague_death | "이동현의 마지막 기억이 봉인됩니다.\n함께했던 기억들도 함께 흐릿해집니다." | "이동현을 기억합니다.\n그 순간의 고통도 함께합니다." |
| soldier_war_order | "철수 명령의 기억이 희미해집니다.\n죄책감도 함께 줄어듭니다." | "그 선택의 기억이 남습니다.\n무거움도 함께 남습니다." |
| soldier_guilt_survive | "그 질문이 작아집니다.\n하지만 답을 찾을 기회도 줄어듭니다." | "질문이 남습니다.\n무게도 남습니다." |
| 나머지 5개 | "이 기억이 희미해집니다." | "이 기억이 남습니다." |

---

## DialogueNodeData — 군인

### soldier_colleague_first (처음 만난 날)

**Node_soldier_colleague_first_root**
- nodeId: `soldier_colleague_first_root`
- patientLine: `훈련소에서 처음 봤어요. 이동현이.\n제 옆 침대였거든요.`
- choices[0]: "어떤 사람이었나요?" / tags=[ColleagueRecognition] / next=Node_soldier_colleague_first_A
- choices[1]: "처음부터 친하게 지냈나요?" / tags=[Fact] / next=Node_soldier_colleague_first_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_first_A**
- patientLine: `말이 별로 없었어요. 근데 뭔가 믿음직했어요.\n훈련 중에 제가 쓰러진 적 있거든요.\n물 들고 온 게 걔였어요. 아무 말 없이.`
- choices[0]: "그때 많이 힘들었겠어요." / tags=[Emotion] / next=Node_soldier_colleague_first_AA
- choices[1]: "그게 계기가 됐나요?" / tags=[Interpretation] / next=null (→ 자동 처리 버튼)
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_first_AA**
- patientLine: `네. 근데 이상하게 그 이후로 마음이 놓였어요.\n말 한마디 없이 그냥 물 한 잔인데.`
- choices[0]: "그런 사람이 있다는 게 다행이었겠어요." / tags=[Emotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_first_B**
- patientLine: `아니요. 처음엔 좀 어색했어요.\n근데 어느 순간 자연스러워졌어요.\n그런 친구 있잖아요. 딱히 뭘 한 것도 아닌데.`
- choices[0]: "지금도 그 얼굴이 선명하게 기억나요?" / tags=[Emotion] / next=null
- choices[1]: "같이 뭘 했을 때가 제일 기억에 남으세요?" / tags=[ColleagueRecognition] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_colleague_training (훈련소 시절)

**Node_soldier_colleague_training_root**
- patientLine: `훈련이 진짜 힘들었어요.\n근데 이상하게 그때가 제일 단순했던 것 같아요.`
- choices[0]: "이동현이랑 같이 버텼나요?" / tags=[ColleagueRecognition] / next=Node_soldier_colleague_training_A
- choices[1]: "단순했다는 게 어떤 의미예요?" / tags=[Interpretation] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_training_A**
- patientLine: `걔가 저보다 체력이 좋았어요.\n제가 처질 때마다 뒤에서 밀어줬어요. 진짜로.\n무거운 군장 들고.\n(짧게 웃음) 그 자식.`
- choices[0]: "웃으시네요." / tags=[Emotion] / next=Node_soldier_colleague_training_AA
- choices[1]: "지금 이 기억은 어떻게 느껴지세요?" / tags=[Interpretation] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_training_AA**
- patientLine: `...\n모르겠어요.\n웃겨야 하는 기억인데.`
- choices[0]: "억지로 웃지 않아도 됩니다." / tags=[Respect] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_war_before (파병 전날 밤)

**Node_soldier_war_before_root**
- patientLine: `파병 전날 밤에...\n걔가 담배 피우냐고 물어봤어요.\n안 핀다고 했더니 그냥 옆에 앉았어요.`
- choices[0]: "무슨 얘기를 했나요?" / tags=[Fact] / next=Node_soldier_war_before_A
- choices[1]: "그날 어떤 기분이었어요?" / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_before_A**
- patientLine: `별 얘기 안 했어요.\n그냥 고향 얘기, 가족 얘기.\n걔 어머니가 된장찌개를 잘 끓인다고.\n...\n그게 마지막 대화였어요.`
- choices[0]: "그 말이 지금도 생각나시겠어요." / tags=[Emotion] / next=null
- choices[1]: "억지로 말 안 하셔도 됩니다." / tags=[Respect] / next=Node_soldier_war_before_AB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_before_AB**
- patientLine: `아니요.\n말해야 할 것 같아요.\n그때 저도 이상하게 그런 생각이 들었어요.\n내일 뭔가 달라질 것 같다는.`
- choices[0]: "그 느낌이 맞았던 건가요." / tags=[Interpretation] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_war_combat (전투 장면, 오염)

**Node_soldier_war_combat_root**
- patientLine: `...\n이건 말하기 어렵네요.`
- choices[0]: "천천히 하셔도 됩니다." / tags=[Respect] / next=Node_soldier_war_combat_A
- choices[1]: "어떤 장면이 자꾸 떠오르나요?" / tags=[Fact] / next=Node_soldier_war_combat_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_combat_A**
- patientLine: `...\n폭발음이요. 귀에서 안 떠나요.\n자다가도 들려요.`
- choices[0]: "그 소리가 나면 어떻게 되세요?" / tags=[Emotion] / next=Node_soldier_war_combat_AA
- choices[1]: "언제부터 그랬나요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_combat_AA**
- patientLine: `깨요. 그냥 앉아있어요.\n애가 놀라니까 이제 방에 혼자 자요.`
- choices[0]: "아들 때문에 여기 오신 건가요?" / tags=[KeyEmotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_combat_B**
- patientLine: `(표정이 굳음)\n...\n그건 못 하겠어요.`
- choices[0]: "알겠습니다. 억지로 안 하셔도 돼요." / tags=[Respect] / next=null
- choices[1]: "그 기억이 얼마나 자주 떠오르나요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_colleague_death (이동현의 마지막, 오염/핵심)

**Node_soldier_colleague_death_root**
- patientLine: `...\n이건... 선생님.\n이건 좀 다른 것 같은데요.`
- choices[0]: "안 보셔도 됩니다." / tags=[Respect] / next=Node_soldier_colleague_death_A
- choices[1]: "어떻게 다른 것 같으세요?" / tags=[Emotion] / next=Node_soldier_colleague_death_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_death_A**
- patientLine: `아니요.\n봐야 할 것 같아요.\n안 보면 평생 못 볼 것 같아서.\n이동현이... 제 앞에서.\n(말을 잇지 못함)`
- choices[0]: "말 안 하셔도 알겠습니다." / tags=[Respect] / next=null
- choices[1]: "이동현 씨는 어떤 사람이었나요?" / tags=[ColleagueRecognition] / next=Node_soldier_colleague_death_AB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_death_AB**
- patientLine: `좋은 사람이었어요.\n저보다 훨씬.\n(긴 침묵)\n그게 더 이상한 것 같아요.\n왜 걔가 아니고 저냐는 게.`
- choices[0]: "그 질문은 답이 없을 수도 있어요." / tags=[Respect] / next=null
- choices[1]: "이동현 씨가 뭐라고 할 것 같아요?" / tags=[ColleagueRecognition] / next=Node_soldier_colleague_death_ABB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_death_ABB**
- patientLine: `...\n웃겠죠.\n그 자식은 그런 애였어요.\n짜증나게.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_colleague_death_B**
- patientLine: `다른 기억들은 그냥 힘들어요.\n근데 이건... 숨이 막혀요.\n이걸 보면 며칠 동안 아무것도 못 해요.`
- choices[0]: "이동현 씨가 어떤 사람인지 말해줄 수 있어요?" / tags=[ColleagueRecognition] / next=null
- choices[1]: "그 기억이 지금 박성우 씨한테 뭘 남겼나요?" / tags=[Interpretation] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_war_order (철수 명령, 오염)

**Node_soldier_war_order_root**
- patientLine: `명령이었어요.\n철수 명령.\n...그냥 따랐어요.`
- choices[0]: "그때 다른 선택을 할 수 있었다고 생각하세요?" / tags=[Interpretation] / next=Node_soldier_war_order_A
- choices[1]: "그 순간 어떤 기분이었나요?" / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_order_A**
- patientLine: `...\n없었어요. 없었다고 생각해요.\n근데 왜 이렇게 생각이 나는지.`
- choices[0]: "명령을 따른 게 잘못이 아니에요." / tags=[Respect] / next=null
- choices[1]: "그 생각이 지금 어떤 형태로 남아있나요?" / tags=[Emotion] / next=Node_soldier_war_order_AB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_war_order_AB**
- patientLine: `그냥 있어요.\n뭔가 해야 했는데, 하는 생각.\n뭘 해야 했는지는 모르겠어요.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_family_return (귀환)

**Node_soldier_family_return_root**
- patientLine: `집에 왔을 때 애가 뛰어나왔어요.\n근데 저는 그냥 서 있었어요.\n웃어야 하는데.`
- choices[0]: "그때 어떤 생각을 하셨나요?" / tags=[Emotion] / next=Node_soldier_family_return_A
- choices[1]: "지금은 다른가요?" / tags=[Interpretation] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_family_return_A**
- patientLine: `아무 생각도 안 났어요.\n그냥 여기가 집이구나.\n근데 여기가 맞는지 모르겠다는.`
- choices[0]: "지금도 그런 느낌이 드세요?" / tags=[Emotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_family_son (아들의 얼굴)

**Node_soldier_family_son_root**
- patientLine: `얘가 저한테 그러더라고요.\n아빠 왜 밤에 소리 질러요.\n일곱 살짜리가.`
- choices[0]: "그 말이 어떻게 느껴지셨어요?" / tags=[Emotion] / next=null
- choices[1]: "그래서 여기 오신 건가요?" / tags=[Interpretation] / next=Node_soldier_family_son_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_family_son_B**
- patientLine: `...\n네.\n저 때문에 걔가 힘들면 안 되니까.`
- choices[0]: "아들을 위해서 오신 거군요." / tags=[Emotion] / next=null
- choices[1]: "본인을 위해서도 괜찮아요." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### soldier_guilt_survive (생존자의 죄책감, 오염)

**Node_soldier_guilt_survive_root**
- patientLine: `왜 저만 왔을까요.\n그냥 그 생각이에요.\n이유가 없어요. 그냥.`
- choices[0]: "그 질문에 답을 찾으려 하셨나요?" / tags=[Interpretation] / next=Node_soldier_guilt_survive_A
- choices[1]: "그 생각이 얼마나 자주 드나요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_guilt_survive_A**
- patientLine: `계속요.\n근데 없어요. 답이.\n그게 더 힘든 것 같아요.`
- choices[0]: "답이 없는 게 맞을 수도 있어요." / tags=[Respect] / next=null
- choices[1]: "이동현 씨는 뭐라고 할 것 같아요?" / tags=[ColleagueRecognition] / next=Node_soldier_guilt_survive_AB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_soldier_guilt_survive_AB**
- patientLine: `...\n모르겠어요.\n(긴 침묵)\n그 자식이 뭐라고 할지는.\n생각해본 적 없어요.`
- choices[0]: "한번 생각해봐도 괜찮을 것 같아요." / tags=[Interpretation] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---
---

# 환자 2 — 이봉수 (노인, 알츠하이머)

## PatientData: `Patient_Elder`

| 필드 | 값 |
|------|----|
| patientName | 이봉수 |
| patientType | Elder |
| optionALabel | 남긴다 |
| optionBLabel | 제거한다 |
| gridSize | x=4, y=3 |
| maxPiecesInFrame | 7 |
| memoryImage | [노인 기억 이미지 Texture2D] |
| memoryImageAlt | (사용 안 함, null 가능) |

---

## MemoryPieceData — 노인 12개

| SO 파일명 | pieceId | pieceName | gridPosition |
|-----------|---------|-----------|--------------|
| Piece_elder_daughter_face | elder_daughter_face | 딸의 얼굴 | (0,0) |
| Piece_elder_daughter_wedding | elder_daughter_wedding | 딸의 결혼식 | (1,0) |
| Piece_elder_daughter_young | elder_daughter_young | 어린 딸 | (2,0) |
| Piece_elder_wife_voice | elder_wife_voice | 아내의 목소리 | (3,0) |
| Piece_elder_wife_morning | elder_wife_morning | 아내와의 아침 | (0,1) |
| Piece_elder_career_classroom | elder_career_classroom | 교단에 서던 날 | (1,1) |
| Piece_elder_career_last | elder_career_last | 마지막 수업 | (2,1) |
| Piece_elder_daily_song | elder_daily_song | 좋아하던 노래 | (3,1) |
| Piece_elder_daily_hometown | elder_daily_hometown | 고향 풍경 | (0,2) |
| Piece_elder_memory_smell | elder_memory_smell | 된장찌개 냄새 | (1,2) |
| Piece_elder_memory_laugh | elder_memory_laugh | 웃음 소리 | (2,2) |
| Piece_elder_memory_hands | elder_memory_hands | 아내의 손 감촉 | (3,2) |

> connectedPieceIds: 노인은 모두 비워도 됨 (연결 조각 페이드 없음)

### 처리 미리보기 텍스트 (공통 패턴)

| pieceId | optionAPreview (남긴다) | optionBPreview (제거한다) |
|---------|------------------------|--------------------------|
| elder_daughter_face | "딸의 얼굴이 남습니다." | "딸의 얼굴이 사라집니다." |
| elder_wife_voice | "아내의 목소리가 남습니다." | "아내의 목소리가 사라집니다." |
| elder_career_classroom | "교단에 서던 기억이 남습니다." | "교단에 서던 기억이 사라집니다." |
| 나머지 9개 | "{pieceName}이(가) 남습니다." | "{pieceName}이(가) 사라집니다." |

---

## DialogueNodeData — 노인

### elder_daughter_face (딸의 얼굴)

**Node_elder_daughter_face_root**
- patientLine: `저 사람... 자주 오는 사람인데.\n이름이... 뭐더라.`
- choices[0]: "따님이에요. 이지영 씨요." / tags=[PatientValue] / next=Node_elder_daughter_face_A
- choices[1]: "알아보시겠어요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_daughter_face_A**
- patientLine: `지영이. 맞다, 지영이.\n(잠시 눈을 감았다 뜸)\n지영이가 몇 살이지?`
- choices[0]: "마흔셋이에요." / tags=[Fact] / next=null
- choices[1]: "따님 얼굴을 보면 어떤 기분이에요?" / tags=[Emotion] / next=Node_elder_daughter_face_AB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_daughter_face_AB**
- patientLine: `...\n따뜻해요.\n뭔지 모르겠는데 따뜻해.\n(작게) 알아봐야 하는데.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_daughter_wedding (딸의 결혼식)

**Node_elder_daughter_wedding_root**
- patientLine: `결혼식이...\n지영이 결혼식이었나.\n내가 울었어요, 그날.`
- choices[0]: "왜 우셨어요?" / tags=[Emotion] / next=Node_elder_daughter_wedding_A
- choices[1]: "어떤 날이었는지 기억나세요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_daughter_wedding_A**
- patientLine: `몰라. 그냥 울었어요.\n이쪽에서.\n(가슴을 가리키며)\n여기가 뭔가 꽉 차는 것 같았어요.`
- choices[0]: "좋은 울음이었겠네요." / tags=[Emotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_daughter_young (어린 딸)

**Node_elder_daughter_young_root**
- patientLine: `어렸을 때...\n지영이가 참 작았어요.\n손이 이만했어요.`
- choices[0]: "어떤 아이였나요?" / tags=[Emotion] / next=null
- choices[1]: "그때가 생각나세요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_wife_voice (아내의 목소리)

**Node_elder_wife_voice_root**
- patientLine: `...\n순자 씨 목소리가...\n가끔 들려요.\n들리는 것 같아요.`
- choices[0]: "아내분이 그리우시겠어요." / tags=[Emotion] / next=null
- choices[1]: "어떤 목소리였나요?" / tags=[PatientValue] / next=Node_elder_wife_voice_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_wife_voice_B**
- patientLine: `낮았어요. 근데 또렷했어요.\n내가 실수하면 딱 한마디 해요.\n'그거 아니에요.'\n(웃음)\n그게 또 맞아요, 항상.`
- choices[0]: "지금도 그 목소리가 들리세요?" / tags=[Emotion] / next=Node_elder_wife_voice_BA
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_wife_voice_BA**
- patientLine: `응.\n(잠시 먼 곳을 바라보다가)\n가끔은.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_wife_morning (아내와의 아침)

**Node_elder_wife_morning_root**
- patientLine: `아침에 일어나면...\n밥이 있었어요, 항상.\n내가 일어나기 전에 이미.`
- choices[0]: "아내분이 먼저 일어나셨군요." / tags=[Fact] / next=null
- choices[1]: "그 아침이 지금 어떻게 느껴지세요?" / tags=[Emotion] / next=Node_elder_wife_morning_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_wife_morning_B**
- patientLine: `...\n지금은 혼자 먹어요.\n맛이 없어요.\n(담담하게)\n밥이 잘못된 게 아닌데.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_career_classroom (교단에 서던 날)

**Node_elder_career_classroom_root**
- patientLine: `교사였어요, 나.\n38년.\n오래 했지.`
- choices[0]: "어떤 선생님이셨어요?" / tags=[PatientValue] / next=Node_elder_career_classroom_A
- choices[1]: "학생들이 기억나세요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_career_classroom_A**
- patientLine: `글쎄요.\n(잠깐 생각하다 또렷해짐)\n엄하진 않았어요. 엄한 척은 했지만.\n애들이 다 알아요, 그거.`
- choices[0]: "좋아하셨겠어요, 그 일." / tags=[Emotion] / next=Node_elder_career_classroom_AA
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_career_classroom_AA**
- patientLine: `네.\n그게 나였어요.\n(천천히, 또렷하게)\n선생님 이봉수.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_career_last (마지막 수업)

**Node_elder_career_last_root**
- patientLine: `마지막 수업이...\n언제였더라.\n아이들이 노래를 불러줬어요.`
- choices[0]: "어떤 노래였나요?" / tags=[Fact] / next=null
- choices[1]: "그때 어떤 기분이었어요?" / tags=[Emotion] / next=Node_elder_career_last_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_career_last_B**
- patientLine: `...\n울지 말아야지 했는데.\n또 울었어요.\n(작게 웃음)`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_daily_song (좋아하던 노래)

**Node_elder_daily_song_root**
- patientLine: `노래가...\n뭔가 생각날 것 같은데.\n(흥얼거리다 멈춤)\n잊어버렸네.`
- choices[0]: "어떤 노래였는지 기억나세요?" / tags=[Fact] / next=null
- choices[1]: "괜찮아요, 천천히." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_daily_hometown (고향 풍경)

**Node_elder_daily_hometown_root**
- patientLine: `고향이...\n강원도예요.\n산이 많았어요.`
- choices[0]: "그 풍경이 지금도 생각나세요?" / tags=[Emotion] / next=null
- choices[1]: "고향에 가보고 싶으세요?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_memory_smell (된장찌개 냄새)

**Node_elder_memory_smell_root**
- patientLine: `냄새가 나요.\n된장찌개.\n순자 씨가 끓이던.`
- choices[0]: "그 냄새가 어떻게 느껴지세요?" / tags=[Emotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_memory_laugh (웃음 소리)

**Node_elder_memory_laugh_root**
- patientLine: `웃음 소리가...\n누구 웃음 소리더라.\n(잠시 멈춤)\n지영이 어렸을 때 같아요.`
- choices[0]: "어떤 웃음이었나요?" / tags=[Emotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### elder_memory_hands (아내의 손 감촉)

**Node_elder_memory_hands_root**
- patientLine: `손이...\n차가웠어요.\n항상.\n근데 잡으면 따뜻해졌어요.`
- choices[0]: "아내분 손이요?" / tags=[PatientValue] / next=Node_elder_memory_hands_A
- choices[1]: "그 감촉이 지금도 기억나세요?" / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_elder_memory_hands_A**
- patientLine: `응.\n순자 씨 손.\n(자신의 손을 내려다보며)\n요즘은 혼자라서.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---
---

# 환자 3 — 서윤 (아동, 학대)

## PatientData: `Patient_Child`

| 필드 | 값 |
|------|----|
| patientName | 서윤 |
| patientType | Child |
| optionALabel | 드러낸다 |
| optionBLabel | 그대로 둔다 |
| gridSize | x=3, y=3 |
| maxPiecesInFrame | -1 (제한 없음) |
| memoryImage | [밝은 가족 그림 Texture2D] ← 가짜 기억 |
| memoryImageAlt | [어두운 진실 Texture2D] ← OptionA 시 교체 |

---

## MemoryPieceData — 아동 9개

| SO 파일명 | pieceId | pieceName | gridPosition |
|-----------|---------|-----------|--------------|
| Piece_child_birthday_cake | child_birthday_cake | 생일 케이크 앞 | (0,0) |
| Piece_child_birthday_wish | child_birthday_wish | 생일 소원 | (1,0) |
| Piece_child_gift_unwrap | child_gift_unwrap | 선물 뜯기 | (2,0) |
| Piece_child_dinner_table | child_dinner_table | 저녁 식사 | (0,1) |
| Piece_child_night_sleep | child_night_sleep | 잠자리 | (1,1) |
| Piece_child_school_morning | child_school_morning | 학교 가는 날 | (2,1) |
| Piece_child_parent_laugh | child_parent_laugh | 웃음소리 | (0,2) |
| Piece_child_mom_hand | child_mom_hand | 엄마의 손 | (1,2) |
| Piece_child_dad_face | child_dad_face | 아빠의 얼굴 | (2,2) |

> connectedPieceIds: 아동도 모두 비워도 됨

### 처리 미리보기 텍스트

| pieceId | optionAPreview (드러낸다) | optionBPreview (그대로 둔다) |
|---------|--------------------------|------------------------------|
| child_birthday_cake | "그날 실제로 있었던 일이 드러납니다." | "서윤이가 기억하는 그대로 남겨둡니다." |
| child_mom_hand | "서윤이가 감추고 싶었던 기억이 드러납니다.\n충격이 클 수 있습니다." | "이 기억은 그대로 남겨둡니다." |
| child_dad_face | "서윤이가 감추고 싶었던 기억이 드러납니다.\n충격이 클 수 있습니다." | "이 기억은 그대로 남겨둡니다." |
| 나머지 6개 | "실제로 있었던 일이 드러납니다." | "서윤이가 기억하는 그대로 남겨둡니다." |

---

## DialogueNodeData — 아동

### child_birthday_cake (생일 케이크 앞)

**Node_child_birthday_cake_root**
- patientLine: `생일이었어요! 케이크도 있었고요.\n초가 아홉 개였어요.`
- choices[0]: "누가 있었어?" / tags=[Fact] / next=Node_child_birthday_cake_A
- choices[1]: "케이크 맛이 어땠어?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_birthday_cake_A**
- patientLine: `엄마랑 아빠랑... 다들 있었어요.\n(목소리가 살짝 작아짐)\n...다들요.`
- choices[0]: "기분이 어땠어?" / tags=[Emotion] / next=Node_child_birthday_cake_AA
- choices[1]: "다들이라면 또 누가?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_birthday_cake_AA**
- patientLine: `좋았어요.\n(짧은 침묵)\n...좋았던 것 같아요.`
- choices[0]: "좋았던 것 같다는 게 무슨 말이야?" / tags=[KeyEmotion] / next=Node_child_birthday_cake_AAA
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_birthday_cake_AAA**
- patientLine: `...\n(인형을 꼭 쥐며)\n모르겠어요.`
- choices[0]: "몰라도 괜찮아." / tags=[Respect] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_birthday_wish (생일 소원)

**Node_child_birthday_wish_root**
- patientLine: `소원을 빌었어요.\n눈을 감고요.`
- choices[0]: "무슨 소원을 빌었어?" / tags=[Fact] / next=Node_child_birthday_wish_A
- choices[1]: "눈을 감으면 어떤 기분이었어?" / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_birthday_wish_A**
- patientLine: `...\n(오래 생각하다가)\n기억 안 나요.`
- choices[0]: "정말? 생각해보면?" / tags=[Fact] / next=Node_child_birthday_wish_AA
- choices[1]: "괜찮아, 기억 안 나도." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_birthday_wish_AA**
- patientLine: `(아주 작은 목소리로)\n...집에서 나가게 해달라고.\n(바로 고개를 들며, 빠르게)\n아, 아니에요. 다른 거였어요.`
- choices[0]: "처음에 한 말이 맞는 것 같아." / tags=[KeyEmotion] / next=null
- choices[1]: "괜찮아. 억지로 말 안 해도 돼." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_gift_unwrap (선물 뜯기)

**Node_child_gift_unwrap_root**
- patientLine: `선물을 받았어요.\n예쁜 거였어요.`
- choices[0]: "어떤 선물이었어?" / tags=[Fact] / next=Node_child_gift_unwrap_A
- choices[1]: "선물 받을 때 기분이 어땠어?" / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_gift_unwrap_A**
- patientLine: `인형이요.\n(잠깐 멈춤)\n...예뻤어요.\n(조금 힘없이)\n근데 금방 없어졌어요.`
- choices[0]: "왜 없어졌어?" / tags=[Fact] / next=Node_child_gift_unwrap_AA
- choices[1]: "없어졌을 때 어땠어?" / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_gift_unwrap_AA**
- patientLine: `...\n깨졌어요.\n(빠르게) 제가 실수로요.`
- choices[0]: "실수로?" / tags=[KeyEmotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_dinner_table (저녁 식사)

**Node_child_dinner_table_root**
- patientLine: `저녁은 항상 같이 먹었어요.\n다 같이요.`
- choices[0]: "어떤 음식을 먹었어?" / tags=[Fact] / next=null
- choices[1]: "저녁 먹을 때 분위기가 어땠어?" / tags=[Emotion] / next=Node_child_dinner_table_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_dinner_table_B**
- patientLine: `(잠시 생각하다가)\n...\n조용했어요.`
- choices[0]: "조용한 게 좋았어?" / tags=[Emotion] / next=null
- choices[1]: "항상 조용했어?" / tags=[Fact] / next=Node_child_dinner_table_BB
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_dinner_table_BB**
- patientLine: `...\n숟가락 소리 내면 안 됐어요.\n(아주 조용하게)`
- choices[0]: "왜?" / tags=[KeyEmotion] / next=null
- choices[1]: "힘들었겠다." / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_night_sleep (잠자리)

**Node_child_night_sleep_root**
- patientLine: `잠은 잘 잤어요.\n이불이 따뜻했어요.`
- choices[0]: "잠들기 전에 뭘 했어?" / tags=[Fact] / next=null
- choices[1]: "잠이 잘 왔어?" / tags=[Emotion] / next=Node_child_night_sleep_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_night_sleep_B**
- patientLine: `(잠깐 멈춤)\n...\n가끔은요.`
- choices[0]: "가끔만?" / tags=[KeyEmotion] / next=Node_child_night_sleep_BA
- choices[1]: "잠이 안 올 때는 어떻게 했어?" / tags=[Fact] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_night_sleep_BA**
- patientLine: `(매우 작게)\n소리 안 나게 있었어요.\n그러면 괜찮았어요.`
- choices[0]: "뭔 소리가 날까봐?" / tags=[KeyEmotion] / next=null
- choices[1]: "그랬구나. 많이 힘들었겠다." / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_school_morning (학교 가는 날)

**Node_child_school_morning_root**
- patientLine: `학교 가는 게 좋았어요.\n친구들도 있고요.`
- choices[0]: "학교에서 제일 좋아하는 건 뭐야?" / tags=[Fact] / next=null
- choices[1]: "집이랑 학교 중에 어디가 더 좋아?" / tags=[KeyEmotion] / next=Node_child_school_morning_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_school_morning_B**
- patientLine: `(바로 대답하려다 멈춤)\n...\n학교요.\n(작게)\n학교가 더 좋아요.`
- choices[0]: "왜?" / tags=[KeyEmotion] / next=Node_child_school_morning_BA
- choices[1]: "그렇구나." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_school_morning_BA**
- patientLine: `...\n(대답 없이 인형을 내려다봄)`
- choices[0]: "대답 안 해도 돼." / tags=[Respect] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_parent_laugh (웃음소리)

**Node_child_parent_laugh_root**
- patientLine: `집에서 웃음소리가 났어요.\n가끔이요.`
- choices[0]: "누가 웃었어?" / tags=[Fact] / next=null
- choices[1]: "그 웃음소리가 어땠어?" / tags=[Emotion] / next=Node_child_parent_laugh_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_parent_laugh_B**
- patientLine: `...\n좋을 때도 있었어요.\n무서울 때도 있었어요.`
- choices[0]: "무서울 때는 어떤 때야?" / tags=[KeyEmotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_mom_hand (엄마의 손, 핵심)

**Node_child_mom_hand_root**
- patientLine: `엄마 손이...\n따뜻했어요.\n(조금 빠르게, 연습한 것처럼)`
- choices[0]: "엄마 손을 잡은 기억이 있어?" / tags=[Fact] / next=null
- choices[1]: "엄마 손이 어떤 느낌이었어?" / tags=[Emotion] / next=Node_child_mom_hand_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_mom_hand_B**
- patientLine: `(대답하다 멈춤)\n...\n(손을 내려다봄)\n뜨거울 때도 있었어요.`
- choices[0]: "뜨겁다는 게 어떤 뜻이야?" / tags=[KeyEmotion] / next=Node_child_mom_hand_BA
- choices[1]: "억지로 말 안 해도 돼." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_mom_hand_BA**
- patientLine: `(긴 침묵)\n(아주 작게)\n때렸어요.\n제가 잘못해서요.`
- choices[0]: "서윤이 잘못이 아니야." / tags=[KeyEmotion] / next=Node_child_mom_hand_BAA
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_mom_hand_BAA**
- patientLine: `(고개를 들어 주인공을 봄)\n...\n정말요?`
- choices[0]: "응. 정말이야." / tags=[KeyEmotion] / next=null
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

### child_dad_face (아빠의 얼굴, 핵심)

**Node_child_dad_face_root**
- patientLine: `아빠는...\n일이 많았어요.\n바빴어요.`
- choices[0]: "아빠랑 같이 한 게 있어?" / tags=[Fact] / next=null
- choices[1]: "아빠 얼굴을 보면 어때?" / tags=[Emotion] / next=Node_child_dad_face_B
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_dad_face_B**
- patientLine: `(바로 대답 못 함)\n...\n무서워요.\n(바로) 아니, 무섭진 않아요.`
- choices[0]: "처음에 한 말이 맞는 것 같아." / tags=[KeyEmotion] / next=Node_child_dad_face_BA
- choices[1]: "괜찮아. 무서우면 무섭다고 해도 돼." / tags=[Respect] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_dad_face_BA**
- patientLine: `(인형을 꽉 쥐며)\n...\n화나면... 달라져요.\n다른 사람 같아요.`
- choices[0]: "그때 서윤이는 어떻게 했어?" / tags=[KeyEmotion] / next=Node_child_dad_face_BAA
- choices[1]: "많이 무서웠겠다." / tags=[Emotion] / next=null
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

**Node_child_dad_face_BAA**
- patientLine: `숨었어요.\n작은 데.\n(잠시 후)\n거기 있으면 못 찾아요.\n(아주 작게) 가끔은.`
- choices[0]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[1]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true
- choices[2]: "이 기억은 여기서 마무리하겠습니다." / isProcessTrigger=true

---

# 생성 순서 권장

DialogueNodeData는 리프 노드부터 만들고 역순으로 진행할 것 (참조 먼저 존재해야 nextNode 지정 가능).

1. 리프 노드 (끝 노드들) 먼저 생성
2. 중간 노드 생성 → 리프 노드 참조
3. root 노드 생성 → 중간 노드 참조
4. MemoryPieceData 생성 → rootNode 참조
5. PatientData 생성 → pieces[] 참조
