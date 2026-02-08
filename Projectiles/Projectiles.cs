using CalamityMod.Projectiles.Magic;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Typeless;
using Terraria.ModLoader;

namespace EverquartzAdventure
{
    internal static partial class CalamityWeakRef
    {
        [JITWhenModsEnabled("Calamitymod")]
        internal static class ProjectileType
        {
            public static int TelluricGlareArrow => ModContent.ProjectileType<TelluricGlareArrow>();
            public static int FriendlyLaserWallBeam => ModContent.ProjectileType<FriendlyLaserWallBeam>();
            public static int DeathhailBeam => ModContent.ProjectileType<DeathhailBeam>();
            public static int HolyLaser => ModContent.ProjectileType<HolyLaser>();
        }
    }
}