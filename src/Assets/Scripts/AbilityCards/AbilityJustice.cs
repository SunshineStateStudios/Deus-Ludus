using UnityEngine;
using TMPro;

public class AbilityJustice : AbilityCard
{
    public override string name => "Justice";
    public override string description => "Swap all your Number Cards' <color=#ff8282>attack</color> values with their <color=#8787ff>defence</color> values.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        GameObject parent = gm.playerNumberCards;
        if (owner == gm.ply2) parent = gm.enemyNumberCards;

        for (int i = 0; i < owner.NumberCards.Count; i++)
        {
            NumberCard card = owner.NumberCards[i];

            int DamageTempValue = card.Damage;
            card.Damage = card.Health;
            card.Health = DamageTempValue;

            Transform cardRepresentation = parent.transform.Find(i.ToString());

            TMP_Text healthText =
                cardRepresentation.transform.Find("Container/Canvas/DefendValue")
                .GetComponent<TMP_Text>();
            TMP_Text damageText =
                cardRepresentation.transform.Find("Container/Canvas/AttackValue")
                .GetComponent<TMP_Text>();

            healthText.text = card.Health.ToString();
            damageText.text = card.Damage.ToString();
        }
        canvasMngr.CalculateText(gm.ply1, gm.ply2, true);
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        if (owner.NumberCards.Count == 0)
            return false;

        int damageTotal = 0;
        int healthTotal = 0;

        foreach (NumberCard card in owner.NumberCards)
        {
            damageTotal += card.Damage;
            healthTotal += card.Health;
        }

        float avgDamage = (float) damageTotal / owner.NumberCards.Count;
        float avgHealth = (float) healthTotal / owner.NumberCards.Count;

        return avgHealth > avgDamage;
    }
}