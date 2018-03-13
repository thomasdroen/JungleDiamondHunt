using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Question", menuName ="Create Question")]
public class Question : Quest {

    public Answer[] answers;

}

[System.Serializable]
public class Answer
{
    public string answerText;
    public bool correct;
    [TextArea]
    public string extraInfo;
}
