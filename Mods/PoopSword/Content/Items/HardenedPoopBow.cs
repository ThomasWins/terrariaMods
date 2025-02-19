using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class HardenedPoopBow : ModItem
	{
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.TutorialMod.hjson' file.
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Shoot;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 18; 
			Item.crit = 4;
            Item.scale = .65f;
            Item.knockBack = 1;
            Item.useTime = 40; 
            Item.useAnimation = 40; 
            Item.autoReuse = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Blue;
            Item.ammo = AmmoID.Arrow;
            Item.shoot = ProjectileID.PoisonFang;
            Item.shootSpeed = 40f;
            Item.UseSound = SoundID.Item16;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.AddIngredient(ItemID.MudBlock, 20);
            recipe.AddRecipeGroup("Wood", 100);
            recipe.AddIngredient(ModContent.ItemType<PoopBow>());
            recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
	}
}
