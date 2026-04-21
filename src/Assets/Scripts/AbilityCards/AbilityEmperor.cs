using UnityEngine;
using TMPro;

public class AbilityEmperor : AbilityCard
{
    public override string name => "Emperor";
    public override string description => "Draws a <color=#ff8282>number</color> card between 5-8";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        NumberCard card = new NumberCard(Random.Range(5,9), Random.Range(1,4));
        owner.NumberCards.Add(card);

        GameObject parent = (owner == gm.ply2)
            ? gm.enemyNumberCards
            : gm.playerNumberCards;

        GameObject cardRepresentation = gm.InstantiateNumberCard(parent.transform);
        cardRepresentation.name = (owner.NumberCards.Count-1).ToString();

        TMP_Text valueText =
            cardRepresentation.transform.Find("Card/Canvas/ValueLabel")
            .GetComponent<TMP_Text>();

        TMP_Text damageText =
            cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
            .GetComponent<TMP_Text>();

        TMP_Text healthText =
            cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
            .GetComponent<TMP_Text>();

        valueText.text = card.Value.ToString();
        damageText.text = card.Damage.ToString();
        healthText.text = card.Health.ToString();

        gm.RepositionCards(parent.transform);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        int currentTotal = owner.BlackjackTotal(gm.BlackjackThreshold, false);

        if (currentTotal >= gm.BlackjackThreshold - 4)
            return false;

        if (currentTotal <= gm.BlackjackThreshold / 2)
            return true;

        if (currentTotal < gm.BlackjackThreshold - 1)
            return Random.value > 0.5f;

        return false;
    }
}