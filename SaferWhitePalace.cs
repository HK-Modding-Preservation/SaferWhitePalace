using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Modding;
using Satchel.BetterMenus;

namespace SaferWhitePalace {
    class SaferWhitePalace: Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings> {
        new public string GetName() => "Safer White Palace";
        public override string GetVersion() => "v1.0.0.0";
        private Menu MenuRef;
        public static GlobalSettings gs = new GlobalSettings();

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects) {
            On.GameManager.OnNextLevelReady += sceneChangeFunc;
        }

        private void sceneChangeFunc(On.GameManager.orig_OnNextLevelReady orig, GameManager self) {
            orig(self);
            if(self.sceneName == null)
                return;
            if(!self.sceneName.StartsWith("White_Palace"))
                return;
            GameObject[] objs = GameObject.FindObjectsOfType<GameObject>();
            foreach(GameObject go in objs) {
                if(go.name == null)
                    continue;
                if(gs.saws) {
                    if(go.name.StartsWith("wp_saw")) {
                        go.SetActive(false);
                    }
                    if(go.name == "Audio Field" && new string[] { "White_Palace_05", "White_Palace_19", "White_Palace_20" }.Contains<string>(self.sceneName)) {
                        go.SetActive(false);
                    }
                }
                if(gs.thorns) {
                    if(go.name.StartsWith("White Thorn Collider")) {
                        go.SetActive(false);
                    }
                }
                if(gs.spikes) {
                    if(go.name.StartsWith("White_ Spikes") || go.name.StartsWith("Cave Spikes Invis") || go.name.StartsWith("Spike Collider") || go.name.StartsWith("wp_trap_spikes")) {
                        go.SetActive(false);
                    }
                }
                if(gs.enemies) {
                    if(go.name.StartsWith("White Palace Fly") || go.name.StartsWith("Battle Scene")) {
                        go.SetActive(false);
                    }
                }
            }
        }

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates) {
            MenuRef ??= new Menu(
                name: "Safer White Palace",
                elements: new Element[] {
                    new HorizontalOption(
                        name: "Remove Saws",
                        description: "Includes saw audio",
                        values: new string[] {"On", "Off" },
                        applySetting: index => {
                            gs.saws = index == 0;
                        },
                        loadSetting: () => gs.saws ? 0 : 1),
                    new HorizontalOption(
                        name: "Remove Thorns",
                        description: "Hitboxes only, does not visually change",
                        values: new string[] {"On", "Off" },
                        applySetting: index => {
                            gs.thorns = index == 0;
                        },
                        loadSetting: () => gs.thorns ? 0 : 1),
                    new HorizontalOption(
                        name: "Remove Spikes",
                        description: "Spikes on some walls, roofs, and the ones that extend",
                        values: new string[] {"On", "Off" },
                        applySetting: index => {
                            gs.spikes = index == 0;
                        },
                        loadSetting: () => gs.spikes ? 0 : 1),
                    new HorizontalOption(
                        name: "Remove Enemies",
                        description: "Wingsmoulds and Kingsmould battle arenas",
                        values: new string[] {"On", "Off" },
                        applySetting: index => {
                            gs.enemies = index == 0;
                        },
                        loadSetting: () => gs.enemies ? 0 : 1),
                }
                );
            return MenuRef.GetMenuScreen(modListMenu);
        }

        public bool ToggleButtonInsideMenu {
            get;
        }

        public void OnLoadGlobal(GlobalSettings s) {
            gs = s;
        }

        public GlobalSettings OnSaveGlobal() {
            return gs;
        }
    }

    public class GlobalSettings {
        public bool enemies, saws, thorns, spikes;
    }
}
