using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 

	public class BrokenPoopBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 10;
            Item.crit = -4;
            Item.scale = .6f;
            Item.knockBack = 2;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.value = 0;
            Item.shoot = ProjectileID.PoisonFang;
            Item.shootSpeed = 20f;
            Item.UseSound = SoundID.Item16;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 20);
            recipe.AddRecipeGroup("Wood", 10);
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
