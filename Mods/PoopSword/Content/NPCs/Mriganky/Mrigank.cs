using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoopSword.Common.Systems;
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
using PoopSword.Content.NPCs.Peter;

namespace PoopSword.Content.NPCs.Mriganky
{
    [AutoloadHead]
    public class Mrigank : ModNPC 
    {
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 25; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs. This is the remaining frames after the walking frames.
			NPCID.Sets.AttackFrameCount[Type] = 4; // The amount of frames in the attacking animation.
			NPCID.Sets.DangerDetectRange[Type] = 300; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 3; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 100; // The denominator for the chance for a Town NPC to attack. Lower numbers make the Town NPC appear more aggressive.
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers() {
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
			};

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<OceanBiome>(AffectionLevel.Like)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) 
				.SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Like) 
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Love)
				.SetNPCAffection(ModContent.NPCType<DummyGuide>(), AffectionLevel.Love);
			;
        }

        public override void SetDefaults() {
			NPC.townNPC = true; 
			NPC.friendly = true; 
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 3;
			NPC.defense = -200;
			NPC.lifeMax = 200;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = -4f;

			AnimationType = NPCID.Guide;
		}


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {

			bestiaryEntry.Info.AddRange([

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("Oh ever so beautiful, my sweet Chomely."),
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
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood);
			}
		}

		public override void OnSpawn(IEntitySource source) {
			if(source is EntitySource_SpawnNPC) {
				// A TownNPC is "unlocked" once it successfully spawns into the world.
				TownNPCRespawnSystem.unlockedExamplePersonSpawn = true;
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs) { // Requirements for the town NPC to spawn.
			if (TownNPCRespawnSystem.unlockedExamplePersonSpawn) {
				return true;
			}

			if(Main.hardMode){
				return true;
			}

			return false;
		}

        public override List<string> SetNPCNameList() {
			return new List<string>() {
				"Mrigank",
                "Mango",
                "Son J.",
                "Mirgank",
                "Mr. Gank",
                "Gank",
			};
		}

        public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add("The secret to good curry? Always more chili! If you do not sweat, you have not truly eaten!");
            chat.Add("I once cooked a dish so spicy, a man cried tears of joy. Or pain. Same thing, really.");
            chat.Add("I once made a curry so hot, a man blacked out for three days. He woke up and thanked me.");
            chat.Add("One time I seasoned this guys food with salt and he ate it without even needing milk.");

            chat.Add("My best friend Toomy is a better Smash player than I am.", 0.3);
            chat.Add("My best friend Toomy always comes before my girlfriend.", 0.3);
            chat.Add("Have you seen the witch nearby? I heard she makes people sleep with pink silk sheets!", 0.2);
            chat.Add("The whicked witch just yelled at me for leaving the toilet seat up.", 0.2);
            chat.Add("Fun Fact: I left poop in the toilet and the people who were inspecting my apartment saw it!", 0.3);
            chat.Add("I would poop in my roommates bathtub and blame it on Yuki.", 0.3);


            chat.Add("Have you seen Yuki? Do you have a bathtub nearby?");
            chat.Add("Have you seen my cat Yuki? You will get used to the smell.");
            chat.Add("Be careful when walking through the front door, Yuki might have made a slippery trap.");
            chat.Add("Yuki? Oh sorry I thought you were my cat! He likes to potty where he shouldn't.");
            chat.Add("Fun Fact: Yuki poops all over himself when he goes in the car!");
            chat.Add("Why does Yuki poop so much? I swear if he poops 1 more time im going to lose it.");


            chat.Add("*You notice he is only wearing Nike*");
            chat.Add("I can get you a discount at the nearest Nike outlet.");
            chat.Add("Do they have wingstop in Poop Arsenal yet?");
            chat.Add("When I'm not working, I am with my girlfriend. She is definetly not holding me hostage!");
            chat.Add("1, 2, 3, 4.. Hey! Sorry I was busy crunching some numbers.");
            chat.Add("Make sure you flush everytime you poop!");
            chat.Add("Did I flush earlier? Ehh.. I'm sure no inspectors will see it.");
            chat.Add("Have you hopped on PoopCoin yet? Its the new big thing.");
            chat.Add("Just put 10 platinum coins in PoopCoin.");
            chat.Add("Invest in PoopCoin!");
            chat.Add("Just checked the wiki and everyone says to invest in PoopCoin.");
            chat.Add("PoopCoin is a better investment than CalamityCoin.");
            chat.Add("*PoopCoin isn't doing to well today..*");
            chat.Add("I hope you like collagen, thats my secret ingredient!");
            chat.Add("CollagenCoin just hit 200 gold, I can sail this to platinum.");
            chat.Add("MangoCoin is at 3 bronze coins, life is suffering.");
            chat.Add("MangoCoin, MrigankCoin, Indiancoin, CurryCoin..");
            chat.Add("I consent to being in this mod! -Mrigank"); // He -in fact- did not consent to being in this mod
            chat.Add("Poop Arsenal changed my life!");
            chat.Add("My dumb girlfriend keeps trying to call me.. I'm busy playing POOP ARSENAL!!");


            int peterID = NPC.FindFirstNPC(ModContent.NPCType<DummyGuide>());
            if (peterID >= 0) // If the Guide exists in the world
            {
                string guideName = Main.npc[peterID].GivenName;
                chat.Add($"I was talking to {guideName} earlier. He has an amazing lego collection!", 0.8);
                chat.Add($"I accidentally gave {guideName} too much curry and he has been pooping for 3 weeks!", 0.8);
                chat.Add($"{guideName} is great, but how can he compare to my best friend Toomy!", 0.7);
                chat.Add($"{guideName}'s cat likes butt slaps.", 0.3);
                chat.Add($"{guideName}'s cat loves being chased. She is fueled by pure terror!", 0.4);
                chat.Add($"{guideName}'s girlfriend hates me because I'm his better Lover.", 0.6);

            }
            else
            {
                chat.Add("Have you seen my handsome friend anywhere? Hes 6'4..hes hard to miss.");
            }


			NumberOfTimesTalkedTo++;
			if (NumberOfTimesTalkedTo >= 20) {
				string playerName = Main.LocalPlayer.name;
				chat.Add($"Hey {playerName}! You should come to this Alvvays concert with me!", 0.2);
			}

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

        public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = "Eat";
			button2 = "Chat";
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				Player player = Main.LocalPlayer;
        
                player.AddBuff(BuffID.WellFed, 60 * 60); // Regeneration for 1 minute (60 seconds * 60 ticks)
				player.AddBuff(BuffID.Swiftness, 180 * 60);
				
				SoundEngine.PlaySound(SoundID.Item2);
                Main.NewText($"*{NPC.GivenName} has blessed you with curry.*", 175, 75, 255);

			} else {
				Main.npcChatText = GetChat();
			}
		}

	

        public override void ModifyNPCLoot(NPCLoot npcLoot) {
			npcLoot.Add(ItemDropRule.Common(ItemID.SpicyPepper));
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