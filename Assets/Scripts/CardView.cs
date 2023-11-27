using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText, powerText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject canAttackPanel;

    public void Show(CardModel cardModel) // cardModelÇÃÉfÅ[É^éÊìæÇ∆îΩâf
    {
        nameText.text = cardModel.name;
        powerText.text = cardModel.power.ToString();
        iconImage.sprite = cardModel.icon;
    }

    public void SetCanAttackPanel(bool flag)
    {
        canAttackPanel.SetActive(flag);
    }
}