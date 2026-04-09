using System;
using UnityEngine;

public enum DialogueTag
{
    None,
    Fact,           // 사실 질문
    Emotion,        // 감정 질문
    Interpretation, // 해석 질문
    Respect,        // 존중 (거부반응 시 멈춤 등)
    ColleagueRecognition, // 동료를 인간으로 바라봄 (군인 전용)
    KeyEmotion,     // 아이의 감정을 물어봄 (아이 전용)
    PatientValue,   // 노인이 소중히 여기는 것 파악 (노인 전용)
}

[Serializable]
public class DialogueChoice
{
    [TextArea(1, 3)]
    public string choiceText;
    public DialogueNodeData nextNode; // null이면 대화 종료
    public DialogueTag[] tags;
    public bool isProcessTrigger; // true면 이 선택 후 처리 화면으로 전환
}

[CreateAssetMenu(fileName = "DialogueNode", menuName = "TheArchivist/DialogueNode")]
public class DialogueNodeData : ScriptableObject
{
    public string nodeId;

    [TextArea(2, 6)]
    public string patientLine;

    // 항상 3개. 마지막은 처리 트리거로 설계
    public DialogueChoice[] choices = new DialogueChoice[3];
}
