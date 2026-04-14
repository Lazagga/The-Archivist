# The Archivist — ScriptableObject 생성 가이드

---

## 목차
1. [시작 전 준비](#1-시작-전-준비)
2. [공통 규칙](#2-공통-규칙)
3. [EpilogueDatabase](#3-epiloguedatabase)
4. [환자 1 — 박성우 (군인)](#4-환자-1--박성우-군인)
5. [환자 2 — 이봉수 (노인)](#5-환자-2--이봉수-노인)
6. [환자 3 — 서윤 (아동)](#6-환자-3--서윤-아동)
7. [GameManager 최종 연결](#7-gamemanager-최종-연결)
8. [작업 체크리스트](#8-작업-체크리스트)

---

## 1. 시작 전 준비

### 폴더 구조 생성
Project 창에서 아래 폴더들을 미리 생성한다.

```
Assets/
└── ScriptableObjects/
    ├── Patients/          ← PatientData 3개
    ├── Epilogue/          ← EpilogueDatabase 1개
    ├── Pieces/
    │   ├── Soldier/       ← MemoryPieceData 9개
    │   ├── Elder/         ← MemoryPieceData 12개
    │   └── Child/         ← MemoryPieceData 9개
    └── Dialogue/
        ├── Soldier/
        │   ├── colleague_first/
        │   ├── colleague_training/
        │   ├── war_before/
        │   ├── war_combat/
        │   ├── colleague_death/
        │   ├── war_order/
        │   ├── family_return/
        │   ├── family_son/
        │   └── guilt_survive/
        ├── Elder/
        │   ├── daughter_face/
        │   ├── daughter_wedding/
        │   ├── daughter_young/
        │   ├── wife_voice/
        │   ├── wife_morning/
        │   ├── career_classroom/
        │   ├── career_last/
        │   ├── daily_song/
        │   ├── daily_hometown/
        │   ├── memory_smell/
        │   ├── memory_laugh/
        │   └── memory_hands/
        └── Child/
            ├── birthday_cake/
            ├── birthday_wish/
            ├── gift_unwrap/
            ├── dinner_table/
            ├── night_sleep/
            ├── school_morning/
            ├── parent_laugh/
            ├── mom_hand/
            └── dad_face/
```

### SO 생성 방법
- **DialogueNodeData**: 해당 폴더에서 우클릭 → Create → TheArchivist → DialogueNode
- **MemoryPieceData**: 해당 폴더에서 우클릭 → Create → TheArchivist → MemoryPieceData
- **PatientData**: Patients/ 폴더에서 우클릭 → Create → TheArchivist → PatientData
- **EpilogueDatabase**: Epilogue/ 폴더에서 우클릭 → Create → TheArchivist → EpilogueDatabase

생성 후 파일명을 아래 가이드에 맞게 변경할 것.

---

## 2. 공통 규칙

### 선택지 표기 약어
가이드 전체에서 아래 약어를 사용한다.

| 약어 | 의미 |
|------|------|
| **▶마무리** | choiceText = "이 기억은 여기서 마무리하겠습니다." / tags = 없음 / isProcessTrigger = ☑ / nextNode = 없음 |
| **없음** | null (아무것도 연결하지 않음) |

### DialogueTag 인코딩 표
| 인스펙터 표시 | 의미 |
|---|---|
| None | 태그 없음 |
| Fact | 사실 질문 |
| Emotion | 감정 질문 |
| Interpretation | 해석 질문 |
| Respect | 존중 (거부 시 멈춤) |
| ColleagueRecognition | 동료를 인간으로 인식 (군인 전용) |
| KeyEmotion | 아동의 감정 파악 (아동 전용) |
| PatientValue | 환자의 소중한 것 파악 (노인 전용) |

### DialogueNodeData 구조
각 노드 에셋의 인스펙터 필드:
- **nodeId**: 파일명과 동일하게 입력
- **patientLine**: 환자 대사 (인스펙터 TextArea에 직접 입력)
- **choices[0~2]**: 선택지 3개 (항상 3개 모두 채울 것)
  - choiceText
  - tags (배열, 여러 개 가능)
  - isProcessTrigger
  - nextNode (다음 DialogueNodeData 에셋 드래그)

---

## 3. EpilogueDatabase

**파일명**: `EpilogueDatabase.asset`
**위치**: `Assets/ScriptableObjects/Epilogue/`

### 3-1. introLines (게임 인트로, 8줄)

| # | speakerName | line |
|---|---|---|
| 0 | (없음) | 오늘도 같은 시간에 눈이 떠졌다. |
| 1 | (없음) | 클리닉 문을 열면 늘 같은 냄새가 난다. 소독제, 종이, 그리고 오래된 나무 냄새. |
| 2 | 접수원 | 선생님, 오늘 예약 세 분이에요. 첫 분은 10분 후에 들어오실 거예요. |
| 3 | 나 | 알겠어요. |
| 4 | (없음) | 잠깐의 정적. |
| 5 | (없음) | 서류를 펼친다. 이름, 나이, 진단명. |
| 6 | (없음) | 종이 위의 글자들은 아직 사람이 아니다. 문이 열리면 그때부터 사람이 된다. |
| 7 | (없음) | 첫 번째. 박성우, 39세. |

> backgroundSprite, characterSprite는 모든 VNLine에서 추후 이미지 완성 시 연결. 지금은 비워둔다.

### 3-2. entries (에필로그 18종)

entries 배열 크기를 **18**로 설정.
모든 entry의 VNLine: speakerName=(없음), backgroundSprite/characterSprite=(추후 연결).

#### 군인 엔딩 (5종)

**entries[0]** `endingId: soldier_retrauma`
| # | line |
|---|---|
| 0 | 박성우 씨는 치료를 중단했다. |
| 1 | 그 뒤로 연락이 없다. |
| 2 | 어떤 기억은 들여다볼 준비가 되기 전에 건드리면 안 된다. |

**entries[1]** `endingId: soldier_embrace_colleague`
| # | line |
|---|---|
| 0 | 박성우 씨는 이제 이동현의 사진을 지갑에 넣고 다닌다. |
| 1 | 힘든 날엔 꺼내본다고 했다. |
| 2 | 잊지 않는 것과 묶여 있는 것은 다르다는 걸, 이제는 조금 안다고 했다. |

**entries[2]** `endingId: soldier_release_colleague`
| # | line |
|---|---|
| 0 | 박성우 씨는 이동현의 기일에 혼자 술 한 잔을 따른다. |
| 1 | 말은 하지 않는다. 그냥 앉아 있다가 간다. |
| 2 | 그것으로 충분하다고 했다. |

**entries[3]** `endingId: soldier_embrace_no_colleague`
| # | line |
|---|---|
| 0 | 박성우 씨는 이제 밤에 잠을 잔다. |
| 1 | 꿈을 꾸기도 한다고 했다. 나쁘지 않은 꿈도 있다고. |
| 2 | 과거를 전부 꺼내지 않아도 앞으로 나아갈 수 있다는 걸 배웠다고 했다. |

**entries[4]** `endingId: soldier_release_no_colleague`
| # | line |
|---|---|
| 0 | 박성우 씨는 여전히 그 시절을 말하지 않는다. |
| 1 | 하지만 더 이상 도움을 거부하지도 않는다. |
| 2 | 아주 천천히, 자기만의 속도로 가고 있는 것 같다. |

#### 노인 엔딩 (5종)

**entries[5]** `endingId: elder_arbitrary`
| # | line |
|---|---|
| 0 | 이봉수 씨는 지금도 가끔 헷갈린다고 한다. |
| 1 | 무엇을 남기고 싶었는지, 스스로도 잘 모르는 것 같다. |
| 2 | 어쩌면 그 물음 자체가 너무 늦게 시작된 것인지도 모른다. |

**entries[6]** `endingId: elder_kept_daughter`
| # | line |
|---|---|
| 0 | 이봉수 씨는 많은 것을 잊어가고 있다. |
| 1 | 하지만 딸의 이름만은 마지막까지 잊지 않았다. |
| 2 | 이지영 씨가 면회를 올 때마다, 그의 눈이 조금 밝아졌다. |

**entries[7]** `endingId: elder_kept_wife`
| # | line |
|---|---|
| 0 | 이봉수 씨는 많은 것을 잊어가고 있다. |
| 1 | 잊어버리는 날에도 아내의 이름만은 입에서 떠나지 않았다. |
| 2 | 가장 오래 남는 것이 무엇인지, 그는 몸으로 알고 있는 것 같았다. |

**entries[8]** `endingId: elder_kept_career`
| # | line |
|---|---|
| 0 | 이봉수 씨는 병실에서도 종종 손을 움직인다. |
| 1 | 칠판에 글씨를 쓰듯, 무언가를 가르치듯. |
| 2 | 38년의 기억이 손끝에 남아 있는 것 같았다. |

**entries[9]** `endingId: elder_kept_misc`
| # | line |
|---|---|
| 0 | 이봉수 씨가 무엇을 남겼는지, 우리는 다 알지 못한다. |
| 1 | 하지만 그것이 그에게 소중했다는 것만은 분명했다. |
| 2 | 기억의 무게는 언제나 당사자만이 안다. |

#### 아동 엔딩 (5종)

**entries[10]** `endingId: child_shock`
| # | line |
|---|---|
| 0 | 서윤이는 그 뒤로 한동안 말을 하지 않았다. |
| 1 | 너무 빨리 들여다봤는지도 모른다. |
| 2 | 어떤 것들은 꺼내기 전에 먼저 안전한 곳이 필요하다. |

**entries[11]** `endingId: child_overcome`
| # | line |
|---|---|
| 0 | 서윤이는 지금 새로운 곳에서 지내고 있다. |
| 1 | 가끔 웃는다고 했다. 억지로 웃는 게 아니라고. |
| 2 | 아이가 웃는 이유를 스스로 알고 있다는 것, 그것으로 충분하다. |

**entries[12]** `endingId: child_revealed_alone`
| # | line |
|---|---|
| 0 | 서윤이의 비밀은 드러났다. |
| 1 | 하지만 그것을 받아줄 사람이 곁에 없었다. |
| 2 | 그 아이가 지금도 마음에 걸린다. |

**entries[13]** `endingId: child_kept_supported`
| # | line |
|---|---|
| 0 | 서윤이는 아직 많은 것을 말하지 못한다. |
| 1 | 하지만 숨기지 않아도 된다는 것을, 조금씩 배우고 있다. |
| 2 | 그것만으로도 지금은 충분하다. |

**entries[14]** `endingId: child_unchanged`
| # | line |
|---|---|
| 0 | 서윤이는 변하지 않았다. |
| 1 | 여전히 밝은 척한다. |
| 2 | 우리가 더 할 수 있었을까. |

#### 주인공 엔딩 (3종)

**entries[15]** `endingId: protagonist_exhausted`
| # | line |
|---|---|
| 0 | 세 사람의 기억이 내 안에 남아 있다. |
| 1 | 때로는 그 무게가 내 것인지 그들의 것인지 헷갈린다. |
| 2 | 아마도 이 일을 오래 하다 보면 그 경계가 흐릿해지는 것 같다. |
| 3 | 그래도 내일 또 진료실 문을 열 것이다. |

**entries[16]** `endingId: protagonist_efficient`
| # | line |
|---|---|
| 0 | 나는 잘 처리했다. |
| 1 | 세 사람 모두, 최선의 방식으로. |
| 2 | 그런데 집에 돌아오는 길이 이상하게 조용했다. |
| 3 | 무언가를 빠뜨린 것 같은 느낌이, 계속 남는다. |

**entries[17]** `endingId: protagonist_balanced`
| # | line |
|---|---|
| 0 | 세 사람을 만났다. |
| 1 | 모두 달랐고, 나도 조금 달라진 것 같다. |
| 2 | 다음 환자가 오면 또 처음부터 시작해야겠지. |
| 3 | 그게 이 일이다. |

---

## 4. 환자 1 — 박성우 (군인)

### 4-A. DialogueNodeData 생성
**위치**: `Assets/ScriptableObjects/Dialogue/Soldier/{조각폴더}/`

> **표 읽는 법**
> - nextNode 열 `→ A` = 같은 폴더 내 `A.asset` 드래그 연결
> - Trigger ☑ = isProcessTrigger 체크
> - ▶마무리 = choiceText "이 기억은 여기서 마무리하겠습니다." / Trigger ☑ / nextNode 없음

---

#### 📁 colleague_first/

**`root.asset`** — nodeId: `soldier_colleague_first_root`
> 박성우: "훈련소에서 처음 봤어요. 이동현이. 제 옆 침대였거든요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 사람이었나요?" | ColleagueRecognition | ☐ | → A |
| 1 | "처음부터 친하게 지냈나요?" | Fact | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_colleague_first_A`
> 박성우: "말이 별로 없었어요. 근데 뭔가 믿음직했어요. 훈련 중에 제가 쓰러진 적 있거든요. 물 들고 온 게 걔였어요. 아무 말 없이."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그때 많이 힘들었겠어요." | Emotion | ☐ | → AA |
| 1 | "그게 계기가 됐나요?" | Interpretation | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `soldier_colleague_first_AA`
> 박성우: "네. 근데 이상하게 그 이후로 마음이 놓였어요. 말 한마디 없이 그냥 물 한 잔인데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그런 사람이 있다는 게 다행이었겠어요." | Emotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `soldier_colleague_first_B`
> 박성우: "아니요. 처음엔 좀 어색했어요. 근데 어느 순간 자연스러워졌어요. 그런 친구 있잖아요. 딱히 뭘 한 것도 아닌데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "지금도 그 얼굴이 선명하게 기억나요?" | Emotion | ☐ | 없음 |
| 1 | "같이 뭘 했을 때가 제일 기억에 남으세요?" | ColleagueRecognition | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 colleague_training/

**`root.asset`** — nodeId: `soldier_colleague_training_root`
> 박성우: "훈련이 진짜 힘들었어요. 근데 이상하게 그때가 제일 단순했던 것 같아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "이동현이랑 같이 버텼나요?" | ColleagueRecognition | ☐ | → A |
| 1 | "단순했다는 게 어떤 의미예요?" | Interpretation | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_colleague_training_A`
> 박성우: "걔가 저보다 체력이 좋았어요. 제가 처질 때마다 뒤에서 밀어줬어요. 진짜로. 무거운 군장 들고. (짧게 웃음) 그 자식."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "웃으시네요." | Emotion | ☐ | → AA |
| 1 | "지금 이 기억은 어떻게 느껴지세요?" | Interpretation | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `soldier_colleague_training_AA`
> 박성우: "... 모르겠어요. 웃겨야 하는 기억인데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "억지로 웃지 않아도 됩니다." | Respect | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 war_before/

**`root.asset`** — nodeId: `soldier_war_before_root`
> 박성우: "파병 전날 밤에... 걔가 담배 피우냐고 물어봤어요. 안 핀다고 했더니 그냥 옆에 앉았어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "무슨 얘기를 했나요?" | Fact | ☐ | → A |
| 1 | "그날 어떤 기분이었어요?" | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_war_before_A`
> 박성우: "별 얘기 안 했어요. 그냥 고향 얘기, 가족 얘기. 걔 어머니가 된장찌개를 잘 끓인다고. ... 그게 마지막 대화였어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 말이 지금도 생각나시겠어요." | Emotion | ☐ | 없음 |
| 1 | "억지로 말 안 하셔도 됩니다." | Respect | ☐ | → AB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AB.asset`** — nodeId: `soldier_war_before_AB`
> 박성우: "아니요. 말해야 할 것 같아요. 그때 저도 이상하게 그런 생각이 들었어요. 내일 뭔가 달라질 것 같다는."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 느낌이 맞았던 건가요." | Interpretation | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 war_combat/

**`root.asset`** — nodeId: `soldier_war_combat_root`
> 박성우: "... 이건 말하기 어렵네요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "천천히 하셔도 됩니다." | Respect | ☐ | → A |
| 1 | "어떤 장면이 자꾸 떠오르나요?" | Fact | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_war_combat_A`
> 박성우: "... 폭발음이요. 귀에서 안 떠나요. 자다가도 들려요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 소리가 나면 어떻게 되세요?" | Emotion | ☐ | → AA |
| 1 | "언제부터 그랬나요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `soldier_war_combat_AA`
> 박성우: "깨요. 그냥 앉아있어요. 애가 놀라니까 이제 방에 혼자 자요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "아들 때문에 여기 오신 건가요?" | KeyEmotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `soldier_war_combat_B`
> 박성우: (표정이 굳음) "... 그건 못 하겠어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "알겠습니다. 억지로 안 하셔도 돼요." | Respect | ☐ | 없음 |
| 1 | "그 기억이 얼마나 자주 떠오르나요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 colleague_death/

**`root.asset`** — nodeId: `soldier_colleague_death_root`
> 박성우: "... 이건... 선생님. 이건 좀 다른 것 같은데요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "안 보셔도 됩니다." | Respect | ☐ | → A |
| 1 | "어떻게 다른 것 같으세요?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_colleague_death_A`
> 박성우: "아니요. 봐야 할 것 같아요. 안 보면 평생 못 볼 것 같아서. 이동현이... 제 앞에서." (말을 잇지 못함)

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "말 안 하셔도 알겠습니다." | Respect | ☐ | 없음 |
| 1 | "이동현 씨는 어떤 사람이었나요?" | ColleagueRecognition | ☐ | → AB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AB.asset`** — nodeId: `soldier_colleague_death_AB`
> 박성우: "좋은 사람이었어요. 저보다 훨씬. (긴 침묵) 그게 더 이상한 것 같아요. 왜 걔가 아니고 저냐는 게."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 질문은 답이 없을 수도 있어요." | Respect | ☐ | 없음 |
| 1 | "이동현 씨가 뭐라고 할 것 같아요?" | ColleagueRecognition | ☐ | → ABB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`ABB.asset`** — nodeId: `soldier_colleague_death_ABB`
> 박성우: "... 웃겠죠. 그 자식은 그런 애였어요. 짜증나게."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `soldier_colleague_death_B`
> 박성우: "다른 기억들은 그냥 힘들어요. 근데 이건... 숨이 막혀요. 이걸 보면 며칠 동안 아무것도 못 해요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "이동현 씨가 어떤 사람인지 말해줄 수 있어요?" | ColleagueRecognition | ☐ | 없음 |
| 1 | "그 기억이 지금 박성우 씨한테 뭘 남겼나요?" | Interpretation | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 war_order/

**`root.asset`** — nodeId: `soldier_war_order_root`
> 박성우: "명령이었어요. 철수 명령. ...그냥 따랐어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그때 다른 선택을 할 수 있었다고 생각하세요?" | Interpretation | ☐ | → A |
| 1 | "그 순간 어떤 기분이었나요?" | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_war_order_A`
> 박성우: "... 없었어요. 없었다고 생각해요. 근데 왜 이렇게 생각이 나는지."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "명령을 따른 게 잘못이 아니에요." | Respect | ☐ | 없음 |
| 1 | "그 생각이 지금 어떤 형태로 남아있나요?" | Emotion | ☐ | → AB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AB.asset`** — nodeId: `soldier_war_order_AB`
> 박성우: "그냥 있어요. 뭔가 해야 했는데, 하는 생각. 뭘 해야 했는지는 모르겠어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 family_return/

**`root.asset`** — nodeId: `soldier_family_return_root`
> 박성우: "집에 왔을 때 애가 뛰어나왔어요. 근데 저는 그냥 서 있었어요. 웃어야 하는데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그때 어떤 생각을 하셨나요?" | Emotion | ☐ | → A |
| 1 | "지금은 다른가요?" | Interpretation | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_family_return_A`
> 박성우: "아무 생각도 안 났어요. 그냥 여기가 집이구나. 근데 여기가 맞는지 모르겠다는."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "지금도 그런 느낌이 드세요?" | Emotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 family_son/

**`root.asset`** — nodeId: `soldier_family_son_root`
> 박성우: "얘가 저한테 그러더라고요. 아빠 왜 밤에 소리 질러요. 일곱 살짜리가."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 말이 어떻게 느껴지셨어요?" | Emotion | ☐ | 없음 |
| 1 | "그래서 여기 오신 건가요?" | Interpretation | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `soldier_family_son_B`
> 박성우: "... 네. 저 때문에 걔가 힘들면 안 되니까."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "아들을 위해서 오신 거군요." | Emotion | ☐ | 없음 |
| 1 | "본인을 위해서도 괜찮아요." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 guilt_survive/

**`root.asset`** — nodeId: `soldier_guilt_survive_root`
> 박성우: "왜 저만 왔을까요. 그냥 그 생각이에요. 이유가 없어요. 그냥."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 질문에 답을 찾으려 하셨나요?" | Interpretation | ☐ | → A |
| 1 | "그 생각이 얼마나 자주 드나요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `soldier_guilt_survive_A`
> 박성우: "계속요. 근데 없어요. 답이. 그게 더 힘든 것 같아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "답이 없는 게 맞을 수도 있어요." | Respect | ☐ | 없음 |
| 1 | "이동현 씨는 뭐라고 할 것 같아요?" | ColleagueRecognition | ☐ | → AB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AB.asset`** — nodeId: `soldier_guilt_survive_AB`
> 박성우: "... 모르겠어요. (긴 침묵) 그 자식이 뭐라고 할지는. 생각해본 적 없어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "한번 생각해봐도 괜찮을 것 같아요." | Interpretation | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

### 4-B. MemoryPieceData 생성
**위치**: `Assets/ScriptableObjects/Pieces/Soldier/`

| 파일명 | pieceId | pieceName | gridPosition | connectedPieceIds | rootNode |
|---|---|---|---|---|---|
| `soldier_colleague_first.asset` | soldier_colleague_first | 처음 만난 날 | (0, 0) | (없음) | colleague_first/root.asset |
| `soldier_colleague_training.asset` | soldier_colleague_training | 훈련소 시절 | (1, 0) | (없음) | colleague_training/root.asset |
| `soldier_war_before.asset` | soldier_war_before | 파병 전날 밤 | (2, 0) | (없음) | war_before/root.asset |
| `soldier_war_combat.asset` | soldier_war_combat | 전투 장면 | (0, 1) | soldier_colleague_death | war_combat/root.asset |
| `soldier_colleague_death.asset` | soldier_colleague_death | 이동현의 마지막 | (1, 1) | soldier_colleague_first, soldier_colleague_training, soldier_war_before | colleague_death/root.asset |
| `soldier_war_order.asset` | soldier_war_order | 철수 명령 | (2, 1) | soldier_guilt_survive | war_order/root.asset |
| `soldier_family_return.asset` | soldier_family_return | 귀환 | (0, 2) | (없음) | family_return/root.asset |
| `soldier_family_son.asset` | soldier_family_son | 아들의 얼굴 | (1, 2) | (없음) | family_son/root.asset |
| `soldier_guilt_survive.asset` | soldier_guilt_survive | 생존자의 죄책감 | (2, 2) | (없음) | guilt_survive/root.asset |

**처리 미리보기 텍스트**

| pieceId | optionAPreview | optionBPreview |
|---|---|---|
| soldier_war_combat | 전투의 기억이 희미해집니다.\n이동현의 마지막 기억도 흐릿해집니다. | 그날의 기억이 남습니다.\n고통도 함께 남습니다. |
| soldier_colleague_death | 이동현의 마지막 기억이 봉인됩니다.\n함께했던 기억들도 함께 흐릿해집니다. | 이동현을 기억합니다.\n그 순간의 고통도 함께합니다. |
| soldier_war_order | 철수 명령의 기억이 희미해집니다.\n죄책감도 함께 줄어듭니다. | 그 선택의 기억이 남습니다.\n무거움도 함께 남습니다. |
| soldier_guilt_survive | 그 질문이 작아집니다.\n하지만 답을 찾을 기회도 줄어듭니다. | 질문이 남습니다.\n무게도 남습니다. |
| 나머지 5개 조각 | 이 기억이 희미해집니다. | 이 기억이 남습니다. |

---

### 4-C. PatientData 생성
**파일명**: `Soldier_PatientData.asset` / **위치**: `Assets/ScriptableObjects/Patients/`

| 필드 | 값 |
|---|---|
| patientName | 박성우 |
| patientType | Soldier |
| optionALabel | 덜어낸다 |
| optionBLabel | 끌어안는다 |
| gridSize | (3, 3) |
| memoryImage | (군인 기억 이미지 Texture2D — 추후 연결) |
| memoryImageAlt | (없음) |
| maxPiecesInFrame | -1 |
| puzzleBackground | (추후 연결) |
| pieces (9개) | 위 9개 MemoryPieceData 드래그 (순서 무관) |

**introLines (8줄)**

| # | speakerName | line |
|---|---|---|
| 0 | 주인공 | 예약은 없으셨던 것 같은데요. |
| 1 | 박성우 | 아는 사람한테 여기 얘기 들었어요. 그냥... 지나가다 들어왔습니다. |
| 2 | 주인공 | 괜찮아요. 앉으세요. 어디서 오셨어요? |
| 3 | 박성우 | 군인이었어요. 전역한 지 한 6년 됐나. |
| 4 | 주인공 | 요즘 어떠세요? |
| 5 | 박성우 | 잠을 잘 못 잡니다. ...그것 말고는 괜찮아요. |
| 6 | 주인공 | 꿈을 꾸세요? |
| 7 | 박성우 | (긴 침묵) 꿈인지 기억인지 모르겠어요. |

**outroLines (4줄)**

| # | speakerName | line |
|---|---|---|
| 0 | 박성우 | 다 됐어요? |
| 1 | 주인공 | 네. 어떠세요? |
| 2 | 박성우 | (잠시 생각하다) 모르겠어요. 아직은. |
| 3 | 주인공 | 그래도 됩니다. |

---

## 5. 환자 2 — 이봉수 (노인)

### 5-A. DialogueNodeData 생성
**위치**: `Assets/ScriptableObjects/Dialogue/Elder/{조각폴더}/`

---

#### 📁 daughter_face/

**`root.asset`** — nodeId: `elder_daughter_face_root`
> 이봉수: "저 사람... 자주 오는 사람인데. 이름이... 뭐더라."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "따님이에요. 이지영 씨요." | PatientValue | ☐ | → A |
| 1 | "알아보시겠어요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `elder_daughter_face_A`
> 이봉수: "지영이. 맞다, 지영이." (잠시 눈을 감았다 뜸) "지영이가 몇 살이지?"

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "마흔셋이에요." | Fact | ☐ | 없음 |
| 1 | "따님 얼굴을 보면 어떤 기분이에요?" | Emotion | ☐ | → AB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AB.asset`** — nodeId: `elder_daughter_face_AB`
> 이봉수: "... 따뜻해요. 뭔지 모르겠는데 따뜻해." (작게) "알아봐야 하는데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 daughter_wedding/

**`root.asset`** — nodeId: `elder_daughter_wedding_root`
> 이봉수: "결혼식이... 지영이 결혼식이었나. 내가 울었어요, 그날."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "왜 우셨어요?" | Emotion | ☐ | → A |
| 1 | "어떤 날이었는지 기억나세요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `elder_daughter_wedding_A`
> 이봉수: "몰라. 그냥 울었어요. 이쪽에서." (가슴을 가리키며) "여기가 뭔가 꽉 차는 것 같았어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "좋은 울음이었겠네요." | Emotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 daughter_young/

> ⚠️ **스토리 미작성 조각**: `elder_daughter_young` 대화 트리가 스토리 문서에 없습니다.
> 아래는 임시 최소 구성입니다. 스토리 확정 후 교체하세요.

**`root.asset`** — nodeId: `elder_daughter_young_root`
> 이봉수: "어렸을 때... 지영이가... 작았는데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어린 지영 씨가 어떤 아이였나요?" | Emotion | ☐ | 없음 |
| 1 | "그때 기억이 선명하게 나세요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 wife_voice/

**`root.asset`** — nodeId: `elder_wife_voice_root`
> 이봉수: "... 순자 씨 목소리가... 가끔 들려요. 들리는 것 같아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "아내분이 그리우시겠어요." | Emotion | ☐ | 없음 |
| 1 | "어떤 목소리였나요?" | PatientValue | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `elder_wife_voice_B`
> 이봉수: "낮았어요. 근데 또렷했어요. 내가 실수하면 딱 한마디 해요. '그거 아니에요.' (웃음) 그게 또 맞아요, 항상."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "지금도 그 목소리가 들리세요?" | Emotion | ☐ | → BA |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BA.asset`** — nodeId: `elder_wife_voice_BA`
> 이봉수: "응." (잠시 먼 곳을 바라보다가) "가끔은."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 wife_morning/

**`root.asset`** — nodeId: `elder_wife_morning_root`
> 이봉수: "아침에 일어나면... 밥이 있었어요, 항상. 내가 일어나기 전에 이미."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "아내분이 먼저 일어나셨군요." | Fact | ☐ | 없음 |
| 1 | "그 아침이 지금 어떻게 느껴지세요?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `elder_wife_morning_B`
> 이봉수: "... 지금은 혼자 먹어요. 맛이 없어요." (담담하게) "밥이 잘못된 게 아닌데."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 career_classroom/

**`root.asset`** — nodeId: `elder_career_classroom_root`
> 이봉수: "교사였어요, 나. 38년. 오래 했지."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 선생님이셨어요?" | PatientValue | ☐ | → A |
| 1 | "학생들이 기억나세요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `elder_career_classroom_A`
> 이봉수: "글쎄요." (잠깐 생각하다 또렷해짐) "엄하진 않았어요. 엄한 척은 했지만. 애들이 다 알아요, 그거."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "좋아하셨겠어요, 그 일." | Emotion | ☐ | → AA |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `elder_career_classroom_AA`
> 이봉수: "네. 그게 나였어요." (천천히, 또렷하게) "선생님 이봉수."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 career_last/

**`root.asset`** — nodeId: `elder_career_last_root`
> 이봉수: "마지막 수업이... 언제였더라. 아이들이 노래를 불러줬어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 노래였나요?" | Fact | ☐ | 없음 |
| 1 | "그때 어떤 기분이었어요?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `elder_career_last_B`
> 이봉수: "... 울지 말아야지 했는데. 또 울었어요." (작게 웃음)

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 daily_song/

**`root.asset`** — nodeId: `elder_daily_song_root`
> 이봉수: "노래가... 뭔가 생각날 것 같은데." (흥얼거리다 멈춤) "잊어버렸네."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 노래였는지 기억나세요?" | Fact | ☐ | 없음 |
| 1 | "괜찮아요, 천천히." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 daily_hometown/

**`root.asset`** — nodeId: `elder_daily_hometown_root`
> 이봉수: "고향이... 강원도예요. 산이 많았어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 풍경이 지금도 생각나세요?" | Emotion | ☐ | 없음 |
| 1 | "고향에 가보고 싶으세요?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 memory_smell/

**`root.asset`** — nodeId: `elder_memory_smell_root`
> 이봉수: "냄새가 나요. 된장찌개. 순자 씨가 끓이던."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그 냄새가 어떻게 느껴지세요?" | Emotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 memory_laugh/

**`root.asset`** — nodeId: `elder_memory_laugh_root`
> 이봉수: "웃음 소리가... 누구 웃음 소리더라." (잠시 멈춤) "지영이 어렸을 때 같아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 웃음이었나요?" | Emotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 memory_hands/

**`root.asset`** — nodeId: `elder_memory_hands_root`
> 이봉수: "손이... 차가웠어요. 항상. 근데 잡으면 따뜻해졌어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "아내분 손이요?" | PatientValue | ☐ | → A |
| 1 | "그 감촉이 지금도 기억나세요?" | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `elder_memory_hands_A`
> 이봉수: "응. 순자 씨 손." (자신의 손을 내려다보며) "요즘은 혼자라서."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

### 5-B. MemoryPieceData 생성
**위치**: `Assets/ScriptableObjects/Pieces/Elder/`

| 파일명 | pieceId | pieceName | gridPosition | rootNode |
|---|---|---|---|---|
| `elder_daughter_face.asset` | elder_daughter_face | 딸의 얼굴 | (0, 0) | daughter_face/root.asset |
| `elder_daughter_wedding.asset` | elder_daughter_wedding | 딸의 결혼식 | (1, 0) | daughter_wedding/root.asset |
| `elder_daughter_young.asset` | elder_daughter_young | 어린 딸 | (2, 0) | daughter_young/root.asset |
| `elder_wife_voice.asset` | elder_wife_voice | 아내의 목소리 | (3, 0) | wife_voice/root.asset |
| `elder_wife_morning.asset` | elder_wife_morning | 아내와의 아침 | (0, 1) | wife_morning/root.asset |
| `elder_career_classroom.asset` | elder_career_classroom | 교단에 서던 날 | (1, 1) | career_classroom/root.asset |
| `elder_career_last.asset` | elder_career_last | 마지막 수업 | (2, 1) | career_last/root.asset |
| `elder_daily_song.asset` | elder_daily_song | 좋아하던 노래 | (3, 1) | daily_song/root.asset |
| `elder_daily_hometown.asset` | elder_daily_hometown | 고향 풍경 | (0, 2) | daily_hometown/root.asset |
| `elder_memory_smell.asset` | elder_memory_smell | 된장찌개 냄새 | (1, 2) | memory_smell/root.asset |
| `elder_memory_laugh.asset` | elder_memory_laugh | 웃음 소리 | (2, 2) | memory_laugh/root.asset |
| `elder_memory_hands.asset` | elder_memory_hands | 아내의 손 감촉 | (3, 2) | memory_hands/root.asset |

> connectedPieceIds: 모든 노인 조각 **(없음)**

**처리 미리보기 텍스트**

| pieceId | optionAPreview | optionBPreview |
|---|---|---|
| elder_daughter_face | 딸의 얼굴이 남습니다. | 딸의 얼굴이 사라집니다. |
| elder_daughter_wedding | 딸의 결혼식 기억이 남습니다. | 딸의 결혼식 기억이 사라집니다. |
| elder_daughter_young | 어린 딸의 기억이 남습니다. | 어린 딸의 기억이 사라집니다. |
| elder_wife_voice | 아내의 목소리가 남습니다. | 아내의 목소리가 사라집니다. |
| elder_wife_morning | 아내와의 아침 기억이 남습니다. | 아내와의 아침 기억이 사라집니다. |
| elder_career_classroom | 교단에 서던 기억이 남습니다. | 교단에 서던 기억이 사라집니다. |
| elder_career_last | 마지막 수업의 기억이 남습니다. | 마지막 수업의 기억이 사라집니다. |
| elder_daily_song | 이 기억이 남습니다. | 이 기억이 사라집니다. |
| elder_daily_hometown | 이 기억이 남습니다. | 이 기억이 사라집니다. |
| elder_memory_smell | 이 기억이 남습니다. | 이 기억이 사라집니다. |
| elder_memory_laugh | 이 기억이 남습니다. | 이 기억이 사라집니다. |
| elder_memory_hands | 아내의 손 감촉이 남습니다. | 아내의 손 감촉이 사라집니다. |

---

### 5-C. PatientData 생성
**파일명**: `Elder_PatientData.asset` / **위치**: `Assets/ScriptableObjects/Patients/`

| 필드 | 값 |
|---|---|
| patientName | 이봉수 |
| patientType | Elder |
| optionALabel | 남긴다 |
| optionBLabel | 제거한다 |
| gridSize | (4, 3) |
| memoryImage | (노인 기억 이미지 Texture2D — 추후 연결) |
| memoryImageAlt | (없음) |
| **maxPiecesInFrame** | **7** ← 알츠하이머 프레임 제한, 반드시 확인 |
| puzzleBackground | (추후 연결) |
| pieces (12개) | 위 12개 MemoryPieceData 드래그 |

**introLines (8줄)**

| # | speakerName | line |
|---|---|---|
| 0 | 이지영 | 아버지, 여기 앉으세요. |
| 1 | 이봉수 | (천천히 앉으며) 여기가 어디야? |
| 2 | 이지영 | 가끔 이러세요. 잠깐 지나가요. |
| 3 | 주인공 | 안녕하세요. 이봉수 선생님이시죠? |
| 4 | 이봉수 | (잠시 생각하다) ...네. 맞아요. 선생님이었어요, 저. |
| 5 | 주인공 | 오늘 많이 피곤하세요? |
| 6 | 이봉수 | 아니요. 괜찮아요. |
| 7 | 이봉수 | (창밖을 보며) 날이 좋네. |

**outroLines (7줄)**

| # | speakerName | line |
|---|---|---|
| 0 | 이봉수 | (주인공을 보며) ...수고했어요. |
| 1 | 주인공 | 네. 어떠세요? |
| 2 | 이봉수 | (잠시 이지영을 바라보다가) 지영아. |
| 3 | 이지영 | (놀라며) ...네, 아버지. |
| 4 | 이봉수 | 밥 먹었어? |
| 5 | 이지영 | (목이 메어) 네. 먹었어요. |
| 6 | 이봉수 | 잘 먹어야 해. |

---

## 6. 환자 3 — 서윤 (아동)

### 6-A. DialogueNodeData 생성
**위치**: `Assets/ScriptableObjects/Dialogue/Child/{조각폴더}/`

---

#### 📁 birthday_cake/

**`root.asset`** — nodeId: `child_birthday_cake_root`
> 서윤: "생일이었어요! 케이크도 있었고요. 초가 아홉 개였어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "누가 있었어?" | Fact | ☐ | → A |
| 1 | "케이크 맛이 어땠어?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `child_birthday_cake_A`
> 서윤: "엄마랑 아빠랑... 다들 있었어요." (목소리가 살짝 작아짐) "...다들요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "기분이 어땠어?" | Emotion | ☐ | → AA |
| 1 | "다들이라면 또 누가?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `child_birthday_cake_AA`
> 서윤: "좋았어요." (짧은 침묵) "...좋았던 것 같아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "좋았던 것 같다는 게 무슨 말이야?" | KeyEmotion | ☐ | → AAA |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AAA.asset`** — nodeId: `child_birthday_cake_AAA`
> 서윤: "..." (인형을 꼭 쥐며) "모르겠어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "몰라도 괜찮아." | Respect | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 birthday_wish/

**`root.asset`** — nodeId: `child_birthday_wish_root`
> 서윤: "소원을 빌었어요. 눈을 감고요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "무슨 소원을 빌었어?" | Fact | ☐ | → A |
| 1 | "눈을 감으면 어떤 기분이었어?" | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `child_birthday_wish_A`
> 서윤: "..." (오래 생각하다가) "기억 안 나요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "정말? 생각해보면?" | Fact | ☐ | → AA |
| 1 | "괜찮아, 기억 안 나도." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `child_birthday_wish_AA`
> 서윤: (아주 작은 목소리로) "...집에서 나가게 해달라고." (바로 고개를 들며, 빠르게) "아, 아니에요. 다른 거였어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "처음에 한 말이 맞는 것 같아." | KeyEmotion | ☐ | 없음 |
| 1 | "괜찮아. 억지로 말 안 해도 돼." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 gift_unwrap/

**`root.asset`** — nodeId: `child_gift_unwrap_root`
> 서윤: "선물을 받았어요. 예쁜 거였어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 선물이었어?" | Fact | ☐ | → A |
| 1 | "선물 받을 때 기분이 어땠어?" | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`A.asset`** — nodeId: `child_gift_unwrap_A`
> 서윤: "인형이요." (잠깐 멈춤) "...예뻤어요." (조금 힘없이) "근데 금방 없어졌어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "왜 없어졌어?" | Fact | ☐ | → AA |
| 1 | "없어졌을 때 어땠어?" | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`AA.asset`** — nodeId: `child_gift_unwrap_AA`
> 서윤: "..." "깨졌어요." (빠르게) "제가 실수로요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "실수로?" | KeyEmotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 dinner_table/

**`root.asset`** — nodeId: `child_dinner_table_root`
> 서윤: "저녁은 항상 같이 먹었어요. 다 같이요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "어떤 음식을 먹었어?" | Fact | ☐ | 없음 |
| 1 | "저녁 먹을 때 분위기가 어땠어?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `child_dinner_table_B`
> 서윤: (잠시 생각하다가) "..." "조용했어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "조용한 게 좋았어?" | Emotion | ☐ | 없음 |
| 1 | "항상 조용했어?" | Fact | ☐ | → BB |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BB.asset`** — nodeId: `child_dinner_table_BB`
> 서윤: "..." "숟가락 소리 내면 안 됐어요." (아주 조용하게)

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "왜?" | KeyEmotion | ☐ | 없음 |
| 1 | "힘들었겠다." | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 night_sleep/

**`root.asset`** — nodeId: `child_night_sleep_root`
> 서윤: "잠은 잘 잤어요. 이불이 따뜻했어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "잠들기 전에 뭘 했어?" | Fact | ☐ | 없음 |
| 1 | "잠이 잘 왔어?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `child_night_sleep_B`
> 서윤: (잠깐 멈춤) "..." "가끔은요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "가끔만?" | KeyEmotion | ☐ | → BA |
| 1 | "잠이 안 올 때는 어떻게 했어?" | Fact | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BA.asset`** — nodeId: `child_night_sleep_BA`
> 서윤: (매우 작게) "소리 안 나게 있었어요. 그러면 괜찮았어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "뭔 소리가 날까봐?" | KeyEmotion | ☐ | 없음 |
| 1 | "그랬구나. 많이 힘들었겠다." | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 school_morning/

**`root.asset`** — nodeId: `child_school_morning_root`
> 서윤: "학교 가는 게 좋았어요. 친구들도 있고요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "학교에서 제일 좋아하는 건 뭐야?" | Fact | ☐ | 없음 |
| 1 | "집이랑 학교 중에 어디가 더 좋아?" | KeyEmotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `child_school_morning_B`
> 서윤: (바로 대답하려다 멈춤) "..." "학교요." (작게) "학교가 더 좋아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "왜?" | KeyEmotion | ☐ | → BA |
| 1 | "그렇구나." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BA.asset`** — nodeId: `child_school_morning_BA`
> 서윤: "..." (대답 없이 인형을 내려다봄)

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "대답 안 해도 돼." | Respect | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 parent_laugh/

**`root.asset`** — nodeId: `child_parent_laugh_root`
> 서윤: "집에서 웃음소리가 났어요. 가끔이요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "누가 웃었어?" | Fact | ☐ | 없음 |
| 1 | "그 웃음소리가 어땠어?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `child_parent_laugh_B`
> 서윤: "..." "좋을 때도 있었어요. 무서울 때도 있었어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "무서울 때는 어떤 때야?" | KeyEmotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 mom_hand/

**`root.asset`** — nodeId: `child_mom_hand_root`
> 서윤: "엄마 손이... 따뜻했어요." (조금 빠르게, 연습한 것처럼)

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "엄마 손을 잡은 기억이 있어?" | Fact | ☐ | 없음 |
| 1 | "엄마 손이 어떤 느낌이었어?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `child_mom_hand_B`
> 서윤: (대답하다 멈춤) "..." (손을 내려다봄) "뜨거울 때도 있었어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "뜨겁다는 게 어떤 뜻이야?" | KeyEmotion | ☐ | → BA |
| 1 | "억지로 말 안 해도 돼." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BA.asset`** — nodeId: `child_mom_hand_BA`
> 서윤: (긴 침묵) (아주 작게) "때렸어요. 제가 잘못해서요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "서윤이 잘못이 아니야." | KeyEmotion | ☐ | → BAA |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BAA.asset`** — nodeId: `child_mom_hand_BAA`
> 서윤: (고개를 들어 주인공을 봄) "..." "정말요?"

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "응. 정말이야." | KeyEmotion | ☐ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

#### 📁 dad_face/

**`root.asset`** — nodeId: `child_dad_face_root`
> 서윤: "아빠는... 일이 많았어요. 바빴어요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "아빠랑 같이 한 게 있어?" | Fact | ☐ | 없음 |
| 1 | "아빠 얼굴을 보면 어때?" | Emotion | ☐ | → B |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`B.asset`** — nodeId: `child_dad_face_B`
> 서윤: (바로 대답 못 함) "..." "무서워요." (바로) "아니, 무섭진 않아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "처음에 한 말이 맞는 것 같아." | KeyEmotion | ☐ | → BA |
| 1 | "괜찮아. 무서우면 무섭다고 해도 돼." | Respect | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BA.asset`** — nodeId: `child_dad_face_BA`
> 서윤: (인형을 꽉 쥐며) "..." "화나면... 달라져요. 다른 사람 같아요."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | "그때 서윤이는 어떻게 했어?" | KeyEmotion | ☐ | → BAA |
| 1 | "많이 무서웠겠다." | Emotion | ☐ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

**`BAA.asset`** — nodeId: `child_dad_face_BAA`
> 서윤: "숨었어요. 작은 데." (잠시 후) "거기 있으면 못 찾아요." (아주 작게) "가끔은."

| # | choiceText | tags | Trigger | nextNode |
|---|---|---|---|---|
| 0 | ▶마무리 | — | ☑ | 없음 |
| 1 | ▶마무리 | — | ☑ | 없음 |
| 2 | ▶마무리 | — | ☑ | 없음 |

---

### 6-B. MemoryPieceData 생성
**위치**: `Assets/ScriptableObjects/Pieces/Child/`

| 파일명 | pieceId | pieceName | gridPosition | rootNode |
|---|---|---|---|---|
| `child_birthday_cake.asset` | child_birthday_cake | 생일 케이크 앞 | (0, 0) | birthday_cake/root.asset |
| `child_birthday_wish.asset` | child_birthday_wish | 생일 소원 | (1, 0) | birthday_wish/root.asset |
| `child_gift_unwrap.asset` | child_gift_unwrap | 선물 뜯기 | (2, 0) | gift_unwrap/root.asset |
| `child_dinner_table.asset` | child_dinner_table | 저녁 식사 | (0, 1) | dinner_table/root.asset |
| `child_night_sleep.asset` | child_night_sleep | 잠자리 | (1, 1) | night_sleep/root.asset |
| `child_school_morning.asset` | child_school_morning | 학교 가는 날 | (2, 1) | school_morning/root.asset |
| `child_parent_laugh.asset` | child_parent_laugh | 웃음소리 | (0, 2) | parent_laugh/root.asset |
| `child_mom_hand.asset` | child_mom_hand | 엄마의 손 | (1, 2) | mom_hand/root.asset |
| `child_dad_face.asset` | child_dad_face | 아빠의 얼굴 | (2, 2) | dad_face/root.asset |

> connectedPieceIds: 모든 아동 조각 **(없음)**

**처리 미리보기 텍스트**

| pieceId | optionAPreview | optionBPreview |
|---|---|---|
| child_birthday_cake | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_birthday_wish | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_gift_unwrap | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_dinner_table | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_night_sleep | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_school_morning | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_parent_laugh | 그날 실제로 있었던 일이 드러납니다. | 서윤이가 기억하는 그대로 남겨둡니다. |
| child_mom_hand | 서윤이가 감추고 싶었던 기억이 드러납니다. 충격이 클 수 있습니다. | 이 기억은 그대로 남겨둡니다. |
| child_dad_face | 서윤이가 감추고 싶었던 기억이 드러납니다. 충격이 클 수 있습니다. | 이 기억은 그대로 남겨둡니다. |

---

### 6-C. PatientData 생성
**파일명**: `Child_PatientData.asset` / **위치**: `Assets/ScriptableObjects/Patients/`

| 필드 | 값 |
|---|---|
| patientName | 서윤 |
| patientType | Child |
| optionALabel | 드러낸다 |
| optionBLabel | 그대로 둔다 |
| gridSize | (3, 3) |
| memoryImage | (아동 기억 이미지 — 가짜 버전 Texture2D, 추후 연결) |
| **memoryImageAlt** | **(아동 기억 이미지 — 진실 버전 Texture2D, 추후 연결) ← Child 전용** |
| maxPiecesInFrame | -1 |
| puzzleBackground | (추후 연결) |
| pieces (9개) | 위 9개 MemoryPieceData 드래그 |

**introLines (8줄)**

| # | speakerName | line |
|---|---|---|
| 0 | 주인공 | 안녕, 서윤아. |
| 1 | 서윤 | (조용히 앉으며) 안녕하세요. |
| 2 | 주인공 | 여기 처음이지? 무섭지 않아? |
| 3 | 서윤 | (고개를 흔들며) 아니요. (잠시 후) 인형 귀엽다. |
| 4 | 주인공 | 마음에 들면 안아도 돼. 오늘은 서윤이랑 얘기 좀 하고 싶어서. |
| 5 | 서윤 | 무슨 얘기요? |
| 6 | 주인공 | 서윤이 기억 얘기. 좋은 것도, 별로인 것도. |
| 7 | 서윤 | (잠깐 생각하다 밝게) 저 기억 많아요. 좋은 거요. |

**outroLines (7줄)**

| # | speakerName | line |
|---|---|---|
| 0 | 서윤 | (주인공을 바라보며) 다 했어요? |
| 1 | 주인공 | 응. 서윤이 고마워. 많이 힘들었지? |
| 2 | 서윤 | (잠깐 생각하다가) ...아니요. |
| 3 | 서윤 | (인형을 내밀며) 이거 잠깐 빌려줘요? |
| 4 | 주인공 | 응, 가져가도 돼. |
| 5 | 서윤 | (작게) 감사합니다. (나가다 멈추며) ...저 잘못한 거 맞죠? |
| 6 | 주인공 | 아니야. |

---

## 7. GameManager 최종 연결

모든 SO 생성 완료 후 Hierarchy → **Managers** → **GameManager** 컴포넌트에 연결.

| 필드 | 연결 |
|---|---|
| patients[0] | Soldier_PatientData.asset |
| patients[1] | Elder_PatientData.asset |
| patients[2] | Child_PatientData.asset |
| epilogueDatabase | EpilogueDatabase.asset |

**TitleManager** 추가 연결:

| 필드 | 연결 |
|---|---|
| morningSprite | 아침 진료실 Sprite (추후 연결) |
| eveningSprite | 저녁 진료실 Sprite (추후 연결) |

**VNManager** 추가 연결:

| 필드 | 연결 |
|---|---|
| defaultBackground | 기본 배경 Sprite (추후 연결) |

> vnManager / puzzleManager / endingTracker / titleManager / fadeCanvas는 씬 자동 생성 시 이미 연결됨.

---

## 8. 작업 체크리스트

```
[ ] EpilogueDatabase 생성 (introLines 8줄 + entries 18종)

[ ] 군인 DialogueNodeData — 총 29개 노드
    [ ] colleague_first   : root, A, AA, B
    [ ] colleague_training: root, A, AA
    [ ] war_before        : root, A, AB
    [ ] war_combat        : root, A, AA, B
    [ ] colleague_death   : root, A, AB, ABB, B
    [ ] war_order         : root, A, AB
    [ ] family_return     : root, A
    [ ] family_son        : root, B
    [ ] guilt_survive     : root, A, AB

[ ] 군인 MemoryPieceData — 9개
[ ] Soldier_PatientData

[ ] 노인 DialogueNodeData — 총 22개 노드
    [ ] daughter_face     : root, A, AB
    [ ] daughter_wedding  : root, A
    [ ] daughter_young    : root  ← 스토리 미작성, 임시 구성 사용
    [ ] wife_voice        : root, B, BA
    [ ] wife_morning      : root, B
    [ ] career_classroom  : root, A, AA
    [ ] career_last       : root, B
    [ ] daily_song        : root
    [ ] daily_hometown    : root
    [ ] memory_smell      : root
    [ ] memory_laugh      : root
    [ ] memory_hands      : root, A

[ ] 노인 MemoryPieceData — 12개
[ ] Elder_PatientData  ← maxPiecesInFrame = 7 반드시 확인

[ ] 아동 DialogueNodeData — 총 29개 노드
    [ ] birthday_cake     : root, A, AA, AAA
    [ ] birthday_wish     : root, A, AA
    [ ] gift_unwrap       : root, A, AA
    [ ] dinner_table      : root, B, BB
    [ ] night_sleep       : root, B, BA
    [ ] school_morning    : root, B, BA
    [ ] parent_laugh      : root, B
    [ ] mom_hand          : root, B, BA, BAA
    [ ] dad_face          : root, B, BA, BAA

[ ] 아동 MemoryPieceData — 9개
[ ] Child_PatientData  ← memoryImageAlt 연결 잊지 말 것

[ ] GameManager.patients[] 연결 (순서: Soldier[0] → Elder[1] → Child[2])
[ ] GameManager.epilogueDatabase 연결
[ ] TitleManager 이미지 연결
[ ] VNManager defaultBackground 연결
[ ] 각 PatientData.memoryImage 연결
[ ] Child_PatientData.memoryImageAlt 연결 (진실 버전 Texture2D)
```
