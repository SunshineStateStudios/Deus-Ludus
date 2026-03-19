using UnityEngine;
using TMPro;

public class AbilityHighPriestess : PromptAbilityCard
{
    public override string name => "High Priestess";
    public override string description => "Sacrifice an ability you hold to reveal the opponent's hidden card.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForNumberCard(owner, "Choose a Number Card!", "The High Priestess card calls for it...", this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        gm.RemoveNumberCard(owner, indexChosen);

        NumberCard firstCard = opponent.NumberCards[0];
        Transform parent = gm.enemyNumberCards.transform;
        if (opponent == gm.ply1) parent = gm.playerNumberCards.transform;

        GameObject firstCardRepresentation = parent.Find("0").gameObject;

        TMP_Text healthText =
            firstCardRepresentation.transform.Find("Card/Canvas/HealthLabel")
            .GetComponent<TMP_Text>();
        TMP_Text damageText =
            firstCardRepresentation.transform.Find("Card/Canvas/DamageLabel")
            .GetComponent<TMP_Text>();
        TMP_Text valueText =
            firstCardRepresentation.transform.Find("Card/Canvas/ValueLabel")
            .GetComponent<TMP_Text>();

        healthText.text = firstCard.Health.ToString();
        damageText.text = firstCard.Damage.ToString();
        valueText.text = firstCard.Value.ToString();
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        return true;
    }
}