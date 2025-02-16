using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items.Accessories
{

    [AutoloadEquip(EquipType.Beard)]
    internal class TutorialAccessory : ModItem
    {

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;

            Item.accessory = true;
            Item.value = Item.buyPrice(silver: 20, copper: 50);
            Item.rare = ItemRarityID.Blue;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetCritChance(DamageClass.Generic) += 2;
            player.GetKnockback(DamageClass.Generic) += .05f;
            player.moveSpeed += .15f;
        }
    }
}