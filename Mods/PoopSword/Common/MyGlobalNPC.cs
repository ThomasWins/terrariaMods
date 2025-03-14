using PoopSword.Content.Items.Accessories;
using PoopSword.Content.Items.Consumables;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Common
{

    public class MyGlobalNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BlueSlime) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TutorialAccessory>(), 50));
            }
            if (npc.type == NPCID.Bunny) {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FirstBossSummonItem>(), 10));
            }
           
        }
    }
}