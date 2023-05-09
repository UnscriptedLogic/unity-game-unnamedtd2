using UnscriptedLogic.Currency;

public class PowerShot : Ability
{
    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 2, 5, 8 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);
    }
}
