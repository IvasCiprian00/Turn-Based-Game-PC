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

        _dialogueText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 1);
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
            Debug.Log(_hero.transform.position);

        }

        if(_dialogueLine.IndexOf("elder:") != -1)
        {
            Debug.Log(_elder.transform.position);
            //_containerRect.transform.position = new Vector2(_elder.transform.position.x, _elder.transform.position.y + 1);
            _containerRect.transform.position = _elder.transform.position;
        }
    }

    /*
    public void ReadDialogueLine()
    {
        _lineHasFinished = false;
        _dialogueLine = _reader.ReadLine();

        if(_dialogueLine == null)
        {
            return;
        }

        for(_index = 0; _index < _dialogueLine.Length; _index++)
        {
            Debug.Log(_index);
            Invoke("ReadCharacter", 0.05f);
        }
    }

    public void ReadCharacter()
    {
        _test.text += _dialogueLine[_index];
    }

    private IEnumerator ReadDialogue()
    {
        _lineHasFinished = false;
        _test.text = null;
        for(int i = 0; i < _dialogueLine.Length; i++)
        {
            _test.text += _dialogueLine[i];
            if (_lineHasFinished)
            {
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
        }

        _lineHasFinished = true;
    }*/
}
