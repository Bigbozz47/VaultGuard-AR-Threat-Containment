using UnityEngine;
using UnityEngine.UI;
using TMPro; // Diperlukan untuk TextMeshPro
using System.Collections; // Diperlukan untuk Coroutine (polesan tombol)

/// <summary>
/// Mengontrol semua elemen UI: panel, teks, dan tombol, sesuai Panduan Tabel 2.
/// Menerima input tombol dan meneruskannya ke GameManager.
/// Tidak berisi logika game, hanya mengontrol tampilan.
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Panel (Hubungkan di Inspector)")]
    [SerializeField] private GameObject panelKuis;
    [SerializeField] private GameObject panelSkor;
    [SerializeField] private GameObject panelLoading;

    [Header("Panel Kuis (Hubungkan di Inspector)")]
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button[] answerButtons; // Menggunakan array lebih bersih

    [Header("Panel Skor (Hubungkan di Inspector)")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Polesan & Animasi (Opsional)")]
    [SerializeField] private Animator panelKuisAnimator;
    [SerializeField] private Animator panelSkorAnimator;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private float highlightDuration = 1.5f;

    private Color defaultButtonColor;

    #region Unity Lifecycle

    void Start()
    {
        // Simpan warna default tombol
        if (answerButtons != null && answerButtons.Length > 0)
        {
            defaultButtonColor = answerButtons[0].GetComponent<Image>().color;
        }

        // Hubungkan tombol ke fungsi (Panduan Hari 4)
        // Ini adalah cara yang bersih dan skalabel
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i; // Penting untuk menghindari masalah closure pada lambda
            answerButtons[i].onClick.AddListener(() => OnAnswerClicked(index));
        }

        // Pastikan semua panel tersembunyi saat mulai
        if (panelKuis) panelKuis.SetActive(false);
        if (panelSkor) panelSkor.SetActive(false);
        if (panelLoading) panelLoading.SetActive(false);
    }

    #endregion

    #region Kontrol Panel Utama

    /// <summary>
    /// Menampilkan panel kuis dan mengisi data dari kuis yang dimuat.
    /// Sesuai Panduan Hari 4.
    /// </summary>
    public void ShowKuisPanel(QuizData kuis)
    {
        if (kuis == null)
        {
            Debug.LogError("UIManager: Menerima Kuis null!");
            return;
        }
        
        // Sembunyikan panel lain
        if (panelSkor) panelSkor.SetActive(false);
        if (panelLoading) panelLoading.SetActive(false);

        // Isi data kuis
        questionText.text = kuis.pertanyaan;
        
        // Isi teks tombol jawaban
        // Asumsi QuizData memiliki field pilihan_a, pilihan_b, dst.
        string[] answers = { kuis.pilihan_a, kuis.pilihan_b, kuis.pilihan_c, kuis.pilihan_d };
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < answers.Length)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answers[i];
                answerButtons[i].gameObject.SetActive(true);
            }
            else
            {
                answerButtons[i].gameObject.SetActive(false); // Sembunyikan tombol jika tidak ada data
            }
        }
        
        ResetButtonColors(); // Reset warna tombol dari kuis sebelumnya

        // Tampilkan panel
        if (panelKuisAnimator != null)
        {
            panelKuisAnimator.SetTrigger("Show"); // Polesan Hari 8
        }
        else
        {
            panelKuis.SetActive(true);
        }
    }

    /// <summary>
    /// Menampilkan panel hasil (Benar/Salah) dan memperbarui skor.
    /// Sesuai Panduan Hari 4.
    /// </summary>
    public void ShowResult(bool isCorrect, int newScore)
    {
        // Sembunyikan panel kuis
        if (panelKuisAnimator != null)
        {
            panelKuisAnimator.SetTrigger("Hide");
        }
        else
        {
            if (panelKuis) panelKuis.SetActive(false);
        }

        // Isi data panel skor
        statusText.text = isCorrect ? "BENAR!" : "SALAH!";
        scoreText.text = "Skor: " + newScore;
        
        // Tampilkan panel skor
        if (panelSkorAnimator != null)
        {
            panelSkorAnimator.SetTrigger("Show");
        }
        else
        {
            if (panelSkor) panelSkor.SetActive(true);
        }
    }

    /// <summary>
    /// Mengontrol visibilitas panel loading (untuk panggilan API).
    /// Sesuai Panduan Hari 6.
    /// </summary>
    public void ShowLoading(bool isActive)
    {
        if (panelLoading) panelLoading.SetActive(isActive);
    }

    #endregion

    #region Input Handler

    /// <summary>
    /// Dipanggil secara internal oleh listener tombol.
    /// Meneruskan event klik ke GameManager.
    /// </summary>
    /// <param name="index">Index tombol (0-3).</param>
    private void OnAnswerClicked(int index)
    {
        // UIManager tidak boleh tahu logikanya,
        // hanya memberi tahu GameManager jawaban apa yang dipilih.
        GameManager.Instance.OnAnswerSelected(index);
    }

    #endregion

    #region Polesan (Highlight & Coroutine)

    /// <summary>
    /// Polesan Hari 8: Mengubah warna tombol yang dipilih.
    /// </summary>
    public void HighlightButton(int selectedIndex, bool isCorrect)
    {
        Color colorToUse = isCorrect ? correctColor : wrongColor;
        
        if (selectedIndex >= 0 && selectedIndex < answerButtons.Length)
        {
            answerButtons[selectedIndex].GetComponent<Image>().color = colorToUse;
        }
        
        // Mulai Coroutine untuk mengembalikan warna setelah beberapa saat
        StartCoroutine(ResetButtonsAfterDelay(highlightDuration));
    }

    private void ResetButtonColors()
    {
        foreach (Button btn in answerButtons)
        {
            btn.GetComponent<Image>().color = defaultButtonColor;
        }
    }

    private IEnumerator ResetButtonsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // ResetButtonColors(); // Dihapus: Reset terjadi saat panel kuis baru muncul
    }

    #endregion
}