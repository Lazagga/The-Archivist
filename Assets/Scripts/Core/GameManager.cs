using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Title, VN, Puzzle }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("환자 데이터 (순서대로)")]
    public PatientData[] patients;

    [Header("인트로 / 에필로그 데이터")]
    public EpilogueDatabase epilogueDatabase;

    [Header("매니저 참조")]
    public VNManager vnManager;
    public PuzzleManager puzzleManager;
    public EndingTracker endingTracker;
    public TitleManager titleManager;

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
        // 씬 시작 시 페이드 캔버스를 불투명하게 초기화 (FadeIn 시작점 보장)
        fadeCanvas.alpha = 1f;
        fadeCanvas.gameObject.SetActive(true);

        // 타이틀: 엔딩을 한 번이라도 봤으면 저녁, 아니면 아침
        bool isEvening = PlayerPrefs.GetInt("HasCompletedGame", 0) == 1;
        yield return ChangeState(GameState.Title);
        titleManager.Show(isEvening);
        yield return FadeIn();
        yield return titleManager.WaitForStart();
        titleManager.Hide();
        yield return FadeOut();

        // 게임 인트로
        yield return ChangeState(GameState.VN);
        if (epilogueDatabase != null && epilogueDatabase.introLines.Length > 0)
            yield return vnManager.PlayLines(epilogueDatabase.introLines);

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

        // 에필로그
        yield return FadeOut();
        yield return ChangeState(GameState.VN);
        yield return FadeIn();

        if (epilogueDatabase != null)
        {
            // 환자별 에필로그 순서대로 재생
            foreach (var patient in patients)
            {
                string endingId = endingTracker.GetPatientEnding(patient.patientType);
                var lines = epilogueDatabase.GetLines(endingId);
                if (lines != null && lines.Length > 0)
                {
                    yield return vnManager.PlayLines(lines);
                    yield return FadeOut();
                    yield return FadeIn();
                }
            }

            // 주인공 에필로그
            string protagonistEndingId = endingTracker.GetMainCharacterEnding();
            var protagonistLines = epilogueDatabase.GetLines(protagonistEndingId);
            if (protagonistLines != null && protagonistLines.Length > 0)
                yield return vnManager.PlayLines(protagonistLines);
        }

        // 엔딩 완료 플래그 저장 후 씬 리로드 → 저녁 타이틀로 복귀
        PlayerPrefs.SetInt("HasCompletedGame", 1);
        PlayerPrefs.Save();
        yield return FadeOut();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ChangeState(GameState state)
    {
        CurrentState = state;
        vnManager.SetActive(state == GameState.VN);
        puzzleManager.SetActive(state == GameState.Puzzle);
        // Title은 titleManager.Show/Hide로 직접 제어
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
