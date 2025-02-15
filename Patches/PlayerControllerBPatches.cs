using GameNetcodeStuff;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NoGrabDelay.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerControllerBPatches
    {
        [HarmonyPatch(nameof(PlayerControllerB.GrabObject), MethodType.Enumerator)]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> GrabObjectTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var delayBetweenInteraction = typeof(NoGrabDelayConfig).GetMethod(nameof(NoGrabDelayConfig.GetInteractionCooldownDecrease), BindingFlags.Static | BindingFlags.Public);

            var instr = new List<CodeInstruction>(instructions);

            for (var i = 0; i < instr.Count; i++)
            {
                var instruction = instr[i];

                yield return instruction;

                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 0.1f
                    && instr.Count > i + 1 && instr[i + 1].opcode == OpCodes.Newobj)
                {
                    yield return new CodeInstruction(OpCodes.Call, delayBetweenInteraction);
                    yield return new CodeInstruction(OpCodes.Ldc_R4, 2f);
                    yield return new CodeInstruction(OpCodes.Div);
                    yield return new CodeInstruction(OpCodes.Sub);
                }

                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 0.2f
                    && instr.Count > i + 1 && instr[i + 1].opcode == OpCodes.Sub)
                {
                    yield return new CodeInstruction(OpCodes.Sub);
                    yield return new CodeInstruction(OpCodes.Call, delayBetweenInteraction);
                }
            }
        }
    }
}
