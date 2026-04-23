using UnityEngine;
using TMPro;

public class AbilityMagician : PromptAbilityCard
{
    public override string name => "Magician";
    public override string description => "Pick an ability card in your inventory to copy it.";
    public override int triesDecayTime => 1;
    
    public override void Apply(GameManager gm, Player owner, Player opponent)
    {
        gm.PromptForAbilityCard(owner, this);
    }

    public override void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen)
    {
        AbilityCard cardChosen = owner.AbilityCards[indexChosen];
        AbilityCard newInstance;
        switch (cardChosen.GetType().Name) { // This unironically fucking sucks but C# is worse so I have to write this
            case "AbilityChariot":
                newInstance = new AbilityChariot();
                break;
            case "AbilityDeath":
                newInstance = new AbilityDeath();
                break;
            case "AbilityDevil":
                newInstance = new AbilityDevil();
                break;
            case "AbilityEmperor":
                newInstance = new AbilityEmperor();
                break;
            case "AbilityEmpress":
                newInstance = new AbilityEmpress();
                break;
            case "AbilityHangedMan":
                newInstance = new AbilityHangedMan();
                break;
            case "AbilityHermit":
                newInstance = new AbilityHermit();
                break;
            case "AbilityHierophant":
                newInstance = new AbilityHierophant();
                break;
            case "AbilityHighPriestess":
                newInstance = new AbilityHighPriestess();
                break;
            case "AbilityJudgement":
                newInstance = new AbilityJudgement();
                break;
            case "AbilityJustice":
                newInstance = new AbilityJustice();
                break;
            case "AbilityLovers":
                newInstance = new AbilityLovers();
                break;
            case "AbilityMagician":
                newInstance = new AbilityMagician();
                break;
            case "AbilityMoon":
                newInstance = new AbilityMoon();
                break;
            case "AbilityStar":
                newInstance = new AbilityStar();
                break;
            case "AbilityStrength":
                newInstance = new AbilityStrength();
                break;
            case "AbilitySun":
                newInstance = new AbilitySun();
                break;
            case "AbilityTemperance":
                newInstance = new AbilityTemperance();
                break;
            case "AbilityTower":
                newInstance = new AbilityTower();
                break;
            case "AbilityWheelOfFortune":
                newInstance = new AbilityWheelOfFortune();
                break;
            default:
                newInstance = new AbilityWorld();
                break;
        }

        newInstance.icon = Resources.Load<Sprite>("Icons/" + newInstance.GetType().Name.Replace("Ability", ""));
        owner.AbilityCards.Add(newInstance);
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