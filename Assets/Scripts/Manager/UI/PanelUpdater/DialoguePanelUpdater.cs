using System.Collections;
using TMPro;
using UnityEngine;

public class DialoguePanelUpdater : BasePanelUpdater<DialogueEvent>
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Sentence;

    private bool _isTyping;
    private string _fullSentence;
    private DialogueEvent _nextDialogue;
    private bool _isCompleted = true;

    public override void UpdatePanel(DialogueEvent eventType)
    {
        if(eventType.DialogueSO == null)
            return;
            
        switch (eventType.DialogueEventType)
        {
            case DialogueEventType.StartDialogue:
                if(!_isCompleted && _isTyping && _nextDialogue.DialogueSO != null && _nextDialogue.DialogueSO.name != eventType.DialogueSO.name)
                    _nextDialogue = eventType;
                else
                    StopAllCoroutines();
                    StartCoroutine(StartDialogue(eventType));
                break;
            case DialogueEventType.EndDialogue:
                EndDialogue();
                break;
        }
    }

    private IEnumerator StartDialogue(DialogueEvent eventType)
    {
        _isCompleted = false;

        foreach (var dialogue in eventType.DialogueSO.Dialogues)
        {
            Name.text = dialogue.Name;

            foreach (var letter in dialogue.Sentences)
            {
                _fullSentence = letter;
                Sentence.text = "";
                _isTyping = true;

                foreach (var character in letter.ToCharArray())
                {
                    Sentence.text += character;
                    yield return new WaitForSeconds(eventType.DialogueSO.DialogueSpeed);

                    if (!_isTyping)
                    {
                        Sentence.text = _fullSentence;
                        break;
                    }
                }

                _isTyping = false;

                yield return new WaitForSeconds(1f);
            }
        }

        if(_nextDialogue.DialogueSO == null)
            DialogueEvent.Trigger(DialogueEventType.EndDialogue, eventType.DialogueSO);
        
        _isCompleted = true;

        if (_nextDialogue.DialogueSO != null)
        {
            StartCoroutine(StartDialogue(_nextDialogue));
            
            _nextDialogue.DialogueSO = null;
        }
    }

    private void Update()
    {
        if (_isTyping && Input.GetMouseButtonDown(0))
        {
            _isTyping = false;
        }
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
    }
}