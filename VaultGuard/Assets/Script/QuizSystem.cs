using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizSystem : MonoBehaviour
{
    [System.Serializable]
    public class Quiz
    {
        public string markerName;

        public string question;
        public string[] choices = new string[4];
        public int correctIndex;

        [Header("GAME OBJECTS")]
        public GameObject monsterObject;      // Virus Dracula (model)
        public GameObject jailObject;         // Penjara (get_caught)

        [Header("ANIMATOR")]
        public Animator monsterAnimator;      // Animator di Virus Dracula
    }

    [Header("DATA")]
    public Quiz[] quizzes;

    [Header("UI REFERENCES")]
    public GameObject panelQuiz;
    public TMP_Text textQuestion;
    public TMP_Text[] textChoices;
    public Button[] buttonChoices;

    [Header("OTHER")]
    public TMP_Text textScore;
    private int score = 0;
    private Quiz currentQuiz;


    void Start()
    {
        // Pastikan semua penjara OFF di awal
        foreach (var q in quizzes)
        {
            if (q.jailObject != null)
                q.jailObject.SetActive(false);
        }
    }

    // ----------------------------------------------------

    public void OpenQuiz(string marker)
    {
        currentQuiz = null;

        // cari quiz berdasarkan nama marker
        foreach (var q in quizzes)
        {
            if (q.markerName == marker)
            {
                currentQuiz = q;
                break;
            }
        }

        if (currentQuiz == null)
        {
            Debug.LogWarning("Marker tidak ditemukan di QuizSystem: " + marker);
            return;
        }

        // tampilkan panel dan isi UI
        panelQuiz.SetActive(true);
        textQuestion.text = currentQuiz.question;

        for (int i = 0; i < textChoices.Length && i < currentQuiz.choices.Length; i++)
        {
            textChoices[i].text = currentQuiz.choices[i];
        }

        // set listener tombol jawaban
        for (int i = 0; i < buttonChoices.Length; i++)
        {
            int index = i;
            buttonChoices[i].onClick.RemoveAllListeners();
            buttonChoices[i].onClick.AddListener(() => Answer(index));
        }
    }

    // ----------------------------------------------------

    public void Answer(int index)
    {
        if (currentQuiz == null) return;

        // JAWABAN BENAR
        if (index == currentQuiz.correctIndex)
        {
            // tambah skor
            score += 10;
            if (textScore != null)
                textScore.text = score.ToString();

            // aktifkan penjara
            if (currentQuiz.jailObject != null)
                currentQuiz.jailObject.SetActive(true);

            // monster hilang
            if (currentQuiz.monsterObject != null)
                currentQuiz.monsterObject.SetActive(false);
        }
        // JAWABAN SALAH
        else
        {
            // mainkan animasi ESCAPE
            if (currentQuiz.monsterAnimator != null)
                currentQuiz.monsterAnimator.SetTrigger("Escape");

            // pastikan penjara tetap nonaktif
            if (currentQuiz.jailObject != null)
                currentQuiz.jailObject.SetActive(false);
        }

        // tutup panel kuis
        panelQuiz.SetActive(false);
        currentQuiz = null;
    }
}
