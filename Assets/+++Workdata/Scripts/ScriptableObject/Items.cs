using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Items", menuName = "Scriptable Objects/Items")]
public class Items : ScriptableObject
{
    [TextArea(3, 10)]
    public string itemDescription;
    public Sprite itemImage;
    public Audiolog textAudio;
}
