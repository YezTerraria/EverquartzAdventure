using EverquartzAdventure.NPCs;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Mono.Cecil.Cil.OpCodes;

namespace EverquartzAdventure.ILEditing
{
    internal static class ILEditingUtils
    {
        public static void LogFailure(string modName, string name, string reason)
        {
            EverquartzAdventureMod.Instance.Logger.Warn($"{modName} IL Edit for {name} failed: {reason}");
        }
    }
    internal static class ILChanges
    {
        internal static void LogFailure(string name, string reason) => ILEditingUtils.LogFailure("Vanilla", name, reason);

        private static void TownNPCCustomDeathMessage(ILContext il)
        {
            //Made this a while ago to fix the edit, disables the "X was slain..." message on a town NPC death if that town npc is Deimos.
            try
            {
                ILCursor c = new ILCursor(il);
                ILLabel l = null;

                if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(NPCID.SkeletonMerchant), i => i.MatchBeq(out l)))
                {
                    LogFailure("TownNPCCustomDeathMessage", "Could not match ldc.i4 453 or beq");
                    return;
                }

                if (l == null)
                {
                    LogFailure("TownNPCCustomDeathMessage", "ILLabel l value not found");
                    return;
                }

                c.Emit(Ldarg_0);
                c.EmitDelegate<Func<NPC, bool>>((n) => n.ModNPC is not EverquartzNPC);
                c.Emit(Brfalse, l);
            }
            catch (Exception e)
            {
                LogFailure("TownNPCCustomDeathMessage", "\n" + e.ToString());
                MonoModHooks.DumpIL(EverquartzAdventureMod.Instance, il);
            }
        }

        internal static void Load()
        {
            //IL.Terraria.Player.PlaceThing_Tiles += PrePlaceThingTilesPatch;
            Terraria.IL_NPC.checkDead += TownNPCCustomDeathMessage;
            //HookEndpointManager.Modify<ILContext.Manipulator>(typeof(NPCLoader).GetMethod("StrikeNPC", BindingFlags.Static | BindingFlags.Public), IAmGoingToHell);
        }

        internal static void Unload()
        {
            //IL.Terraria.Player.PlaceThing_Tiles -= PrePlaceThingTilesPatch;
            Terraria.IL_NPC.checkDead -= TownNPCCustomDeathMessage;
            //HookEndpointManager.Unmodify<ILContext.Manipulator>(typeof(NPCLoader).GetMethod("StrikeNPC", BindingFlags.Static | BindingFlags.Public), IAmGoingToHell);
        }
    }
}