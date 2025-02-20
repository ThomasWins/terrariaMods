using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using PoopSword.Content.Items.BossDrops;
using PoopSword.Content.Items.Projectiles;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 

	public class HardenedPoopSword : ModItem
	{
		public int attackType = 0; // keeps track of which attack it is
		public int comboExpireTimer = 0;

		public override void SetDefaults()
		{
			Item.damage = 15; 
			Item.DamageType = DamageClass.Melee;
			Item.crit = 4;
			Item.width = 1;
			Item.height = 1;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override bool? UseItem(Player player)
		{
			for (int i = 0; i < 5; i++) // Number of dust particles
    		{
        		Vector2 dustPosition = player.Center + Main.rand.NextVector2Circular(20f, 20f); // Random offset
        		Dust dust = Dust.NewDustDirect(dustPosition, 0, 0, DustID.FartInAJar, 0f, 0f, 100, default, 1.5f);
        		dust.velocity *= 0.5f;
        		dust.noGravity = true; // Make the dust float
    		}

    		int delay = 30; // Half a second (30 ticks)
    		Task.Run(async () =>
    		{
        		await Task.Delay(delay * 5); // Convert game ticks to milliseconds
        		if (player.HeldItem.type == ModContent.ItemType<HardenedPoopSword>())
        		{
            		Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem),
                                     player.Center,
                                     player.DirectionTo(Main.MouseWorld) * 5f,
                                     ModContent.ProjectileType<SwordProjectile>(),
                                     10, // Damage
                                     1f, // Knockback
                                     player.whoAmI);
        		}
    		});
    		return true;
		}
		

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<MoldablePoo>());
            recipe.AddIngredient(ModContent.ItemType<PoopSword>());
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
	}
}
