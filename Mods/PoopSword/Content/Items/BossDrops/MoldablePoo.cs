using Terraria;
using Terraria.ModLoader;

namespace PoopSword.Content.Items.BossDrops
{

    public class MoldablePoo : ModItem{

        public override void SetStaticDefaults() {

			Item.ResearchUnlockCount = 100;
		}

		public override void SetDefaults() {
			Item.width = 20; // The item texture's width
			Item.height = 20; // The item texture's height

			Item.maxStack = Item.CommonMaxStack; // The item's max stack value
			Item.value = Item.buyPrice(silver: 70); // The value of the item in copper coins. Item.buyPrice & Item.sellPrice are helper methods that returns costs in copper coins based on platinum/gold/silver/copper arguments provided to it.
		}
    }
}