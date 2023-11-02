using System.Collections;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI textComponent;
    public string[] lines;
    private int index;

    public float textSpeed;

    private bool playerIsClose;

    private bool isTyping; // Menandakan apakah sedang mengetik teks atau tidak

    void Start()
    {
        textComponent.text = string.Empty;
        dialoguePanel.SetActive(false);
        isTyping = false;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) && playerIsClose)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                // Jika panel dialog aktif dan masih mengetik, tampilkan seluruh teks sekaligus
                if (isTyping)
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                    isTyping = false;
                }
                else
                {
                    NextLine();
                }
            }
            else
            {
                dialoguePanel.SetActive(true);
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    public void NextLine()
    {
        // Menambah index untuk beralih ke baris berikutnya
        index++;

        // Cek apakah semua baris telah ditampilkan
        if (index < lines.Length)
        {
            // Jika belum, reset teks dan mulai menampilkan baris baru
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            // Jika semua baris telah ditampilkan, matikan panel dialog
            dialoguePanel.SetActive(false);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            StartDialogue();
        }
    }
}
