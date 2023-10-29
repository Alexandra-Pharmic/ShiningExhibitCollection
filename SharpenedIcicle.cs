using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using static LBoLEntitySideloader.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Base.Extensions;
using System.Collections;
using LBoL.Presentation;
using LBoL.EntityLib.Cards.Neutral.Blue;
using Mono.Cecil;
using LBoL.Core.StatusEffects;
using System.Linq;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Misfortune;
using static UnityEngine.TouchScreenKeyboard;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Presentation.UI.Panels;
using UnityEngine.InputSystem.Controls;
using LBoL.EntityLib.Exhibits;
using JetBrains.Annotations;
using LBoL.Core.Stations;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.Cards.Character.Marisa;

namespace ShiningExhibitCollection
{
    public sealed class SharpenedIcicleDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SharpenedIcicle);
        }
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(BepinexPlugin.embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.ExhibitsEn.yaml");
            return locFiles;
        }
        public override ExhibitSprites LoadSprite()
        {
            // embedded resource folders are separated by a dot
            var folder = "";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", BepinexPlugin.embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }
        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: 0,
                Id: "",
                Order: 10,
                IsDebug: true,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Shining,
                Value1: 1,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Blue,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(SharpenedIcicleDef))]
        [UsedImplicitly]
        [ExhibitInfo(WeighterType = typeof(SharpenedIcicleWeighter))]
        public sealed class SharpenedIcicle : ShiningExhibit
        {
            public override string OverrideIconName
            {
                get
                {
                    if (base.Counter != 0)
                    {
                        return base.Id + "Inactive";
                    }
                    return base.Id;
                }
            }

            // Token: 0x06000546 RID: 1350 RVA: 0x0000C84E File Offset: 0x0000AA4E
            protected override void OnEnterBattle()
            {
                base.Counter = 0;
                base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarting, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarting));
                base.ReactBattleEvent<StatisticalDamageEventArgs>(base.Battle.Player.StatisticalTotalDamageDealt, new EventSequencedReactor<StatisticalDamageEventArgs>(this.OnStatisticalDamageDealt));
            }

            private IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
            {
                base.Counter = 0;
                yield break;
            }

            // Token: 0x06000547 RID: 1351 RVA: 0x0000C879 File Offset: 0x0000AA79
            private IEnumerable<BattleAction> OnStatisticalDamageDealt(StatisticalDamageEventArgs args)
            {
                if (base.Battle.BattleShouldEnd || base.Counter == 1)
                {
                    yield break;
                }
                bool activated = false;
                foreach (KeyValuePair<Unit, IReadOnlyList<DamageEventArgs>> keyValuePair in args.ArgsTable)
                {
                    Unit unit;
                    IReadOnlyList<DamageEventArgs> readOnlyList;
                    keyValuePair.Deconstruct(out unit, out readOnlyList);
                    Unit unit2 = unit;
                    IReadOnlyList<DamageEventArgs> source = readOnlyList;
                    if (unit2.IsAlive)
                    {
                        if (source.Count(delegate (DamageEventArgs damageAgs)
                        {
                            DamageInfo damageInfo = damageAgs.DamageInfo;
                            return damageInfo.DamageType == DamageType.Attack && damageInfo.Amount > 0f;
                        }) > 0)
                        {
                            if (!activated)
                            {
                                base.NotifyActivating();
                                base.Counter = 1;
                                yield return new ApplyStatusEffectAction<LBoL.EntityLib.StatusEffects.Cirno.Cold>(unit2, 0, 0, 0, 0, 0f, true);
                            }

                        }
                    }
                }
                activated = true;
                IEnumerator<KeyValuePair<Unit, IReadOnlyList<DamageEventArgs>>> enumerator = null;
                yield break;
            }

            private class SharpenedIcicleWeighter : IExhibitWeighter
            {
                public float WeightFor(Type type, GameRunController gameRun)
                {
                    if (gameRun.Player.HasExhibit<CirnoU>() || gameRun.Player.HasExhibit<CirnoG>())
                    {
                        return 1f;
                    }
                    return 0f;
                }
            }
        }
    }
}