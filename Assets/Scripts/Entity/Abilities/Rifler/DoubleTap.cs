using UnscriptedLogic.Currency;
using UnscriptedLogic.MathUtils;

public class DoubleTap : Ability
{
    private float quickReloadTime = 0.25f;
    private int procChancePercent = 10;

    public override void OnAdded()
    {
        maxLevel = 4;

        levelRequirements = new int[3] { 9, 10, 11 };

        levelHandler = new CurrencyHandler(1, max: maxLevel);
    }

    protected override void OnProjectileFired()
    {
        if (RandomLogic.IntZeroTo(100) < procChancePercent)
        {
            tower.CurrentReloadtime = quickReloadTime;
        }
    }

    protected override void OnLevelUp()
    {
        if (levelHandler.Current == 2)
        {
            procChancePercent = 14;
        }
        else if (levelHandler.Current == 3)
        {
            procChancePercent = 19;
        }
        else if (levelHandler.Current == 4)
        {
            procChancePercent = 25;
        }
    }
}