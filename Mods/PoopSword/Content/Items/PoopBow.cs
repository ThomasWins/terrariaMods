using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class PoopBow : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.TutorialMod.hjson' file.
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 15;
            Item.scale = .62f;
            Item.knockBack = 2;
            Item.useTime = 50;
            Item.useAnimation = 50;
            Item.autoReuse = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Green;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ProjectileID.PoisonDart;
            Item.shootSpeed = 6f;
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
