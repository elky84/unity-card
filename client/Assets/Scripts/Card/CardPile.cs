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
        var uiGrid = transform.GetComponent<UIGrid>();
        card.transform.SetParent(uiGrid.transform);

        card.GetComponent<CardDragDrop>().forceMove = forceMove;
        card.GetComponent<CardDragDrop>().enabled = useDragDrop;

        var cardData = card.GetComponent<CardView>().Data;
        cards.Add(cardData);

        if (transform.name.Contains("pile"))
        {
            card.transform.RemoveTrigger(false);
        }

        uiGrid.transform.GridRepositionNextFrame(this);
        return true;
    }

}