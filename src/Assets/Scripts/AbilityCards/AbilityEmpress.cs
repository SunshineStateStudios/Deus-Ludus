using UnityEngine;
using TMPro;

public class AbilityEmpress : AbilityCard
{
    public override string name => "Empress";
    public override string description => "Draws a <color=#ff8282>number</color> card between 1-4";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        NumberCard card = new NumberCard(Random.Range(1,5), Random.Range(1,4));
        owner.NumberCards.Add(card);

        if (owner == gm.ply1 && owner.BlackjackTotal(gm.BlackjackThreshold, false) >= 21) gm.canvasObject.GetComponent<CanvasManager>().ShowDrawButton(false);

        GameObject parent = (owner == gm.ply2)
            ? gm.enemyNumberCards
            : gm.playerNumberCards;

        GameObject cardRepresentation = gm.InstantiateNumberCard(parent.transform);
        cardRepresentation.name = (owner.NumberCards.Count-1).ToString();

        TMP_Text valueText =
            cardRepresentation.transform.Find("Container/Canvas/ValueText")
            .GetComponent<TMP_Text>();

        TMP_Text valueShadowText =
            cardRepresentation.transform.Find("Container/Canvas/ValueTextShadow")
            .GetComponent<TMP_Text>();

        TMP_Text damageText =
            cardRepresentation.transform.Find("Container/Canvas/AttackValue")
            .GetComponent<TMP_Text>();

        TMP_Text healthText =
            cardRepresentation.transform.Find("Container/Canvas/DefendValue")
            .GetComponent<TMP_Text>();

        valueText.text = card.Value.ToString();
        valueShadowText.text = card.Value.ToString();
        damageText.text = card.Damage.ToString();
        healthText.text = card.Health.ToString();

        gm.RepositionCards(parent.transform);
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        gm.BlackjackThreshold -= 3;
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        int currentTotal = owner.BlackjackTotal(gm.BlackjackThreshold, false);

        if (currentTotal >= gm.BlackjackThreshold)
            return false;

        if (currentTotal >= gm.BlackjackThreshold - 3)
            return Random.value > 0.3f;

        return true;
    }
}