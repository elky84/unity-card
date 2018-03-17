using UnityEngine;
using System.Collections;

public class CharacterView : MonoBehaviour
{
    public Character Data;

    void Start()
    {
    }

    void Update()
    {

    }

    public void SetCharacter(Character data)
    {
        Data = data;

        if (Data.hp <= 0)
            transform.Find("hp").SetActive(false);
        else
            transform.Find("hp").SetText(Data.hp.ToString());

        transform.Find("attack").SetText(Data.attack.ToString());
    }

    public void Damage(int damage)
    {
        Data.hp -= damage;
        transform.Find("hp").SetText(Data.hp.ToString());
    }
}
