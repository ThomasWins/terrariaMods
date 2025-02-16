using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 
	public class PoopBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 15;
            Item.scale = .62f;
            Item.knockBack = 2;
            Item.useTime = 55;
            Item.useAnimation = 55;
            Item.autoReuse = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Green;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ProjectileID.PoisonFang;
            Item.shootSpeed = 30f;
            Item.UseSound = SoundID.Item16;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DirtBlock, 50);
            recipe.AddRecipeGroup("Wood", 50);
            recipe.AddIngredient(ModContent.ItemType<BrokenPoopBow>());
            recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}
