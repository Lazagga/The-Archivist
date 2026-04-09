# 환자 1 — 박성우 (39세) / PTSD 군인

## 기본 설정
- 직업: 전역 군인 (현재 무직)
- 사건: 2013년 해외 파병 중 전우 이동현 전사
- 증상: 플래시백, 불면증, 대인기피
- 처리 선택지: **덜어낸다** / **끌어안는다**
- gridSize: 3×3

---

## 조각 목록

| pieceId                    | pieceName        | gridPosition | 오염 | connectedPieceIds                                              |
|----------------------------|------------------|--------------|------|----------------------------------------------------------------|
| soldier_colleague_first    | 처음 만난 날     | (0,0)        | X    |                                                                |
| soldier_colleague_training | 훈련소 시절      | (1,0)        | X    |                                                                |
| soldier_war_before         | 파병 전날 밤     | (2,0)        | X    |                                                                |
| soldier_war_combat         | 전투 장면        | (0,1)        | O    | soldier_colleague_death                                        |
| soldier_colleague_death    | 이동현의 마지막  | (1,1)        | O    | soldier_colleague_first, soldier_colleague_training, soldier_war_before |
| soldier_war_order          | 철수 명령        | (2,1)        | O    | soldier_guilt_survive                                          |
| soldier_family_return      | 귀환             | (0,2)        | X    |                                                                |
| soldier_family_son         | 아들의 얼굴      | (1,2)        | X    |                                                                |
| soldier_guilt_survive      | 생존자의 죄책감  | (2,2)        | O    |                                                                |

### 처리 미리보기 텍스트

**soldier_war_combat**
- optionAPreview: "전투의 기억이 희미해집니다.\n이동현의 마지막 기억도 흐릿해집니다."
- optionBPreview: "그날의 기억이 남습니다.\n고통도 함께 남습니다."

**soldier_colleague_death**
- optionAPreview: "이동현의 마지막 기억이 봉인됩니다.\n함께했던 기억들도 함께 흐릿해집니다."
- optionBPreview: "이동현을 기억합니다.\n그 순간의 고통도 함께합니다."

**soldier_war_order**
- optionAPreview: "철수 명령의 기억이 희미해집니다.\n죄책감도 함께 줄어듭니다."
- optionBPreview: "그 선택의 기억이 남습니다.\n무거움도 함께 남습니다."

**soldier_guilt_survive**
- optionAPreview: "그 질문이 작아집니다.\n하지만 답을 찾을 기회도 줄어듭니다."
- optionBPreview: "질문이 남습니다.\n무게도 남습니다."

**soldier_colleague_first / soldier_colleague_training / soldier_war_before**
- optionAPreview: "이 기억이 희미해집니다."
- optionBPreview: "이 기억이 남습니다."

**soldier_family_return / soldier_family_son**
- optionAPreview: "이 기억이 희미해집니다."
- optionBPreview: "이 기억이 남습니다."

---

## VN — 치료 전 대화

```
[배경: 진료실. 낮. 창밖으로 햇빛이 들어옴.]

주인공: 예약은 없으셨던 것 같은데요.

박성우: 아는 사람한테 여기 얘기 들었어요.
        그냥... 지나가다 들어왔습니다.

주인공: 괜찮아요. 앉으세요.
        어디서 오셨어요?

박성우: 군인이었어요. 전역한 지 한 6년 됐나.

주인공: 요즘 어떠세요?

박성우: 잠을 잘 못 잡니다.
        ...그것 말고는 괜찮아요.

주인공: 꿈을 꾸세요?

박성우: (긴 침묵)
        꿈인지 기억인지 모르겠어요.
```

---

## 조각별 대화 트리

---

### soldier_colleague_first — 처음 만난 날

**rootNode:**
```
박성우: "훈련소에서 처음 봤어요. 이동현이."
        "제 옆 침대였거든요."

선택지:
  [A] "어떤 사람이었나요?"           [tags: ColleagueRecognition]
  [B] "처음부터 친하게 지냈나요?"    [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
박성우: "말이 별로 없었어요. 근데 뭔가 믿음직했어요."
        "훈련 중에 제가 쓰러진 적 있거든요."
        "물 들고 온 게 걔였어요. 아무 말 없이."

선택지:
  [A] "그때 많이 힘들었겠어요."       [tags: Emotion]
  [B] "그게 계기가 됐나요?"           [tags: Interpretation]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA-A:**
```
박성우: "네. 근데 이상하게 그 이후로 마음이 놓였어요."
        "말 한마디 없이 그냥 물 한 잔인데."

선택지:
  [A] "그런 사람이 있다는 게 다행이었겠어요."  [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."      [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."      [isProcessOption: true]
```

**nodeB:**
```
박성우: "아니요. 처음엔 좀 어색했어요."
        "근데 어느 순간 자연스러워졌어요."
        "그런 친구 있잖아요. 딱히 뭘 한 것도 아닌데."

선택지:
  [A] "지금도 그 얼굴이 선명하게 기억나요?"          [tags: Emotion]
  [B] "같이 뭘 했을 때가 제일 기억에 남으세요?"       [tags: ColleagueRecognition]
  [C] "이 기억은 여기서 마무리하겠습니다."             [isProcessOption: true]
```

---

### soldier_colleague_training — 훈련소 시절

**rootNode:**
```
박성우: "훈련이 진짜 힘들었어요."
        "근데 이상하게 그때가 제일 단순했던 것 같아요."

선택지:
  [A] "이동현이랑 같이 버텼나요?"     [tags: ColleagueRecognition]
  [B] "단순했다는 게 어떤 의미예요?"  [tags: Interpretation]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
박성우: "걔가 저보다 체력이 좋았어요."
        "제가 처질 때마다 뒤에서 밀어줬어요. 진짜로."
        "무거운 군장 들고."
        "(짧게 웃음) 그 자식."

선택지:
  [A] "웃으시네요."                          [tags: Emotion]
  [B] "지금 이 기억은 어떻게 느껴지세요?"   [tags: Interpretation]
  [C] "이 기억은 여기서 마무리하겠습니다."   [isProcessOption: true]
```

**nodeA-A:**
```
박성우: "..."
        "모르겠어요."
        "웃겨야 하는 기억인데."

선택지:
  [A] "억지로 웃지 않아도 됩니다."           [tags: Respect]
  [B] "이 기억은 여기서 마무리하겠습니다."   [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."   [isProcessOption: true]
```

---

### soldier_war_before — 파병 전날 밤

**rootNode:**
```
박성우: "파병 전날 밤에..."
        "걔가 담배 피우냐고 물어봤어요."
        "안 핀다고 했더니 그냥 옆에 앉았어요."

선택지:
  [A] "무슨 얘기를 했나요?"           [tags: Fact]
  [B] "그날 어떤 기분이었어요?"       [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
박성우: "별 얘기 안 했어요."
        "그냥 고향 얘기, 가족 얘기."
        "걔 어머니가 된장찌개를 잘 끓인다고."
        "..."
        "그게 마지막 대화였어요."

선택지:
  [A] "그 말이 지금도 생각나시겠어요."  [tags: Emotion]
  [B] "억지로 말 안 하셔도 됩니다."    [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA-B:**
```
박성우: "아니요."
        "말해야 할 것 같아요."
        "그때 저도 이상하게 그런 생각이 들었어요."
        "내일 뭔가 달라질 것 같다는."

선택지:
  [A] "그 느낌이 맞았던 건가요."       [tags: Interpretation]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### soldier_war_combat — 전투 장면 [오염]

**rootNode:**
```
박성우: "..."
박성우: "이건 말하기 어렵네요."

선택지:
  [A] "천천히 하셔도 됩니다."              [tags: Respect]
  [B] "어떤 장면이 자꾸 떠오르나요?"       [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
박성우: "..."
        "폭발음이요. 귀에서 안 떠나요."
        "자다가도 들려요."

선택지:
  [A] "그 소리가 나면 어떻게 되세요?"      [tags: Emotion]
  [B] "언제부터 그랬나요?"                 [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA-A:**
```
박성우: "깨요. 그냥 앉아있어요."
        "애가 놀라니까 이제 방에 혼자 자요."

선택지:
  [A] "아들 때문에 여기 오신 건가요?"      [tags: KeyEmotion]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeB:**
```
박성우: (표정이 굳음)
        "..."
        "그건 못 하겠어요."

선택지:
  [A] "알겠습니다. 억지로 안 하셔도 돼요." [tags: Respect]
  [B] "그 기억이 얼마나 자주 떠오르나요?"  [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### soldier_colleague_death — 이동현의 마지막 [오염, 핵심]

**rootNode:**
```
박성우: "..."
        "이건... 선생님."
        "이건 좀 다른 것 같은데요."

선택지:
  [A] "안 보셔도 됩니다."                  [tags: Respect]
  [B] "어떻게 다른 것 같으세요?"           [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA:**
```
박성우: "아니요."
        "봐야 할 것 같아요."
        "안 보면 평생 못 볼 것 같아서."
        "이동현이... 제 앞에서."
        (말을 잇지 못함)

선택지:
  [A] "말 안 하셔도 알겠습니다."                    [tags: Respect]
  [B] "이동현 씨는 어떤 사람이었나요?"              [tags: ColleagueRecognition]
  [C] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
```

**nodeA-B:**
```
박성우: "좋은 사람이었어요."
        "저보다 훨씬."
        "(긴 침묵)"
        "그게 더 이상한 것 같아요."
        "왜 걔가 아니고 저냐는 게."

선택지:
  [A] "그 질문은 답이 없을 수도 있어요."    [tags: Respect]
  [B] "이동현 씨가 뭐라고 할 것 같아요?"   [tags: ColleagueRecognition]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeA-B-B:**
```
박성우: "..."
        "웃겠죠."
        "그 자식은 그런 애였어요."
        "짜증나게."

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

**nodeB:**
```
박성우: "다른 기억들은 그냥 힘들어요."
        "근데 이건... 숨이 막혀요."
        "이걸 보면 며칠 동안 아무것도 못 해요."

선택지:
  [A] "이동현 씨가 어떤 사람인지 말해줄 수 있어요?"  [tags: ColleagueRecognition]
  [B] "그 기억이 지금 박성우 씨한테 뭘 남겼나요?"   [tags: Interpretation]
  [C] "이 기억은 여기서 마무리하겠습니다."            [isProcessOption: true]
```

---

### soldier_war_order — 철수 명령 [오염]

**rootNode:**
```
박성우: "명령이었어요."
        "철수 명령."
        "...그냥 따랐어요."

선택지:
  [A] "그때 다른 선택을 할 수 있었다고 생각하세요?" [tags: Interpretation]
  [B] "그 순간 어떤 기분이었나요?"                  [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
```

**nodeA:**
```
박성우: "..."
        "없었어요. 없었다고 생각해요."
        "근데 왜 이렇게 생각이 나는지."

선택지:
  [A] "명령을 따른 게 잘못이 아니에요."            [tags: Respect]
  [B] "그 생각이 지금 어떤 형태로 남아있나요?"      [tags: Emotion]
  [C] "이 기억은 여기서 마무리하겠습니다."           [isProcessOption: true]
```

**nodeA-B:**
```
박성우: "그냥 있어요."
        "뭔가 해야 했는데, 하는 생각."
        "뭘 해야 했는지는 모르겠어요."

선택지:
  [A] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [B] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."  [isProcessOption: true]
```

---

### soldier_family_return — 귀환

**rootNode:**
```
박성우: "집에 왔을 때 애가 뛰어나왔어요."
        "근데 저는 그냥 서 있었어요."
        "웃어야 하는데."

선택지:
  [A] "그때 어떤 생각을 하셨나요?"               [tags: Emotion]
  [B] "지금은 다른가요?"                          [tags: Interpretation]
  [C] "이 기억은 여기서 마무리하겠습니다."         [isProcessOption: true]
```

**nodeA:**
```
박성우: "아무 생각도 안 났어요."
        "그냥 여기가 집이구나."
        "근데 여기가 맞는지 모르겠다는."

선택지:
  [A] "지금도 그런 느낌이 드세요?"               [tags: Emotion]
  [B] "이 기억은 여기서 마무리하겠습니다."         [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."         [isProcessOption: true]
```

---

### soldier_family_son — 아들의 얼굴

**rootNode:**
```
박성우: "얘가 저한테 그러더라고요."
        "아빠 왜 밤에 소리 질러요."
        "일곱 살짜리가."

선택지:
  [A] "그 말이 어떻게 느껴지셨어요?"            [tags: Emotion]
  [B] "그래서 여기 오신 건가요?"                [tags: Interpretation]
  [C] "이 기억은 여기서 마무리하겠습니다."       [isProcessOption: true]
```

**nodeB:**
```
박성우: "..."
        "네."
        "저 때문에 걔가 힘들면 안 되니까."

선택지:
  [A] "아들을 위해서 오신 거군요."              [tags: Emotion]
  [B] "본인을 위해서도 괜찮아요."               [tags: Respect]
  [C] "이 기억은 여기서 마무리하겠습니다."       [isProcessOption: true]
```

---

### soldier_guilt_survive — 생존자의 죄책감 [오염]

**rootNode:**
```
박성우: "왜 저만 왔을까요."
        "그냥 그 생각이에요."
        "이유가 없어요. 그냥."

선택지:
  [A] "그 질문에 답을 찾으려 하셨나요?"         [tags: Interpretation]
  [B] "그 생각이 얼마나 자주 드나요?"           [tags: Fact]
  [C] "이 기억은 여기서 마무리하겠습니다."       [isProcessOption: true]
```

**nodeA:**
```
박성우: "계속요."
        "근데 없어요. 답이."
        "그게 더 힘든 것 같아요."

선택지:
  [A] "답이 없는 게 맞을 수도 있어요."          [tags: Respect]
  [B] "이동현 씨는 뭐라고 할 것 같아요?"        [tags: ColleagueRecognition]
  [C] "이 기억은 여기서 마무리하겠습니다."       [isProcessOption: true]
```

**nodeA-B:**
```
박성우: "..."
        "모르겠어요."
        "(긴 침묵)"
        "그 자식이 뭐라고 할지는."
        "생각해본 적 없어요."

선택지:
  [A] "한번 생각해봐도 괜찮을 것 같아요."       [tags: Interpretation]
  [B] "이 기억은 여기서 마무리하겠습니다."       [isProcessOption: true]
  [C] "이 기억은 여기서 마무리하겠습니다."       [isProcessOption: true]
```

---

## VN — 치료 후 대화

```
[배경: 진료실. 치료 후.]

박성우: 다 됐어요?

주인공: 네. 어떠세요?

박성우: (잠시 생각하다)
        모르겠어요. 아직은.

주인공: 그래도 됩니다.
```

---

## 에필로그 텍스트

**soldier_embrace_colleague**
```
그해 11월, 그는 처음으로 전우회에 나갔다.
이동현의 어머니가 아직 거기 나오신다는 말을 들었다며.
된장찌개 얘기를 전해드렸다고 했다.
```

**soldier_release_colleague**
```
악몽의 빈도는 줄었다.
가끔 이동현이라는 이름을 중얼거리다 멈춘다.
누구냐고 아들이 물으면, 친구였다고만 한다.
```

**soldier_embrace_no_colleague**
```
여전히 힘든 날이 있다.
하지만 아들이 뛰어올 때
이제는 무릎을 꿇고 안아준다.
```

**soldier_release_no_colleague**
```
편해졌다고 했다.
조금 공허하다고도 했다.
뭘 잃어버린 건지는 모르겠다고.
```

**soldier_retrauma**
```
치료 후 한동안 연락이 닿지 않았다.
석 달 뒤 짧은 문자가 왔다.
'좀 더 천천히 할걸 그랬어요.'
```
