# 환자 2 — 이봉수 (79세) / 알츠하이머

## 기본 설정
- 직업: 은퇴한 초등학교 교사 (38년 재직)
- 가족: 딸 이지영, 아내 故 김순자 (5년 전 별세)
- 상황: 알츠하이머 진단 2년. 딸을 알아보지 못하는 날이 늘어남. 딸이 치료 의뢰.
- 처리 선택지: **남긴다** / **제거한다**
- gridSize: 4×3 (12조각, 프레임 7칸)

---

## 조각 목록

| pieceId                  | pieceName        | gridPosition | PatientValue 힌트              |
|--------------------------|------------------|--------------|-------------------------------|
| elder_daughter_face      | 딸의 얼굴        | (0,0)        | 딸을 알아보고 싶다는 소망       |
| elder_daughter_wedding   | 딸의 결혼식      | (1,0)        |                               |
| elder_daughter_young     | 어린 딸          | (2,0)        |                               |
| elder_wife_voice         | 아내의 목소리    | (3,0)        | 아내를 그리워함                |
| elder_wife_morning       | 아내와의 아침    | (0,1)        |                               |
| elder_career_classroom   | 교단에 서던 날   | (1,1)        | 교사로서의 정체성              |
| elder_career_last        | 마지막 수업      | (2,1)        |                               |
| elder_daily_song         | 좋아하던 노래    | (3,1)        |                               |
| elder_daily_hometown     | 고향 풍경        | (0,2)        |                               |
| elder_memory_smell       | 된장찌개 냄새    | (1,2)        |                               |
| elder_memory_laugh       | 웃음 소리        | (2,2)        |                               |
| elder_memory_hands       | 아내의 손 감촉   | (3,2)        | 아내를 그리워함                |

### 처리 미리보기 텍스트

**elder_daughter_face**
- optionAPreview: "딸의 얼굴이 남습니다."
- optionBPreview: "딸의 얼굴이 사라집니다."

**elder_wife_voice**
- optionAPreview: "아내의 목소리가 남습니다."
- optionBPreview: "아내의 목소리가 사라집니다."

**elder_career_classroom**
- optionAPreview: "교단에 서던 기억이 남습니다."
- optionBPreview: "교단에 서던 기억이 사라집니다."

*(나머지 조각들도 동일한 형식)*

---

## VN — 치료 전 대화

```
[배경: 진료실. 오후. 딸 이지영과 함께 입장.]

이지영: 아버지, 여기 앉으세요.

이봉수: (천천히 앉으며) 여기가 어디야?

이지영: (주인공에게 작은 목소리로)
        가끔 이러세요. 잠깐 지나가요.

주인공: (이봉수에게) 안녕하세요.
        이봉수 선생님이시죠?

이봉수: (잠시 생각하다) ...네. 맞아요.
        선생님이었어요, 저.

주인공: 오늘 많이 피곤하세요?

이봉수: 아니요. 괜찮아요.
        (창밖을 보며) 날이 좋네.
```

---

## 조각별 대화 트리

*(이 환자는 대화가 흐릿하게 시작했다가 가끔 선명해짐.
 대화 중 선명한 순간에 PatientValue 태그 부여.)*

---

### elder_daughter_face — 딸의 얼굴

**rootNode:**
```
이봉수: "저 사람... 자주 오는 사람인데."
        "이름이... 뭐더라."

선택지:
  [A] "따님이에요. 이지영 씨요."      [tags: PatientValue]
  [B] "알아보시겠어요?"              [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
이봉수: "지영이. 맞다, 지영이."
        (잠시 눈을 감았다 뜸)
        "지영이가 몇 살이지?"

선택지:
  [A] "마흔셋이에요."                        [tags: Fact]
  [B] "따님 얼굴을 보면 어떤 기분이에요?"    [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."    [isProcessOption: true]
```

**nodeA-B:**
```
이봉수: "..."
        "따뜻해요."
        "뭔지 모르겠는데 따뜻해."
        (작게) "알아봐야 하는데."    ← PatientValue 핵심 대사

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_daughter_wedding — 딸의 결혼식

**rootNode:**
```
이봉수: "결혼식이..."
        "지영이 결혼식이었나."
        "내가 울었어요, 그날."

선택지:
  [A] "왜 우셨어요?"                 [tags: Emotion]
  [B] "어떤 날이었는지 기억나세요?"  [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
이봉수: "몰라. 그냥 울었어요."
        "이쪽에서."
        (가슴을 가리키며)
        "여기가 뭔가 꽉 차는 것 같았어요."

선택지:
  [A] "좋은 울음이었겠네요."                [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_wife_voice — 아내의 목소리

**rootNode:**
```
이봉수: "..."
        "순자 씨 목소리가..."
        "가끔 들려요."
        "들리는 것 같아요."

선택지:
  [A] "아내분이 그리우시겠어요."            [tags: Emotion]
  [B] "어떤 목소리였나요?"                  [tags: PatientValue]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeB:**
```
이봉수: "낮았어요. 근데 또렷했어요."
        "내가 실수하면 딱 한마디 해요."
        "'그거 아니에요.'"
        (웃음)
        "그게 또 맞아요, 항상."

선택지:
  [A] "지금도 그 목소리가 들리세요?"        [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeB-A:**
```
이봉수: "응."
        (잠시 먼 곳을 바라보다가)
        "가끔은."

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_wife_morning — 아내와의 아침

**rootNode:**
```
이봉수: "아침에 일어나면..."
        "밥이 있었어요, 항상."
        "내가 일어나기 전에 이미."

선택지:
  [A] "아내분이 먼저 일어나셨군요."          [tags: Fact]
  [B] "그 아침이 지금 어떻게 느껴지세요?"   [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."   [isProcessOption: true]
```

**nodeB:**
```
이봉수: "..."
        "지금은 혼자 먹어요."
        "맛이 없어요."
        (담담하게)
        "밥이 잘못된 게 아닌데."

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_career_classroom — 교단에 서던 날

**rootNode:**
```
이봉수: "교사였어요, 나."
        "38년."
        "오래 했지."

선택지:
  [A] "어떤 선생님이셨어요?"              [tags: PatientValue]
  [B] "학생들이 기억나세요?"              [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
이봉수: "글쎄요."
        (잠깐 생각하다 또렷해짐)
        "엄하진 않았어요. 엄한 척은 했지만."
        "애들이 다 알아요, 그거."

선택지:
  [A] "좋아하셨겠어요, 그 일."            [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA-A:**
```
이봉수: "네."
        "그게 나였어요."
        (천천히, 또렷하게)
        "선생님 이봉수."    ← PatientValue 핵심 대사

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_career_last — 마지막 수업

**rootNode:**
```
이봉수: "마지막 수업이..."
        "언제였더라."
        "아이들이 노래를 불러줬어요."

선택지:
  [A] "어떤 노래였나요?"                 [tags: Fact]
  [B] "그때 어떤 기분이었어요?"          [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeB:**
```
이봉수: "..."
        "울지 말아야지 했는데."
        "또 울었어요."
        (작게 웃음)

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_daily_song — 좋아하던 노래

**rootNode:**
```
이봉수: "노래가..."
        "뭔가 생각날 것 같은데."
        (흥얼거리다 멈춤)
        "잊어버렸네."

선택지:
  [A] "어떤 노래였는지 기억나세요?"    [tags: Fact]
  [B] "괜찮아요, 천천히."            [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_daily_hometown — 고향 풍경

**rootNode:**
```
이봉수: "고향이..."
        "강원도예요."
        "산이 많았어요."

선택지:
  [A] "그 풍경이 지금도 생각나세요?"   [tags: Emotion]
  [B] "고향에 가보고 싶으세요?"       [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_memory_smell — 된장찌개 냄새

**rootNode:**
```
이봉수: "냄새가 나요."
        "된장찌개."
        "순자 씨가 끓이던."

선택지:
  [A] "그 냄새가 어떻게 느껴지세요?"       [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_memory_laugh — 웃음 소리

**rootNode:**
```
이봉수: "웃음 소리가..."
        "누구 웃음 소리더라."
        (잠시 멈춤)
        "지영이 어렸을 때 같아요."

선택지:
  [A] "어떤 웃음이었나요?"             [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### elder_memory_hands — 아내의 손 감촉

**rootNode:**
```
이봉수: "손이..."
        "차가웠어요."
        "항상."
        "근데 잡으면 따뜻해졌어요."

선택지:
  [A] "아내분 손이요?"                           [tags: PatientValue]
  [B] "그 감촉이 지금도 기억나세요?"             [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."        [isProcessOption: true]
```

**nodeA:**
```
이봉수: "응."
        "순자 씨 손."
        (자신의 손을 내려다보며)
        "요즘은 혼자라서."

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

## VN — 치료 후 대화

```
[배경: 진료실. 치료 후. 딸 이지영도 있음.]

이봉수: (주인공을 보며) ...수고했어요.

주인공: 네. 어떠세요?

이봉수: (잠시 이지영을 바라보다가)
        지영아.

이지영: (놀라며) ...네, 아버지.

이봉수: 밥 먹었어?

이지영: (목이 메어) 네. 먹었어요.

이봉수: 잘 먹어야 해.
```

---

## 에필로그 텍스트

**elder_kept_daughter**
```
면회 때마다 이름은 몰랐지만
딸이 오면 웃었다.
따님은 그걸로 충분하다고 했다.
```

**elder_kept_wife**
```
아내의 이름을 자주 불렀다.
오래전에 떠난 분이었다.
혼자서도 웃을 때가 있었다.
```

**elder_kept_career**
```
선생님이라고 부르면 또렷해졌다.
요양원 직원들이 그렇게 불러드리기로 했다.
```

**elder_arbitrary**
```
무엇이 남았는지,
그것이 그에게 의미 있는 것이었는지
알 수 없었다.
```
