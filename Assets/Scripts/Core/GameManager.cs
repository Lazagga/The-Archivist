using System.Collections;
using UnityEngine;

public enum GameState { VN, Puzzle, Ending }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("환자 데이터 (순서대로)")]
    public PatientData[] patients;

    [Header("매니저 참조")]
    public VNManager vnManager;
    public PuzzleManager puzzleManager;
    public EndingTracker endingTracker;

    [Header("씬 전환 캔버스")]
    public CanvasGroup fadeCanvas;

    public GameState CurrentState { get; private set; }
    public PatientData CurrentPatient => patientIndex < patients.Length ? patients[patientIndex] : null;

    int patientIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartCoroutine(RunGame());
    }

    IEnumerator RunGame()
    {
        yield return FadeIn();

        // 인트로 (선택적으로 VN 라인 추가 가능)
        // yield return vnManager.PlayLines(introLines);

        for (patientIndex = 0; patientIndex < patients.Length; patientIndex++)
        {
            var patient = patients[patientIndex];
            endingTracker.BeginPatient(patient.patientType);

            // VN - 치료 전 대화
            yield return ChangeState(GameState.VN);
            yield return vnManager.PlayLines(patient.introLines);

            // 전환 연출
            yield return FadeOut();
            yield return ChangeState(GameState.Puzzle);
            yield return FadeIn();

            // 퍼즐
            puzzleManager.LoadPatient(patient);
            yield return new WaitUntil(() => puzzleManager.IsComplete);

            // 전환 연출
            yield return FadeOut();
            yield return ChangeState(GameState.VN);
            yield return FadeIn();

            // VN - 치료 후 대화
            yield return vnManager.PlayLines(patient.outroLines);
        }

        // 엔딩
        yield return FadeOut();
        yield return ChangeState(GameState.Ending);
        yield return FadeIn();

        // TODO: 엔딩 씬 연출
        Debug.Log("Main ending: " + endingTracker.GetMainCharacterEnding());
    }

    IEnumerator ChangeState(GameState state)
    {
        CurrentState = state;
        vnManager.SetActive(state == GameState.VN);
        puzzleManager.SetActive(state == GameState.Puzzle);
        yield return null;
    }

    public IEnumerator FadeOut(float duration = 0.8f)
    {
        float t = 0;
        fadeCanvas.gameObject.SetActive(true);
        while (t < duration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = t / duration;
            yield return null;
        }
        fadeCanvas.alpha = 1f;
    }

    public IEnumerator FadeIn(float duration = 0.8f)
    {
        float t = duration;
        fadeCanvas.gameObject.SetActive(true);
        while (t > 0)
        {
            t -= Time.deltaTime;
            fadeCanvas.alpha = t / duration;
            yield return null;
        }
        fadeCanvas.alpha = 0f;
        fadeCanvas.gameObject.SetActive(false);
    }
}
