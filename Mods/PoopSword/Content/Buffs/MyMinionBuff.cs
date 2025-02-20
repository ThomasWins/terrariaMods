using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PoopSword.Content.Items.Projectiles;

namespace PoopSword.Content.Buffs
{
    public class MyMinionBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // Buff is not preserved, dont need to savr info with minions
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true; // Not time based
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Contains # of projectiles that belong to the player
            // Because we override shoot method in staff, passed player as the owner
            if (player.ownedProjectileCounts[ModContent.ProjectileType<myMinion>()] > 0) {
                player.buffTime[buffIndex] = 20000;
                return;
            }

            // Minion projectile does not exist = remove buff
            player.DelBuff(buffIndex);
            buffIndex--;
        }
    }
}