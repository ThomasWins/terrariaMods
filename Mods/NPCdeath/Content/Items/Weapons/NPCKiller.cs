using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NPCdeath.Content.Items.Weapons
{ 
	public class NPCKiller : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 1000;
			Item.DamageType = DamageClass.Melee;
			Item.width = 5;
			Item.height = 5;
			Item.useTime = 18;
			Item.useAnimation = 18;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 5;
			Item.value = 0;
			Item.rare = ItemRarityID.Green;
			Item.autoReuse = true;
		}

		// Item can hit friendly NPCs
        public override bool? CanHitNPC(Player player, NPC target)
        {
            return true;
       }
    }
}