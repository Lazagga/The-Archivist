using System.Collections.Generic;
using UnityEngine;

// 각 조각의 처리 결과
public class PieceRecord
{
    public string pieceId;
    public PieceProcessState processState;
    public List<DialogueTag> usedTags = new List<DialogueTag>();
    public int dialogueCount; // 이 조각에서 나눈 대화 횟수
}

// 각 환자의 최종 기록
public class PatientRecord
{
    public PatientType patientType;
    public List<PieceRecord> pieceRecords = new List<PieceRecord>();

    public int GetTagCount(DialogueTag tag)
    {
        int count = 0;
        foreach (var r in pieceRecords)
            foreach (var t in r.usedTags)
                if (t == tag) count++;
        return count;
    }

    public int GetTotalDialogueCount()
    {
        int count = 0;
        foreach (var r in pieceRecords)
            count += r.dialogueCount;
        return count;
    }

    public int GetProcessCount(PieceProcessState state)
    {
        int count = 0;
        foreach (var r in pieceRecords)
            if (r.processState == state) count++;
        return count;
    }
}

public class EndingTracker : MonoBehaviour
{
    public static EndingTracker Instance { get; private set; }

    // 환자별 기록
    readonly Dictionary<PatientType, PatientRecord> records
        = new Dictionary<PatientType, PatientRecord>();

    // 현재 진행 중인 환자 기록
    PatientRecord currentRecord;
    // 현재 진행 중인 조각 기록
    PieceRecord currentPiece;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ── 환자 시작 ──────────────────────────────
    public void BeginPatient(PatientType type)
    {
        currentRecord = new PatientRecord { patientType = type };
        records[type] = currentRecord;
    }

    // ── 조각 선택 시작 ─────────────────────────
    public void BeginPiece(string pieceId)
    {
        currentPiece = new PieceRecord { pieceId = pieceId };
    }

    // ── 대화 선택지 선택 시 태그 기록 ──────────
    public void RecordTags(DialogueTag[] tags)
    {
        if (currentPiece == null) return;
        currentPiece.dialogueCount++;
        foreach (var tag in tags)
            if (tag != DialogueTag.None)
                currentPiece.usedTags.Add(tag);
    }

    // ── 처리 결정 ──────────────────────────────
    public void RecordProcess(PieceProcessState state)
    {
        if (currentPiece == null) return;
        currentPiece.processState = state;
        currentRecord?.pieceRecords.Add(currentPiece);
        currentPiece = null;
    }

    // 조각 대화를 끝내지 않고 다른 조각으로 이동 시 현재 조각 저장
    public void PausePiece()
    {
        if (currentPiece != null && currentRecord != null)
        {
            // 아직 미처리된 조각은 기록에 보류 (pieceId로 나중에 업데이트)
        }
    }

    // ── 엔딩 계산 ──────────────────────────────
    public string GetPatientEnding(PatientType type)
    {
        if (!records.TryGetValue(type, out var record)) return "default";

        switch (type)
        {
            case PatientType.Soldier:   return CalcSoldierEnding(record);
            case PatientType.Elder:     return CalcElderEnding(record);
            case PatientType.Child:     return CalcChildEnding(record);
        }
        return "default";
    }

    // ── 군인 엔딩 ──────────────────────────────
    string CalcSoldierEnding(PatientRecord r)
    {
        bool hasColleagueTag = r.GetTagCount(DialogueTag.ColleagueRecognition) > 0;
        bool hasRespectTag   = r.GetTagCount(DialogueTag.Respect) > 0;
        int embraceCount     = r.GetProcessCount(PieceProcessState.OptionB);
        int releaseCount     = r.GetProcessCount(PieceProcessState.OptionA);
        int totalDialogue    = r.GetTotalDialogueCount();

        // 재트라우마: 거부반응에도 계속 파고들었음 (Respect 태그 없이 대화 과다)
        if (!hasRespectTag && totalDialogue > 10)
            return "soldier_retrauma";

        if (hasColleagueTag && embraceCount >= releaseCount)
            return "soldier_embrace_colleague"; // 끌어안음 + 동료 인식
        if (hasColleagueTag && releaseCount > embraceCount)
            return "soldier_release_colleague"; // 덜어냄 + 동료 인식
        if (!hasColleagueTag && embraceCount >= releaseCount)
            return "soldier_embrace_no_colleague";
        return "soldier_release_no_colleague";
    }

    // ── 노인 엔딩 ──────────────────────────────
    string CalcElderEnding(PatientRecord r)
    {
        bool discoveredValue = r.GetTagCount(DialogueTag.PatientValue) > 0;
        int keptCount        = r.GetProcessCount(PieceProcessState.OptionA);

        if (!discoveredValue)
            return "elder_arbitrary"; // 대화 없이 임의로 남김

        // 어떤 기억을 남겼는지는 PieceRecord에서 확인
        var keptPieces = new List<string>();
        foreach (var p in r.pieceRecords)
            if (p.processState == PieceProcessState.OptionA)
                keptPieces.Add(p.pieceId);

        // pieceId 규칙: "elder_daughter_*", "elder_wife_*", "elder_career_*"
        bool keptDaughter = keptPieces.Exists(id => id.Contains("daughter"));
        bool keptWife     = keptPieces.Exists(id => id.Contains("wife"));
        bool keptCareer   = keptPieces.Exists(id => id.Contains("career"));

        if (keptDaughter) return "elder_kept_daughter";
        if (keptWife)     return "elder_kept_wife";
        if (keptCareer)   return "elder_kept_career";
        return "elder_kept_misc";
    }

    // ── 아이 엔딩 ──────────────────────────────
    string CalcChildEnding(PatientRecord r)
    {
        int revealCount      = r.GetProcessCount(PieceProcessState.OptionA); // 드러낸다
        int keepCount        = r.GetProcessCount(PieceProcessState.OptionB); // 그대로 둔다
        int totalDialogue    = r.GetTotalDialogueCount();
        bool hasEmotionTag   = r.GetTagCount(DialogueTag.KeyEmotion) > 0;

        // 대화 없이 드러냄 → 충격
        if (revealCount > 0 && totalDialogue < 3)
            return "child_shock";

        if (revealCount > 0 && hasEmotionTag)
            return "child_overcome"; // 드러냄 + 감정 파악 → 극복
        if (revealCount > 0 && !hasEmotionTag)
            return "child_revealed_alone"; // 드러냄 but 감정 지원 없음
        if (revealCount == 0 && totalDialogue >= 4)
            return "child_kept_supported"; // 그대로 뒀지만 대화 충분
        return "child_unchanged"; // 아무것도 안 함
    }

    // ── 주인공 엔딩 계산 ───────────────────────
    public string GetMainCharacterEnding()
    {
        int embraceTotal = 0;
        int releaseTotal = 0;
        int revealTotal  = 0;

        foreach (var record in records.Values)
        {
            embraceTotal += record.GetProcessCount(PieceProcessState.OptionB);
            releaseTotal += record.GetProcessCount(PieceProcessState.OptionA);
        }

        if (embraceTotal > releaseTotal + 2)
            return "protagonist_exhausted";    // 너무 많이 끌어안음
        if (releaseTotal > embraceTotal + 2)
            return "protagonist_efficient";    // 효율적이었지만 공허함
        return "protagonist_balanced";         // 균형
    }
}
