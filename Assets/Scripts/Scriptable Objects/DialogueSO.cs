using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue")]
public class DialogueSO : ScriptableObject
{
    [System.Serializable]
    public class Dialogue
    {
        public string Name;
        public string[] Sentences;
    }

    public List<Dialogue> Dialogues;
    public float DialogueSpeed = 0.05f;
}
