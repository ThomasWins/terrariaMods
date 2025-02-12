using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class BrokenPoopBow : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.TutorialMod.hjson' file.
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
            Item.rare = ItemRarityID.White;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ProjectileID.PoisonDart;
            Item.shootSpeed = 5f;
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
