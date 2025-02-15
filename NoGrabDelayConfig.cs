using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NoGrabDelay
{
    internal class NoGrabDelayConfig
    {
        internal NoGrabDelayConfig(ConfigFile cfg)
        {
            delayBetweenInteraction = cfg.Bind(
                section: "General",
                key: "DelayBetweenInteraction",
                defaultValue: 0.01f,
                description: "Delay between grabbing scrap. Min: 0.01. Max: 0.2. Default in vanilla lethal company is 0.2 seconds."
            );

            cfg.Save();
        }

        public static NoGrabDelayConfig Config { get; internal set; } = null!;

        public static float GetInteractionCooldownDecrease()
        {
            return 0.2f - Mathf.Clamp(Config.delayBetweenInteraction.Value, 0.01f, 0.2f);
        }

        public readonly ConfigEntry<float> delayBetweenInteraction;
    }
}
