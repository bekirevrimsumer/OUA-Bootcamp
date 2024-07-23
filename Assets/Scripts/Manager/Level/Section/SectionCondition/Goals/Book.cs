using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
public string Symbol;
    public int Code;

    private bool isPickedUp = false;

    private void OnMouseDown()
    {
        if (!isPickedUp)
        {
            isPickedUp = true;
            PickBook();
        }
        else
        {
            isPickedUp = false;
            PlaceBook();
        }
    }

    private void PickBook()
    {
        RaycastHit hit;
        var mousePosition = Input.mousePosition;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit))
        {
            transform.position = hit.point;
        }
    }

    private void PlaceBook()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
        }
    }
}
