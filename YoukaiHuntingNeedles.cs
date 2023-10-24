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
using LBoL.EntityLib.Cards.Other.Enemy;

namespace ShiningExhibitCollection
{
    public sealed class YoukaiHuntingNeedlesDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(YoukaiHuntingNeedles);
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
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.CantLose,
                Rarity: Rarity.Shining,
                Value1: 3,
                Value2: 1,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.Red,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(YoukaiHuntingNeedlesDef))]
        [UsedImplicitly]
        [ExhibitInfo(WeighterType = typeof(YoukaiHuntingNeedlesWeighter))]
        public sealed class YoukaiHuntingNeedles : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {
                base.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
            }


            private IEnumerable<BattleAction> OnPlayerTurnStarted(GameEventArgs args)
            {
                if (base.Battle.Player.TurnCounter == 1)
                {
                    NotifyActivating();
                    EnemyType enemyType = base.Battle.EnemyGroup.EnemyType;
                    if (enemyType == EnemyType.Elite || enemyType == EnemyType.Boss)
                    {
                        yield return new ApplyStatusEffectAction<Firepower>(base.Owner, new int?(base.Value1), null, null, null, 0f, true);
                    }
                    else
                    {
                        yield return new ApplyStatusEffectAction<Firepower>(base.Owner, new int?(base.Value2 * Battle.AllAliveEnemies.Count()), null, null, null, 0f, true);
                    }
                }
                yield break;
            }

            private class YoukaiHuntingNeedlesWeighter : IExhibitWeighter
            {
                public float WeightFor(Type type, GameRunController gameRun)
                {
                    if (gameRun.Player.HasExhibit<ReimuR>() || gameRun.Player.HasExhibit<ReimuW>())
                    {
                        return 1f;
                    }
                    return 0f;
                }
            }
        }
    }
}