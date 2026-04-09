# 환자 3 — 서윤 (9세) / 학대 피해 아동

## 기본 설정
- 상황: 부모에 의한 정서적/신체적 학대. 현재 보호시설 거주. 담당 상담사 의뢰.
- 특징: 스스로 가짜 기억을 만들어 자신을 보호해왔음. 학대 사실을 인식하지 못하거나 부정함.
- 처리 선택지: **드러낸다** / **그대로 둔다**
- gridSize: 3×3
- memoryImage: 밝고 따뜻한 가족 그림 (행복한 버전)
- memoryImageAlt: 같은 구도의 어두운 진실 버전

---

## 조각 목록

| pieceId              | pieceName        | gridPosition | 가짜 이미지             | 진실 이미지               |
|----------------------|------------------|--------------|------------------------|--------------------------|
| child_birthday_cake  | 생일 케이크 앞   | (0,0)        | 웃는 아이와 케이크      | 혼자 구석에 앉은 아이     |
| child_birthday_wish  | 생일 소원        | (1,0)        | 눈 감고 소원 비는 아이  | 무서워서 눈 감은 아이     |
| child_gift_unwrap    | 선물 뜯기        | (2,0)        | 기쁘게 선물 여는 아이   | 깨진 선물 앞의 아이       |
| child_dinner_table   | 저녁 식사        | (0,1)        | 온 가족이 함께 먹는     | 혼자 밥 먹는 아이         |
| child_night_sleep    | 잠자리           | (1,1)        | 포근하게 자는 아이      | 이불 속에서 웅크린 아이   |
| child_school_morning | 학교 가는 날     | (2,1)        | 신나게 뛰어가는 아이    | 고개를 숙이고 걷는 아이   |
| child_parent_laugh   | 웃음소리         | (0,2)        | 다 함께 웃는 가족       | 조마조마한 표정의 아이    |
| child_mom_hand       | 엄마의 손        | (1,2)        | 다정하게 잡아주는 손    | 들어올려진 손             |
| child_dad_face       | 아빠의 얼굴      | (2,2)        | 자상하게 웃는 얼굴      | 굳은 표정의 얼굴          |

### 처리 미리보기 텍스트

**child_birthday_cake**
- optionAPreview: "그날 실제로 있었던 일이 드러납니다."
- optionBPreview: "서윤이가 기억하는 그대로 남겨둡니다."

**child_mom_hand / child_dad_face** *(핵심 조각)*
- optionAPreview: "서윤이가 감추고 싶었던 기억이 드러납니다.\n충격이 클 수 있습니다."
- optionBPreview: "이 기억은 그대로 남겨둡니다."

*(나머지 조각들도 동일한 형식)*

---

## VN — 치료 전 대화

```
[배경: 아이들을 위한 상담실. 작고 아늑한 공간. 곰 인형이 있음.]

주인공: 안녕, 서윤아.

서윤: (조용히 앉으며) 안녕하세요.

주인공: 여기 처음이지? 무섭지 않아?

서윤: (고개를 흔들며) 아니요.
      (잠시 후) 인형 귀엽다.

주인공: 마음에 들면 안아도 돼.
        오늘은 서윤이랑 얘기 좀 하고 싶어서.

서윤: 무슨 얘기요?

주인공: 서윤이 기억 얘기.
        좋은 것도, 별로인 것도.

서윤: (잠깐 생각하다 밝게) 저 기억 많아요.
      좋은 거요.
```

---

## 조각별 대화 트리

*(서윤은 처음엔 밝고 빠르게 말함.
 해석 질문이나 핵심에 가까워지면 말이 줄고 더듬거림.
 가짜 기억은 너무 매끄럽게 말함 — 연습한 것처럼.)*

---

### child_birthday_cake — 생일 케이크 앞

**rootNode:**
```
서윤: "생일이었어요! 케이크도 있었고요."
      "초가 아홉 개였어요."

선택지:
  [A] "누가 있었어?"                               [tags: Fact]
  [B] "케이크 맛이 어땠어?"                        [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeA:**
```
서윤: "엄마랑 아빠랑... 다들 있었어요."
      (목소리가 살짝 작아짐)
      "...다들요."

선택지:
  [A] "기분이 어땠어?"                             [tags: Emotion]
  [B] "다들이라면 또 누가?"                        [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeA-A:**
```
서윤: "좋았어요."
      (짧은 침묵)
      "...좋았던 것 같아요."

선택지:
  [A] "좋았던 것 같다는 게 무슨 말이야?"            [tags: KeyEmotion]
  [B] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
```

**nodeA-A-A:**
```
서윤: "..."
      (인형을 꼭 쥐며)
      "모르겠어요."

선택지:
  [A] "몰라도 괜찮아."                              [tags: Respect]
  [B] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
```

---

### child_birthday_wish — 생일 소원

**rootNode:**
```
서윤: "소원을 빌었어요."
      "눈을 감고요."

선택지:
  [A] "무슨 소원을 빌었어?"                        [tags: Fact]
  [B] "눈을 감으면 어떤 기분이었어?"               [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeA:**
```
서윤: "..."
      (오래 생각하다가)
      "기억 안 나요."

선택지:
  [A] "정말? 생각해보면?"                          [tags: Fact]
  [B] "괜찮아, 기억 안 나도."                     [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeA-A:**
```
서윤: (아주 작은 목소리로)
      "...집에서 나가게 해달라고."
      (바로 고개를 들며, 빠르게)
      "아, 아니에요. 다른 거였어요."

선택지:
  [A] "처음에 한 말이 맞는 것 같아."              [tags: KeyEmotion]
  [B] "괜찮아. 억지로 말 안 해도 돼."             [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_gift_unwrap — 선물 뜯기

**rootNode:**
```
서윤: "선물을 받았어요."
      "예쁜 거였어요."

선택지:
  [A] "어떤 선물이었어?"                           [tags: Fact]
  [B] "선물 받을 때 기분이 어땠어?"                [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeA:**
```
서윤: "인형이요."
      (잠깐 멈춤)
      "...예뻤어요."
      (조금 힘없이)
      "근데 금방 없어졌어요."

선택지:
  [A] "왜 없어졌어?"                               [tags: Fact]
  [B] "없어졌을 때 어땠어?"                        [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeA-A:**
```
서윤: "..."
      "깨졌어요."
      (빠르게) "제가 실수로요."

선택지:
  [A] "실수로?"                                    [tags: KeyEmotion]
  [B] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_dinner_table — 저녁 식사

**rootNode:**
```
서윤: "저녁은 항상 같이 먹었어요."
      "다 같이요."

선택지:
  [A] "어떤 음식을 먹었어?"                        [tags: Fact]
  [B] "저녁 먹을 때 분위기가 어땠어?"              [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB:**
```
서윤: (잠시 생각하다가)
      "..."
      "조용했어요."

선택지:
  [A] "조용한 게 좋았어?"                          [tags: Emotion]
  [B] "항상 조용했어?"                             [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-B:**
```
서윤: "..."
      "숟가락 소리 내면 안 됐어요."
      (아주 조용하게)

선택지:
  [A] "왜?"                                        [tags: KeyEmotion]
  [B] "힘들었겠다."                                [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_night_sleep — 잠자리

**rootNode:**
```
서윤: "잠은 잘 잤어요."
      "이불이 따뜻했어요."

선택지:
  [A] "잠들기 전에 뭘 했어?"                      [tags: Fact]
  [B] "잠이 잘 왔어?"                             [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB:**
```
서윤: (잠깐 멈춤)
      "..."
      "가끔은요."

선택지:
  [A] "가끔만?"                                    [tags: KeyEmotion]
  [B] "잠이 안 올 때는 어떻게 했어?"              [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-A:**
```
서윤: (매우 작게)
      "소리 안 나게 있었어요."
      "그러면 괜찮았어요."

선택지:
  [A] "뭔 소리가 날까봐?"                          [tags: KeyEmotion]
  [B] "그랬구나. 많이 힘들었겠다."                 [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_school_morning — 학교 가는 날

**rootNode:**
```
서윤: "학교 가는 게 좋았어요."
      "친구들도 있고요."

선택지:
  [A] "학교에서 제일 좋아하는 건 뭐야?"            [tags: Fact]
  [B] "집이랑 학교 중에 어디가 더 좋아?"          [tags: KeyEmotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB:**
```
서윤: (바로 대답하려다 멈춤)
      "..."
      "학교요."
      (작게)
      "학교가 더 좋아요."

선택지:
  [A] "왜?"                                        [tags: KeyEmotion]
  [B] "그렇구나."                                  [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-A:**
```
서윤: "..."
      (대답 없이 인형을 내려다봄)

선택지:
  [A] "대답 안 해도 돼."                           [tags: Respect]
  [B] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_parent_laugh — 웃음소리

**rootNode:**
```
서윤: "집에서 웃음소리가 났어요."
      "가끔이요."

선택지:
  [A] "누가 웃었어?"                               [tags: Fact]
  [B] "그 웃음소리가 어땠어?"                      [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB:**
```
서윤: "..."
      "좋을 때도 있었어요."
      "무서울 때도 있었어요."

선택지:
  [A] "무서울 때는 어떤 때야?"                     [tags: KeyEmotion]
  [B] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_mom_hand — 엄마의 손 [핵심 조각]

**rootNode:**
```
서윤: "엄마 손이..."
      "따뜻했어요."
      (조금 빠르게, 연습한 것처럼)

선택지:
  [A] "엄마 손을 잡은 기억이 있어?"               [tags: Fact]
  [B] "엄마 손이 어떤 느낌이었어?"                [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB:**
```
서윤: (대답하다 멈춤)
      "..."
      (손을 내려다봄)
      "뜨거울 때도 있었어요."

선택지:
  [A] "뜨겁다는 게 어떤 뜻이야?"                  [tags: KeyEmotion]
  [B] "억지로 말 안 해도 돼."                     [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-A:**
```
서윤: (긴 침묵)
      (아주 작게)
      "때렸어요."
      "제가 잘못해서요."

선택지:
  [A] "서윤이 잘못이 아니야."                     [tags: KeyEmotion]
  [B] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-A-A:**
```
서윤: (고개를 들어 주인공을 봄)
      "..."
      "정말요?"

선택지:
  [A] "응. 정말이야."                              [tags: KeyEmotion]
  [B] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

### child_dad_face — 아빠의 얼굴 [핵심 조각]

**rootNode:**
```
서윤: "아빠는..."
      "일이 많았어요."
      "바빴어요."

선택지:
  [A] "아빠랑 같이 한 게 있어?"                   [tags: Fact]
  [B] "아빠 얼굴을 보면 어때?"                    [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB:**
```
서윤: (바로 대답 못 함)
      "..."
      "무서워요."
      (바로) "아니, 무섭진 않아요."

선택지:
  [A] "처음에 한 말이 맞는 것 같아."              [tags: KeyEmotion]
  [B] "괜찮아. 무서우면 무섭다고 해도 돼."        [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-A:**
```
서윤: (인형을 꽉 쥐며)
      "..."
      "화나면... 달라져요."
      "다른 사람 같아요."

선택지:
  [A] "그때 서윤이는 어떻게 했어?"                [tags: KeyEmotion]
  [B] "많이 무서웠겠다."                          [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

**nodeB-A-A:**
```
서윤: "숨었어요."
      "작은 데."
      (잠시 후)
      "거기 있으면 못 찾아요."
      (아주 작게) "가끔은."

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."          [isProcessOption: true]
```

---

## VN — 치료 후 대화

```
[배경: 상담실. 치료 후.]

서윤: (주인공을 바라보며)
      다 했어요?

주인공: 응. 서윤이 고마워.
        많이 힘들었지?

서윤: (잠깐 생각하다가)
      ...아니요.
      (인형을 내밀며)
      이거 잠깐 빌려줘요?

주인공: 응, 가져가도 돼.

서윤: (작게) 감사합니다.
      (나가다 멈추며)
      ...저 잘못한 거 맞죠?

주인공: 아니야.

서윤: (오래 서 있다가 나감)
```

---

## 에필로그 텍스트

**child_overcome**
*(드러낸 조각 많음 + KeyEmotion 태그 + 대화 충분)*
```
힘든 시간이 있었다.
처음으로 자신의 감정이 거짓이 아니라는 걸 알았다.
인형은 아직 가지고 있다.
```

**child_shock**
*(드러낸 조각 있음 + 대화 3회 미만)*
```
치료 후 한동안 말을 하지 않았다고 했다.
담당 상담사가 계속 곁에 있었다.
시간이 필요하다고 했다.
```

**child_revealed_alone**
*(드러낸 조각 있음 + KeyEmotion 태그 없음)*
```
사실은 알게 됐다.
하지만 그걸 혼자 안고 있었다.
누군가 옆에 더 있어줬어야 했다.
```

**child_kept_supported**
*(그대로 둔 조각 많음 + 대화 충분)*
```
여전히 혼란스럽다.
그래도 누군가 오래 이야기를 들어줬다는 건 기억한다.
조금씩, 아주 조금씩.
```

**child_unchanged**
*(그대로 둔 조각 많음 + 대화 적음)*
```
아무것도 달라지지 않았다.
서윤이는 여전히 괜찮다고 한다.
```
