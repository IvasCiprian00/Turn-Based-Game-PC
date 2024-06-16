using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _chatterSound;

    [Header("Dialogue")]
    [SerializeField] private List<Dialogue> _dialogue;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _speakerName;
    [SerializeField] private Image _speakerSprite;
    private int _dialogueIndex;
    private float _initialPitch = 1f;

    private void Start()
    {
        _speakerSprite.sprite = _dialogue[_dialogueIndex].speaker;
        _speakerName.text = _dialogue[_dialogueIndex].name;
        if (_dialogue[_dialogueIndex].chatter != null)
        {
            _chatterSound = _dialogue[_dialogueIndex].chatter;
        }
        StartCoroutine(ReadDialogue(_dialogue[_dialogueIndex].line));
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && _dialogueIndex < _dialogue.Count)
        {
            UpdateDialogue();
        }
    }


    public void UpdateDialogue()
    {
        _speakerSprite.enabled = true;
        _speakerSprite.sprite = _dialogue[_dialogueIndex].speaker;
        _speakerName.text = _dialogue[_dialogueIndex].name;


        if (_dialogueText.text.Length < _dialogue[_dialogueIndex].line.Length)
        {
            StopAllCoroutines();
            _dialogueText.text = _dialogue[_dialogueIndex].line;
            return;
        }

        _dialogueIndex++;

        if (_dialogue[_dialogueIndex].chatter != null)
        {
            _chatterSound = _dialogue[_dialogueIndex].chatter;
        }

        if (_dialogue[_dialogueIndex].trigger != "")
        {
            _animator.SetTrigger(_dialogue[_dialogueIndex].trigger);
            ClearDialogueBox();
            return;
        }
        if (_dialogueIndex >= _dialogue.Count)
        {
            return;
        }

        _speakerSprite.sprite = _dialogue[_dialogueIndex].speaker;
        _speakerName.text = _dialogue[_dialogueIndex].name;
        StartCoroutine(ReadDialogue(_dialogue[_dialogueIndex].line));
    }

    public void ClearDialogueBox()
    {
        _speakerSprite.enabled = false;
        _speakerName.text = "";
        _dialogueText.text = "";
    }

    public void ExitIntro()
    {
        SceneManager.LoadScene("Camp Screen");
    }

    private IEnumerator ReadDialogue(string line)
    {
        _dialogueText.text = string.Empty;

        for(int i = 0; i < line.Length; i++)
        {
            if(i % 3 == 0){
                _audioSource.pitch = _initialPitch;
                PlayChatterSound();
            }
            _dialogueText.text += line[i];
            yield return new WaitForSeconds(0.04f);
        }
    }

    public void EnterTavern()
    {
        SoundManager soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
        soundManager.PlayMusic(soundManager.tavern);
        soundManager.SetMusicVolume(1);
        UpdateDialogue();
    }
    public void PlayChatterSound()
    {
        
        float random = Random.Range(-0.1f, 0.1f);

        _audioSource.pitch += random;
        _audioSource.PlayOneShot(_chatterSound);
    }
}
