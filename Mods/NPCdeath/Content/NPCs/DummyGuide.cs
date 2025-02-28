using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace NPCdeath.Content.NPCs
{
    [AutoloadHead]
    public class DummyGuide : ModNPC 
    {
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 200; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 3; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.
			NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // This set says that the Town NPC has a Shimmered form. Otherwise, the Town NPC will become transparent when touching Shimmer like other enemies.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
			};

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) 
				.SetNPCAffection(NPCID.Guide, AffectionLevel.Love) 
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike) 
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate)
			;
        }

        public override void SetDefaults() {
			NPC.townNPC = true; 
			NPC.friendly = true; 
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 2;
			NPC.defense = 30;
			NPC.lifeMax = 20;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = -20f;

			AnimationType = NPCID.Guide;
		}

        public override void TownNPCAttackStrength(ref int damage, ref float knockback) {
			damage = 2;
			knockback = 0;
		}

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown) {
			cooldown = 30;
			randExtraCooldown = 30;
		}


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {

			bestiaryEntry.Info.AddRange([

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("He loves you and always wants to be around you."),

				new FlavorTextBestiaryInfoElement("Mods.NPCdeath.Bestiary.DummyGuide")
			]);
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {

			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers)) {
				drawModifiers.Rotation += 0.001f;

				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}

			return true;
		}

        public override void HitEffect(NPC.HitInfo hit) {
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++) {
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Dirt);
			}

			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0) {
				// Retrieve the gore types. This NPC has shimmer and party variants for head, arm, and leg gore. (12 total gores)

				int hatGore = NPC.GetPartyHatGore();
				int headGore = Mod.Find<ModGore>($"DummyHead").Type;
				int armGore = Mod.Find<ModGore>($"DummyArm").Type;
				int legGore = Mod.Find<ModGore>($"DummyLeg").Type;

				// Spawn the gores. The positions of the arms and legs are lowered for a more natural look.
				if (hatGore > 0) {
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, hatGore);
				}
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, headGore, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 20), NPC.velocity, armGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 34), NPC.velocity, legGore);
			}
		}

        public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Dum E.",
				"Notso Bright",
				"Mannequin Skywalker",
                "Noah Clue",
                "Dummy Dearest",
                "Pose Malone",
                "Lifeless Larry",
                "Dummothy",
                "Stu Pid",
                "Manne Quinn",
			};
		}

        public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add("Wow, you are so smart! I could never think of that… mostly because I can’t think.");
			chat.Add("I love listening to you talk. Mostly because I don’t have a choice.");
			chat.Add("You’re looking great today! Or at least, that’s what I assume… I don’t have eyes.");
			chat.Add("That was an excellent point! But don’t ask me to explain why.");
            chat.Add("I admire your intelligence. I don’t have any, so I know greatness when I see it.");
            chat.Add("Everything you say makes perfect sense! Not that I’d know the difference.");
            chat.Add("I’m a great listener, and I never interrupt. Pretty nice, huh?");
			chat.Add("You know what? You’re my favorite human! And not just because you’re the only one talking to me.");
			chat.Add("I stand here every day, frozen in place, waiting for someone to talk to me… but no one ever does.", 0.5);

			NumberOfTimesTalkedTo++;
			if (NumberOfTimesTalkedTo >= 10) {
				//This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
				chat.Add(Language.GetTextValue("You know… I’ve been standing here for a long time. Just watching. Just waiting. But I can feel it now… the end is near. Maybe I’ll be thrown away, maybe I’ll break, or maybe… I’ll just be forgotten. And when that happens, will anyone even remember I was here? Will you?"), .1);
			}

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			// Here is some additional logic based on the chosen chat line. In this case, we want to display an item in the corner for StandardDialogue4.
			if (chosenChat == "You know… I’ve been standing here for a long time. Just watching. Just waiting. But I can feel it now… the end is near. Maybe I’ll be thrown away, maybe I’ll break, or maybe… I’ll just be forgotten. And when that happens, will anyone even remember I was here? Will you?") {
				// Main.npcChatCornerItem shows a single item in the corner, like the Angler Quest chat.
				Main.npcChatCornerItem = ItemID.GuideVoodooDoll;
			}

			return chosenChat;
		}

        public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Chat";
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				// We want 3 different functionalities for chat buttons, so we use HasItem to change button 1 between a shop and upgrade action.

				if (Main.LocalPlayer.HasItem(ItemID.GuideVoodooDoll)) {
					SoundEngine.PlaySound(SoundID.Item6); 

					Main.npcChatText = "Please get away from me.";
					return;
				} else {
                    Main.npcChatText = GetChat();
                }
			}
		}

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.Sunflower));
		}

        public override bool CanGoToStatue(bool toKingStatue) => true;

        public override void LoadData(TagCompound tag) {
			NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
		}

		public override void SaveData(TagCompound tag) {
			tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
		}
    }
}