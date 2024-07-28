using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCondition : BaseCondition
{
    public List<Book> Books;
    private bool _isCompleted = false;

    private void Start()
    {
        PlayerController.BookChanged += CheckResults;
    }

    private void CheckResults()
    {
        foreach (var book in Books)
        {
            if(book.BookSlot != null && book.bookColor == book.BookSlot.bookColor)
            {
                _isCompleted = true;
            }
            else
            {
                _isCompleted = false;
                break;
            }
        }

        if (_isCompleted)
        {
            PlayerController.BookChanged -= CheckResults;
            SectionEvent.Trigger(SectionEventType.SectionCompleted);
        }
    }

    public override bool IsCompleted()
    {
        return _isCompleted;
    }
}
