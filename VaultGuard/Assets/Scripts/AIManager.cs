using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Manager utama untuk menangani quiz system dengan AI integration
/// Bertanggung jawab untuk: loading quiz dari local backup, request quiz dari AI API,
/// validasi jawaban pemain, dan error handling dengan fallback mechanism
/// </summary>
public class AIManager : MonoBehaviour
{
    #region Singleton Pattern
    
    /// <summary>Singleton instance untuk global access</summary>
    public static AIManager Instance { get; private set; }
    
    #endregion
    
    #region API Configuration
    
    [Header("AI API Configuration")]
    [SerializeField] private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";
    [SerializeField] private string apiKey = ""; // ISI DI INSPECTOR, JANGAN HARDCODE!
    
    [Header("API Settings")]
    [SerializeField] private float requestTimeout = 15f;
    [SerializeField] private int maxRetries = 3;
    
    [Header("Demo Mode")]
    [SerializeField] private bool forceOfflineMode = false;
    
    #endregion
    
    #region Private Variables
    
    private QuizDatabase quizDatabase;
    private const string BACKUP_FILE_NAME = "quiz_backup";
    private int activeRequests = 0;
    private const int MAX_CONCURRENT_REQUESTS = 3;
    private StringBuilder promptBuilder;
    
    #endregion
    
    #region Unity Lifecycle
    
    void Awake()
    {
        // Singleton implementation dengan DontDestroyOnLoad
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeQuizDatabase();
            promptBuilder = new StringBuilder(1024);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    #region Initialization
    
    /// <summary>
    /// Inisialisasi database quiz dari Resources folder
    /// </summary>
    private void InitializeQuizDatabase()
    {
        try
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(BACKUP_FILE_NAME);
            
            if (jsonFile != null)
            {
                quizDatabase = JsonUtility.FromJson<QuizDatabase>(jsonFile.text);
                
                if (quizDatabase != null && quizDatabase.daftar_quiz != null)
                {
                    VaultGuardLogger.LogSuccess("AIManager", $"Quiz database loaded successfully. Total quiz: {quizDatabase.daftar_quiz.Count}");
                }
                else
                {
                    VaultGuardLogger.LogError("AIManager", "Quiz database structure invalid!");
                }
            }
            else
            {
                VaultGuardLogger.LogError("AIManager", "quiz_backup.json tidak ditemukan di Resources folder!");
            }
        }
        catch (System.Exception e)
        {
            VaultGuardLogger.LogError("AIManager", $"Error loading quiz database: {e.Message}");
        }
    }
    
    #endregion
    
    #region Local Quiz Functions
    
    /// <summary>
    /// Load quiz berdasarkan topik virus tertentu dari database lokal
    /// Fungsi ini akan dipanggil oleh GameManager atau sebagai fallback jika AI gagal
    /// </summary>
    /// <param name="topikVirus">Nama topik virus (Phishing, Ransomware, Trojan, dll)</param>
    /// <returns>QuizData atau null jika tidak ditemukan</returns>
    public QuizData LoadQuiz(string topikVirus)
    {
        // Validasi input
        if (string.IsNullOrWhiteSpace(topikVirus))
        {
            VaultGuardLogger.LogError("AIManager", "Topic virus tidak boleh kosong!");
            return null;
        }
        
        // Sanitize input
        topikVirus = topikVirus.Trim();
        
        if (quizDatabase == null || quizDatabase.daftar_quiz == null)
        {
            VaultGuardLogger.LogError("AIManager", "Quiz database belum diinisialisasi!");
            return null;
        }

        // Cari quiz yang sesuai dengan topik
        QuizData quiz = quizDatabase.daftar_quiz.Find(q => q.topik_virus == topikVirus);

        if (quiz != null)
        {
            VaultGuardLogger.Log("AIManager", $"Quiz loaded for topic '{topikVirus}': {quiz.pertanyaan}");
            return quiz;
        }
        else
        {
            VaultGuardLogger.LogWarning("AIManager", $"Quiz dengan topik '{topikVirus}' tidak ditemukan! Using random fallback.");
            // Fallback: return quiz random
            return GetRandomQuiz();
        }
    }

    /// <summary>
    /// Get random quiz dari database (fallback mechanism)
    /// Digunakan ketika topik spesifik tidak ditemukan atau untuk variasi
    /// </summary>
    /// <returns>QuizData random atau null jika database kosong</returns>
    public QuizData GetRandomQuiz()
    {
        if (quizDatabase == null || quizDatabase.daftar_quiz == null || quizDatabase.daftar_quiz.Count == 0)
        {
            VaultGuardLogger.LogError("AIManager", "Tidak ada quiz di database!");
            return null;
        }

        int randomIndex = Random.Range(0, quizDatabase.daftar_quiz.Count);
        QuizData quiz = quizDatabase.daftar_quiz[randomIndex];
        
        VaultGuardLogger.Log("AIManager", $"Random quiz selected: {quiz.pertanyaan}");
        
        return quiz;
    }

    /// <summary>
    /// Alias untuk LoadQuiz() sesuai dengan naming convention
    /// Load quiz dari database lokal (backup) ketika AI tidak tersedia atau gagal
    /// </summary>
    /// <param name="topikVirus">Nama topik virus</param>
    /// <returns>QuizData dari local database</returns>
    public QuizData LoadQuizLokal(string topikVirus)
    {
        VaultGuardLogger.Log("AIManager", $"LoadQuizLokal() called for topic: {topikVirus}");
        return LoadQuiz(topikVirus);
    }
    
    /// <summary>
    /// Overload tanpa parameter - return random quiz dari local database
    /// Berguna untuk fallback atau testing
    /// </summary>
    /// <returns>Random QuizData dari local database</returns>
    public QuizData LoadQuizLokal()
    {
        VaultGuardLogger.Log("AIManager", "LoadQuizLokal() called without parameter - returning random quiz");
        return GetRandomQuiz();
    }

    /// <summary>
    /// Validasi jawaban pemain dengan quiz data
    /// Case insensitive untuk fleksibilitas (A = a)
    /// </summary>
    /// <param name="quiz">QuizData yang sedang aktif</param>
    /// <param name="jawabanPemain">Jawaban pemain (A, B, C, atau D)</param>
    /// <returns>true jika benar, false jika salah</returns>
    public bool ValidasiJawaban(QuizData quiz, string jawabanPemain)
    {
        if (quiz == null)
        {
            VaultGuardLogger.LogError("AIManager", "Quiz data null! Tidak bisa validasi jawaban.");
            return false;
        }
        
        if (string.IsNullOrWhiteSpace(jawabanPemain))
        {
            VaultGuardLogger.LogError("AIManager", "Jawaban pemain kosong!");
            return false;
        }

        bool isCorrect = quiz.jawaban_benar.ToUpper() == jawabanPemain.ToUpper();
        
        if (isCorrect)
        {
            VaultGuardLogger.LogSuccess("AIManager", $"Jawaban BENAR! Pemain: {jawabanPemain} | Correct: {quiz.jawaban_benar}");
        }
        else
        {
            VaultGuardLogger.Log("AIManager", $"Jawaban SALAH. Pemain: {jawabanPemain} | Correct: {quiz.jawaban_benar}");
        }
        
        return isCorrect;
    }
    
    #endregion
    
    #region AI API Functions
    
    /// <summary>
    /// Request quiz dari AI API dengan fallback ke backup
    /// Entry point untuk mendapatkan quiz dari Google Gemini API
    /// </summary>
    /// <param name="topikVirus">Topik virus untuk generate pertanyaan</param>
    /// <param name="onSuccess">Callback ketika berhasil mendapat quiz</param>
    /// <param name="onFailure">Callback ketika gagal (akan fallback ke local)</param>
    public void RequestQuizFromAI(string topikVirus, System.Action<QuizData> onSuccess, System.Action<string> onFailure)
    {
        // Check demo mode
        if (forceOfflineMode)
        {
            VaultGuardLogger.LogWarning("AIManager", "⚙️ DEMO MODE: Using local quiz only");
            StartCoroutine(FallbackToLocalQuiz(topikVirus, onSuccess));
            return;
        }
        
        // Check network
        if (!IsNetworkAvailable())
        {
            VaultGuardLogger.LogWarning("AIManager", "Network not available. Using local quiz.");
            onFailure?.Invoke("No network connection");
            StartCoroutine(FallbackToLocalQuiz(topikVirus, onSuccess));
            return;
        }
        
        // Check API configuration
        if (!IsAPIConfigured())
        {
            VaultGuardLogger.LogError("AIManager", "API not configured properly. Using local quiz.");
            onFailure?.Invoke("API not configured");
            StartCoroutine(FallbackToLocalQuiz(topikVirus, onSuccess));
            return;
        }
        
        // Check concurrent requests limit
        if (!CanMakeRequest())
        {
            VaultGuardLogger.LogWarning("AIManager", "Too many concurrent requests. Using local quiz.");
            onFailure?.Invoke("Rate limit exceeded");
            StartCoroutine(FallbackToLocalQuiz(topikVirus, onSuccess));
            return;
        }
        
        StartCoroutine(GetQuizFromAI(topikVirus, onSuccess, onFailure));
    }
    
    /// <summary>
    /// Request quiz dengan retry logic dan exponential backoff
    /// Lebih robust daripada RequestQuizFromAI standar
    /// </summary>
    /// <param name="topikVirus">Topik virus</param>
    /// <param name="onSuccess">Callback success</param>
    /// <param name="onFailure">Callback failure</param>
    public void RequestQuizWithRetry(string topikVirus, System.Action<QuizData> onSuccess, System.Action<string> onFailure)
    {
        StartCoroutine(GetQuizWithRetry(topikVirus, onSuccess, onFailure));
    }

    private IEnumerator GetQuizFromAI(string topik, System.Action<QuizData> onSuccess, System.Action<string> onFailure)
    {
        activeRequests++;
        
        // Validasi API Key
        if (string.IsNullOrEmpty(apiKey))
        {
            VaultGuardLogger.LogError("AIManager", "API Key tidak ditemukan! Menggunakan backup quiz.");
            onFailure?.Invoke("API Key tidak tersedia");
            yield return FallbackToLocalQuiz(topik, onSuccess);
            activeRequests--;
            yield break;
        }

        // Build prompt
        string prompt = BuildPrompt(topik);
        
        // Build request body (Gemini API format)
        string requestBody = BuildGeminiRequestBody(prompt);

        // Prepare UnityWebRequest
        string fullUrl = $"{apiEndpoint}?key={apiKey}";
        using (UnityWebRequest request = new UnityWebRequest(fullUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(requestBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.timeout = (int)requestTimeout;

            VaultGuardLogger.Log("AIManager", $"📤 Mengirim request ke AI untuk topik: {topik}");

            // Send request
            yield return request.SendWebRequest();

            // Handle response
            bool needsFallback = false;
            string errorMessage = "";
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string response = request.downloadHandler.text;
                    VaultGuardLogger.Log("AIManager", "📥 Response received dari AI");

                    QuizData quiz = ParseGeminiResponse(response);
                    
                    if (quiz != null)
                    {
                        quiz.topik_virus = topik; // Set topik
                        VaultGuardLogger.LogSuccess("AIManager", "Quiz berhasil di-parse dari AI!");
                        onSuccess?.Invoke(quiz);
                    }
                    else
                    {
                        needsFallback = true;
                        errorMessage = "Failed to parse quiz data";
                    }
                }
                catch (System.Exception e)
                {
                    VaultGuardLogger.LogError("AIManager", $"Error parsing response: {e.Message}");
                    onFailure?.Invoke(e.Message);
                    needsFallback = true;
                    errorMessage = e.Message;
                }
            }
            else
            {
                VaultGuardLogger.LogError("AIManager", $"API Request Failed: {request.error}");
                onFailure?.Invoke(request.error);
                needsFallback = true;
                errorMessage = request.error;
            }
            
            if (needsFallback)
            {
                yield return FallbackToLocalQuiz(topik, onSuccess);
            }
        }
        
        activeRequests--;
    }
    
    private IEnumerator GetQuizWithRetry(string topik, System.Action<QuizData> onSuccess, System.Action<string> onFailure)
    {
        int attempts = 0;
        bool success = false;

        while (attempts < maxRetries && !success)
        {
            attempts++;
            VaultGuardLogger.Log("AIManager", $"🔄 Attempt {attempts}/{maxRetries} untuk topik: {topik}");

            bool requestCompleted = false;
            QuizData resultQuiz = null;
            string errorMessage = "";

            // Inner callback untuk capture result
            StartCoroutine(GetQuizFromAI(
                topik,
                (quiz) => {
                    resultQuiz = quiz;
                    success = true;
                    requestCompleted = true;
                },
                (error) => {
                    errorMessage = error;
                    requestCompleted = true;
                }
            ));

            // Wait for request to complete
            yield return new WaitUntil(() => requestCompleted);

            if (success && resultQuiz != null)
            {
                onSuccess?.Invoke(resultQuiz);
                yield break;
            }
            else if (attempts < maxRetries)
            {
                // Exponential backoff
                float waitTime = Mathf.Pow(2, attempts);
                VaultGuardLogger.LogWarning("AIManager", $"⏳ Retry in {waitTime} seconds...");
                yield return new WaitForSeconds(waitTime);
            }
        }

        // If all retries failed
        if (!success)
        {
            VaultGuardLogger.LogError("AIManager", $"All {maxRetries} attempts failed. Using fallback.");
            onFailure?.Invoke($"Failed after {maxRetries} attempts");
            yield return FallbackToLocalQuiz(topik, onSuccess);
        }
    }

    /// <summary>
    /// Build prompt untuk AI dengan format strict JSON
    /// Prompt dirancang agar AI menghasilkan JSON yang valid dan sesuai format QuizData
    /// </summary>
    private string BuildPrompt(string topik)
    {
        promptBuilder.Clear();
        
        promptBuilder.AppendLine("Anda adalah asisten pembuat kuis keamanan siber untuk sebuah game edukasi AR.");
        promptBuilder.AppendLine("Tugas Anda adalah membuat 1 pertanyaan kuis pilihan ganda tentang keamanan siber.");
        promptBuilder.AppendLine();
        promptBuilder.AppendLine($"TOPIK: {topik}");
        promptBuilder.AppendLine();
        promptBuilder.AppendLine("PERSYARATAN:");
        promptBuilder.AppendLine("- Pertanyaan harus edukatif dan sesuai untuk pemain umum");
        promptBuilder.AppendLine("- Gunakan bahasa Indonesia yang baik dan benar");
        promptBuilder.AppendLine("- Hanya 1 jawaban yang benar");
        promptBuilder.AppendLine("- Hindari pertanyaan yang terlalu teknis");
        promptBuilder.AppendLine();
        promptBuilder.AppendLine("FORMAT OUTPUT: Berikan HANYA JSON dalam format berikut (tanpa markdown, tanpa backticks):");
        promptBuilder.AppendLine("{");
        promptBuilder.AppendLine("  \"pertanyaan\": \"teks pertanyaan di sini\",");
        promptBuilder.AppendLine("  \"pilihan_a\": \"teks pilihan A\",");
        promptBuilder.AppendLine("  \"pilihan_b\": \"teks pilihan B\",");
        promptBuilder.AppendLine("  \"pilihan_c\": \"teks pilihan C\",");
        promptBuilder.AppendLine("  \"pilihan_d\": \"teks pilihan D\",");
        promptBuilder.AppendLine("  \"jawaban_benar\": \"A atau B atau C atau D\"");
        promptBuilder.AppendLine("}");

        return promptBuilder.ToString();
    }

    /// <summary>
    /// Build request body untuk Gemini API dengan proper JSON escaping
    /// </summary>
    private string BuildGeminiRequestBody(string prompt)
    {
        // Escape quotes dan newlines untuk JSON
        string escapedPrompt = prompt.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "");
        
        StringBuilder body = new StringBuilder();
        body.Append("{");
        body.Append("\"contents\": [{");
        body.Append("\"parts\": [{");
        body.Append($"\"text\": \"{escapedPrompt}\"");
        body.Append("}]");
        body.Append("}],");
        body.Append("\"generationConfig\": {");
        body.Append("\"temperature\": 0.7,");
        body.Append("\"topK\": 40,");
        body.Append("\"topP\": 0.95,");
        body.Append("\"maxOutputTokens\": 1024");
        body.Append("}");
        body.Append("}");

        return body.ToString();
    }

    /// <summary>
    /// Parse response dari Gemini API
    /// Gemini API mengembalikan nested structure yang perlu di-extract
    /// </summary>
    private QuizData ParseGeminiResponse(string jsonResponse)
    {
        try
        {
            // Response structure dari Gemini:
            // { "candidates": [ { "content": { "parts": [ { "text": "actual json here" } ] } } ] }
            
            GeminiResponse geminiResp = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

            if (geminiResp?.candidates != null && geminiResp.candidates.Length > 0)
            {
                string quizJsonText = geminiResp.candidates[0].content.parts[0].text;
                
                // Clean up the response (remove markdown if any)
                quizJsonText = CleanJsonString(quizJsonText);
                
                // Validate JSON format
                if (!IsValidJson(quizJsonText))
                {
                    VaultGuardLogger.LogError("AIManager", "Invalid JSON format from AI! Using fallback.");
                    return null;
                }
                
                VaultGuardLogger.Log("AIManager", $"Quiz JSON extracted: {quizJsonText.Substring(0, Mathf.Min(100, quizJsonText.Length))}...");
                
                QuizData quiz = JsonUtility.FromJson<QuizData>(quizJsonText);
                return quiz;
            }
            
            return null;
        }
        catch (System.Exception e)
        {
            VaultGuardLogger.LogError("AIManager", $"Parse error: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Clean JSON string dari markdown atau karakter aneh
    /// AI kadang mengembalikan response dengan markdown wrapper
    /// </summary>
    private string CleanJsonString(string jsonText)
    {
        if (string.IsNullOrEmpty(jsonText))
            return jsonText;
            
        // Remove json dan  jika ada
        jsonText = jsonText.Replace("json", "").Replace("", "").Trim();
        
        // Cari first { dan last }
        int firstBrace = jsonText.IndexOf('{');
        int lastBrace = jsonText.LastIndexOf('}');
        
        if (firstBrace >= 0 && lastBrace > firstBrace)
        {
            jsonText = jsonText.Substring(firstBrace, (lastBrace - firstBrace) + 1);
        }
        
        return jsonText;
    }
    
    /// <summary>
    /// Validate JSON format sebelum parsing
    /// </summary>
    private bool IsValidJson(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;
            
        text = text.Trim();
        return (text.StartsWith("{") && text.EndsWith("}")) ||
            (text.StartsWith("[") && text.EndsWith("]"));
    }

    /// <summary>
    /// Fallback ke quiz lokal jika AI gagal
    /// Memastikan game tetap berjalan meski AI error
    /// </summary>
    private IEnumerator FallbackToLocalQuiz(string topik, System.Action<QuizData> onSuccess)
    {
        VaultGuardLogger.LogWarning("AIManager", "🔄 Menggunakan quiz backup dari local database");
        
        QuizData quiz = LoadQuiz(topik);
        if (quiz == null)
        {
            quiz = GetRandomQuiz();
        }
        
        onSuccess?.Invoke(quiz);
        yield return null;
    }
    
    #endregion
    
    #region Utility Functions
    
    /// <summary>
    /// Check network connectivity sebelum request
    /// </summary>
    public bool IsNetworkAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    /// <summary>
    /// Validate API configuration
    /// </summary>
    public bool IsAPIConfigured()
    {
        return !string.IsNullOrEmpty(apiKey) && !string.IsNullOrEmpty(apiEndpoint);
    }
    
    /// <summary>
    /// Check apakah bisa membuat request baru (rate limiting)
    /// </summary>
    public bool CanMakeRequest()
    {
        return activeRequests < MAX_CONCURRENT_REQUESTS;
    }

    /// <summary>
    /// Get system status untuk debugging
    /// Menampilkan status lengkap AI system
    /// </summary>
    public string GetSystemStatus()
    {
        StringBuilder status = new StringBuilder();
        status.AppendLine("=== VaultGuard AI System Status ===");
        status.AppendLine($"Network: {(IsNetworkAvailable() ? "✅ Connected" : "❌ Offline")}");
        status.AppendLine($"API Config: {(IsAPIConfigured() ? "✅ Configured" : "❌ Not Configured")}");
        status.AppendLine($"Local Database: {(quizDatabase != null && quizDatabase.daftar_quiz != null ? $"✅ Loaded ({quizDatabase.daftar_quiz.Count} quiz)" : "❌ Not Loaded")}");
        status.AppendLine($"Active Requests: {activeRequests}/{MAX_CONCURRENT_REQUESTS}");
        status.AppendLine($"Timeout: {requestTimeout}s");
        status.AppendLine($"Max Retries: {maxRetries}");
        status.AppendLine($"Demo Mode: {(forceOfflineMode ? "✅ Enabled" : "❌ Disabled")}");
        return status.ToString();
    }
    
    #endregion
    
    #region Unity Editor Debug Helpers
    
#if UNITY_EDITOR
    [ContextMenu("Test Load Random Quiz")]
    private void TestLoadRandomQuiz()
    {
        QuizData quiz = GetRandomQuiz();
        if (quiz != null)
        {
            Debug.Log($"=== Random Quiz ===");
            Debug.Log($"Pertanyaan: {quiz.pertanyaan}");
            Debug.Log($"A: {quiz.pilihan_a}");
            Debug.Log($"B: {quiz.pilihan_b}");
            Debug.Log($"C: {quiz.pilihan_c}");
            Debug.Log($"D: {quiz.pilihan_d}");
            Debug.Log($"Jawaban Benar: {quiz.jawaban_benar}");
            Debug.Log($"Topik: {quiz.topik_virus}");
        }
    }

    [ContextMenu("Test Load Phishing Quiz")]
    private void TestLoadPhishingQuiz()
    {
        QuizData quiz = LoadQuiz("Phishing");
        if (quiz != null)
        {
            Debug.Log($"=== Phishing Quiz ===");
            Debug.Log($"Pertanyaan: {quiz.pertanyaan}");
            Debug.Log($"Jawaban Benar: {quiz.jawaban_benar}");
        }
    }
    
    [ContextMenu("Test Load Ransomware Quiz")]
    private void TestLoadRansomwareQuiz()
    {
        QuizData quiz = LoadQuiz("Ransomware");
        if (quiz != null)
        {
            Debug.Log($"=== Ransomware Quiz ===");
            Debug.Log($"Pertanyaan: {quiz.pertanyaan}");
            Debug.Log($"Jawaban Benar: {quiz.jawaban_benar}");
        }
    }
    
    [ContextMenu("Test LoadQuizLokal()")]
    private void TestLoadQuizLokal()
    {
        Debug.Log("=== Testing LoadQuizLokal() Function ===");
        
        // Test dengan parameter
        QuizData quiz1 = LoadQuizLokal("Phishing");
        if (quiz1 != null)
        {
            Debug.Log($"✅ LoadQuizLokal('Phishing'): {quiz1.pertanyaan}");
        }
        
        // Test tanpa parameter (random)
        QuizData quiz2 = LoadQuizLokal();
        if (quiz2 != null)
        {
            Debug.Log($"✅ LoadQuizLokal() random: {quiz2.pertanyaan}");
        }
    }
    
    [ContextMenu("Show System Status")]
    private void ShowSystemStatus()
    {
        Debug.Log(GetSystemStatus());
    }
    
    [ContextMenu("Test Full AI Flow (Requires API Key)")]
    private void TestFullAIFlow()
    {
        if (!IsAPIConfigured())
        {
            Debug.LogError("API Key not configured! Please set API Key in Inspector first.");
            return;
        }
        
        RequestQuizFromAI("Phishing",
            onSuccess: (quiz) => {
                VaultGuardLogger.LogSuccess("TEST", $"✅ AI Quiz Success!");
                Debug.Log($"Pertanyaan: {quiz.pertanyaan}");
                Debug.Log($"Pilihan A: {quiz.pilihan_a}");
                Debug.Log($"Pilihan B: {quiz.pilihan_b}");
                Debug.Log($"Pilihan C: {quiz.pilihan_c}");
                Debug.Log($"Pilihan D: {quiz.pilihan_d}");
                Debug.Log($"Jawaban Benar: {quiz.jawaban_benar}");
                
                // Test validation
                bool isCorrect = ValidasiJawaban(quiz, quiz.jawaban_benar);
                Debug.Log($"Validation Test: {(isCorrect ? "PASS" : "FAIL")}");
            },
            onFailure: (error) => {
                VaultGuardLogger.LogError("TEST", $"❌ AI Quiz Failed: {error}");
            }
        );
    }
    
    [ContextMenu("Test Validation - Correct Answer")]
    private void TestValidationCorrect()
    {
        QuizData quiz = GetRandomQuiz();
        if (quiz != null)
        {
            bool result = ValidasiJawaban(quiz, quiz.jawaban_benar);
            Debug.Log($"Test Correct Answer: {(result ? "✅ PASS" : "❌ FAIL")}");
        }
    }
    
    [ContextMenu("Test Validation - Wrong Answer")]
    private void TestValidationWrong()
    {
        QuizData quiz = GetRandomQuiz();
        if (quiz != null)
        {
            // Get wrong answer
            string wrongAnswer = quiz.jawaban_benar == "A" ? "B" : "A";
            bool result = ValidasiJawaban(quiz, wrongAnswer);
            Debug.Log($"Test Wrong Answer: {(result ? "❌ FAIL (should be false)" : "✅ PASS")}");
        }
    }
#endif
    
    #endregion
}

#region Gemini API Response Helper Classes

/// <summary>
/// Root response structure dari Gemini API
/// </summary>
[System.Serializable]
public class GeminiResponse
{
    public GeminiCandidate[] candidates;
}

/// <summary>
/// Candidate structure dalam Gemini response
/// </summary>
[System.Serializable]
public class GeminiCandidate
{
    public GeminiContent content;
}

/// <summary>
/// Content structure dalam Gemini response
/// </summary>
[System.Serializable]
public class GeminiContent
{
    public GeminiPart[] parts;
}

/// <summary>
/// Part structure yang berisi actual text response
/// </summary>
[System.Serializable]
public class GeminiPart
{
    public string text;
}

#endregion