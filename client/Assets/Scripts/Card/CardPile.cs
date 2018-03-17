using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

class CardPile : MonoBehaviour
{
    public int MaxPileCount = 9;

    public bool useDragDrop = false;
    public List<Card> cards = new List<Card>();

    public bool AddCard(GameObject card, bool forceMove = false)
    {
        var ui_grid = transform.GetComponentInChildren<UIGrid>();
        card.transform.SetParent(ui_grid.transform);

        ui_grid.transform.GridRepositionNextFrame(this);

        card.GetComponent<CardDragDrop>().forceMove = forceMove;
        card.GetComponent<CardDragDrop>().enabled = useDragDrop;

        var cardData = card.GetComponent<CardView>().Data;
        cards.Add(cardData);

        if (transform.parent.name.Contains("pile"))
        {
            card.transform.RemoveTrigger(false);
        }
   
        return true;
    }
}