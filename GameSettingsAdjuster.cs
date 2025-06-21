using System;
using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

[BepInPlugin("Shibi.GameSettingsAdjuster", "GameSettingsAdjuster", "0.1.0")]
public class GameSettingsAdjuster : BaseUnityPlugin
{

    // --- Research multipliers ---
    public static ConfigEntry<float> mul_IncreaseMonsterLimit;
    public static ConfigEntry<float> mul_IncreaseHeroLootLimit;
    public static ConfigEntry<float> mul_IncreaseTrapStocks;
    public static ConfigEntry<float> mul_IncreaseCampfireStocks;
    public static ConfigEntry<float> mul_BuffVisitors;
    public static ConfigEntry<float> mul_BuffHeroSpeed;
    public static ConfigEntry<float> mul_ToughnessPoint;
    public static ConfigEntry<float> mul_BuffExtraSoulChance;
    public static ConfigEntry<float> mul_BuffEliteChance;
    public static ConfigEntry<float> mul_BuffLuckyChance;

    // --- Vendor multipliers ---
    public static ConfigEntry<float> mul_VendorDailyStock;
    public static ConfigEntry<float> mul_VendorUsedStock;
    public static ConfigEntry<float> mul_VendorPlayerChosenPrice;
    public static ConfigEntry<float> mul_VendorStatusEffectDuration;
    public static ConfigEntry<float> mul_VendorStatusEffectMultiplier;
    public static ConfigEntry<float> mul_VendorStatusEffectEnhancedDuration;
    public static ConfigEntry<float> mul_VendorStatusEffectEnhancedMultiplier;
    public static ConfigEntry<float> mul_VendorInteractTime;

    // --- Enchantment activation amounts ---
    public static ConfigEntry<int> cfg_Act_ReplenishingCharm;
    public static ConfigEntry<int> cfg_Act_HealthAura;
    public static ConfigEntry<int> cfg_Act_EnergizingAura;
    public static ConfigEntry<int> cfg_Act_StrengthBeacon;
    public static ConfigEntry<int> cfg_Act_AttackAura;
    public static ConfigEntry<float> cfg_Act_UnityCharm;
    public static ConfigEntry<int> cfg_Act_SecondWind;

    // --- Enchantment deactivation amounts ---
    public static ConfigEntry<int> cfg_Deact_ReplenishingCharm;
    public static ConfigEntry<int> cfg_Deact_HealthAura;
    public static ConfigEntry<int> cfg_Deact_EnergizingAura;
    public static ConfigEntry<int> cfg_Deact_StrengthBeacon;
    public static ConfigEntry<int> cfg_Deact_AttackAura;
    public static ConfigEntry<float> cfg_Deact_UnityCharm;
    public static ConfigEntry<int> cfg_Deact_SecondWind;

    // --- Config for time to spawn multiplier ---
    public static ConfigEntry<float> cfg_TimeToSpawnMultiplier;

    // --- Reflection for Vendor ---
    private static FieldInfo fiDailyStock;

    private void Awake()
    {
        Logger.LogWarning("[GSA] Awake() called");

        // --- Config for time to spawn multiplier ---
        cfg_TimeToSpawnMultiplier = Config.Bind("Spawner", "TimeToSpawnMultiplier", 1.0f,"Multiplier for the time it takes to spawn monsters");

        // --- Enchantment Activate config ---
        cfg_Act_ReplenishingCharm = Config.Bind("Enchantments.Activate", "ReplenishingCharm", 1, "StockBuff added on activation");
        cfg_Act_HealthAura = Config.Bind("Enchantments.Activate", "HealthAura", 5, "HealthBuff added on activation");
        cfg_Act_EnergizingAura = Config.Bind("Enchantments.Activate", "EnergizingAura", 15, "EnergyBuff added on activation");
        cfg_Act_StrengthBeacon = Config.Bind("Enchantments.Activate", "StrengthBeacon", 5, "TierUpChance added on activation");
        cfg_Act_AttackAura = Config.Bind("Enchantments.Activate", "AttackAura", 15, "AttackBuff added on activation");
        cfg_Act_UnityCharm = Config.Bind("Enchantments.Activate", "UnityCharm", 0.2f, "GroupBuffMultiplier added on activation");
        cfg_Act_SecondWind = Config.Bind("Enchantments.Activate", "SecondWind", 5, "RespawnChance added on activation");

        // --- Enchantment Deactivate config ---
        cfg_Deact_ReplenishingCharm = Config.Bind("Enchantments.Deactivate", "ReplenishingCharm", 1, "StockBuff subtracted on deactivation");
        cfg_Deact_HealthAura = Config.Bind("Enchantments.Deactivate", "HealthAura", 5, "HealthBuff subtracted on deactivation");
        cfg_Deact_EnergizingAura = Config.Bind("Enchantments.Deactivate", "EnergizingAura", 15, "EnergyBuff subtracted on deactivation");
        cfg_Deact_StrengthBeacon = Config.Bind("Enchantments.Deactivate", "StrengthBeacon", 5, "TierUpChance subtracted on deactivation");
        cfg_Deact_AttackAura = Config.Bind("Enchantments.Deactivate", "AttackAura", 15, "AttackBuff subtracted on deactivation");
        cfg_Deact_UnityCharm = Config.Bind("Enchantments.Deactivate", "UnityCharm", 0.2f, "GroupBuffMultiplier subtracted on deactivation");
        cfg_Deact_SecondWind = Config.Bind("Enchantments.Deactivate", "SecondWind", 5, "RespawnChance subtracted on deactivation");

        // --- Research multipliers ---
        mul_IncreaseMonsterLimit = Config.Bind("Research.Limits", "IncreaseMonsterLimit", 1.0f,"Monster limit multiplier");
        mul_IncreaseHeroLootLimit = Config.Bind("Research.Limits", "IncreaseHeroLootLimit", 1.0f,"Hero loot limit multiplier");
        mul_IncreaseTrapStocks = Config.Bind("Research.Stocks", "IncreaseTrapStocks", 1.0f,"Trap stocks multiplier");
        mul_IncreaseCampfireStocks = Config.Bind("Research.Stocks", "IncreaseCampfireStocks", 1.0f,"Campfire stocks multiplier");
        mul_BuffVisitors = Config.Bind("Research.Buffs", "Buff_Visitors", 1.0f,"Visitor buff multiplier");
        mul_BuffHeroSpeed = Config.Bind("Research.Buffs", "Buff_HeroSpeed", 1.0f,"Hero speed buff multiplier");
        mul_ToughnessPoint = Config.Bind("Research.Points", "ToughnessPoint", 1.0f,"Toughness point multiplier");
        mul_BuffExtraSoulChance = Config.Bind("Research.Chances", "Buff_ExtraSoulChancePercent", 1.0f,"Extra soul chance multiplier");
        mul_BuffEliteChance = Config.Bind("Research.Chances", "Buff_EliteMonsterSummonChancePercent", 1.0f,"Elite spawn chance multiplier");
        mul_BuffLuckyChance = Config.Bind("Research.Chances", "Buff_LuckyMonsterSummonChancePercent", 1.0f,"Lucky spawn chance multiplier");

        // --- Vendor multipliers ---
        mul_VendorDailyStock = Config.Bind("Vendor", "DailyStockMultiplier", 1.0f,"Vendor daily stock multiplier");
        mul_VendorUsedStock = Config.Bind("Vendor", "UsedStockMultiplier", 1.0f,"Vendor used stock multiplier");
        mul_VendorPlayerChosenPrice = Config.Bind("Vendor", "PlayerChosenPriceMultiplier", 1.0f,"Vendor price multiplier");
        mul_VendorStatusEffectDuration = Config.Bind("Vendor", "StatusEffectDurationMultiplier", 1.0f,"Status effect duration multiplier");
        mul_VendorStatusEffectMultiplier = Config.Bind("Vendor", "StatusEffectMultiplier", 1.0f,"Status effect strength multiplier");
        mul_VendorStatusEffectEnhancedDuration = Config.Bind("Vendor", "StatusEffectEnhancedDurationMultiplier", 1.0f,"Enhanced status effect duration multiplier");
        mul_VendorStatusEffectEnhancedMultiplier = Config.Bind("Vendor", "StatusEffectEnhancedMultiplier", 1.0f,"Enhanced status effect strength multiplier");

        // --- Reflection for Vendor ---
        var vendorType = typeof(Vendor);
        fiDailyStock = vendorType.GetField("_dailyStock", BindingFlags.Instance | BindingFlags.NonPublic);
        if (fiDailyStock == null)
            Logger.LogError("[GSA] Could not find Vendor private field '_dailyStock'");

        // --- Apply Harmony patches ---
        Harmony harmony = new Harmony("Shibi.GameSettingsAdjuster");
        harmony.PatchAll();
        
        // log all config values

        // --- Launch coroutines ---
        StartCoroutine(ModifyResearchNodesNextFrame());
        StartCoroutine(ModifyVendorsNextFrame());
    }

    private IEnumerator ModifyResearchNodesNextFrame()
    {
        yield return null;
        yield return null;

        var all = Resources.FindObjectsOfTypeAll<ResearchNode>();
        Logger.LogInfo($"[GSA] Found {all.Length} research nodes");
        foreach (var node in all)
        {
            node.increaseMonsterLimit = Mathf.RoundToInt(node.increaseMonsterLimit * mul_IncreaseMonsterLimit.Value);
            node.increaseHeroLootLimit = Mathf.RoundToInt(node.increaseHeroLootLimit * mul_IncreaseHeroLootLimit.Value);
            node.increaseTrapStocks = Mathf.RoundToInt(node.increaseTrapStocks * mul_IncreaseTrapStocks.Value);
            node.increaseCampfireStocks = Mathf.RoundToInt(node.increaseCampfireStocks * mul_IncreaseCampfireStocks.Value);
            node.buff_Visitors = Mathf.RoundToInt(node.buff_Visitors * mul_BuffVisitors.Value);
            node.buff_hero_speed = Mathf.RoundToInt(node.buff_hero_speed * mul_BuffHeroSpeed.Value);
            node.toughnessPoint = Mathf.RoundToInt(node.toughnessPoint * mul_ToughnessPoint.Value);
            node.buff_extraSoulChancePercent = Mathf.RoundToInt(node.buff_extraSoulChancePercent * mul_BuffExtraSoulChance.Value);
            node.buff_eliteMonsterSummonChancePercent = Mathf.RoundToInt(node.buff_eliteMonsterSummonChancePercent * mul_BuffEliteChance.Value);
            node.buff_luckyMonsterSummonChancePercent = Mathf.RoundToInt(node.buff_luckyMonsterSummonChancePercent * mul_BuffLuckyChance.Value);
        }
    }

    private IEnumerator ModifyVendorsNextFrame()
    {
        yield return null;
        yield return null;

        var all = Resources.FindObjectsOfTypeAll<Vendor>();
        Logger.LogInfo($"[GSA] Found {all.Length} vendors");
        foreach (var v in all)
        {
            if (fiDailyStock != null)
            {
                var orig = (int)fiDailyStock.GetValue(v);
                fiDailyStock.SetValue(v, Mathf.RoundToInt(orig * mul_VendorDailyStock.Value));
            }
            v.usedStock = Mathf.RoundToInt(v.usedStock * mul_VendorUsedStock.Value);
            v.playerChosenPrice = Mathf.RoundToInt(v.playerChosenPrice * mul_VendorPlayerChosenPrice.Value);
            v.statusEffectDuration *= mul_VendorStatusEffectDuration.Value;
            v.statusEffectMultiplier *= mul_VendorStatusEffectMultiplier.Value;
            v.statusEffectEnhancedDuration *= mul_VendorStatusEffectEnhancedDuration.Value;
            v.statusEffectEnhancedMultiplier *= mul_VendorStatusEffectEnhancedMultiplier.Value;
        }
    }
    // --- Patch für erfolgreiche Aktivierung (private bool TryActivateEnchantment(Enchantment)) ---
    [HarmonyPatch(typeof(DungeonEnchantments), "TryActivateEnchantment", new Type[] { typeof(Enchantment) })]
    static class Patch_ActivateEnchantment
    {
        // __result = Rückgabewert, enchantment = Parameter
        static void Postfix(bool __result, Enchantment enchantment)
        {
            if (!__result) return;

            Debug.Log($"[GSA] Activated {enchantment.type}, adjusting by config…");

            switch (enchantment.type)
            {
                case EnchantmentType.ReplenishingCharm:
                    Game.Instance.stockBuff += cfg_Act_ReplenishingCharm.Value - 1;
                    break;
                case EnchantmentType.HealthAura:
                    Game.Instance.heroHealthAmountBuff += cfg_Act_HealthAura.Value - 5;
                    break;
                case EnchantmentType.EnergizingAura:
                    Game.Instance.heroEnergyBuff += cfg_Act_EnergizingAura.Value - 15;
                    break;
                case EnchantmentType.StrengthBeacon:
                    Game.Instance.instaTierUpChance += cfg_Act_StrengthBeacon.Value - 5;
                    break;
                case EnchantmentType.AttackAura:
                    Game.Instance.heroAttackBuff += cfg_Act_AttackAura.Value - 15;
                    break;
                case EnchantmentType.UnityCharm:
                    Game.Instance.groupAttackBuffMultiplier += cfg_Act_UnityCharm.Value - 0.2f;
                    Game.Instance.groupHealthBuffMultiplier += cfg_Act_UnityCharm.Value - 0.2f;
                    break;
                case EnchantmentType.SecondWind:
                    Game.Instance.instaRespawnChance += cfg_Act_SecondWind.Value - 5;
                    break;
                case EnchantmentType.SoulGem:
                    // nichts ändern
                    break;
            }

            Debug.Log($"[GSA] New buffs -> stockBuff: {Game.Instance.stockBuff}, heroHealth: {Game.Instance.heroHealthAmountBuff}, …");
        }
    }

    // --- Patch für Deaktivierung (private void DeactivateEnchantment(Enchantment, bool)) ---
    [HarmonyPatch(typeof(DungeonEnchantments), "DeactivateEnchantment", new Type[] { typeof(Enchantment), typeof(bool) })]
    static class Patch_DeactivateEnchantment
    {
        static void Prefix(Enchantment enchantment)
        {
            Debug.Log($"[GSA] Deactivating {enchantment.type}, adjusting by config…");

            switch (enchantment.type)
            {
                case EnchantmentType.ReplenishingCharm:
                    Game.Instance.stockBuff -= cfg_Deact_ReplenishingCharm.Value;
                    break;
                case EnchantmentType.HealthAura:
                    Game.Instance.heroHealthAmountBuff -= cfg_Deact_HealthAura.Value;
                    break;
                case EnchantmentType.EnergizingAura:
                    Game.Instance.heroEnergyBuff -= cfg_Deact_EnergizingAura.Value;
                    break;
                case EnchantmentType.StrengthBeacon:
                    Game.Instance.instaTierUpChance -= cfg_Deact_StrengthBeacon.Value;
                    break;
                case EnchantmentType.AttackAura:
                    Game.Instance.heroAttackBuff -= cfg_Deact_AttackAura.Value;
                    break;
                case EnchantmentType.UnityCharm:
                    Game.Instance.groupAttackBuffMultiplier -= cfg_Deact_UnityCharm.Value;
                    Game.Instance.groupHealthBuffMultiplier -= cfg_Deact_UnityCharm.Value;
                    break;
                case EnchantmentType.SecondWind:
                    Game.Instance.instaRespawnChance -= cfg_Deact_SecondWind.Value;
                    break;
                case EnchantmentType.SoulGem:
                    // nichts ändern
                    break;
            }

            Debug.Log($"[GSA] New buffs -> stockBuff: {Game.Instance.stockBuff}, heroHealth: {Game.Instance.heroHealthAmountBuff}, …");
        }
    }

    // patch for monster spawn time
    [HarmonyPatch(typeof(Spawner), "Start")]
    static class Patch_Spawner_Start
    {
        static void Prefix(Spawner __instance)
          => __instance.timeToSpawn *= cfg_TimeToSpawnMultiplier.Value;
    }
}