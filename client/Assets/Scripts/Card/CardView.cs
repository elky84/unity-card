using UnityEngine;
using System.Collections;

public class CardView : MonoBehaviour {

    [HideInInspector]
    public Card Data;

    [HideInInspector]
    public bool isOnPile = false;

	void Start ()
    {	
	}
	
	void Update ()
    {
	
	}

    public void SetCard(Card data, bool front)
    {
        Data = data;

        if (front == false)
            transform.Find("back").SetActive(true);

        transform.Find("cost").SetText(Data.cost.ToString());
        if (Data.hp <= 0)
            transform.Find("hp").SetActive(false);
        else
            transform.Find("hp").SetText(Data.hp.ToString());

        transform.Find("attack").SetText(Data.attack.ToString());
    }

    public void Play()
    {
        isOnPile = true;
    }

    public void Turn()
    {
        transform.Find("action").SetActive(true);
        GetComponent<CardDragDrop>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void Action()
    {
        transform.Find("action").SetActive(false);
        GetComponent<CardDragDrop>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }
}
