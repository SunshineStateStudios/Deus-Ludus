using UnityEngine;
using TMPro;

public class AbilityJustice : AbilityCard
{
    public override string name => "Justice";
    public override string description => "Swap all your Number Cards' attack values with their defence values.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
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
                cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
                .GetComponent<TMP_Text>();
            TMP_Text damageText =
                cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
                .GetComponent<TMP_Text>();

            healthText.text = card.Health.ToString();
            damageText.text = card.Damage.ToString();
        }
        gm.UpdateAttackDefendText();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {
        
    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}