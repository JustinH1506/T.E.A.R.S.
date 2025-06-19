using UnityEngine;

[CreateAssetMenu(fileName = "Journals", menuName = "Scriptable Objects/Journals")]
public class 
    Journals : ScriptableObject
{
    [TextArea(3,10)]
    public string journalText;
    public Audiolog textAudio;
}
