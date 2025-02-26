using System.Collections.Generic;
using NPCdeath.Common.GlobalNPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NPCdeath.Common.Systems
{

    public class NoRespawnSystem : ModSystem
    {

        public override void PreUpdatePlayers()
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (player.active)
                {
                    foreach (int npcType in NoRespawnNPC.deadNPCs)
                    {
                        if (npcType != -1) // Skip if invalid
                        {
                            Main.townNPCCanSpawn[npcType] = false;
                        }
                    }
                }   
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            Mod.Logger.Info($"Saving deadNPCs: {string.Join(", ", NoRespawnNPC.deadNPCs)}");
            tag["deadNPCs"] = new List<int>(NoRespawnNPC.deadNPCs);
        }

        public override void LoadWorldData(TagCompound tag)
        {
            if (tag.ContainsKey("deadNPCs"))
            {
                NoRespawnNPC.deadNPCs = new HashSet<int>(tag.Get<List<int>>("deadNPCs"));
                Mod.Logger.Info($"Loaded deadNPCs: {string.Join(", ", NoRespawnNPC.deadNPCs)}");

            }
        }

        // Remove saved data on world erase
        public override void ClearWorld()
        {
            NoRespawnNPC.deadNPCs.Clear();
        }
    }
}