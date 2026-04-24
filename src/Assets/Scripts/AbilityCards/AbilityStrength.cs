using UnityEngine;
using TMPro;

public class AbilityStrength : PromptAbilityCard
{
    public override string name => "Strength";
    public override string description => "Pick a number card you hold to double its <color=#ff8282>attack</color>.";
    public override int triesDecayTime => 1;

    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForNumberCard(owner, this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        CanvasManager canvasMngr = gm.canvasObject.GetComponent<CanvasManager>();
        owner.NumberCards[indexChosen].Damage *= 2;

        GameObject parent = gm.playerNumberCards;
        if (owner == gm.ply2) parent = gm.enemyNumberCards;

        Transform cardRepresentation = parent.transform.Find(indexChosen.ToString());

        TMP_Text damageLabel =
            cardRepresentation.transform.Find("Card/Canvas/DamageLabel")
            .GetComponent<TMP_Text>();

        damageLabel.text = owner.NumberCards[indexChosen].Damage.ToString();

        canvasMngr.CalculateText(gm.ply1, gm.ply2, true);
    }

    public override int AICardDecision(Player player) {
        int chosenIndex = 0;

        for (int i = 1; i < player.NumberCards.Count; i++) {
            NumberCard oldCard = player.NumberCards[chosenIndex];
            NumberCard currentCard = player.NumberCards[i];

            if (currentCard.Damage > oldCard.Damage) {
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
        return owner.NumberCards.Count >= 2;
    }
}
