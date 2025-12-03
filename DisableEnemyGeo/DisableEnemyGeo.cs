using Modding;
using System;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;

namespace DisableEnemyGeo
{
    public class DisableEnemyGeoMod : Mod
    {
        private static DisableEnemyGeoMod? _instance;

        internal static DisableEnemyGeoMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(DisableEnemyGeoMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public DisableEnemyGeoMod() : base("DisableEnemyGeo")
        {
            _instance = this;
        }

        public override void Initialize()
        {
            Log("Initializing");

            On.PlayMakerFSM.OnEnable += OnFSMEnable;
            On.HealthManager.Die += OnDie;

            Log("Initialized");
        }
        private void OnFSMEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            orig(self);

            if (self.gameObject.name == "Corpse Big Fly Burster(Clone)" && self.FsmName == "burster")
            {
                self.GetFsmAction<FlingObjectsFromGlobalPool>("Geo", 0).Enabled = false;
            }
        }

        //10X all geo drops
        private void OnDie(On.HealthManager.orig_Die orig, HealthManager self, float? attackDirection, AttackTypes attackType, bool ignoreEvasion)
{
    int small = self.geoSmall;
    int medium = self.geoMedium;
    int large = self.geoLarge;

    // Multiply by 10
    self.SetGeoSmall(small * 10);
    self.SetGeoMedium(medium * 10);
    self.SetGeoLarge(large * 10);

    // Run original function
    orig(self, attackDirection, attackType, ignoreEvasion);
}
    }
}
