# GameSettingsAdjuster Mod for Dungeon Tycoon

**Version:** 0.1.0
**Author:** Shibiii

---

## 📦 Installation

1. **Install BepInEx**

   * Download BepInEx (x64) for Unity 2023.2 from the [BepInEx releases page](https://github.com/BepInEx/BepInEx/releases).
   * Unzip the contents **into your** `DungeonTycoon` **folder**, i.e. the same directory as `DungeonTycoon.exe`.

     ```
     DungeonTycoon\
     ├─ DungeonTycoon.exe
     ├─ BepInEx\
     ├─ doorstop_config.ini
     ├─ winhttp.dll
     └─ ...
     ```

2. **Copy the DLL**

   * Download the latest `GameSettingsAdjuster.dll` from this Nexus page.
   * Place it into:

     ```
     DungeonTycoon\BepInEx\plugins\
     ```

3. **First Run**

   * Launch Dungeon Tycoon once to generate BepInEx’s config files.
   * You should see in your log (`BepInEx/LogOutput.log`):

     ```
     [Info   : BepInEx] Loading [GameSettingsAdjuster 0.1.0]
     [Info   : GameSettingsAdjuster] Awake() called
     ```

4. **Configure**

   * Open `BepInEx/config/com.Shibi.GameSettingsAdjuster.cfg` with any text editor.
   * The **Research** sections control the multipliers applied *only* when you actually learn that research in-game. If you already have it unlocked at startup, changing these has no effect until you learn it anew.

   ```ini
   [Research.Buffs]

   ## Visitor buff multiplier
   # Setting type: Single
   # Default value: 1
   Buff_Visitors = 1

   ## Hero speed buff multiplier
   # Setting type: Single
   # Default value: 1
   Buff_HeroSpeed = 1

   [Research.Chances]

   ## Extra soul chance multiplier
   # Setting type: Single
   # Default value: 1
   Buff_ExtraSoulChancePercent = 1

   ## Elite spawn chance multiplier
   # Setting type: Single
   # Default value: 1
   Buff_EliteMonsterSummonChancePercent = 1

   ## Lucky spawn chance multiplier
   # Setting type: Single
   # Default value: 1
   Buff_LuckyMonsterSummonChancePercent = 1

   [Research.Limits]

   ## Monster limit multiplier
   # Setting type: Single
   # Default value: 1
   IncreaseMonsterLimit = 1

   ## Hero loot limit multiplier
   # Setting type: Single
   # Default value: 1
   IncreaseHeroLootLimit = 1

   [Research.Points]

   ## Toughness point multiplier
   # Setting type: Single
   # Default value: 1
   ToughnessPoint = 1

   [Research.Stocks]

   ## Trap stocks multiplier
   # Setting type: Single
   # Default value: 1
   IncreaseTrapStocks = 1

   ## Campfire stocks multiplier
   # Setting type: Single
   # Default value: 1
   IncreaseCampfireStocks = 1
   ```

5. **Enjoy!**

   * Restart the game after saving your config edits.
   * Your learned researches will now apply the configured multipliers.

---

> **Tip:** You can also tweak other sections—Spawner timing, Vendor rates, or Enchantment buffs/debuffs—in the same config file. All settings are documented in the generated `.cfg`. Have fun customizing your Dungeon Tycoon experience!
