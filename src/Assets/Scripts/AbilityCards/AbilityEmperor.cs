using UnityEngine;
using TMPro;

public class AbilityEmperor : AbilityCard
{
    public override string name => "Emperor";
    public override string description => "Draws a number card between 5-8";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        NumberCard card = new NumberCard(Random.Range(5,9), Random.Range(1,4));
        owner.NumberCards.Add(card);

        gm.UpdateProgressText();
        gm.UpdateDrawNumberCardText();

        /*TMP_Text YouDefenseTxt = gm.YouDefense.GetComponent<TMP_Text>();
        TMP_Text YouAttackTxt = gm.YouAttack.GetComponent<TMP_Text>();
        TMP_Text OppDefenseTxt = gm.OppDefense.GetComponent<TMP_Text>();
        TMP_Text OppAttackTxt = gm.OppAttack.GetComponent<TMP_Text>();
        YouDefenseTxt.text = gm.ply1.TotalHealth(gm.BlackjackThreshold, false).ToString();
        YouAttackTxt.text = gm.ply1.TotalDamage(false).ToString();
        OppDefenseTxt.text = gm.ply2.TotalHealth(gm.BlackjackThreshold).ToString() + "?";
        OppAttackTxt.text = gm.ply2.TotalDamage().ToString() + "?";*/

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
        return true;
    }
}