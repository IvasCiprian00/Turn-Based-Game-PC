using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    private string _path;
    private StreamReader _reader;
    [SerializeField] private Animator _animator;

    [Header("Dialogue")]
    [SerializeField] private TextMeshProUGUI _narratorText;
    [SerializeField] private RectTransform _containerRect;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    private string _dialogueLine;

    [Header("Speakers")]
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _elder;
    [SerializeField] private GameObject _elderSprite;

    private void Start()
    {
        _path = "Assets/Dialogue/intro_dialogue.txt";
        _reader = new StreamReader(_path);

        _dialogueLine = _reader.ReadLine();
        StartCoroutine(ReadDialogue(_narratorText));
        //_narratorText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 1);
    }

    public void Update()
    {
        _containerRect.sizeDelta = new Vector2(800, _dialogueText.preferredHeight);

        if (Input.GetMouseButtonUp(0))
        {
            GetDialogueLine();
        }
    }

    public void SetElderPosition(float yPos)
    {
        _elderSprite.transform.position = new Vector2(_elderSprite.transform.position.x, yPos);

        BeginDialogue();
    }

    public void BeginDialogue()
    {
        _containerRect.gameObject.SetActive(true);
        GetDialogueLine();
    }

    public void GetDialogueLine()
    {
        if(_dialogueText.text.Length != _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 2).Length)
        {
            StopAllCoroutines();
            _dialogueText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 2);
            return;
        }

        int maxIterations = 0;
        while (true)
        {
            maxIterations++;
            _dialogueLine = _reader.ReadLine();

            if (maxIterations >= 5)
            {
                break;
            }

            if (_reader.EndOfStream)
            {
                _containerRect.gameObject.SetActive(false);
                _animator.SetTrigger("exit intro");
                break;
            }

            if(_dialogueLine.IndexOf("*begin cutscene*") != -1)
            {
                _animator.SetTrigger("begin cutscene");
                _narratorText.gameObject.SetActive(false);
                return;
            }

            if (_dialogueLine.IndexOf(":") != -1)
            {
                break;
            }

        }

        if (_dialogueLine == null)
        {
            return;
        }

        SetSpeaker();

        StartCoroutine(ReadDialogue(_dialogueText));
        //_dialogueText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 1);
    }

    public void ExitIntro()
    {
        SceneManager.LoadScene("Camp Screen");
    }

    public void SetSpeaker()
    {
        if(_dialogueLine.IndexOf("hero:") != -1)
        {
            //_containerRect.transform.position = new Vector2(_hero.transform.position.x, _hero.transform.position.y + 1);
            _containerRect.transform.position = _hero.transform.position;

        }

        if(_dialogueLine.IndexOf("elder:") != -1)
        {
            //_containerRect.transform.position = new Vector2(_elder.transform.position.x, _elder.transform.position.y + 1);
            _containerRect.transform.position = _elder.transform.position;
        }

        if(_dialogueLine.IndexOf("narrator:") != -1)
        {
            _narratorText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 2);

            StartCoroutine(ReadDialogue(_narratorText));
        }
    }

    

    private IEnumerator ReadDialogue(TextMeshProUGUI field)
    {
        field.text = string.Empty;
        int index = _dialogueLine.IndexOf(":") + 1;

        for(int i = index; i < _dialogueLine.Length; i++)
        {
            field.text += _dialogueLine[i];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
