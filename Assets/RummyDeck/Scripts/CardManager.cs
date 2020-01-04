using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    //Instance so we can access it from all scripts
    public static CardManager instance;
    [SerializeField] private List<Sprite> cardImages;   //ref to card sprites

    //important gameobjects
    [SerializeField] private GameObject cardHolder, cardPrefab, parentHolder, dummyCardPrefab;

    private CardView selectedCard, previousCard, nextCard;  //ref to cards
    int k, childCount;                                      
    private GameObject dummyCardObj;

    public CardView SelectedCard { get => selectedCard; }
    public GameObject ParentHolder { get => parentHolder; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 13; i++)
        {
            k = i;
            SpawnCard();
        }
        
    }

    /// <summary>
    /// Method called when we tap on any card
    /// </summary>
    /// <param name="card">Reference of tapped card</param>
    public void SelectCard(CardView card)
    {
        //save the selected card SiblingIndex [Child Index for its parent]
        int selectedIndex = card.transform.GetSiblingIndex();
        selectedCard = card;                        //set selectedCard to card
        selectedCard.ChildIndex = selectedIndex;    //set the selectedCard ChildIndex
        GetDummyCard().SetActive(true);             //activate dummy card
        GetDummyCard().transform.SetSiblingIndex(selectedIndex);    //set dummy card index
                                                    //change the parent of selectedCard
        selectedCard.transform.SetParent(CardManager.instance.ParentHolder.transform);

        childCount = cardHolder.transform.childCount;

        //check if selectedIndex is less than total childCount
        if (selectedIndex + 1 < childCount)
        {
            //set the next card of the selected card
            nextCard = cardHolder.transform.GetChild(selectedIndex + 1).GetComponent<CardView>();
        }

        if (selectedIndex - 1 >= 0)
        {
            //set the previous card of selected card
            previousCard = cardHolder.transform.GetChild(selectedIndex - 1).GetComponent<CardView>();
        }

        
    }

    /// <summary>
    /// Method called on release of tap
    /// </summary>
    public void OnCardRelease()
    {
        //if SelectedCard is not null
        if (SelectedCard != null)
        {
            GetDummyCard().SetActive(false);        //Deactivate Dummy card
            selectedCard.transform.SetParent(cardHolder.transform); //set selectedCard parent
                                                                    //set selectedCard SetSiblingIndex

            if (Mathf.Abs(selectedCard.transform.position.y - GetDummyCard().transform.position.y) > 80)
            {
                GetDummyCard().transform.SetParent(CardManager.instance.ParentHolder.transform);
                selectedCard.transform.SetSiblingIndex(selectedCard.ChildIndex);
            }
            else
            {
                selectedCard.transform.SetSiblingIndex(GetDummyCard().transform.GetSiblingIndex());
                GetDummyCard().transform.SetParent(CardManager.instance.ParentHolder.transform);
            }
            selectedCard = null;    //make selectedCard null
        }
    }

    /// <summary>
    /// Method called on drag of card
    /// </summary>
    /// <param name="position"></param>
    public void MoveSelectedCard(Vector2 position)
    {
        if (selectedCard != null)                           //if SelectedCard is not null
        {
            selectedCard.transform.position = position;     //set selectedCard position
            CheckWithNextCard();                            //check for next card
            CheckWithPreviousCard();                        //check for previous card
            //selectedCard.MoveCard();
        }
    }

    void CheckWithNextCard()
    {
        if (nextCard != null)
        {
            if (selectedCard.transform.position.x > nextCard.transform.position.x)
            {
                int index = nextCard.transform.GetSiblingIndex();
                nextCard.transform.SetSiblingIndex(dummyCardObj.transform.GetSiblingIndex());
                dummyCardObj.transform.SetSiblingIndex(index);

                previousCard = nextCard;
                if (index + 1 < childCount)
                {
                    nextCard = cardHolder.transform.GetChild(index + 1).GetComponent<CardView>();
                }
                else
                {
                    nextCard = null;
                }
            }
        }
    }

    void CheckWithPreviousCard()
    {
        if (previousCard != null)
        {
            if (selectedCard.transform.position.x < previousCard.transform.position.x)
            {
                int index = previousCard.transform.GetSiblingIndex();
                previousCard.transform.SetSiblingIndex(dummyCardObj.transform.GetSiblingIndex());
                dummyCardObj.transform.SetSiblingIndex(index);

                nextCard = previousCard;
                if (index - 1 >= 0)
                {
                    previousCard = cardHolder.transform.GetChild(index - 1).GetComponent<CardView>();
                }
                else
                {
                    previousCard = null;
                }
            }
        }
    }

    //Must be in another script
    #region Spawn Logic
    void SpawnCard()
    {
        GameObject card = Instantiate(cardPrefab);
        card.name = "Card " + k;
        card.transform.SetParent(cardHolder.transform);
        card.GetComponent<CardView>().SetCardImg(cardImages[k]);
    }

    GameObject GetDummyCard()
    {
        if (dummyCardObj == null)
        {
            dummyCardObj = Instantiate(dummyCardPrefab);
            dummyCardObj.name = "DummyCard";
            dummyCardObj.transform.SetParent(cardHolder.transform);
        }
        else
        {
            if (dummyCardObj.transform.parent != cardHolder.transform)
            {
                dummyCardObj.transform.SetParent(cardHolder.transform);
            }
            return dummyCardObj;
        }

        return dummyCardObj;
    }
    #endregion
}
