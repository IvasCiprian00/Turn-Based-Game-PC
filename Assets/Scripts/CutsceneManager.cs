using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _test;
    [SerializeField] private TextAsset _testFile;
    private string _dialogueLine;
    private string _path;
    private StreamReader _reader;

    private void Start()
    {
        _path = "Assets/Dialogue/test.txt";
        _reader = new StreamReader(_path);

        _dialogueLine = _reader.ReadLine();
        StartCoroutine(ReadDialogue());
    }
    
    private IEnumerator ReadDialogue()
    {
        _test.text = null;
        for(int i = 0; i < _dialogueLine.Length; i++)
        {
            _test.text += _dialogueLine[i];
            yield return new WaitForSeconds(0.05f);
        }

        if((_dialogueLine = _reader.ReadLine()) != null)
        {
            StartCoroutine(ReadDialogue());
        }
    }
}
