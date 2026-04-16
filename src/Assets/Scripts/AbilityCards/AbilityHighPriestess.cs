using UnityEngine;
using TMPro;

public class AbilityHighPriestess : PromptAbilityCard
{
    public override string name => "High Priestess";
    public override string description => "Sacrifice an ability you hold to reveal the opponent's hidden card.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForAbilityCard(owner, this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        gm.RemoveAbilityCard(owner, indexChosen);

        if (opponent == gm.ply1) return;

        GameObject hiddenCard = gm.enemyNumberCards.transform.Find("0").gameObject;
        NumberCard enemyFirstCard = opponent.NumberCards[0];

        TMP_Text valueText =
        hiddenCard.transform.Find("Card/Canvas/ValueLabel")
        .GetComponent<TMP_Text>();

        TMP_Text damageText =
        hiddenCard.transform.Find("Card/Canvas/DamageLabel")
        .GetComponent<TMP_Text>();

        TMP_Text healthText =
        hiddenCard.transform.Find("Card/Canvas/HealthLabel")
        .GetComponent<TMP_Text>();

        valueText.text = enemyFirstCard.Value.ToString();
        damageText.text = enemyFirstCard.Damage.ToString();
        healthText.text = enemyFirstCard.Health.ToString();
    }

    public override int AICardDecision(Player player) {
        int chosenIndex = 0;

        for (int i = 1; i < player.NumberCards.Count; i++) {
            NumberCard oldCard = player.NumberCards[chosenIndex];
            NumberCard currentCard = player.NumberCards[i];

            if (currentCard.Damage < oldCard.Damage || currentCard.Health < oldCard.Health) {
                chosenIndex = i;
            }
        }

        return chosenIndex;
    }

    public override void Remove(GameManager gm, Player owner, Player opponent)
    {

    }

    public override bool AIShouldDraw(GameManager gm, Player owner)
    {
        if (owner.AbilityCards.Count <= 1)
            return false;

        Player opponent = gm.ply1;
        NumberCard opponentFirstCard = opponent.NumberCards[0];

        int threatValue = opponentFirstCard.Value + opponentFirstCard.Damage + opponentFirstCard.Health;

        return threatValue >= 8;
    }
}
