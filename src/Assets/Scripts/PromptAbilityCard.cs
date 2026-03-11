public abstract class PromptAbilityCard : AbilityCard
{
    public abstract void PromptChosen(GameManager gm, Player owner, Player opponent, int indexChosen);
}
