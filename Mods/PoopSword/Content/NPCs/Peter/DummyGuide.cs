using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PoopSword.Common.Systems;
using PoopSword.Content.NPCs.Mriganky;
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

namespace PoopSword.Content.NPCs.Peter
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
			NPCID.Sets.DangerDetectRange[Type] = 300; // The amount of pixels away from the center of the NPC that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0; // The type of attack the Town NPC performs. 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
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
				.SetNPCAffection(NPCID.Clothier, AffectionLevel.Love) 
				.SetNPCAffection(NPCID.Pirate, AffectionLevel.Dislike) 
				.SetNPCAffection(NPCID.ArmsDealer, AffectionLevel.Hate)
				.SetNPCAffection(ModContent.NPCType<Mrigank>(), AffectionLevel.Love);
			;
        }

        public override void SetDefaults() {
			NPC.townNPC = true; 
			NPC.friendly = true; 
			NPC.width = 18;
			NPC.height = 40;
			NPC.aiStyle = 7;
			NPC.damage = 3;
			NPC.defense = -100;
			NPC.lifeMax = 100;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = -20f;

			AnimationType = NPCID.Guide;
		}


        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {

			bestiaryEntry.Info.AddRange([

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("This guy really knows fashion!"),
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
				"Peter",
				"Detsch",
				"Rock Cossy",
				"Cleo's Master",
				"Toomy's Good Boy",
				"Guy from Room A",
				"Peter (6'4)"
			};
		}

        public override string GetChat() {
			WeightedRandom<string> chat = new WeightedRandom<string>();

			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add("Oh, this old thing? It’s vintage… from last Tuesday.");
			chat.Add("Fashion is pain. These shoes are actively trying to kill me.");
			chat.Add("I don’t follow trends. I set them… mostly by accident.");
			chat.Add("Oh, this outfit? Pfft. I threw it on. No thought. Completely accidental perfection, as usual.");
            chat.Add("Oh, you fight in your clothes? That’s cute. I merely exist in mine — and the world thanks me for it.");
            chat.Add("My best friend Toomy has a WAY better fashion sense than me!", 0.4);
            chat.Add("Oh, this jacket? Found it in a dusty old trunk in a shop no one’s heard of. Probably cursed. But look at this stitching—totally worth it.");
			chat.Add("I didn't even need the goblin tinkerer to reforge this fit, its already legendary.");
			chat.Add("Have you seen Cleo? I recommend looking under parked cars if you want to find her.", 0.4);
			chat.Add("You know, I found this amazing outfit on Instagram this morning. I must've watched like 15 reels just to figure out how to get it. It's totally worth it, though.");
			chat.Add("I could probably recite the latest fashion trends by heart just from my hours of YouTube shorts. If I see one more 'how to layer your jackets' video, I might actually start styling my whole wardrobe from it.");
			chat.Add("If you ever need a new outfit idea, just hit me up. I’ve seen every fashion reel under the sun. Just don’t ask me to make anything. I’m way too busy scrolling.");
			chat.Add("I just watched this awesome car crash YouTube short!");
			chat.Add("My dog actually likes when people fart on him!", 0.3);
			chat.Add("My mom said Toomy is a great guy!", 0.3);
			chat.Add("When I'm not building legos, I like toppling governments.");
			chat.Add("Come play legos with me! Just kidding! I know you're just an NPC.");
			chat.Add("I love eating legos!");
			chat.Add("When I'm not playing with my legos, I am probably eating them.");
			chat.Add("I cant decide what tastes better.. Legos? or Play-Do.");
			chat.Add("Just met this kid named finger the other day.");
			chat.Add("My girlfriend keeps me away from doing fun stuff with my friends!");
			chat.Add("My name is Peter and I love the Poop Arsenal mod!");
			chat.Add("One thing this mod needs more of.. POOP!");
			chat.Add("Please! Please! Please! Sell me poop!!");
			chat.Add("Red legos taste better than blue ones. And green tastes fruiter than yellow.");
			chat.Add("Humdy dumby dee. Im as gay as can be!", 0.1);
			chat.Add("I love spending time with my girlfriend. NOT!!");
			chat.Add("Send me pictures of your poop and be the next person in the poop arsenal mod!");
			chat.Add("Pooooooop.");
			chat.Add("POOOP!");
			chat.Add("If I'm not playing legos or terraria, I'm playing with my poop.");
			chat.Add("I consent to being an NPC in the Poop Arsenal Mod. -Peter");
			chat.Add("I consent to have my poop stolen -Peter");
			chat.Add("I hate my girlfriend and love Toomy -Peter", 0.4);
			chat.Add("Cleooooooo. CLEOOO!! Have you seen Cleo?");
			chat.Add("I like to roll in mud!", 0.4);
			chat.Add("Finish Poop Milkshake.. Check!");
			chat.Add("One time I ate a whole hotdog in 1 bite!");
			chat.Add("Ask me about my colonoscopy!");
			chat.Add("*farts*");
			chat.Add("Oh thats not poop I just sat in mud.");
			chat.Add("I think Toomy got carried away making some of these chat messages.");
			chat.Add("50% Man, 30% Fashion Guru, 10% Master builder, 20% Genius.");
			chat.Add("I have a hidden piece of code that can blow me up periodically!");
			chat.Add("*Ticking sound*");

			int guideID = NPC.FindFirstNPC(ModContent.NPCType<Mrigank>());
            if (guideID >= 0) // If the Guide exists in the world
            {
                string guideName = Main.npc[guideID].GivenName;
                chat.Add($"I was talking to {guideName} earlier. He has an amazing callagen collection!", 0.7);
                chat.Add($"I accidentally gave {guideName}'s cat laxatives. He blew up.", 0.4);
                chat.Add($"{guideName} is great, but how can he compare to my best friend Toomy!");
                chat.Add($"{guideName}'s trapped at his girlfriend's house.", 0.6);
                chat.Add($"Don't tell {guideName} I told you this.. He sleeps with pink silk sheets.", 0.8);
                chat.Add($"{guideName}'s girlfriend hates me because I'm his better Lover.", 0.3);

            }



			NumberOfTimesTalkedTo++;
			if (NumberOfTimesTalkedTo >= 20) {
				//This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
				chat.Add(Language.GetTextValue("Don't ask me about my ex, but if you have to.. Shes a total B#$%@!"), 0.1);
			}

			string chosenChat = chat; // chat is implicitly cast to a string. This is where the random choice is made.

			return chosenChat;
		}

        public override void SetChatButtons(ref string button, ref string button2) { // What the chat buttons are when you open up the chat UI
			button = Language.GetTextValue("LegacyInterface.28"); // Shop
			button2 = "Chat";
		}

        public override void OnChatButtonClicked(bool firstButton, ref string shop) {
			if (firstButton) {
				shop = "MyShop";

			} else {
				Main.npcChatText = GetChat();
			}
		}

		public override void AddShops()
    	{
        	var shop = new NPCShop(Type, "MyShop");

			int rnd = Main.rand.Next();
        	int randomSet = (int)(rnd % 23);

			if(NumberOfTimesTalkedTo >= 30){
                shop.Add(ItemID.BadgersHat);
                shop.Add(ItemID.Fedora);
                shop.Add(ItemID.GangstaHat);
            }

        	// --- Daily Outfits ---
        	switch (randomSet)
    		{
        case 0:
            shop.Add(ItemID.TopHat);
            shop.Add(ItemID.TuxedoShirt);
            shop.Add(ItemID.TuxedoPants);
            break;

        case 1:
            shop.Add(ItemID.GypsyRobe);
            shop.Add(ItemID.MagicHat);
            shop.Add(ItemID.WizardHat);
            break;

        case 2:
            shop.Add(ItemID.PartyHat);
            shop.Add(ItemID.ClothierJacket);
            shop.Add(ItemID.ClothierPants);
            break;

        case 3:
            shop.Add(ItemID.SantaHat);
            shop.Add(ItemID.SantaShirt);
            shop.Add(ItemID.SantaPants);
            break;

        case 4:
            shop.Add(ItemID.MummyMask);
            shop.Add(ItemID.MummyShirt);
            shop.Add(ItemID.MummyPants);
            break;

        case 5:
            shop.Add(ItemID.NinjaHood);
            shop.Add(ItemID.NinjaShirt);
            shop.Add(ItemID.NinjaPants);
            break;

        case 6:
            shop.Add(ItemID.CreeperMask);
            shop.Add(ItemID.CreeperShirt);
            shop.Add(ItemID.CreeperPants);
            break;

        case 7:
            shop.Add(ItemID.CatMask);
            shop.Add(ItemID.CatShirt);
			shop.Add(ItemID.CatPants);
            break;

        case 8:
            shop.Add(ItemID.AviatorSunglasses);
            shop.Add(ItemID.BallaHat);
            shop.Add(ItemID.BunnyHood);
            break;

        case 9:
            shop.Add(ItemID.SteampunkGoggles);
            shop.Add(ItemID.SteampunkShirt);
            shop.Add(ItemID.SteampunkPants);
            break;

        case 10:
            shop.Add(ItemID.DevilHorns);
            shop.Add(ItemID.EyePatch);
            shop.Add(ItemID.Eyebrella);
            break;

        case 11:
			shop.Add(ItemID.FuneralHat);
			shop.Add(ItemID.FuneralCoat);
			shop.Add(ItemID.FuneralPants);
            break;

        case 12:
            shop.Add(ItemID.GiantBow);
            shop.Add(ItemID.HeartHairpin);
            break;

        case 13:
            shop.Add(ItemID.BadgersHat);
            shop.Add(ItemID.Fedora);
            shop.Add(ItemID.GangstaHat);
            break;

        case 14:
            shop.Add(ItemID.HiTekSunglasses);
            shop.Add(ItemID.SnowHat);
            break;

        case 15:
            shop.Add(ItemID.WolfMask);
            shop.Add(ItemID.WolfShirt);
            shop.Add(ItemID.WolfPants);
            break;

        case 16:
            shop.Add(ItemID.RobotHat);
            shop.Add(ItemID.RobotShirt);
            shop.Add(ItemID.RobotPants);
            break;

        case 17:
			shop.Add(ItemID.HerosHat);
			shop.Add(ItemID.HerosShirt);
			shop.Add(ItemID.HerosPants);
			shop.Add(ItemID.HeroShield);
            break;

        case 18:
            shop.Add(ItemID.VampireMask);
            shop.Add(ItemID.VampireShirt);
            shop.Add(ItemID.VampirePants);
            break;

        case 19:
            shop.Add(ItemID.PixieShirt);
            shop.Add(ItemID.PixiePants);
            break;

		case 20:
			shop.Add(ItemID.ParkaHood);
			shop.Add(ItemID.ParkaCoat);
			shop.Add(ItemID.ParkaPants);
			break;

		case 21:
			shop.Add(ItemID.CowboyHat);
			shop.Add(ItemID.CowboyJacket);
			shop.Add(ItemID.CowboyPants);
			break;

		case 22:
			shop.Add(ItemID.FishCostumeMask);
			shop.Add(ItemID.FishCostumeShirt);
			shop.Add(ItemID.FishCostumeFinskirt);
			break;
		}
        shop.Register();
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