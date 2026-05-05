using UnityEngine;
using TMPro;

public class AbilityEmperor : AbilityCard
{
    public override string name => "Emperor";
    public override string description => "Draws a <color=#ff8282>number</color> card between 5-8";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        NumberCard card = new NumberCard(Random.Range(5,9), Random.Range(1,4));
        owner.NumberCards.Add(card);

        if (owner == gm.ply1 && owner.BlackjackTotal(gm.BlackjackThreshold, false) >= 21) gm.canvasObject.GetComponent<CanvasManager>().ShowDrawButton(false);

        GameObject parent = (owner == gm.ply2)
            ? gm.enemyNumberCards
            : gm.playerNumberCards;

        GameObject cardRepresentation = gm.InstantiateNumberCard(parent.transform);
        gm.RepositionCards(parent.transform);
        cardRepresentation.name = (owner.NumberCards.Count - 1).ToString();

        NumberCardVisuals cardVisualsScript = cardRepresentation.GetComponent<NumberCardVisuals>();

        if (owner.NumberCards.Count == 1 && owner == gm.ply2) {
            cardVisualsScript.cardSuit = 978;
        } else {
            cardVisualsScript.cardSuit = card.Suit - 1;
        }

        TMP_Text valueText = cardRepresentation.transform.Find("Container/Canvas/ValueText").GetComponent<TMP_Text>();
        TMP_Text valueShadowText = cardRepresentation.transform.Find("Container/Canvas/ValueTextShadow").GetComponent<TMP_Text>();
        TMP_Text damageText = cardRepresentation.transform.Find("Container/Canvas/AttackValue").GetComponent<TMP_Text>();
        TMP_Text healthText = cardRepresentation.transform.Find("Container/Canvas/DefendValue").GetComponent<TMP_Text>();

        if (owner == gm.ply2 && gm.ply2.NumberCards.Count == 1)
        {
            valueText.text = "?";
            valueShadowText.text = "?";
            damageText.text = "?";
            healthText.text = "?";
        }
        else
        {
            if (card.Value == 12) {
                valueText.text = "";
                valueShadowText.text = "";

                GameObject cardModel = cardVisualsScript.CardVariations[cardVisualsScript.cardSuit];
                Renderer renderer = cardModel.GetComponent<Renderer>();
                Material[] mats = renderer.materials;
                
                int frontIndex = -1;
                int backIndex = -1;

                for (int i = 0; i < mats.Length; i++) {
                    if (mats[i].name.Contains("-Front")) {
                        frontIndex = i;
                    } else if (mats[i].name.Contains("-Back")) {
                        backIndex = i;
                    }
                }

                if (frontIndex != -1 && backIndex != -1) {
                    mats[frontIndex] = mats[backIndex];
                    renderer.materials = mats;
                }
            } else {
                valueText.text = card.Value.ToString();
                valueShadowText.text = card.Value.ToString();
            }
            damageText.text = card.Damage.ToString();
            healthText.text = card.Health.ToString();
        }

        gm.canvasObject.GetComponent<CanvasManager>().CalculateText(gm.ply1, gm.ply2, true, true, 2f);
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