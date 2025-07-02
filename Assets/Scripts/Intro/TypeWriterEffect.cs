using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Typewriter Settings")]
    public float defaultDelay = 0.05f; // Jeda waktu default antara setiap karakter

    [Header("Dialog Lines")]
    // Ganti List<string> menjadi List<DialogLine>
    public List<DialogLine> dialogLines; // Daftar semua objek DialogLine Anda

    private TextMeshProUGUI textMeshPro;
    private AudioSource audioSource; // Komponen AudioSource
    private Coroutine typingCoroutine;

    public bool isTyping { get; private set; } = false;
    private int currentLineIndex = 0;
    private float lastSoundTime = 0f; // Waktu terakhir suara diputar

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        textMeshPro.text = "";

        // Dapatkan atau tambahkan AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // Jangan putar otomatis
            audioSource.spatialBlend = 0; // 2D Sound
        }
    }

    void Start()
    {
        // Mulai menampilkan dialog pertama saat game dimulai
        StartCurrentLineTyping();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            HandleInput();
        }
    }

    void StartCurrentLineTyping()
    {
        if (dialogLines == null || dialogLines.Count == 0)
        {
            Debug.LogWarning("No dialog lines assigned in TypewriterEffect!");
            EndDialog();
            return;
        }

        if (currentLineIndex < dialogLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            isTyping = true;
            textMeshPro.text = ""; // Kosongkan teks sebelum memulai baris baru

            DialogLine currentLineData = dialogLines[currentLineIndex];
            typingCoroutine = StartCoroutine(ShowText(currentLineData));
        }
        else
        {
            EndDialog(); // Semua dialog sudah ditampilkan
        }
    }

    IEnumerator ShowText(DialogLine lineData)
    {
        // Gunakan delay dari lineData, jika tidak ada, gunakan defaultDelay
        float effectiveDelay = lineData.delay > 0 ? lineData.delay : defaultDelay;
        float effectiveSoundPlayDelay = lineData.soundPlayDelay > 0 ? lineData.soundPlayDelay : defaultDelay;

        for (int i = 0; i < lineData.text.Length + 1; i++)
        {
            if (!isTyping) // Jika di-skip, langsung tampilkan semua teks dan keluar
            {
                textMeshPro.text = lineData.text;
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                yield break;
            }

            string currentText = lineData.text.Substring(0, i);
            textMeshPro.text = currentText;

            // Logika untuk memutar suara ketikan
            // Gunakan typingSound dari lineData
            if (lineData.typingSound != null && Time.time - lastSoundTime > effectiveSoundPlayDelay)
            {
                // Opsional: Hindari memutar suara jika karakter adalah spasi atau tanda baca tertentu
                if (i > 0 && !char.IsWhiteSpace(lineData.text[i - 1]) && !char.IsPunctuation(lineData.text[i - 1]))
                {
                    audioSource.PlayOneShot(lineData.typingSound);
                    lastSoundTime = Time.time;
                }
            }

            yield return new WaitForSeconds(effectiveDelay);
        }
        isTyping = false; // Pengetikan baris ini selesai
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void HandleInput()
    {
        if (isTyping)
        {
            SkipTyping();
        }
        else
        {
            currentLineIndex++;
            StartCurrentLineTyping();
        }
    }

    void SkipTyping()
    {
        if (isTyping)
        {
            isTyping = false;
        }
    }

    void EndDialog()
    {
        Debug.Log("Semua dialog selesai ditampilkan!");
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        // Tambahkan logika akhir di sini (misal: memuat scene berikutnya)
    }
}