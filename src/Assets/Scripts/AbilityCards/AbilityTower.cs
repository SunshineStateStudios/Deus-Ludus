using UnityEngine;
using TMPro;

public class AbilityTower : PromptAbilityCard
{
    public override string name => "Tower";
    public override string description => "Choose a Number Card you hold to copy the highest defense the opponent has.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForNumberCard(owner, "Choose a Number Card!", "The Tower card calls for it...", this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        NumberCard chosenCard = owner.NumberCards[indexChosen];
        NumberCard oppCard = opponent.NumberCards[0];

        GameObject parent = gm.playerNumberCards;
        if (owner == gm.ply2) parent = gm.enemyNumberCards;

        foreach (NumberCard card in opponent.NumberCards) {
            if (card.Health > oppCard.Health) {
                oppCard = card;
            }
        }

        chosenCard.Health = oppCard.Health;

        Transform cardRepresentation = parent.transform.Find(indexChosen.ToString());
        TMP_Text healthText =
            cardRepresentation.transform.Find("Card/Canvas/HealthLabel")
            .GetComponent<TMP_Text>();

        healthText.text = oppCard.Health.ToString();
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