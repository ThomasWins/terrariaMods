using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
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

        public override void OnSpawn(NPC npc, IEntitySource source)
        {
            if (npc.type == NPCID.WallofFlesh){
                deadNPCs.Remove(NPCID.Guide);
            }
        }

        public static bool HasBeenKilled(int npcType)
        {
            return deadNPCs.Contains(npcType);
        }
    }
}