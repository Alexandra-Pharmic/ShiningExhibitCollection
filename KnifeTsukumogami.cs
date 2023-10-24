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
using System.Reflection.Emit;

namespace ShiningExhibitCollection
{
    public sealed class KnifeTsukumogamiDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(KnifeTsukumogami);
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
                Value1: 4,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: ManaColor.White,
                BaseManaAmount: 1,
                HasCounter: false,
                InitialCounter: null,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { "Knife" }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(KnifeTsukumogamiDef))]
        [UsedImplicitly]
        [ExhibitInfo(WeighterType = typeof(KnifeTsukumogamiWeighter))]
        public sealed class KnifeTsukumogami : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {

                foreach (Card card in Battle.EnumerateAllCards())
                {
                    if (card is Knife)
                    {
                        card.DeltaDamage = this.Value1;
                        card.DeltaValue1 = this.Value1;
                    }
                }
                base.ReactBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToDiscard, new EventSequencedReactor<CardsEventArgs>(this.OnAddCard));
                base.ReactBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToHand, new EventSequencedReactor<CardsEventArgs>(this.OnAddCard));
                base.ReactBattleEvent<CardsEventArgs>(base.Battle.CardsAddedToExile, new EventSequencedReactor<CardsEventArgs>(this.OnAddCard));
                base.ReactBattleEvent<CardsAddingToDrawZoneEventArgs>(base.Battle.CardsAddedToDrawZone, new EventSequencedReactor<CardsAddingToDrawZoneEventArgs>(this.OnCardsAddedToDrawZone));
            }
            private IEnumerable<BattleAction> OnAddCard(CardsEventArgs args)
            {
                NotifyActivating();
                yield return this.ImproveKnives(args.Cards);
                yield break;
            }

            private IEnumerable<BattleAction> OnCardsAddedToDrawZone(CardsAddingToDrawZoneEventArgs args)
            {
                NotifyActivating();
                yield return this.ImproveKnives(args.Cards);
                yield break;
            }

            private BattleAction ImproveKnives(IEnumerable<Card> cards)
            {
                foreach (Card card in cards)
                {
                    if (card is Knife)
                    {
                        card.DeltaDamage = this.Value1;
                        card.DeltaValue1 = this.Value1;
                        card.DecreaseBaseCost(new ManaGroup() { Any = 1 });
                    }
                }
                return null;
            }

            private class KnifeTsukumogamiWeighter : IExhibitWeighter
            {
                public float WeightFor(Type type, GameRunController gameRun)
                {
                    if (gameRun.Player.HasExhibit<SakuyaW>() || gameRun.Player.HasExhibit<SakuyaU>())
                    {
                        return 1f;
                    }
                    return 0f;
                }
            }
        }
    }
}