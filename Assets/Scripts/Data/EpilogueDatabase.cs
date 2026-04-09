using System;
using UnityEngine;

[Serializable]
public class EpilogueEntry
{
    public string endingId;
    public VNLine[] lines;
}

[CreateAssetMenu(fileName = "EpilogueDatabase", menuName = "TheArchivist/EpilogueDatabase")]
public class EpilogueDatabase : ScriptableObject
{
    [Header("게임 인트로")]
    public VNLine[] introLines;

    [Header("에필로그 (endingId → VNLine[])")]
    public EpilogueEntry[] entries;

    public VNLine[] GetLines(string endingId)
    {
        foreach (var entry in entries)
            if (entry.endingId == endingId)
                return entry.lines;
        Debug.LogWarning($"[EpilogueDatabase] 엔딩 ID '{endingId}'를 찾을 수 없습니다.");
        return null;
    }
}
