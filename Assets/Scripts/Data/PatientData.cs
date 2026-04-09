using System;
using UnityEngine;

public enum PatientType { Soldier, Elder, Child }

[Serializable]
public class VNLine
{
    public string speakerName;
    [TextArea(2, 5)]
    public string line;
    public Sprite backgroundSprite;
    public Sprite characterSprite; // null이면 캐릭터 숨김
}

[CreateAssetMenu(fileName = "PatientData", menuName = "TheArchivist/PatientData")]
public class PatientData : ScriptableObject
{
    [Header("기본 정보")]
    public string patientName;
    public PatientType patientType;

    [Header("처리 선택지 레이블")]
    public string optionALabel; // 덜어낸다 / 남긴다 / 드러낸다
    public string optionBLabel; // 끌어안는다 / 제거한다 / 그대로 둔다

    [Header("기억 조각")]
    public MemoryPieceData[] pieces;
    public Vector2Int gridSize; // 예: (3,3) → 3×3 분할

    [Header("퍼즐 이미지")]
    public Texture2D memoryImage;    // 기본 이미지 (분할 원본)
    public Texture2D memoryImageAlt; // 처리 후 이미지 (아이 전용: 진실 버전)

    [Header("VN - 치료 전 대화")]
    public VNLine[] introLines;

    [Header("VN - 치료 후 대화")]
    public VNLine[] outroLines;

    [Header("퍼즐 배경")]
    public Sprite puzzleBackground;

    [Header("알츠하이머 전용: 프레임 최대 조각 수")]
    public int maxPiecesInFrame = -1; // -1이면 제한 없음
}
