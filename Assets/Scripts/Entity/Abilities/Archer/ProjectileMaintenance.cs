using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class ProjectileMaintenance : Ability
{
    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 5, 7, 9 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);
    }

    protected override void OnLevelUp()
    {
        tower.PierceHandler.Modify(ModifyType.Add, 2);
    }
}
