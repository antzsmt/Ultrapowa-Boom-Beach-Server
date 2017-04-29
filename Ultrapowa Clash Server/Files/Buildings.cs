using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCS.Files
{
    internal class Buildings : Data
    {
        public Buildings(Row Row, DataTable DataTable) : base(Row, DataTable)
        {
            Load(Row);
        }

        public string Name { get; set; }
        public int[] DefenseValue { get; set; }
        public string BuildingClass { get; set; }
        public int[] BuildTimeD { get; set; }
        public int[] BuildTimeH { get; set; }
        public int[] BuildTimeM { get; set; }
        public int[] BuildTimeS { get; set; }
        public string[] BuildCost { get; set; }
        public int[] TownHallLevel { get; set; }
        public string[] MaxStoredResource { get; set; }
        public int UnlockProtoAtLevel { get; set; }
        public string PrototypeCost { get; set; }
        public int[] HousingSpace { get; set; }
        public string ProducesResource { get; set; }
        public int[] ResourcePerHour { get; set; }
        public int[] ResourceMax { get; set; }
        public int[] UnitProduction { get; set; }
        public bool UpgradesUnits { get; set; }
        public int[] Hitpoints { get; set; }
        public int[] RegenTime { get; set; }
        public int AttackRange { get; set; }
        public int AttackRate { get; set; }
        public int[] Damage { get; set; }
        public int MinAttackRange { get; set; }
        public int DamageRadius { get; set; }
        public int AttackSpread { get; set; }
        public int BoostTimeMs { get; set; }
        public int SpeedBoost { get; set; }
        public int ArmorBoost { get; set; }
        public int DamageBoost { get; set; }
        public int CombatSetupTimeMs { get; set; }
        public int CombatTeardownTimeMs { get; set; }
        public int CombatTeardownDelayMs { get; set; }
        public bool ProtectedOutsideCombat { get; set; }
        public string ExportNameSetup { get; set; }
        public string SetupEffect { get; set; }
        public int TurretAngleSetup { get; set; }
        public string ExportNameTeardown { get; set; }
        public string TeardownEffect { get; set; }
        public int TurretAngleTeardown { get; set; }
        public string ExportNameProtected { get; set; }
        public int AmplifierRange { get; set; }
        public int AmplifierDamage { get; set; }
        public int HQShieldPercentage { get; set; }
        public int LaserDistance { get; set; }
        public bool CreatesArtifacts { get; set; }
        public int ArtifactType { get; set; }
        public int[] ArtifactCapacity { get; set; }
        public int[] PrototypeCapacity { get; set; }
        public int[] ArtifactStorageCapacity { get; set; }
        public int[] DeepseaDepth { get; set; }
        public int[] StartingEnergy { get; set; }
        public int[] EnergyGain { get; set; }
        public bool CanNotMove { get; set; }
        public int[] ExplorableRegions { get; set; }
        public int ReloadTime { get; set; }
        public int ShotsBeforeReload { get; set; }
        public int[] ResourceProtectionPercent { get; set; }
        public int[] DamageOverFiveSeconds { get; set; }
        public int[] XpGain { get; set; }
        public int[] StunTimeMS { get; set; }
        public string[] TID { get; set; }
        public string[] InfoTID { get; set; }
        public string[] SubtitleTID { get; set; }
        public string[] SWF { get; set; }
        public string[] ExportName { get; set; }
        public string[] ExportNameTop { get; set; }
        public string[] ExportNameNpc { get; set; }
        public string[] ExportNameConstruction { get; set; }
        public string[] BarrelType { get; set; }
        public string ModelName { get; set; }
        public string[] TextureName { get; set; }
        public string[] EnemyTextureName { get; set; }
        public string[] MeshName { get; set; }
        public string[] ShadowModelName { get; set; }
        public string[] ShadowMeshName { get; set; }
        public string[] ShadowTextureName { get; set; }
        public int[] Width { get; set; }
        public int[] Height { get; set; }
        public bool Passable { get; set; }
        public string[] ExportNameBuildAnim { get; set; }
        public string[] DestroyEffect { get; set; }
        public string[] ShatterEffect { get; set; }
        public string[] AttackEffect { get; set; }
        public string[] AttackEffect2 { get; set; }
        public string AttackEffect3 { get; set; }
        public int[] TurretX { get; set; }
        public string[] HitEffect { get; set; }
        public string HitEffect2 { get; set; }
        public string[] Projectile { get; set; }
        public string[] ExportNameDamaged { get; set; }
        public string[] ExportNameBase { get; set; }
        public string[] ExportNameBaseIce { get; set; }
        public string[] ExportNameBaseFire { get; set; }
        public string[] ExportNameBaseCoop { get; set; }
        public string[] ExportNameBaseCrab { get; set; }
        public string[] PickUpEffect { get; set; }
        public string[] PlacingEffect { get; set; }
        public string DefenderCharacter { get; set; }
        public int[] DefenderCount { get; set; }
        public int DefenderZ { get; set; }
        public string MissEffect { get; set; }
        public int[] VillagerProbability { get; set; }
        public string[] BuildingReadyEffect { get; set; }
        public int[] ProjSkewing { get; set; }
        public int[] ProjYScaling { get; set; }
        public int[] ProjXOffset { get; set; }
        public bool RandomFrame { get; set; }
        public string PrisonerCharacter { get; set; }
        public int PrisonerCount { get; set; }
        public int PrisonerLevel { get; set; }
        public string ExportNameDestruct { get; set; }
        public int DestructDelayMs { get; set; }
        public int DestructRadius { get; set; }
        public int DestructDamage { get; set; }
        public int DestructSpeedBoost { get; set; }
        public int DestructDamageBoost { get; set; }
        public int DestructBoostDurationMs { get; set; }
        public string DestructEffect { get; set; }
        public string SpawnCharacter { get; set; }
        public bool Invisible { get; set; }
        public int[] TroopPresets { get; set; }
        public int AntiAirAttacks { get; set; }
        public int TauntRange { get; set; }
        public int Push { get; set; }
        public int PanicDurationMs { get; set; }
        public bool AttackMovingOnly { get; set; }
    }
}
