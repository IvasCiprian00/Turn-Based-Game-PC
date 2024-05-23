using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextAsset _testFile;
    private string _dialogueLine;
    private string _path;
    private StreamReader _reader;
    private IEnumerator _dialogueCoroutine;
    private bool _lineHasFinished;

    private int _index;

    private void Start()
    {
        _path = "Assets/Dialogue/intro_dialogue.txt";
        _reader = new StreamReader(_path);

        _dialogueLine = _reader.ReadLine();
        _dialogueText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 1);
    }

    public void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            int maxIterations = 0;
            while(true)
            {
                maxIterations++;
                _dialogueLine = _reader.ReadLine();

                if(maxIterations >= 5)
                {
                    break;
                }

                if(_reader.EndOfStream)
                {
                    break;
                }

                if(_dialogueLine.IndexOf(":") != -1)
                {
                    break;
                }

            }

            if(_dialogueLine == null)
            {
                return;
            }
            _dialogueText.text = _dialogueLine.Substring(_dialogueLine.IndexOf(":") + 1);
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
