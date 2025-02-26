using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace NPCdeath.Common.GlobalNPCs
{
    public class NoRespawnNPC : GlobalNPC
    {
        public static HashSet<int> deadNPCs = new HashSet<int>();

        public override void OnKill(NPC npc)
        {
            if (npc.townNPC)
            {
                deadNPCs.Add(npc.type);
            }
        }

        public static bool HasBeenKilled(int npcType)
        {
            return deadNPCs.Contains(npcType);
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (deadNPCs.Contains(npc.type))
            {
                npc.active = false; // Forcefully disable NPC before it spawns
            }
        }
    }
}