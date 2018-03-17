using UnityEngine;
using System.Collections;

public class Card
{
    public int attack;
    public int hp;
    public int cost;

    public Card(int _attack, int _hp, int _cost)
    {
        attack = _attack;
        hp = _hp;
        cost = _cost;
    }
}
