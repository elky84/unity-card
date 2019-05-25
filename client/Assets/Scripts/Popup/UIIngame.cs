using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class UIIngame : MonoBehaviour
{
    Deck deck = new Deck();
    public GameObject cardObject;
    public GameObject hand0;
    public GameObject hand1;
    public GameObject pile0;
    public GameObject pile1;

    public GameObject character0;
    public GameObject character1;

    int turn = 0;
    bool isMyTurn = true;

    public GameObject turnEnd;

    void Start()
    {
        for(int n = 0; n < 5; ++n)
        {
            var card = deck.pop();
            var card_obj = Instantiate(cardObject) as GameObject;
            card_obj.transform.parent = hand0.transform;
            card_obj.transform.localScale = Vector3.one;

            var card_component = card_obj.GetComponent<CardView>();
            card_component.SetCard(card, true);
            hand0.GetComponent<CardPile>().AddCard(card_obj);
        }

        character0.GetComponent<CharacterView>().SetCharacter(new Character(0, 30));

        for (int n = 0; n < 5; ++n)
        {
            var card = deck.pop();
            var card_obj = Instantiate(cardObject) as GameObject;
            card_obj.transform.parent = hand1.transform;
            card_obj.transform.localScale = Vector3.one;

            var card_component = card_obj.GetComponent<CardView>();
            card_component.SetCard(card, false);
            hand1.GetComponent<CardPile>().AddCard(card_obj);
        }

        character1.GetComponent<CharacterView>().SetCharacter(new Character(0, 30));
        onMyTurn();
    }


    void Update()
    {

    }

    void setTurnEndButton()
    {
        turnEnd.transform.Find("button").SetTrigger(() =>
        {
            if (isMyTurn)
                isMyTurn = false;

            foreach (Transform card in hand0.transform)
            {
                card.GetComponent<CardView>().Action();
            }

            foreach (Transform card in pile0.transform.GetComponentInChildren<UIGrid>().transform)
            {
                card.GetComponent<CardView>().Action();
            }

            turnEnd.transform.Find("button").RemoveTrigger();
            turnEnd.transform.Find("button").SetActive(false);

            StartCoroutine(AiTurn());

        }, false);
    }

    void onMyTurn()
    {
        setTurnEndButton();

        foreach (Transform card in hand0.transform)
        {
            card.GetComponent<CardView>().Turn();
        }

        foreach (Transform card in pile0.transform.GetComponentInChildren<UIGrid>().transform)
        {
            card.GetComponent<CardView>().Turn();
        }
    }

    IEnumerator AiTurn()
    {
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

    void NextTurn()
    {
        turn += 1;
        setTurnEndButton();
        turnEnd.transform.Find("button").SetActive(true);
        onMyTurn();
    }
}
