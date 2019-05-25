using UnityEngine;
using System.Collections;

public class CardDragDrop : UIDragDropItem
{
    public static CardDragDrop dragging = null;

    public bool forceMove = false;

    protected override void OnDragStart()
    {
        base.OnDragStart();
        dragging = this;
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        dragging = null;
    }

    protected override void OnClone(GameObject ori)
    {
        var cardview = ori.GetComponent<CardView>();
        var oriData = cardview.Data;
        GetComponent<CardView>().SetCard(oriData, false);
        base.OnClone(ori);
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        if (forceMove)
            return;

        if (surface != null)
        {
            var cardView = gameObject.GetComponent<CardView>();
            var cardData = cardView.Data;

            if (surface.transform.parent && surface.transform.parent.name == "holders")
            {
                var parentName = surface.name.Replace("_holder", "");
                var holderParent = GameObject.Find(parentName);
                var cardPile = holderParent.GetComponent<CardPile>();

                base.OnDragDropRelease(surface);
                if (cardView.isOnPile)
                {
                    return;
                }
                    
                cardView.Play();
                cardView.Action();
                cardPile.AddCard(gameObject);
            }
            else if(cardView.isOnPile && surface.name.Contains("character1")) // 임시 하드 코딩! 공겨겨겨격!!
            {
                var characterView = surface.GetComponent<CharacterView>();
                characterView.Damage(cardData.attack);

                cardView.Action();
                base.OnDragDropRelease(surface);
            }
            else
            {
                base.OnDragDropRelease(surface);
            }
        }
    }
}
