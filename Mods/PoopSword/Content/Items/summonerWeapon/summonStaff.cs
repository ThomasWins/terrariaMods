using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PoopSword.Content.Buffs;
using PoopSword.Content.Items.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace PoopSword.Content.Items.summonerWeapon
{

    public class summonStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
            ItemID.Sets.LockOnIgnoresCollision[Type] = true;

            ItemID.Sets.StaffMinionSlotsRequired[Type] = 1f;
        }

        public override void SetDefaults()
        {
            Item.width = 10;
            Item.height = 10;
            Item.scale = .1f;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item44;

            Item.DamageType = DamageClass.Summon;
            Item.damage = 8;
            Item.knockBack = 2f;
            Item.mana = 10;
            Item.noMelee = true;

            Item.value = Item.buyPrice(silver: 19, copper: 50);
            Item.rare = ItemRarityID.Blue;

            Item.shoot = ModContent.ProjectileType<myMinion>();
            Item.buffType = ModContent.BuffType<MyMinionBuff>();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2); // Provies the buff to the player

            // Direction of projectile we create
            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            projectile.originalDamage = Item.damage; // Set our damage

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Stinger, 5);
			recipe.AddIngredient(ItemID.Gel, 50);
            recipe.AddRecipeGroup("Wood", 100);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
        }
    }
}