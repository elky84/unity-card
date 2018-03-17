using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class UIIngame : MonoBehaviour
{
    Deck deck = new Deck();
    public GameObject card_object;
    public GameObject hand_0;
    public GameObject hand_1;
    public GameObject pile_0;
    public GameObject pile_1;

    public GameObject character_0;
    public GameObject character_1;

    int turn = 0;
    bool is_my_turn = true;

    public GameObject turn_end;

    void Start()
    {
        for(int n = 0; n < 5; ++n)
        {
            var card = deck.pop();
            var card_obj = Instantiate(card_object) as GameObject;
            card_obj.transform.parent = hand_0.transform;
            card_obj.transform.localScale = Vector3.one;

            var card_component = card_obj.GetComponent<CardView>();
            card_component.SetCard(card, true);
            hand_0.GetComponent<CardPile>().AddCard(card_obj);

            hand_0.GetComponent<UIGrid>().Reposition();
        }

        character_0.GetComponent<CharacterView>().SetCharacter(new Character(0, 30));

        for (int n = 0; n < 5; ++n)
        {
            var card = deck.pop();
            var card_obj = Instantiate(card_object) as GameObject;
            card_obj.transform.parent = hand_1.transform;
            card_obj.transform.localScale = Vector3.one;

            var card_component = card_obj.GetComponent<CardView>();
            card_component.SetCard(card, false);
            hand_1.GetComponent<CardPile>().AddCard(card_obj);

            hand_1.GetComponent<UIGrid>().Reposition();
        }

        character_1.GetComponent<CharacterView>().SetCharacter(new Character(0, 30));
        on_my_turn();
    }


    void Update()
    {

    }

    void set_turn_end_button()
    {
        turn_end.transform.Find("button").SetTrigger(() =>
        {
            if (is_my_turn)
                is_my_turn = false;

            foreach (Transform card in hand_0.transform)
            {
                card.GetComponent<CardView>().Action();
            }

            foreach (Transform card in pile_0.transform.GetComponentInChildren<UIGrid>().transform)
            {
                card.GetComponent<CardView>().Action();
            }

            turn_end.transform.Find("button").RemoveTrigger();
            turn_end.transform.Find("button").SetActive(false);

            StartCoroutine(ai_turn());

        }, false);
    }

    void on_my_turn()
    {
        set_turn_end_button();

        foreach (Transform card in hand_0.transform)
        {
            card.GetComponent<CardView>().Turn();
        }

        foreach (Transform card in pile_0.transform.GetComponentInChildren<UIGrid>().transform)
        {
            card.GetComponent<CardView>().Turn();
        }
    }

    IEnumerator ai_turn()
    {
        yield return new WaitForSeconds(1f);
        next_turn();
    }

    void next_turn()
    {
        turn += 1;
        set_turn_end_button();
        turn_end.transform.Find("button").SetActive(true);
        on_my_turn();
    }
}
