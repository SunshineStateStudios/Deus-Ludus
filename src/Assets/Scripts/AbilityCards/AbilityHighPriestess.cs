using UnityEngine;
using TMPro;

public class AbilityHighPriestess : PromptAbilityCard
{
    public override string name => "High Priestess";
    public override string description => "Sacrifice an <color=#ff8282>ability</color> you hold to reveal the opponent's <color=#ff8282>hidden</color> card.";
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
        hiddenCard.transform.Find("Container/Canvas/ValueText")
        .GetComponent<TMP_Text>();

        TMP_Text valueShadowText =
        hiddenCard.transform.Find("Container/Canvas/ValueTextShadow")
        .GetComponent<TMP_Text>();

        TMP_Text damageText =
        hiddenCard.transform.Find("Container/Canvas/DefendValue")
        .GetComponent<TMP_Text>();

        TMP_Text healthText =
        hiddenCard.transform.Find("Container/Canvas/AttackValue")
        .GetComponent<TMP_Text>();

        NumberCardVisuals cardVisualsScript = hiddenCard.GetComponent<NumberCardVisuals>();
        GameObject cardModel = cardVisualsScript.CardVariations[4];
        if (enemyFirstCard.Suit-1 >= 0 && enemyFirstCard.Suit-1 < cardVisualsScript.CardVariations.Length) cardModel = cardVisualsScript.CardVariations[enemyFirstCard.Suit-1];

        cardModel.SetActive(true);
        foreach (GameObject chosenCard in cardVisualsScript.CardVariations) {
            if (chosenCard == cardModel) continue;
            chosenCard.SetActive(false);
        }

        if (enemyFirstCard.Value == 12) {
            valueText.text = "";
            valueShadowText.text = "";

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
            valueText.text = enemyFirstCard.Value.ToString();
            valueShadowText.text = enemyFirstCard.Value.ToString();
        }
        
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
