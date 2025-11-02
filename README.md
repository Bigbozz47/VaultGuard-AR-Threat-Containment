# VaultGuard-AR-Threat-Containment

Sebuah game AR di mana pemain "menjelajahi" lingkungan mereka untuk menemukan ancaman siber (virus) yang "bocor" dari "Vault". Untuk "menangkap" dan mengembalikan virus ke Vault, pemain harus menjawab kuis keamanan siber yang dibuat secara real-time oleh AI.



Konsep: "The Vault" adalah tempat di mana virus dan malware berbahaya dikurung. Saat hackathon, "Vault" tersebut "bocor" dan ancaman siber (diwujudkan sebagai monster 3D) tersebar di dunia nyata.



Gameplay (Menggunakan AR):



Exploring: Pemain berjalan di sekitar area acara. Menggunakan AR, mereka dapat melihat "virus" (monster 3D) melayang di berbagai tempat.



Cyber-Game: Pemain mengetuk monster tersebut untuk "menangkapnya".



Edukasi: Untuk berhasil menangkap, pemain harus menjawab pertanyaan kuis tentang keamanan siber.



Contoh: "Manakah di bawah ini yang merupakan contoh phishing?"



Contoh: "Apa fungsi dari Two-Factor Authentication (2FA)?"



Containment: Jika jawaban benar, monster itu "ditangkap" dan dikirim kembali ke "Vault" (yang berfungsi sebagai inventory atau Pokedex pemain).



Tujuan: Menangkap semua virus/malware tercepat atau dengan skor tertinggi.



🗺️ Storyboard / Alur Permainan

Scene 1: Pembukaan.



Pengguna membuka aplikasi. Kamera AR aktif.



UI: "PERINGATAN! Vault Keamanan Siber telah bocor! 'Virus' dan 'Malware' kabur ke dunia nyata. Tangkap mereka!"



UI: "Virus Ditangkap: 0 / 3".



Scene 2: Penampakan Virus.



Saat pengguna melihat sekeliling, tiba-tiba \[Interaksi AR] sebuah model 3D "Virus" (misal: monster merah spiky) muncul mengambang di depan mereka.



Tips MVP: Jangan pakai GPS. Cukup buat virusnya muncul 2 meter di depan kamera setelah 5 detik.



Scene 3: Proses Penangkapan (Cyber-Game).



Pengguna mengetuk (tap) model 3D "Virus" tersebut.



UI: Panel Kuis Keamanan Siber muncul.



UI: "Untuk menangkap 'Virus-Phishing' ini, jawab pertanyaannya: Apa ciri-ciri email phishing? (A) Tata bahasa buruk, (B) Meminta password, (C) Semua benar."



Scene 4: Hasil.



Kasus A (Benar): Pengguna memilih (C).



UI: "JAWABAN BENAR!"



\[Animasi AR] Sebuah "Jaring Digital" terbang dari layar dan "menangkap" model 3D Virus. Model 3D itu lalu hilang (hancur).



UI: "Virus Ditangkap! (1 / 3)".



Kasus B (Salah): Pengguna memilih (A).



UI: "JAWABAN SALAH! Virus kabur!"



\[Animasi AR] Model 3D Virus terbang menjauh dan hilang.



UI: "Cari virus lainnya!"



Scene 5: Repeat \& Finale.



Pengguna menunggu 5 detik lagi, "Virus" ke-2 muncul.



Pengguna menangkapnya (atau gagal).



"Virus" ke-3 muncul dan ditangkap.



UI: "SELAMAT! ANDA TELAH MEMBANTU MENGAMANKAN VAULT! (Skor: 3/3)".



🛠️



1\. 🎯 Konsep Inti yang Difokuskan

Nama Ide: VaultGuard: AR Threat Containment



Logline: Sebuah game AR di mana pemain "menjelajahi" lingkungan mereka untuk menemukan ancaman siber (virus) yang "bocor" dari "Vault". Untuk "menangkap" dan mengembalikan virus ke Vault, pemain harus menjawab kuis keamanan siber yang dibuat secara real-time oleh AI.



Peran AI: Menjadi Quiz Generator (Pembuat Kuis) yang Adaptif. AI membuat setiap pertanyaan menjadi unik, sehingga replayability game ini tak terbatas dan kontennya selalu segar.



2\. 🗺️ Alur Permainan (Storyboard) yang Diperbarui

Ini adalah alur langkah demi langkah dari apa yang akan dilihat pemain (dan juri).



Scene 1: Pembukaan (Eksplorasi)



Pemain membuka aplikasi. Kamera AR aktif.



UI: "PERINGATAN! Vault Keamanan Siber telah bocor! Pindai sekitarmu untuk menemukan 'Malware' yang kabur."



UI: "Ancaman Ditampung: 0"



Scene 2: Penampakan Ancaman



Setelah beberapa detik, sebuah model 3D "Virus" (misal: monster merah bernama "Ransomware") muncul mengambang di depan pemain (teknik marker-less, ditempatkan beberapa meter di depan kamera).



UI: "Ancaman terdeteksi! Ketuk untuk menampung!"



Scene 3: Interaksi \& Panggilan AI (Momen Kunci)



Pemain mengetuk (tap) model 3D "Ransomware" tersebut.



Model 3D berhenti bergerak.



UI: Panel Loading muncul. Teks: "Menganalisis ancaman... Menghubungi Vault AI untuk prosedur penahanan..."



Di Latar Belakang: Aplikasi Anda secara diam-diam mengirim prompt ke API AI (misal: Google Gemini API). Prompt-nya: "Buatkan saya kuis JSON tentang 'Ransomware'".



Scene 4: Cyber-Game (Kuis AI)



Di Latar Belakang: API AI merespons dengan JSON yang berisi pertanyaan dan jawaban.



UI: Panel Loading hilang. Panel Kuis muncul, diisi dengan data dari AI.



Contoh Tampilan Kuis:



Pertanyaan: (Dari AI) "Apa nama teknik di mana penyerang mengenkripsi file Anda dan meminta uang tebusan?"



Pilihan A: (Dari AI) "Phishing"



Pilihan B: (Dari AI) "Ransomware"



Pilihan C: (Dari AI) "Trojan"



Pilihan D: (Dari AI) "Spyware"



Pemain memilih jawaban (misal: B).



Scene 5: Hasil \& Penahanan (Vaulting)



UI: "JAWABAN BENAR! Ancaman 'Ransomware' berhasil ditampung!"



Animasi AR: Model 3D "Ransomware" memainkan animasi "ditangkap" (misal: terhisap ke dalam portal digital) lalu hilang.



UI: "Ancaman Ditampung: 1".



Game kembali ke Scene 2 untuk mencari virus berikutnya ("Phishing", "Trojan", dll.).



3\. 🧠 Komponen Kunci AI: Prompt API

Ini adalah "otak" dari game Anda. Anda perlu prompt yang kuat untuk memastikan AI memberi Anda format JSON yang bisa dibaca oleh Unity.



Gunakan prompt ini saat memanggil API AI (seperti Google Gemini):



Anda adalah asisten pembuat kuis keamanan siber untuk sebuah game. Tugas Anda adalah membuat 1 (satu) pertanyaan kuis pilihan ganda yang menantang namun edukatif tentang topik yang saya berikan.



Topik: \[TOPIK\_VIRUS\_DI\_SINI]



Format respons Anda WAJIB dalam bentuk JSON yang ketat (strict JSON) agar bisa dibaca oleh mesin. JSON harus berisi 6 kunci:



"pertanyaan": (string) Teks pertanyaan.



"pilihan\_a": (string) Teks untuk pilihan A.



"pilihan\_b": (string) Teks untuk pilihan B.



"pilihan\_c": (string) Teks untuk pilihan C.



"pilihan\_d": (string) Teks untuk pilihan D.



"jawaban\_benar": (string) Hanya hurufnya saja (misal: "A", "B", "C", atau "D").



Berikan HANYA JSON, tanpa teks pembuka atau penutup seperti ```json.



Cara Penggunaan: Saat pemain mengetuk virus "Ransomware", skrip Anda akan mengganti \[TOPIK\_VIRUS\_DI\_SINI] dengan "Ransomware". Jika mereka mengetuk virus "Phishing", ganti dengan "Phishing".



4\. 🛠️ Komponen Kunci Teknis: Skrip C# (Unity)

Anda perlu dua skrip utama untuk ini.



PENTING: Ini adalah contoh konseptual. Anda perlu mendapatkan API Key dan Endpoint URL Anda sendiri (misalnya dari Google AI Studio untuk Gemini API).



A. Skrip Data (QuizData.cs)

Buat skrip C# baru bernama QuizData.cs. Ini HANYA untuk menyimpan data JSON.



C#



// QuizData.cs

// Skrip ini tidak perlu di-attach ke GameObject manapun.

// Ini hanya sebuah cetakan (class) untuk menampung data dari JSON.



\[System.Serializable]

public class QuizData

{

&nbsp;   public string pertanyaan;

&nbsp;   public string pilihan\_a;

&nbsp;   public string pilihan\_b;

&nbsp;   public string pilihan\_c;

&nbsp;   public string pilihan\_d;

&nbsp;   public string jawaban\_benar;

}

B. Skrip Manager (QuizManager.cs)

Buat skrip ini dan letakkan di GameObject kosong (misal: "GameManager").



C#



// QuizManager.cs

using UnityEngine;

using UnityEngine.Networking;

using System.Collections;

using TMPro; // Jika Anda menggunakan TextMeshPro untuk UI



public class QuizManager : MonoBehaviour

{

&nbsp;   // --- Kredensial API (HARUS DIISI) ---

&nbsp;   private string API\_URL = "ISI\_DENGAN\_URL\_ENDPOINT\_API\_ANDA"; 

&nbsp;   private string API\_KEY = "ISI\_DENGAN\_API\_KEY\_ANDA";



&nbsp;   // --- Referensi ke UI Anda ---

&nbsp;   public GameObject panelLoading;

&nbsp;   public GameObject panelKuis;

&nbsp;   public TMP\_Text textPertanyaan;

&nbsp;   public TMP\_Text textPilihanA;

&nbsp;   public TMP\_Text textPilihanB;

&nbsp;   public TMP\_Text textPilihanC;

&nbsp;   public TMP\_Text textPilihanD;



&nbsp;   private string jawabanBenarSaatIni;

&nbsp;   

&nbsp;   // Dipanggil oleh skrip 'VirusInteraction.cs' saat virus diketuk

&nbsp;   public void RequestQuiz(string topikVirus)

&nbsp;   {

&nbsp;       StartCoroutine(GetQuizFromAI(topikVirus));

&nbsp;   }



&nbsp;   private IEnumerator GetQuizFromAI(string topik)

&nbsp;   {

&nbsp;       // 1. Tampilkan UI Loading

&nbsp;       panelLoading.SetActive(true);

&nbsp;       panelKuis.SetActive(false);



&nbsp;       // 2. Siapkan Prompt untuk dikirim

&nbsp;       string prompt = $"Anda adalah asisten pembuat kuis... \[SALIN PROMPT DARI ATAS] ... Topik: \\"{topik}\\" ... Berikan HANYA JSON...";

&nbsp;       

&nbsp;       // 3. Buat Request Body (Contoh untuk Gemini API)

&nbsp;       // Format body bisa berbeda antar provider AI

&nbsp;       string requestBodyJson = $@"{{

&nbsp;           ""contents"": \[

&nbsp;               {{ ""parts"": \[ {{ ""text"": ""{prompt}"" }} ] }}

&nbsp;           ]

&nbsp;       }}";



&nbsp;       // 4. Siapkan UnityWebRequest

&nbsp;       using (UnityWebRequest request = new UnityWebRequest(API\_URL + API\_KEY, "POST"))

&nbsp;       {

&nbsp;           byte\[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestBodyJson);

&nbsp;           request.uploadHandler = new UploadHandlerRaw(bodyRaw);

&nbsp;           request.downloadHandler = new DownloadHandlerBuffer();

&nbsp;           request.SetRequestHeader("Content-Type", "application/json");



&nbsp;           // 5. Kirim Request

&nbsp;           yield return request.SendWebRequest();



&nbsp;           // 6. Sembunyikan UI Loading

&nbsp;           panelLoading.SetActive(false);



&nbsp;           // 7. Cek Hasil

&nbsp;           if (request.result != UnityWebRequest.Result.Success)

&nbsp;           {

&nbsp;               Debug.LogError("Error API: " + request.error);

&nbsp;               // Tampilkan pesan error di UI

&nbsp;           }

&nbsp;           else

&nbsp;           {

&nbsp;               // Sukses!

&nbsp;               string jsonResponse = request.downloadHandler.text;



&nbsp;               // Ini adalah bagian PENTING:

&nbsp;               // Seringkali, respons AI dibungkus dalam JSON lain.

&nbsp;               // Anda mungkin perlu membersihkan responsnya terlebih dahulu.

&nbsp;               // Misal: (Respons Gemini)

&nbsp;               // { "candidates": \[ { "content": { "parts": \[ { "text": "{\\"pertanyaan\\":...}" } ] } } ] }

&nbsp;               

&nbsp;               // TODO: Parsing JSON respons untuk mendapatkan teks JSON kuis-nya.

&nbsp;               // Untuk hackathon, Anda bisa copy-paste respons mentah dan 

&nbsp;               // hardcode parsing-nya jika rumit.

&nbsp;               

&nbsp;               // Asumsikan Anda sudah berhasil mengekstrak JSON kuisnya:

&nbsp;               string kuisJsonString = EkstrakJsonKuis(jsonResponse); // Anda harus buat fungsi ini



&nbsp;               // 8. Ubah JSON string menjadi Objek C#

&nbsp;               QuizData data = JsonUtility.FromJson<QuizData>(kuisJsonString);



&nbsp;               // 9. Tampilkan di UI

&nbsp;               DisplayQuizUI(data);

&nbsp;           }

&nbsp;       }

&nbsp;   }



&nbsp;   // Fungsi 'EkstrakJsonKuis' ini SANGAT bergantung pada provider AI Anda.

&nbsp;   // Anda harus melihat output mentah di Debug.Log(jsonResponse) dan menyesuaikannya.

&nbsp;   private string EkstrakJsonKuis(string responsMentah)

&nbsp;   {

&nbsp;       // Ini HANYA CONTOH. Kemungkinan besar salah.

&nbsp;       // Anda harus menyesuaikan ini.

&nbsp;       try

&nbsp;       {

&nbsp;           // Coba cari awal dan akhir JSON kuisnya

&nbsp;           int start = responsMentah.IndexOf("{");

&nbsp;           int end = responsMentah.LastIndexOf("}");

&nbsp;           string json = responsMentah.Substring(start, (end - start) + 1);

&nbsp;           return json;

&nbsp;       }

&nbsp;       catch (System.Exception e)

&nbsp;       {

&nbsp;           Debug.LogError("Gagal parse JSON: " + e.Message);

&nbsp;           return null;

&nbsp;       }

&nbsp;   }





&nbsp;   void DisplayQuizUI(QuizData data)

&nbsp;   {

&nbsp;       if (data == null)

&nbsp;       {

&nbsp;           Debug.LogError("Data kuis null!");

&nbsp;           return;

&nbsp;       }

&nbsp;       

&nbsp;       textPertanyaan.text = data.pertanyaan;

&nbsp;       textPilihanA.text = "A. " + data.pilihan\_a;

&nbsp;       textPilihanB.text = "B. " + data.pilihan\_b;

&nbsp;       textPilihanC.text = "C. " + data.pilihan\_c;

&nbsp;       textPilihanD.text = "D. " + data.pilihan\_d;

&nbsp;       jawabanBenarSaatIni = data.jawaban\_benar;



&nbsp;       panelKuis.SetActive(true);

&nbsp;   }



&nbsp;   // Anda perlu membuat fungsi ini yang terhubung ke Tombol-Tombol UI

&nbsp;   public void OnPilihanADitekan() { CekJawaban("A"); }

&nbsp;   public void OnPilihanBDitekan() { CekJawaban("B"); }

&nbsp;   public void OnPilihanCDitekan() { CekJawaban("C"); }

&nbsp;   public void OnPilihanDDitekan() { CekJawaban("D"); }



&nbsp;   void CekJawaban(string pilihanPemain)

&nbsp;   {

&nbsp;       if (pilihanPemain == jawabanBenarSaatIni)

&nbsp;       {

&nbsp;           Debug.Log("BENAR!");

&nbsp;           // Panggil fungsi untuk "menangkap" virus

&nbsp;           // (misal: GameManager.instance.TangkapVirus())

&nbsp;       }

&nbsp;       else

&nbsp;       {

&nbsp;           Debug.Log("SALAH!");

&nbsp;           // Tampilkan pesan salah

&nbsp;       }

&nbsp;       

&nbsp;       panelKuis.SetActive(false);

&nbsp;   }

}

5\. 💡 Tips Kunci untuk Hackathon 48 Jam

Backup Plan (Wajib!): Apa yang terjadi jika internet mati saat demo?



Buat 1 file JSON kuis cadangan (misal: backup\_quiz.json) di dalam proyek Anda.



Di skrip QuizManager, jika request.result != Success, jangan tampilkan error. Alih-alih, baca file JSON cadangan itu dan tampilkan kuis dari sana.



Juri akan tetap melihat game Anda berfungsi, dan Anda bisa jelaskan bahwa idealnya itu ditarik dari AI.



API Key: Jangan pernah memasukkan API Key Anda di GitHub publik. Untuk hackathon, simpan di skrip tidak apa-apa, tapi jangan di-push.



Fokus di UI: Buat UI loading dan UI kuis yang terlihat bagus. Tampilan sangat penting.







