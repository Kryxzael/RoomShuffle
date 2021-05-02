using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreditsDebugPage : DebugPage
{
    public override string Header { get; } = "Game Credits";

    protected override void RunItems(DebugMenu caller)
    {
        ReadOnly("-- D E V E L O P E D   B Y --");
        ReadOnly("KASPER RAMSKJELL");
        ReadOnly("Room Generation System Developer");
        ReadOnly("Weapon System Developer");
        ReadOnly("Hitbox/Hurtbox System Developer");
        ReadOnly("Level Designer");
        ReadOnly("Tileset, Items, Weapon and HUD artist");
        ReadOnly("Soundtrack composer");
        Separator();
        ReadOnly("TORMOD KVITBERG");
        ReadOnly("Enemy Programming");
        ReadOnly("Weapon Designer");
        ReadOnly("Audio System");
        ReadOnly("HUD Programmer");
        ReadOnly("Level Designer");
        ReadOnly("Website Builder");
        Separator();
        ReadOnly("MAIJA U.");
        ReadOnly("Player, Enemy and Background artist");
        Separator();
        ReadOnly("FREDRIK HAUGEN");
        ReadOnly("Early feedback and insight");
        Separator();
        ReadOnly("-- S P E C I A L   T H A N K S --");
        ReadOnly("OUR PLAYTESTERS");
        ReadOnly("Eivind Kvitberg");
        ReadOnly("Fredrik Ramskjell");
        ReadOnly("Fredrik Haugen");
        ReadOnly("Maija U.");
        ReadOnly("AND YOU!");
    }
}