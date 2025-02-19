using System;
// using System.Numerics;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using PoopSword.Content.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace PoopSword.Content.Items.Projectiles 
{
    public class myMinion : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // 4 Frames of animation
            Main.projFrames[Projectile.type] = 4; 

            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // Minions are registered as pets- despite being projectiles
            // Pets are also coded through projectiles
            Main.projPet[Projectile.type] = true; 

            // If a new minion is summoned this minion will delete
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

            // Adds minion damage resistance to certain bosses with this enabled
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.scale = 1.5f;

            Projectile.tileCollide = false;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1; // Hits multiple enemies
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (owner.dead || !owner.active){
                owner.ClearBuff(ModContent.BuffType<MyMinionBuff>());
                return;
            }

            if (owner.HasBuff(ModContent.BuffType<MyMinionBuff>())) {
                Projectile.timeLeft = 2;
            }

            AIGeneral(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition);
            AISearchForTarget(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            AIMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition);

            AIUpdateAnimation();
        }

        private void AIGeneral(Player owner, out Vector2 vectorToIdlePostion, out float distanceToIdlePosition) {
            Vector2 idlePosition = owner.Center;
            idlePosition.Y -= 48f;


            float minionPostionOffsetX = (10 + Projectile.minionPos * 40) * -owner.direction;
            idlePosition.X += minionPostionOffsetX;

            vectorToIdlePostion = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePostion.Length();

/*  We use Main.myPlayer so that we can work out the correct position and then
    have this update the network so that it is in the correct position for
    other players without having to check relative to each player */
            if(Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            float overlapVelocity = 0.04f;

            for (int i = 0; i < Main.maxProjectiles; i++) {
                Projectile other = Main.projectile[i];

                if (other.whoAmI != Projectile.whoAmI && 
                other.owner == Projectile.owner && 
                Math.Abs(Projectile.position.X - other.position.X) + Math.Abs(Projectile.position.Y - other.position.Y) < Projectile.width
                )
                {
                    if (Projectile.position.X < other.position.X) 
                    {
                        Projectile.velocity.X -= overlapVelocity;
                    } else
                    {
                        Projectile.velocity.X += overlapVelocity;
                    }

                    if (Projectile.position.Y < other.position.Y) 
                    {
                        Projectile.velocity.Y -= overlapVelocity;
                    } else
                    {
                        Projectile.velocity.Y += overlapVelocity;
                    }
                }
            }
        }

        private void AISearchForTarget(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                if (between < 1200f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
            if (!foundTarget)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if(npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);

                        bool closeThroughWall = between < 100f;

                    // If any of the conditions are met- we set the value for our new target
                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            Projectile.friendly = foundTarget;
        }

        private void AIMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition)
        {

            float speed = 8f;
            float inertia = 20f;

            if(foundTarget)
            {
                if(distanceFromTarget > 40f)
                {
                    Vector2 direction = targetCenter - Projectile.Center;
                    // Normalizing a vector2 will result in X & Y being between -1 and 1 based on cos and sin
                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
                return;
            }
// minion far from player = speed up || minion close to player = slow down
            if(distanceToIdlePosition > 600f)
            {
                speed = 12f;
                inertia = 60f;
            } else {
                speed = 4f;
                inertia = 80f;
            }

            if (distanceToIdlePosition > 20f)
            {
                vectorToIdlePosition.Normalize();
                vectorToIdlePosition *= speed;

                Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
            } else if (Projectile.velocity == Vector2.Zero)
            {
                Projectile.velocity.X = -0.15f;
                Projectile.velocity.Y = -0.05f;
            }
        }

        private void AIUpdateAnimation()
        {
            Projectile.rotation = Projectile.velocity.X * 0.05f;

            int frameSpeed = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            Lighting.AddLight(Projectile.Center, Color.Brown.ToVector3() * 0.65f);
        }
    }

}