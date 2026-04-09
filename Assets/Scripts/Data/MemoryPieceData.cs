using System;
using UnityEngine;

public enum PieceProcessState
{
    Unprocessed,
    OptionA, // 덜어낸다 / 남긴다 / 드러낸다
    OptionB, // 끌어안는다 / 제거한다 / 그대로 둔다
}

[CreateAssetMenu(fileName = "MemoryPieceData", menuName = "TheArchivist/MemoryPieceData")]
public class MemoryPieceData : ScriptableObject
{
    public string pieceId;
    public string pieceName;

    // 스프라이트는 PatientData.memoryImage를 gridPosition 기준으로 분할해서 사용
    // 개별 스프라이트 지정 불필요

    [Header("그리드 위치")]
    public Vector2Int gridPosition; // (col, row), 0부터 시작

    [Header("연결 조각")]
    // 이 조각을 OptionA로 처리했을 때 흐릿해지는 조각 IDs
    public string[] connectedPieceIds;

    [Header("대화 트리")]
    public DialogueNodeData rootNode;

    [Header("처리 미리보기 텍스트")]
    [TextArea(1, 3)]
    public string optionAPreview; // "고통이 줄어듭니다\n동료 기억이 흐릿해집니다"
    [TextArea(1, 3)]
    public string optionBPreview;
}
