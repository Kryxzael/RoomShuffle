using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LevelsDebugPage : DebugPage
{
    public override string Header { get; } = "Levels";

    protected override void RunItems(DebugMenu caller)
    {
        ReadOnly($"Health Level: {Commons.PlayerProgression.HealthLevel}");
        if (Button("Level Up Health"))
        {
            Commons.PlayerProgression.LevelUpHealth();
        }

        Separator();

        ReadOnly($"Damage Level: {Commons.PlayerProgression.DamageLevel}");
        if (Button("Level Up Damage"))
        {
            Commons.PlayerProgression.LevelUpHealth();
        }

        Separator();

        ReadOnly($"Current Health: {Commons.PlayerHealth.Health}");
        if (Button("Full Heal"))
        {
            Commons.PlayerHealth.FullyHeal();
        }

        if (Button("Heal 100 HP"))
        {
            Commons.PlayerHealth.Heal(100);
        }

        if (Button("Damage 100 HP"))
        {
            Commons.PlayerHealth.DealDamage(100);
        }

        if (Button("Kill"))
        {
            Commons.PlayerHealth.Kill();
        }
    }
}
