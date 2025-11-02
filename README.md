<!-- PROJECT LOGO -->
<p align="center">
  <img src="https://img.icons8.com/external-flaticons-flat-flat-icons/512/external-cyber-security-cryptocurrency-flaticons-flat-flat-icons.png" alt="Logo" width="120" height="120">
</p>

<h1 align="center">🧩 VaultGuard: AR Threat Containment</h1>

<p align="center">
  <b>Game Augmented Reality Edukatif Berbasis AI</b><br>
  Menangkap ancaman siber di dunia nyata sambil belajar keamanan digital.<br>
  <a href="#tentang-proyek"><strong>Jelajahi Proyek »</strong></a>
</p>

<p align="center">
  <a href="https://unity.com/">Unity</a> •
  <a href="https://ai.google.dev/">Google Gemini API</a> •
  <a href="https://github.com/">GitHub</a>
</p>

---

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2022.3+-black?logo=unity&style=for-the-badge">
  <img src="https://img.shields.io/badge/AI-Gemini%20API-blue?logo=google&style=for-the-badge">
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge">
  <img src="https://img.shields.io/badge/Status-In%20Development-orange?style=for-the-badge">
</p>

---

## 🧾 Tentang Proyek

**VaultGuard** adalah game **Augmented Reality (AR)** yang menggabungkan *cybersecurity awareness* dengan gameplay berbasis **AI real-time quiz generation**.  
Pemain menjelajahi lingkungan nyata untuk mencari dan menahan “virus” digital yang bocor dari **The Vault** — dengan cara menjawab kuis keamanan siber yang dibuat oleh AI.

> 🧠 *"The Vault adalah penjara digital untuk malware. Kini para pemain harus membantu menampung ancaman yang melarikan diri ke dunia nyata!"*

---

## 📱 Fitur Utama

| Fitur | Deskripsi |
|-------|------------|
| 🎯 **Exploration (AR Mode)** | Pemain menggunakan kamera AR untuk mencari virus 3D yang tersebar di dunia nyata. |
| 🧠 **AI Cyber Quiz** | AI menghasilkan pertanyaan keamanan siber secara real-time berdasarkan jenis ancaman. |
| 🕹️ **Containment Challenge** | Pemain menjawab kuis untuk menangkap dan menahan virus di dalam Vault. |
| 🏆 **Scoring System** | Tangkap virus tercepat atau dapatkan skor tertinggi untuk menang. |
| 💾 **Offline Mode** | Backup file `backup_quiz.json` memastikan game tetap berjalan tanpa koneksi internet. |

---

## 🗺️ Storyboard Gameplay

### 🎬 Scene 1 — Pembukaan
- Kamera AR aktif  
- UI menampilkan pesan peringatan:
  > “⚠️ Vault Keamanan Siber telah bocor! Tangkap semua virus yang tersebar!”

---

### 👾 Scene 2 — Penampakan Virus
- Virus 3D (contoh: *Ransomware Monster*) muncul di depan pemain.
- UI: “Ancaman terdeteksi! Ketuk untuk menampung!”

---

### 🧩 Scene 3 — Proses Penangkapan
- Pemain mengetuk virus.
- Game menghubungi **Vault AI** → menghasilkan kuis JSON real-time.
- UI menampilkan pertanyaan seperti:

```
Apa fungsi dari Two-Factor Authentication (2FA)?
A. Menambah lapisan keamanan
B. Menyimpan password
C. Menghapus virus
D. Mempercepat login
```

---

### 💥 Scene 4 — Hasil
- ✅ **Jawaban benar:** Virus tertangkap → “Ancaman berhasil ditampung!”
- ❌ **Jawaban salah:** Virus kabur → “Virus lolos, coba lagi!”

---

### 🏁 Scene 5 — Akhir
> “🎉 SELAMAT! Anda telah membantu mengamankan Vault! Skor akhir: 3/3.”

---

## 🧠 Integrasi AI

VaultGuard menggunakan **AI Generator** untuk membuat pertanyaan kuis dinamis.

Contoh Prompt:
```text
Anda adalah asisten pembuat kuis keamanan siber.
Buat satu pertanyaan pilihan ganda tentang topik [TOPIK_VIRUS].
Output HARUS dalam format JSON:
{
  "pertanyaan": "",
  "pilihan_a": "",
  "pilihan_b": "",
  "pilihan_c": "",
  "pilihan_d": "",
  "jawaban_benar": "A/B/C/D"
}
```

---

## ⚙️ Struktur Teknis

### 📄 QuizData.cs
```csharp
[System.Serializable]
public class QuizData {
    public string pertanyaan;
    public string pilihan_a;
    public string pilihan_b;
    public string pilihan_c;
    public string pilihan_d;
    public string jawaban_benar;
}
```

---

### 📄 QuizManager.cs (Konsep Utama)
```csharp
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class QuizManager : MonoBehaviour
{
    private string API_URL = "ISI_DENGAN_URL_ENDPOINT_API_ANDA";
    private string API_KEY = "ISI_DENGAN_API_KEY_ANDA";

    public GameObject panelLoading;
    public GameObject panelKuis;
    public TMP_Text textPertanyaan;
    public TMP_Text textPilihanA;
    public TMP_Text textPilihanB;
    public TMP_Text textPilihanC;
    public TMP_Text textPilihanD;

    private string jawabanBenarSaatIni;

    public void RequestQuiz(string topikVirus)
    {
        StartCoroutine(GetQuizFromAI(topikVirus));
    }

    private IEnumerator GetQuizFromAI(string topik)
    {
        panelLoading.SetActive(true);
        panelKuis.SetActive(false);

        string prompt = $"Anda adalah asisten pembuat kuis keamanan siber untuk sebuah game. Buat satu pertanyaan tentang topik {topik}. Format JSON wajib berisi kunci: pertanyaan, pilihan_a, pilihan_b, pilihan_c, pilihan_d, jawaban_benar.";

        string requestBodyJson = $@"{{
            ""contents"": [
                {{ ""parts"": [ {{ ""text"": ""{prompt}"" }} ] }}
            ]
        }}";

        using (UnityWebRequest request = new UnityWebRequest(API_URL + API_KEY, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBodyJson);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            panelLoading.SetActive(false);

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error API: " + request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                string kuisJsonString = EkstrakJsonKuis(jsonResponse);
                QuizData data = JsonUtility.FromJson<QuizData>(kuisJsonString);
                DisplayQuizUI(data);
            }
        }
    }

    private string EkstrakJsonKuis(string responsMentah)
    {
        try
        {
            int start = responsMentah.IndexOf("{");
            int end = responsMentah.LastIndexOf("}");
            string json = responsMentah.Substring(start, (end - start) + 1);
            return json;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Gagal parse JSON: " + e.Message);
            return null;
        }
    }

    void DisplayQuizUI(QuizData data)
    {
        if (data == null) return;

        textPertanyaan.text = data.pertanyaan;
        textPilihanA.text = "A. " + data.pilihan_a;
        textPilihanB.text = "B. " + data.pilihan_b;
        textPilihanC.text = "C. " + data.pilihan_c;
        textPilihanD.text = "D. " + data.pilihan_d;
        jawabanBenarSaatIni = data.jawaban_benar;
        panelKuis.SetActive(true);
    }

    public void OnPilihanADitekan() { CekJawaban("A"); }
    public void OnPilihanBDitekan() { CekJawaban("B"); }
    public void OnPilihanCDitekan() { CekJawaban("C"); }
    public void OnPilihanDDitekan() { CekJawaban("D"); }

    void CekJawaban(string pilihanPemain)
    {
        if (pilihanPemain == jawabanBenarSaatIni)
        {
            Debug.Log("BENAR! Ancaman ditangkap.");
        }
        else
        {
            Debug.Log("SALAH! Virus kabur.");
        }
        panelKuis.SetActive(false);
    }
}
```

---

## 📁 Struktur Folder

```
📦 VaultGuard-AR-Threat-Containment
 ┣ 📂 Assets/
 ┃ ┣ 📂 Scripts/
 ┃ ┣ 📂 Models/
 ┃ ┣ 📂 UI/
 ┣ 📂 Resources/
 ┣ 📄 backup_quiz.json
 ┗ 📄 README.md
```

---

## 🧩 Tips Hackathon

| Tips | Keterangan |
|------|-------------|
| 💡 **Gunakan Backup JSON** | Siapkan `backup_quiz.json` agar tetap bisa dimainkan offline. |
| 🔐 **Jangan push API Key** | Simpan API key di file lokal, jangan di GitHub publik. |
| 🎨 **Perkuat UI/UX** | Gunakan tema “Cyber-Futuristic” untuk imersi maksimal. |
| ⏱️ **Fokus MVP** | Pastikan versi demo bisa dimainkan sepenuhnya terlebih dahulu. |

---

## 🖼️ Preview Game

<p align="center">
  <img src="https://i.imgur.com/k3FJZ1Z.gif" width="420" alt="Demo Gameplay">
  <br>
  <em>Contoh tampilan gameplay AR — menangkap virus dengan kuis keamanan siber</em>
</p>

---

## 🤝 Kolaborasi

Kontribusi sangat terbuka untuk:
- 👾 Game Design (AR, UX/UI)
- 🤖 AI Prompt & API Integration
- 🧱 Unity C# Development
- 🎨 3D Modeling (Virus & Monster)
- 🔊 Sound FX & Visual Effects

### Panduan Kontribusi:
1. Fork repository  
2. Buat branch baru:  
   `git checkout -b fitur/nama-fitur`
3. Commit perubahan Anda:  
   `git commit -m 'Menambahkan fitur kuis AR'`
4. Push ke branch:  
   `git push origin fitur/nama-fitur`
5. Ajukan Pull Request ke branch `main`

---

## 📜 Lisensi

Proyek ini dirilis di bawah **MIT License** — silakan gunakan dan modifikasi dengan tetap mencantumkan atribusi.

---

## 🌐 Kontak Tim

| Kontak | Informasi |
|--------|------------|
| 💬 Discord | `VaultGuard Dev Team` |
| 📧 Email | `vaultguard.project@gmail.com` |
| 🧠 Dokumentasi Teknis | [Wiki Project (Coming Soon)](https://github.com/) |

---

<p align="center">
  🛡️ <b>“Secure the Vault. Educate the World.”</b>
</p>
