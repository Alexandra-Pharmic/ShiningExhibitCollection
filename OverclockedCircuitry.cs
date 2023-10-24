﻿using LBoL.ConfigData;
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
    public sealed class OverclockedCircuitryDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(OverclockedCircuitry);
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
                Value1: null,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Philosophy = 2 },
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
        [EntityLogic(typeof(OverclockedCircuitryDef))]
        [UsedImplicitly]
        [ExhibitInfo(WeighterType = typeof(OverclockedCircuitryWeighter))]
        public sealed class OverclockedCircuitry : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {
                base.ReactBattleEvent<StatusEffectApplyEventArgs>(base.Battle.Player.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnStatusEffectAdded));
            }
            private IEnumerable<BattleAction> OnStatusEffectAdded(StatusEffectApplyEventArgs args)
            {
                if (args.Effect is Burst)
                {
                    yield return new GainManaAction(this.Mana);
                }
                yield break;
            }

            private class OverclockedCircuitryWeighter : IExhibitWeighter
            {
                public float WeightFor(Type type, GameRunController gameRun)
                {
                    if (gameRun.Player.HasExhibit<MarisaR>())
                    {
                        return 1f;
                    }
                    return 0f;
                }
            }
        }
    }
}