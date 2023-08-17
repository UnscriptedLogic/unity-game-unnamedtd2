public record TowerStats(int RangePercent = 0, int AttackSpeed = 0, int Damage = 0, int ExpGainPercent = 0, int CritRatePercent = 0, int CritDmgPercent = 0, float ProjectileSpeed = 0f, float ProjectileLifetime = 0f, int ProjectilePierce = 0)
{
    public int RangePercent { get; } = RangePercent;
    public int AttackSpeed { get; } = AttackSpeed;
    public int Damage { get; } = Damage;
    public int ExpGainPercent { get; } = ExpGainPercent;
    public int CritRatePercent { get; } = CritRatePercent;
    public int CritDmgPercent { get; } = CritDmgPercent;

    public float ProjectileSpeed { get; } = ProjectileSpeed;
    public float ProjectileLifetime { get; } = ProjectileLifetime;
    public int ProjectilePierce { get; } = ProjectilePierce;

    private int RangeDistanceLimitReference => 20; //20 units of range covers the entire map so typically we want something of this percent

    public float RangeDistance => RangePercent / 100f * RangeDistanceLimitReference;

    public static TowerStats operator +(TowerStats statsA, TowerStats statsB)
    {
        TowerStats stats = new TowerStats(
            statsA.RangePercent + statsB.RangePercent,
            statsA.AttackSpeed + statsB.AttackSpeed,
            statsA.Damage + statsB.Damage,
            statsA.ExpGainPercent + statsB.ExpGainPercent,
            statsA.CritRatePercent + statsB.CritRatePercent,
            statsA.CritDmgPercent + statsB.CritDmgPercent,
            statsA.ProjectileSpeed + statsB.ProjectileSpeed,
            statsA.ProjectileLifetime + statsB.ProjectileLifetime,
            statsA.ProjectilePierce + statsB.ProjectilePierce
            );

        return stats;
    }

    public static TowerStats operator -(TowerStats statsA, TowerStats statsB)
    {
        TowerStats stats = new TowerStats(
            statsA.RangePercent - statsB.RangePercent,
            statsA.AttackSpeed - statsB.AttackSpeed,
            statsA.Damage - statsB.Damage,
            statsA.ExpGainPercent - statsB.ExpGainPercent,
            statsA.CritRatePercent - statsB.CritRatePercent,
            statsA.CritDmgPercent - statsB.CritDmgPercent,
            statsA.ProjectileSpeed - statsB.ProjectileSpeed,
            statsA.ProjectileLifetime - statsB.ProjectileLifetime,
            statsA.ProjectilePierce - statsB.ProjectilePierce
            );

        return stats;
    }

    public static TowerStats operator *(TowerStats statsA, TowerStats statsB)
    {
        TowerStats stats = new TowerStats(
            statsA.RangePercent * statsB.RangePercent,
            statsA.AttackSpeed * statsB.AttackSpeed,
            statsA.Damage * statsB.Damage,
            statsA.ExpGainPercent * statsB.ExpGainPercent,
            statsA.CritRatePercent * statsB.CritRatePercent,
            statsA.CritDmgPercent * statsB.CritDmgPercent,
            statsA.ProjectileSpeed * statsB.ProjectileSpeed,
            statsA.ProjectileLifetime * statsB.ProjectileLifetime,
            statsA.ProjectilePierce * statsB.ProjectilePierce
            );

        return stats;
    }

    public static TowerStats operator /(TowerStats statsA, TowerStats statsB)
    {
        TowerStats stats = new TowerStats(
            statsA.RangePercent / statsB.RangePercent,
            statsA.AttackSpeed / statsB.AttackSpeed,
            statsA.Damage / statsB.Damage,
            statsA.ExpGainPercent / statsB.ExpGainPercent,
            statsA.CritRatePercent / statsB.CritRatePercent,
            statsA.CritDmgPercent / statsB.CritDmgPercent,
            statsA.ProjectileSpeed / statsB.ProjectileSpeed,
            statsA.ProjectileLifetime / statsB.ProjectileLifetime,
            statsA.ProjectilePierce / statsB.ProjectilePierce
            );

        return stats;
    }
}