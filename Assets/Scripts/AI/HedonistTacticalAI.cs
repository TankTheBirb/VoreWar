using OdinSerializer;
using System;
using System.Collections.Generic;
using System.Linq;

public class HedonistTacticalAI : TacticalAI
{
    public HedonistTacticalAI(List<Actor_Unit> actors, TacticalTileType[,] tiles, int AISide, bool defendingVillage = false) : base(actors, tiles, AISide, defendingVillage)
    {
    }

    protected override void GetNewOrder(Actor_Unit actor)
    {
        foundPath = false;
        didAction = false; // Very important fix: surrounded retreaters sometimes just skipped doing attacks because this was never set to false in or before "fightwithoutmoving"

        path = null;
        if (retreating && actor.Unit.Type != UnitType.Summon && actor.Unit.Type != UnitType.SpecialMercenary && actor.Unit.HasTrait(Traits.Fearless) == false && actor.Unit.GetStatusEffect(StatusEffectType.Charmed) == null && TacticalUtilities.GetPreferredSide(actor, AISide, enemySide) == AISide)
        {
            int retreatY;
            if (State.GameManager.TacticalMode.IsDefender(actor) == false)
                retreatY = Config.TacticalSizeY - 1;
            else
                retreatY = 0;
            if (actor.Position.y == retreatY)
            {
                State.GameManager.TacticalMode.AttemptRetreat(actor, true);
                FightWithoutMoving(actor);
                actor.Movement = 0;
                return;
            }
            WalkToYBand(actor, retreatY);
            if (path == null || path.Path.Count == 0)
            {
                FightWithoutMoving(actor);
                actor.Movement = 0;
            }

            return;
        }

        //do action

        if (Config.KuroTenkoEnabled && actor.PredatorComponent != null)
        {
            if (actor.PredatorComponent.CanFeed())
            {
                RunFeed(actor, "breast", true);
                if (foundPath || didAction) return;
            }
            if (actor.PredatorComponent.CanFeedCum())
            {
                RunFeed(actor, "cock", true);
                if (foundPath || didAction) return;
            }
        }

        int spareMp = CheckActionEconomyOfActorFromPositionWithAP(actor, actor.Position, actor.Movement);
        int thirdMovement = actor.MaxMovement() / 3;
        if (spareMp >= thirdMovement)
        {
            RunBellyRub(actor, spareMp);
            if (path != null)
                return;
            if (didAction) return;
        }


        if (actor.Unit.HasTrait(Traits.Pounce) && actor.Movement >= 2)
        {
            RunVorePounce(actor);
            if (path != null)
                return;
            if (didAction) return;

        }

        RunPred(actor);
        if (didAction || foundPath)
            return;

        TryResurrect(actor);

        if (State.Rand.Next(2) == 0 || actor.Unit.HasWeapon == false)
            RunSpells(actor);
        if (path != null)
            return;
        if (actor.Unit.HasTrait(Traits.Pounce) && actor.Movement >= 2)
        {
            if (IsRanged(actor) == false)
            {
                RunMeleePounce(actor);
                if (didAction) return;
            }
        }
        if (foundPath || didAction) return;
        if (IsRanged(actor))
            RunRanged(actor);
        else
            RunMelee(actor);
        if (foundPath || didAction) return;

        if (Config.KuroTenkoEnabled && actor.PredatorComponent != null)
        {
            if ((Config.FeedingType == FeedingType.Both || Config.FeedingType == FeedingType.BreastfeedOnly) && actor.PredatorComponent.CanFeed())
            {
                RunFeed(actor, "breast");
                if (foundPath || didAction) return;
            }
            if ((Config.FeedingType == FeedingType.Both || Config.FeedingType == FeedingType.CumFeedOnly) && actor.PredatorComponent.CanFeedCum())
            {
                RunFeed(actor, "cock");
                if (foundPath || didAction) return;
            }
            RunSuckle(actor);
            if (foundPath || didAction) return;
        }

        RunBellyRub(actor, actor.Movement);
        if (foundPath || didAction) return;
        //Search for surrendered targets outside of vore range
        //If no path to any targets, will sit out its turn
        RunPred(actor, true);
        if (foundPath || didAction) return;
        actor.ClearMovement();
    }
}
