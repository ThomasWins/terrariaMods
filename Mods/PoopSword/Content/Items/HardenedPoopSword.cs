using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items
{ 

	public class HardenedPoopSword : ModItem
	{
		public override void SetDefaults()
		{
			Item.damage = 12; 
			Item.DamageType = DamageClass.Melee;
			Item.crit = 4;
			Item.width = 1;
			Item.height = 1;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Gel, 50);
			recipe.AddIngredient(ItemID.MudBlock, 20);
			recipe.AddRecipeGroup("Wood", 100);
            recipe.AddIngredient(ModContent.ItemType<PoopSword>());
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
		}
	}
}
