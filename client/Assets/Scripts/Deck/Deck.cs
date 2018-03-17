using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Deck
{
    List<Card> cards = new List<Card>();

    public Deck()
    {
        for(int n = 0; n < 100; n++)
        {
            var card = new Card(n % 10 + 1, (n + 1) % 10 + 1, (n + 2) % 10 + 1);
            cards.Add(card);
        }
    }

    public Card pop()
    {
        var card = cards[0];
        cards.Remove(card);
        return card;
    }

}
