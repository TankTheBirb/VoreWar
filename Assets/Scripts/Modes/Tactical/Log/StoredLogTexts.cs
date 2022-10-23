﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TacticalMessageLog;
using static LogUtilities;

static class StoredLogTexts
{
    static StoredLogTexts()
    {
        InitializeLists();
    }

    internal class EventString
    {
        internal Race ActorRace;
        internal Race TargetRace;
        internal Func<EventLog, string> GetString;
        internal Predicate<EventLog> Conditional;
        internal int Priority;

        public EventString(Func<EventLog, string> getString, int priority = 0, Race actorRace = (Race)4000, Race targetRace = (Race)4000, Predicate<EventLog> conditional = null)
        {
            Priority = priority;
            ActorRace = actorRace;
            TargetRace = targetRace;
            GetString = getString;
            if (conditional == null)
                Conditional = (s) => true;
            else
                Conditional = conditional;
        }
    }

    internal enum MessageTypes
    {
        SwallowMessages,
        RandomDigestionMessages,
        BellyRubMessages,
        CockVoreMessages,
        AnalVoreMessages,
        UnbirthMessages,
        TailVoreMessages,
        DigestionDeathMessages,
        BreastVoreMessages,
        AbsorptionMessages,
        BreastRubMessages,
        TailRubMessages,
        BallMassageMessages,
        TransferMessages,
        VoreStealMessages,
        BreastFeedMessages,
        CumFeedMessages,
        GreatEscapeKeep,
        GreatEscapeFlee,
    }

    static internal List<EventString> Redirect(MessageTypes type)
    {
        switch (type)
        {
            case MessageTypes.SwallowMessages:
                return SwallowMessages;
            case MessageTypes.RandomDigestionMessages:
                return RandomDigestionMessages;
            case MessageTypes.BellyRubMessages:
                return BellyRubMessages;
            case MessageTypes.CockVoreMessages:
                return CockVoreMessages;
            case MessageTypes.AnalVoreMessages:
                return AnalVoreMessages;
            case MessageTypes.UnbirthMessages:
                return UnbirthMessages;
            case MessageTypes.TailVoreMessages:
                return TailVoreMessages;
            case MessageTypes.DigestionDeathMessages:
                return DigestionDeathMessages;
            case MessageTypes.BreastVoreMessages:
                return BreastVoreMessages;
            case MessageTypes.AbsorptionMessages:
                return AbsorptionMessages;
            case MessageTypes.BreastRubMessages:
                return BreastRubMessages;
            case MessageTypes.TailRubMessages:
                return TailRubMessages;
            case MessageTypes.BallMassageMessages:
                return BallMassageMessages;
            case MessageTypes.TransferMessages:
                return TransferMessages;
            case MessageTypes.VoreStealMessages:
                return VoreStealMessages;
            case MessageTypes.BreastFeedMessages:
                return BreastFeedMessages;
            case MessageTypes.CumFeedMessages:
                return CumFeedMessages;
            case MessageTypes.GreatEscapeKeep:
                return GreatEscapeKeepMessages;
            case MessageTypes.GreatEscapeFlee:
                return GreatEscapeFleeMessages;
            default:
                return SwallowMessages;
        }
    }

    internal static List<EventString> SwallowMessages;
    internal static List<EventString> RandomDigestionMessages;
    internal static List<EventString> BellyRubMessages;
    internal static List<EventString> CockVoreMessages;
    internal static List<EventString> AnalVoreMessages;
    internal static List<EventString> UnbirthMessages;
    internal static List<EventString> TailVoreMessages;
    internal static List<EventString> DigestionDeathMessages;
    internal static List<EventString> BreastVoreMessages;
    internal static List<EventString> AbsorptionMessages;
    internal static List<EventString> BreastRubMessages;
    internal static List<EventString> TailRubMessages;
    internal static List<EventString> BallMassageMessages;
    internal static List<EventString> TransferMessages;
    internal static List<EventString> VoreStealMessages;
    internal static List<EventString> BreastFeedMessages;
    internal static List<EventString> CumFeedMessages;
    internal static List<EventString> GreatEscapeKeepMessages;
    internal static List<EventString> GreatEscapeFleeMessages;

    internal static void InitializeLists()
    {
        if (SwallowMessages != null)
            return;
        bool PreyDead(EventLog s) => s.Prey.IsDead;
        bool PreyCumgested(EventLog s) => TacticalUtilities.GetPredatorComponentOfUnit(s.Unit)?.PreyInLocation(PreyLocation.balls, false) == 1;
        bool CanBurp(EventLog s) => Config.BurpFraction > .1f;
        bool Farts(EventLog s) => Config.FartOnAbsorb;
        bool Scat(EventLog s) => Config.Scat && (s.preyLocation == PreyLocation.stomach || s.preyLocation == PreyLocation.stomach2);
        bool Lewd(EventLog s) => Config.LewdDialog;
        bool HardVore(EventLog s) => Config.HardVoreDialog;
        bool HardVoreInStomach(EventLog s) => Config.HardVoreDialog && (s.preyLocation == PreyLocation.stomach || s.preyLocation == PreyLocation.stomach2);
        bool InStomach(EventLog s) => s.preyLocation == PreyLocation.stomach || s.preyLocation == PreyLocation.stomach2;
        bool InWomb(EventLog s) => s.preyLocation == PreyLocation.womb;
        bool InStomachOrWomb(EventLog s) => s.preyLocation == PreyLocation.stomach || s.preyLocation == PreyLocation.stomach2 || s.preyLocation == PreyLocation.womb;
        bool InBreasts(EventLog s) => s.preyLocation == PreyLocation.breasts || s.preyLocation == PreyLocation.leftBreast || s.preyLocation == PreyLocation.rightBreast;
        bool InBalls(EventLog s) => s.preyLocation == PreyLocation.balls;
        bool FirstTime(EventLog s) => s.Unit.DigestedUnits == 0 && s.Unit.Level < 10 && s.Unit.Type != UnitType.Mercenary && s.Unit.Type != UnitType.SpecialMercenary && State.GameManager.PureTactical == false;
        bool FirstTimeAbsorption(EventLog s) => s.Unit.DigestedUnits == 1 && s.Unit.Level < 10 && s.Unit.Type != UnitType.Mercenary && s.Unit.Type != UnitType.SpecialMercenary && State.GameManager.PureTactical == false;
        bool TargetFirstTime(EventLog s) => s.Target.DigestedUnits == 0 && s.Target.Level < 10 && s.Target.Type != UnitType.Mercenary && s.Target.Type != UnitType.SpecialMercenary && State.GameManager.PureTactical == false;
        bool Friendly(EventLog s) => s.Unit.Side == s.Target.Side;
        bool ActorHumanoid(EventLog s) => s.Unit.Race < Race.Vagrants || s.Unit.Race >= Race.Selicia;
        bool HasGreatEscape(EventLog s) => s.Target.HasTrait(Traits.TheGreatEscape);
        bool Cursed(EventLog s) => s.Target.GetStatusEffect(StatusEffectType.WillingPrey) != null;
        bool Shrunk(EventLog s) => s.Target.GetStatusEffect(StatusEffectType.Diminished) != null;
        //bool ReqSSW(EventLog s) => SameSexWarrior(s.Unit) != "NULL";
        bool ReqOSW(EventLog s) => AttractedWarrior(s.Unit) != null;
        bool ReqOSWLewd(EventLog s) => AttractedWarrior(s.Unit) != null && Lewd(s);
        bool ReqOSWStomach(EventLog s) => AttractedWarrior(s.Unit) != null && InStomach(s);
        bool ReqOSWBelly(EventLog s) => AttractedWarrior(s.Unit) != null && InStomachOrWomb(s);
        bool ReqSSWAndOSW(EventLog s) => CompetitionWarrior(s.Unit) != null && AttractedWarrior(s.Unit) != null;
        bool ReqTargetCompatible(EventLog s) => RomanticTarget(s.Unit, s.Target);
        bool ReqTargetCompatibleLewd(EventLog s) => RomanticTarget(s.Unit, s.Target) && Lewd(s);
        bool ReqTargetClothingOn(EventLog s) => s.Target.ClothingType != 0;
        bool ReqTargetClothingOff(EventLog s) => s.Target.ClothingType == 0;
        bool WeightGain(EventLog s) => Config.WeightGain;
        bool BonesDisposal(EventLog s) => Config.Bones && (s.preyLocation == PreyLocation.stomach || s.preyLocation == PreyLocation.stomach2);
        bool TargetBoobs(EventLog s) => s.Target.HasBreasts;
        bool ActorBoobs(EventLog s) => s.Unit.HasBreasts;
        bool ActorTail(EventLog s) => RaceParameters.GetTraitData(s.Unit).HasTail;
        bool TargetLeader(EventLog s) => s.Target.Type == UnitType.Leader;
        bool ActorLeader(EventLog s) => s.Unit.Type == UnitType.Leader;
        bool TargetHumanoid(EventLog s) => s.Target.Race < Race.Vagrants || s.Target.Race >= Race.Selicia;
        bool CanAddressPlayer(EventLog s) => Config.FourthWallBreakType == FourthWallBreakType.On ||
                                                TacticalUtilities.IsUnitControlledByAIEnemy(s.Unit) && Config.FourthWallBreakType == FourthWallBreakType.EnemyOnly ||
                                                TacticalUtilities.IsUnitControlledByPlayer(s.Unit) && Config.FourthWallBreakType == FourthWallBreakType.FriendlyOnly;

        SwallowMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Unit.Name}</b> thrusts its tentacles in <b>{i.Target.Name}'s</b> body, grasping {GPPHis(i.Target)} core and reeling it inside their mouth!",
            actorRace: Race.Vagrants, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"Despite {GPPHis(i.Target)} struggling <b>{i.Target.Name}</b> finds {GPPHimself(i.Target)} bulging out <b>{i.Unit.Name}'s</b> {GetPredDesc(i.Unit)} body!",
            actorRace: Race.Vagrants, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> overcomes <b>{i.Target.Name}'s</b> resistance, engulfing the unfortunate vagrant inside their {PreyLocStrings.ToSyn(PreyLocation.stomach)}!",
            actorRace: Race.Vagrants, targetRace: Race.Vagrants, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b> with their tentacles and drags the struggling {GetPreyDesc(i.Target)} prey into their mouth!",
            actorRace: Race.Vagrants, targetRace: Race.Vagrants, priority: 10),
            new EventString((i) => $"Despite its struggling <b>{i.Target.Name}</b> finds itself bulging out <b>{i.Unit.Name}'s</b> {GetPredDesc(i.Unit)} body!",
            actorRace: Race.Vagrants, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> dives down onto <b>{i.Target.Name}</b>, stuffing {GPPHim(i.Target)} inside!",
            actorRace: Race.Vagrants, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> forces a part of {GPPHimself(i.Unit)} inside <b>{i.Target.Name}'s</b> body, engulfing the other slimes core and tearing it from {GPPHis(i.Target)} form!",
            actorRace: Race.Slimes, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> launches {GPPHimself(i.Unit)} at <b>{i.Target.Name}</b>, engulfing {GPPHim(i.Target)} almost instantly!",
            actorRace: Race.Slimes, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> turns into a puddle and oozes under <b>{i.Target.Name}</b>, engulfing {GPPHim(i.Target)} from below!",
            actorRace: Race.Slimes, targetRace: Race.Slimes, priority: 10),

            new EventString((i) => $"With a squelch <b>{i.Unit.Name}</b> punches {GPPHis(i.Unit)} fist in <b>{i.Target.Name}'s</b>, grabbing the {GetRaceDescSingl(i.Target)}'s core and {PreyLocStrings.GetOralVoreVPCT()} it in a hurry!",
            actorRace: Race.Bunnies, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> bounces on <b>{i.Target.Name}</b>, stuffing the {GetRaceDescSingl(i.Target)} in {GPPHis(i.Target)} {PreyLocStrings.ToSyn(PreyLocation.stomach)} within moments!",
            actorRace: Race.Bunnies, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"Surprised to see a fellow {GetRaceDescSingl(i.Unit)} charging at {GPPHim(i.Target)}, <b>{i.Target.Name}</b> barely manages a token resistance before finding {GPPHimself(i.Target)} in <b>{i.Unit.Name}'s</b> {PreyLocStrings.ToSyn(PreyLocation.stomach)}!",
            actorRace: Race.Bunnies, targetRace: Race.Bunnies, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}</b> appears stunned as <b>{i.Unit.Name}</b> stuffs {GPPHis(i.Unit)} fellow {GetRaceDescSingl(i.Target)}'s arms in {GPPHis(i.Unit)} mouth and swallows. <b>{i.Target.Name}</b> only manages a quiet \"Why?\" before {GPPHis(i.Target)} head is engulfed too, rest soon following.",
            actorRace: Race.Bunnies, targetRace: Race.Bunnies, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> bounces at <b>{i.Target.Name}</b>, slamming {GPPHim(i.Target)} to the ground and gobbling the {GetRaceDescSingl(i.Target)} like a carrot!",
            actorRace: Race.Bunnies, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> barely realises what is going on before <b>{i.Unit.Name}</b> kicks {GPPHis(i.Target)} legs from under {GPPHim(i.Target)}, then proceeds to devour the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)}.",
            actorRace: Race.Bunnies, priority: 9),

            new EventString((i) => $"Swatting away a glob of slime, <b>{i.Unit.Name}</b> pulls at <b>{i.Target.Name}'s</b> exposed core, depositing the {GetPreyDesc(i.Target)} prey's nucleus in {PreyLocStrings.GetOralVoreVPCT()}!",
            actorRace: Race.Cats, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pounces on <b>{i.Target.Name}</b>, lapping the {GetRaceDescSingl(i.Target)} in {GPPHis(i.Target)} hungry {PreyLocStrings.ToSyn(PreyLocation.stomach)} in no time!",
            actorRace: Race.Cats, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"Rarely has there been such hissing and spitting as now with <b>{i.Unit.Name}</b> {PreyLocStrings.GetOralVoreVPCT()} <b>{i.Target.Name}</b>!",
            actorRace: Race.Cats, targetRace: Race.Cats, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}'s</b> fur stands on end as {GPPHeIs(i.Target)} forced down <b>{i.Unit.Name}'s</b> throat and forced to curl in {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}! Well, it's warm at least.",
            actorRace: Race.Cats, targetRace: Race.Cats, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> stalks <b>{i.Target.Name}</b>, pouncing at the {GetRaceDescSingl(i.Target)} and only closing {GPPHis(i.Unit)} jaws once the {GetPreyDesc(i.Target)} prey has vanished beyond {GPPHim(i.Unit)}!",
            actorRace: Race.Cats, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> doesn't even have time to scream before {GPPHeIs(i.Target)} swallowed whole by <b>{i.Unit.Name}</b>.",
            actorRace: Race.Cats, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> thinks nothing of making a meal out of <b>{i.Target.Name}</b>, {GPPHe(i.Target)} would do the same to {GPPHim(i.Unit)}.",
            actorRace: Race.Alligators, targetRace: Race.Alligators, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}'s</b> ridged tail tickles nicely as it slides down <b>{i.Unit.Name}'s</b> gullet.",
            actorRace: Race.Alligators, targetRace: Race.Alligators, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}'s</b> growling is fully lost beneath <b>{i.Unit.Name}'s</b> rumble of satisfaction as {GPPHe(i.Unit)} stuffs the other {GetRaceDescSingl(i.Target)} in {GPPHis(i.Unit)} jaws!",
            actorRace: Race.Alligators, targetRace: Race.Alligators, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> charges at <b>{i.Target.Name}</b>, globs of {GetRaceDescSingl(i.Target)} flying everywhere as {GPPHe(i.Unit)} bites down on the slime's core and swallows it!",
            actorRace: Race.Alligators, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}</b> has no time to react as <b>{i.Unit.Name}'s</b> jaws fall on {GPPHim(i.Target)}, sending the {GetRaceDescSingl(i.Target)} down into the {GPPHe(i.Unit)} gator's {PreyLocStrings.ToSyn(PreyLocation.stomach)}.",
            actorRace: Race.Alligators, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lumbers towards <b>{i.Target.Name}</b>, taking no heed of the {GetRaceDescSingl(i.Target)}'s attempt to warn {GPPHim(i.Unit)} off and {PreyLocStrings.GetOralVoreVPCT()} {GPPHis(i.Unit)} {GetPreyDesc(i.Target)} prey with gusto!",
            actorRace: Race.Alligators, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> struggles in <b>{i.Unit.Name}'s</b> jaws make no difference, the {GetPredDesc(i.Unit)} {GetRaceDescSingl(i.Unit)} having soon swallowed {GPPHis(i.Unit)} {GetPreyDesc(i.Target)} prey down!",
            actorRace: Race.Alligators, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> swoops down, {GPPHis(i.Unit)} jaws scooping up a sizeable portion of <b>{i.Target.Name}</b>, sending the {GetRaceDescSingl(i.Target)}'s core down the {GetRaceDescSingl(i.Unit)}'s throat!",
            actorRace: Race.Wyvern, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> world goes dark as <b>{i.Unit.Name}</b> descends on {GPPHim(i.Target)}, {PreyLocStrings.GetOralVoreVPCT()} the slime in one gulp and locking the {GetRaceDescSingl(i.Target)} in {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.stomach)}",
            actorRace: Race.Wyvern, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"The shark known as <b>{i.Target.Name}</b> jerks in surprise as {GPPHe(i.Target)} gets intercepted mid-flight, swimming down <b>{i.Unit.Name}'s</b> {GetRandomStringFrom("gullet", "neck", "throat")} as a large, {GetPreyDesc(i.Target)} bulge.",
            actorRace: Race.Wyvern, targetRace: Race.FeralSharks, priority: 9),
            new EventString((i) => $"Coming to a stop just as {GPPHe(i.Target)} was about to dive, <b>{i.Target.Name}</b> finds {GPPHis(i.Target)} tail firmly held by the talons of <b>{i.Unit.Name}</b>, a set of mighty jaws soon sending the {GetRaceDescSingl(i.Target)} down into the {GetRaceDescSingl(i.Unit)}'s {PreyLocStrings.ToSyn(PreyLocation.stomach)}.",
            actorRace: Race.Wyvern, targetRace: Race.FeralSharks, priority: 9),
            new EventString((i) => $"Getting the better of {GPPHis(i.Unit)} rival, <b>{i.Unit.Name}</b> clamps {GPPHis(i.Unit)} jaws around <b>{i.Target.Name}'s</b> head, soon to stuffing the beaten wyvern down into {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.stomach)}.",
            actorRace: Race.Wyvern, targetRace: Race.Wyvern, priority: 9),
            new EventString((i) => $"Finding a chance, <b>{i.Unit.Name}</b> gives in to {GPPHis(i.Unit)} hunger and snatches up an unfamiliar {GetRaceDescSingl(i.Target)}, savouring <b>{i.Target.Name}</b> for a moment before gulping down {GPPHis(i.Unit)} {GetPreyDesc(i.Target)} prey.",
            actorRace: Race.Wyvern, targetRace: Race.YoungWyvern, priority: 9),
            new EventString((i) => $"Picking up a snack on a fly by, <b>{i.Unit.Name}</b> slurps up {GPPHis(i.Unit)} {GetRaceDescSingl(i.Target)} victim as {GPPHe(i.Unit)} flies on.",
            actorRace: Race.Wyvern, targetRace: Race.Serpents, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> opens extra wide before swooping down, scooping up <b>{i.Target.Name}</b>'s {GetPreyDesc(i.Target)} body into {GPPHis(i.Unit)} maw and banishing {GPPHim(i.Target)} into a huge, scaly {i.preyLocation.ToSyn()} bulge.",
            actorRace: Race.Wyvern, targetRace: Race.Selicia, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lands before <b>{i.Target.Name}</b>, wrapping {GPPHim(i.Target)} in {GPPHis(i.Unit)} wings and {PreyLocStrings.GetOralVoreVPCT()} {GPPHim(i.Target)} in several heavy swallows, dumped into the wyvern's {PreyLocStrings.ToSyn(PreyLocation.stomach)}.",
            actorRace: Race.Wyvern, targetRace: Race.Selicia, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b> in {GPPHis(i.Unit)} talons in a passing flight, tossing the {GetRaceDescSingl(i.Target)} in {GPPHis(i.Unit)} jaws and down into {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.stomach)}.",
            actorRace: Race.Wyvern, priority: 9),
            new EventString((i) => $"Landing briefly, <b>{i.Unit.Name}</b> snarfs down <b>{i.Target.Name}</b>, the {GetRaceDescSingl(i.Target)} now feeding the {GetPredDesc(i.Unit)} predator.",
            actorRace: Race.Wyvern, priority: 9),

            new EventString((i) => $"\"It'll be better use of your flesh to have it be part of me\", <b>{i.Unit.Name}</b> says, guiding <b>{i.Target.Name}</b> gently but firmly in {GPPHis(i.Unit)} jaws.",
            actorRace: Race.Crux, targetRace: Race.Crux, priority: 10),
            new EventString((i) => $"Stuffing <b>{i.Target.Name}</b> rear-first down {GPPHis(i.Unit)} gullet, <b>{i.Unit.Name}</b> smiles as {GPPHe(i.Unit)} looks at the curly ears of {GPPHis(i.Unit)} fellow crux, the last thing visible of {GPPHim(i.Target)} before {GPPHe(i.Target)} too vanish{EsIfSingular(i.Target)} from sight.",
            actorRace: Race.Crux, targetRace: Race.Crux, priority: 10),
            new EventString((i) => $"Giggling quietly to {GPPHimself(i.Unit)}, <b>{i.Unit.Name}</b> runs a finger under <b>{i.Target.Name}'s</b> chin, before giving a whack at the back of the {GetRaceDescSingl(i.Target)}'s head to send the {GetPreyDesc(i.Target)} morsel on {GPPHis(i.Target)} way down the wildly smiling Crux's gullet.",
            actorRace: Race.Crux, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> swiftly gulps down <b>{i.Target.Name}</b>, giving a hefty burp before cheerily saying: \"Thank you for your contribution to science!\"",
            actorRace: Race.Crux, priority: 9, conditional: CanBurp),

            new EventString((i) => $"<b>{i.Unit.Name}</b> parts {GPPHis(i.Unit)} lips, and shoves <b>{i.Target.Name}</b>’s head into {GPPHis(i.Unit)} mouth and swallows. {GPPHe(i.Unit)} enjoys the taste of hot blood splashing down {GPPHis(i.Unit)} gullet as one of {GPPHis(i.Unit)} sharp fangs scrapes <b>{i.Target.Name}</b>’s skin.",
            actorRace: Race.Panthers, priority: 9, conditional: s => HardVore(s) && InStomach(s)),
            new EventString((i) => $"In a stunning display of martial prowess and voracity <b>{i.Unit.Name}</b> overpowers <b>{i.Target.Name}</b> and sends {GPPHim(i.Target)} down {GPPHis(i.Unit)} sloshing {PreyLocStrings.ToSyn(i.preyLocation)}.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> begs for mercy, but <b>{i.Unit.Name}</b> feels no pity, only hunger, and before long <b>{i.Target.Name}</b> is sliding down the panther's throat to form a tight bulge on {GPPHis(i.Unit)} frame.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"As <b>{i.Target.Name}</b> slides into <b>{i.Unit.Name}</b>’s stomach, the victor teases {GPPHim(i.Target)}, patting {GPPHis(i.Unit)} swollen stomach: “Go ahead and struggle all you like. My belly is stronger than you ever were!”",
            actorRace: Race.Panthers, priority: 9),
             new EventString((i) => $"<b>{i.Unit.Name}</b> honors {GPPHis(i.Unit)} ancestors in the only proper way - by stuffing {GPPHis(i.Unit)} foe down into {GPPHis(i.Unit)} waiting belly.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> thanks the spirits for this wonderful meal - <b>{i.Target.Name}</b> was delicious and filling.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> licks {GPPHis(i.Unit)} lips and pats {GPPHis(i.Unit)} swelling stomach as <b>{i.Target.Name}</b> slides in.",
            actorRace: Race.Panthers, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> is taken by surprise as <b>{i.Target.Name}</b> throws {GPPHis(i.Target)} weapon away and forces {GPPHis(i.Target)} way down {GPPHis(i.Target)} throat.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> pounces onto the nearest predator and literally jumps down {GPPHis(i.Unit)} gullet. The experience leaves <b>{i.Unit.Name}</b> with confusion, a coughing fit, and a happily wobbling midsection.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> has to do little more than open {GPPHis(i.Unit)} mouth as <b>{i.Target.Name}</b> eagerly slips down {GPPHis(i.Unit)} esophagus.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> is overcome with a sudden urge to find a tight, warm space. <b>{i.Unit.Name}</b> kindly offers to let {GPPHim(i.Target)} use {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}. The bewitched idiot happily accepts!",
            priority: 9, conditional: Cursed),
            new EventString((i) => $"<b>{i.Target.Name}</b> shakes {GPPHis(i.Target)} head as a strange spell is cast on {GPPHim(i.Target)}. <b>{i.Unit.Name}</b> uses this opportunity to stuff the cursed warrior down into {GPPHis(i.Unit)} waiting belly.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),

            new EventString((i) => $"Grasping her prey gently in her paws, <b>{i.Unit.Name}</b> sends <b>{i.Target.Name}</b> on a supposedly one way trip down to her hungering guts. ",
            actorRace: Race.Abakhanskya, priority: 11),
            new EventString((i) => $"Playfully falling upon her hopelessly outmatched food, <b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> into her jaws leisurely, <b>{i.Target.Name}</b> protesting on {GPPHis(i.Target)} way down, only to be muted by the chorus of <b>{i.Unit.Name}</b>'s stomach.",
            actorRace: Race.Abakhanskya, priority: 11),
            new EventString((i) => $"<b>{i.Unit.Name}</b> bolts forth with stunning speed, flicking up <b>{i.Target.Name}</b> in to the air before expertly catching {GPPHim(i.Target)} in herr jaws, letting gravity do the rest. ",
            actorRace: Race.Abakhanskya, priority: 11),
            new EventString((i) => $"Giving a mocking laugh, <b>{i.Unit.Name}</b> steps forth, startling <b>{i.Target.Name}</b> before cruelly gobbling {GPPHim(i.Target)} up. After getting {GPPHis(i.Target)} flavour, of course. ",
            actorRace: Race.Abakhanskya, priority: 11),
            new EventString((i) => $"Having the upper hand, <b>{i.Unit.Name}</b> seizes the opportunity and descends upon <b>{i.Target.Name}</b>, who soon is but a rounded bulge in the chunky dragoness's throat, and then belly.",
            actorRace: Race.Abakhanskya, priority: 11),




            new EventString((i) => $"Before <b>{i.Target.Name}</b> can even react, <b>{i.Unit.Name}'s</b> tubular maw pierces {GPPHis(i.Unit)} surface, the {GetRaceDescSingl(i.Unit)} swiftly sucking most of the {GetRaceDescSingl(i.Target)} in the monster's belly, leaving only a small puddle behind.",
            actorRace: Race.Collectors, targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"As a long, clawed leg lands on <b>{i.Target.Name}</b> and slams {GPPHim(i.Target)} against the ground, the {GetRaceDescSingl(i.Target)} can only watch as <b>{i.Unit.Name}'s</b> head turns towards {GPPHim(i.Target)}, the {GetRaceDescSingl(i.Unit)}'s beak opening and the tooth lined tube within extending towards {GPPHim(i.Target)}.",
            actorRace: Race.Collectors, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> leans over <b>{i.Target.Name}</b>, the {GetRaceDescSingl(i.Unit)}'s feeding appendage clasping on the {GetRaceDescSingl(i.Target)}'s head, sucking in the {GetPreyDesc(i.Target)} prey.",
            actorRace: Race.Collectors, priority: 10),

            new EventString((i) => $"<b>{i.Target.Name}</b> saw a cake. The {GetRaceDescSingl(i.Unit)} ate the {GetRaceDescSingl(i.Target)}.",
            actorRace: Race.Cake, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}</b> had heard that cakes were delicious. Unfortunately for {GPPHim(i.Target)}, <b>{i.Unit.Name}</b> thought the same about {GetRaceDescSingl(i.Target)}s.",
            actorRace: Race.Cake, priority: 10),
            new EventString((i) => $"Never had a {GetRaceDescSingl(i.Unit)} moved as fast as when <b>{i.Unit.Name}</b> charged to engulf <b>{i.Target.Name}</b>, the {GetRaceDescSingl(i.Target)} finding out that this cake was very moist indeed.",
            actorRace: Race.Cake, priority: 10),

            new EventString((i) => $"Looming over her small prey, {i.Unit.Name} leans forward and engulfs {i.Target.Name} with her jaws, swallowing down the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)}.",
            actorRace: Race.Selicia, priority: 10, conditional: (i) => State.RaceSettings.GetBodySize(i.Target.Race) <= 15),
            new EventString((i) => $"Pouncing on {i.Target.Name}, {i.Unit.Name} licks her lips at the prospect of getting a good tasty meal out of the {GetRaceDescSingl(i.Target)}, sending the {GetPreyDesc(i.Target)} down in her belly as a great bulge.",
            actorRace: Race.Selicia, priority: 10),
            new EventString((i) => {
                if (TacticalUtilities.GetPredatorComponentOfUnit(i.Unit)?.PreyCount > 1)
                    return $"Yawning wide, <b>{i.Unit.Name}</b> sends <b>{i.Target.Name}</b> down her {GetRandomStringFrom("gullet", "throat")}, the {GetRaceDescSingl(i.Target)} joining others already filling her {PreyLocStrings.ToSyn(PreyLocation.stomach)}.";
                else if (TacticalUtilities.GetPredatorComponentOfUnit(i.Unit)?.AlivePrey > 1)
                    return $"Yawning wide, <b>{i.Unit.Name}</b> lets <b>{i.Target.Name}</b> hear the noise made by her {PreyLocStrings.ToSyn(PreyLocation.stomach)}'s current occupant, then sends the {GetRaceDescSingl(i.Target)} down to meet the source.";
                else if (TacticalUtilities.GetPredatorComponentOfUnit(i.Unit)?.PreyCount == 0)
                    return $"Yawning wide, <b>{i.Unit.Name}</b> lets <b>{i.Target.Name}</b> hear the noise made by her empty stomach, then gets to filling it with the {GetRaceDescSingl(i.Target)}.";
                else return $"Yawning wide, <b>{i.Unit.Name}</b> lets <b>{i.Target.Name}</b> hear the noises of digestion coming from her {PreyLocStrings.ToSyn(PreyLocation.stomach)}, then sends the {GetRaceDescSingl(i.Target)} in for a closer look.";
                },
            actorRace: Race.Selicia, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> falls on <b>{i.Target.Name}</b>, {GetRandomStringFrom("gulping down", "devouring", "gobbling up")} her {GetPreyDesc(i.Target)} prey in seconds, the dragon's tail finding its way into her {GetRandomStringFrom("slit", "snatch")} as the {GetRaceDescSingl(i.Target)} bulges out her {PreyLocStrings.ToSyn(PreyLocation.stomach)}.",
            actorRace: Race.Selicia, priority: 10, conditional: (i) => Lewd(i) && i.Target.HasBreasts && (i.Target.HasDick == false || Config.HermsCanUB)) ,

            new EventString((i) => $"\"You better get comfy, I have four stomachs\" <b>{i.Unit.Name}</b> teases <b>{i.Target.Name}</b> after swallowing the {GetRaceDescSingl(i.Target)} down.",
            actorRace: Race.Taurus, priority: 9, conditional: InStomach),

            new EventString((i) => $"<b>{i.Unit.Name}</b> reaches for <b>{i.Target.Name}</b>’s core, swallowing it along with surrounding slime!",
            targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b>’s viscous form and shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} mouth!",
            targetRace: Race.Slimes, priority: 10),

            new EventString((i) => $"<b>{i.Unit.Name}</b> slurps up <b>{i.Target.Name}</b> like a noodle.",
            targetRace: Race.EasternDragon, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slurps up <b>{i.Target.Name}</b> and sighs after the serpentine dragon curls up in {GPPHis(i.Unit)} stomach.",
            targetRace: Race.EasternDragon, priority: 9),

            new EventString((i) => $"<b>{i.Target.Name}</b> lets out a distressed yelp as {GPPHeIs(i.Target)} shoved headfirst into <b>{i.Unit.Name}</b>’s mouth!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b> and hungrily swallows {GPPHim(i.Target)} whole!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> casts aside {GPPHis(i.Unit)} weapon and grabs onto <b>{i.Target.Name}</b>, stuffing {GPPHim(i.Target)} into {GPPHis(i.Unit)} gullet!",
            priority: 8, conditional: (s) => s.Unit.HasWeapon && s.Unit.FixedGear == false),
            new EventString((i) => $"<b>{i.Unit.Name}</b> swiftly swallows <b>{i.Target.Name}</b>’s head before lifting {GPPHim(i.Target)} and letting the gravity do the rest!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> onto the ground and pushes {GPPHim(i.Target)} legfirst into {GPPHis(i.Unit)} mouth!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b>, shoving the {GetRaceDescSingl(i.Target)} down {GPPHis(i.Target)} throat!",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> struggles as <b>{i.Unit.Name}</b> begins eating {GPPHim(i.Target)}, the {GetPredDesc(i.Unit)} predator finishing with a burp!",
            priority: 8, conditional: CanBurp),
            new EventString((i) => $"<b>{i.Target.Name}</b> cries out as {GPPHeIs(i.Target)} swallowed, clawing out all the way down <b>{i.Unit.Name}</b>’s slick gullet!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> overpowers <b>{i.Target.Name}</b> and swiftly shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> {PreyLocStrings.GetOralVoreVPT()} <b>{i.Target.Name}</b>, squeezing {GPPHim(i.Target)} down into {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.stomach)}!",
            priority: 8),

            new EventString((i) => $"<b>{i.Unit.Name}</b> begins to gag as {GPPHis(i.Unit)} prey’s hips get caught in {GPPHis(i.Unit)} throat but with a final gulp sends <b>{i.Target.Name}</b> down into {GPPHis(i.Unit)} stomach.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> flirts with <b>{i.Target.Name}</b> who lowers {GPPHis(i.Target)} guard. By the time {GPPHe(i.Target)} think{SIfSingular(i.Target)} of a response, <b>{i.Unit.Name}</b> kisses {GPPHim(i.Target)} which quickly turns into a swallow, sending the betrayed meal into digestive juices.",
            priority: 8, conditional: Lewd),
            new EventString((i) => $"<b>{i.Target.Name}</b> begs for mercy as <b>{i.Unit.Name}</b> greedily shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} mouth.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> panics and tries to run but can’t escape <b>{i.Unit.Name}</b>’s hunger and soon finds {GPPHeIsAbbr(i.Target)} sliding down {GPPHis(i.Unit)} throat.",
            priority: 8),
            new EventString((i) => $"\"You're mine!\", says <b>{i.Unit.Name}</b>, before swiftly sending <b>{i.Target.Name}</b> down {GPPHis(i.Unit)} pulsating gullet with immense indulgence. \"Now start squirming, or you'll be nothing but shit soon.\"",
            priority: 8, conditional: s => Scat(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s fellow warriors cheer {GPPHim(i.Unit)} on as {GPPHe(i.Unit)} slowly forces <b>{i.Target.Name}</b> down into {GPPHis(i.Unit)} belly.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s comrades look away as {GPPHeIs(i.Unit)} sucked down by <b>{i.Unit.Name}</b>",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> closes {GPPHis(i.Unit)} eyes and charges with a scream but trips.  When {GPPHe(i.Unit)} open{SIfSingular(i.Unit)} {GPPHis(i.Unit)} eyes {GPPHe(i.Unit)} find{SIfSingular(i.Unit)} <b>{i.Target.Name}</b> angrily struggling from inside {GPPHis(i.Unit)} stomach.",
            priority: 8),

            new EventString((i) => $"Despite {GPPHis(i.Unit)} inexperience, <b>{i.Unit.Name}</b> manages to wrap {GPPHis(i.Unit)} lips around <b>{i.Target.Name}</b>’s head and with a difficult gulp swallows {GPPHis(i.Unit)} first prey whole",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> swallows <b>{i.Target.Name}</b> but begins gagging as {GPPHe(i.Unit)} {HasHave(i.Unit)} to press down on the top of {GPPHis(i.Unit)} belly to keep {GPPHis(i.Unit)} first prey in",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is surprised by how difficult it is to swallow <b>{i.Target.Name}</b>. {Capitalize(GPPHe(i.Unit))} thought it would be much easier but with a desperate swallow {GPPHe(i.Target)} is forced down.",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"As the fight turns in <b>{i.Unit.Name}</b>’s favor {GPPHe(i.Unit)} commit{SIfSingular(i.Unit)} to the move taught to {GPPHim(i.Unit)}, lifting <b>{i.Target.Name}</b> into the air while wrapping {GPPHis(i.Unit)} lips around {GPPHis(i.Target)} head. Sure enough, gravity takes over and sends the prey down {GPPHis(i.Unit)} gullet. However, the weight hitting the bottom of {GPPHis(i.Unit)} sagging gut tips {GPPHim(i.Unit)} over, leaning the new pred across {GPPHis(i.Unit)} wobbling belly.",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"Getting <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} throat was easy. Swallowing {GPPHim(i.Target)} was proving more difficult as <b>{i.Unit.Name}</b> begins to choke on {GPPHis(i.Unit)} huge meal. Desperate to get {GPPHim(i.Target)} down, the virgin predator begins to use {GPPHis(i.Unit)} arms to guide {GPPHis(i.Unit)} wiggling meal down. Thankfully the effort pays off, resulting a massive gut and tired pred.",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"Everything happens so fast that <b>{i.Unit.Name}</b> never even got a chance to taste {GPPHis(i.Unit)} meal before it entered {GPPHis(i.Unit)} stomach. The new pred laments the fact that {GPPHe(i.Unit)} doesn’t even remember the taste of {GPPHis(i.Unit)} first prey. <b>{i.Target.Name}</b>, for {GPPHis(i.Target)} part is also angry at the situation.",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lets out a muffled groan of pain as {GPPHis(i.Unit)} belly stretches to accommodate {GPPHis(i.Unit)} first living meal. The virgin predator wraps {GPPHis(i.Unit)} arms around {GPPHis(i.Unit)} growing midsection, trying to keep {GPPHimself(i.Unit)} from bursting at the seams. Thankfully the expansion stops as <b>{i.Target.Name}</b> balls up in the pit of {GPPHis(i.Unit)} gut, leaving a gently jiggling bulge and proud first timer.",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),






            new EventString((i) => $"<b>{i.Target.Name}</b> is shocked when <b>{i.Unit.Name}</b> attacks and sends {GPPHim(i.Target)} sliding down {GPPHis(i.Unit)} throat.",
            priority: 9, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> receives orders to devour <b>{i.Target.Name}</b>, doing so without hesitation, sending {GPPHis(i.Unit)} ally into a gurgling doom.",
            priority: 9, conditional: Friendly),
            new EventString((i) => $"<b>{i.Target.Name}</b>'s screams for help are muffled as <b>{i.Unit.Name}</b>’s esophagus drags {GPPHis(i.Unit)} former ally down.",
            priority: 9, conditional: Friendly),
            new EventString((i) => $"With a quick apology, <b>{i.Unit.Name}</b> swallows {GPPHis(i.Unit)} struggling comrade before {GPPHe(i.Target)} can realize what’s happening! ",
            priority: 9, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pushes <b>{i.Target.Name}</b> over, shoves {GPPHis(i.Target)} legs into {GPPHis(i.Unit)} mouth, lifts {GPPHis(i.Unit)} former ally up into the air and sends {GPPHis(i.Unit)} current meal to rest in {GPPHis(i.Unit)} tummy.",
            priority: 9, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is disgusted by <b>{i.Target.Name}</b>’s weakness and quickly swallows {GPPHim(i.Target)}.",
            priority: 9, conditional: Friendly),

            new EventString((i) => $"As <b>{i.Unit.Name}</b> begins to scarf down <b>{i.Target.Name}</b>, {GPPHe(i.Unit)} makes sure that <b>{AttractedWarrior(i.Unit).Name}</b> has a good view of the show as {GPPHis(i.Unit)} prey wiggles down {GPPHis(i.Unit)} throat.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> seductively eyes <b>{AttractedWarrior(i.Unit).Name}</b> as {GPPHe(i.Unit)} swallows <b>{i.Target.Name}</b>.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{AttractedWarrior(i.Unit).Name}</b> watches in awe as <b>{i.Target.Name}</b> becomes little more than a bulge on <b>{i.Unit.Name}</b>’s enticing frame.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> almost spits <b>{i.Target.Name}</b> up when <b>{AttractedWarrior(i.Unit).Name}</b> winks at {GPPHim(i.Unit)} but manages to recover and force the rival warrior down.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sees <b>{i.Target.Name}</b> approaching <b>{AttractedWarrior(i.Unit).Name}</b> with malicious intent but heroically stuffs the villain into {GPPHis(i.Unit)} tummy prison.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lovingly points at <b>{AttractedWarrior(i.Unit).Name}</b> as {GPPHe(i.Unit)} gulps <b>{i.Target.Name}</b> down to let {GPPHim(AttractedWarrior(i.Unit))} know this is for {GPPHim(AttractedWarrior(i.Unit))}. However, this is mistaken as an intimidating threat.",
            priority: 8, conditional: ReqOSW),

            new EventString((i) => $"<b>{i.Target.Name}</b> finds himself unguarded by {GPPHis(i.Unit)} troops when <b>{i.Unit.Name}</b> grabs hold of {GPPHim(i.Target)} and gobbles {GPPHim(i.Target)} up! \"Their leader was most delicious!\" <b>{i.Unit.Name}</b> proclaims proudly, rubbing {GPPHis(i.Unit)} tummy.",
            priority: 10, conditional: s => ActorHumanoid(s) && InStomach(s) && TargetLeader(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slams <b>{i.Target.Name}</b> to the ground with a heavy blow. \"I have been waiting to do this for a long time.\" {GPPHe(i.Unit)} whisper{SIfSingular(i.Unit)} to <b>{i.Target.Name}</b>'s ear before hungrily swallows {GPPHim(i.Target)} whole!",
            priority: 10, conditional: s => ActorLeader(s) && InStomach(s) && TargetLeader(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> bellows {GPPHis(i.Unit)} triumph as {GPPHe(i.Unit)} pull{SIfSingular(i.Unit)} {GPPHis(i.Unit)} rival leader <b>{i.Target.Name}</b> down {GPPHis(i.Unit)} throat.",
            priority: 10, conditional: s => ActorLeader(s) && InStomach(s) && TargetLeader(s)),


            new EventString((i) => $"<b>{i.Unit.Name}</b> excitedly throws {GPPHis(i.Unit)} weapons to the ground and leaps at the huge cake, mouth open! In a frenzy, {GPPHe(i.Unit)} quickly scarfs down the whole cake. All that's left is some whip cream, which {GPPHe(i.Unit)} licks up, and a very {GetRandomStringFrom("fat", "full", "stuffed")} and happy <b>{i.Unit.Name}</b>.",
            targetRace: Race.Cake, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> goes into a frenzied pigout as {GPPHe(i.Unit)} starts stuffing {GPPHimself(i.Unit)} with the {GetRaceDescSingl(i.Target)}!",
            targetRace: Race.Cake, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> quickly scarfs down the {GetRaceDescSingl(i.Target)} in front of {GPPHim(i.Unit)}. {Capitalize(GPPHe(i.Unit))} licks {GPPHis(i.Unit)} lips and savours the sweet taste",
            targetRace: Race.Cake, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s mouth waters as {GPPHe(i.Unit)} runs towards the large dessert in front of {GPPHim(i.Unit)}. The {GetRaceDescSingl(i.Unit)}, unable to resist, begins grabbing chunks of the cake and stuffing them into {GPPHis(i.Unit)} mouth. After {GPPHe(i.Unit)} finishes, {GPPHe(i.Unit)} falls on {GPPHis(i.Unit)} back and belches loudly, happily rubbing {GPPHis(i.Unit)} {GetRandomStringFrom("fat", "big", "full", "stuffed")} full belly!",
            targetRace: Race.Cake, priority: 10, conditional: s => CanBurp(s) && HardVore(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs the shrunken <b>{i.Target.Name}</b> and holds {GPPHim(i.Target)} in front of {GPPHis(i.Unit)} face. <b>{i.Unit.Name}</b> gives {GPPHim(i.Target)} a lick before stuffing the tiny {GetRaceDescSingl(i.Target)} in {GPPHis(i.Unit)} mouth and swallowing.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $" A shrunken <b>{i.Target.Name}</b> screams in horror as the massive hand of <b>{i.Unit.Name}</b> envelops {GPPHis(i.Target)} entire body. <b>{i.Unit.Name}</b> smirks before tossing the scared {GetRaceDescSingl(i.Target)} into {GPPHis(i.Unit)} mouth.",
            priority: 10, conditional: s=> Shrunk(s) && ActorHumanoid(s)),
            new EventString((i) => $" A shrunken <b>{i.Target.Name}</b> tries to run away as <b>{i.Unit.Name}</b> reaches for {GPPHim(i.Target)} but wasn't fast enough and gets caught. <b>{i.Unit.Name}</b> brings {GPPHim(i.Target)} up to {GPPHis(i.Unit)} mouth before tossing {GPPHim(i.Target)} {GetRaceDescSingl(i.Target)} inside.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $"<b>{i.Unit.Name}</b> looms over the now-tiny <b>{i.Target.Name}</b>. The {GetRaceDescSingl(i.Unit)} reaches down and grabs {GPPHim(i.Target)}, making fun of {GPPHis(i.Target)} new size before chucking the screaming {GetRaceDescSingl(i.Target)} into {GPPHis(i.Unit)} mouth.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $"<b>{i.Target.Name}</b> tries to get away from <b>{i.Unit.Name}</b> but at this tiny size, the {GetRaceDescSingl(i.Unit)} catches {GPPHim(i.Target)} easily and quickly gulps {GPPHim(i.Target)} down.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $"<b>{i.Target.Name}</b> tries to hit back at <b>{i.Unit.Name}</b> but at this tiny size, {GPPHe(i.Target)} doesn't do any real damage. The {GetRaceDescSingl(i.Unit)} grabs {GPPHim(i.Target)} and laughs in {GPPHis(i.Target)} face before playfully tossing <b>{i.Target.Name}</b> into {GPPHis(i.Unit)}",
            priority: 10, conditional: Shrunk),

            new EventString((i) => $"<b>{i.Unit.Name}</b> opens {GPPHis(i.Unit)} maw wide, engulfing <b>{i.Target.Name}</b>'s form... But as  {GPPHis(i.Unit)} prey rounds out  {GPPHis(i.Unit)} gullet, there's no familiar gurgling sound of digestion.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> swallows <b>{i.Target.Name}</b> and goes back to battle, unbothered by cries of protest from within.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"\"Let's see how slippery you really are\" - grins <b>{i.Unit.Name}</b> as he swallows <b>{i.Target.Name}</b.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"As {GPPHe(i.Target)} slips into <b>{i.Unit.Name}</b>'s maw, <b>{i.Target.Name}</b> is planning something.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> plays with <b>{i.Target.Name}</b>, keeping {GPPHis(i.Target)} face just out of {GPPHis(i.Unit)} throat and teasing {GPPHim(i.Target)}, saying that this light will be the last in {GPPHis(i.Target)} life. Angry, <b>{i.Target.Name}</b> is already plotting something...",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> commends <b>{i.Target.Name}</b> on how smoothly {GPPHe(i.Target)} goes in. <b>{i.Target.Name}</b> plans to go out exactly as smoothly.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> finds Erin unable to defend herself and gulps the Nyangel down, pausing only to lick at her most sensitive parts before sending her to her destination.",
            targetRace: Race.Erin, priority: 26, conditional: (s) => Lewd(s) && HasGreatEscape(s)),
            new EventString((i) => $"Erin lets out a scream of horror as <b>{i.Unit.Name}</b> grabs her and shoves her into their mouth, slowly swallowing the nyangel down.",
            targetRace: Race.Erin, priority: 26, conditional: HasGreatEscape),
            new EventString((i) => $"Erin finds herself alone with <b>{i.Unit.Name}</b>, who quickly grabs the Nyangel and begins gulping her down.",
            targetRace: Race.Erin, priority: 26, conditional: HasGreatEscape),
            new EventString((i) => $"Erin, though normally terrified, makes no move to resist as <b>{i.Unit.Name}</b> begins to swallow her down.",
            targetRace: Race.Erin, priority: 26, conditional: (s) => Cursed(s) && HasGreatEscape(s)),
            new EventString((i) => $"As the {GetRaceDescSingl(i.Unit)}'s curse begins to set in to Erin's mind, the normally unwilling Nyangel makes no move to resist as <b>{i.Unit.Name}</b> begins to gulp her down, licking between her thighs easily without her normal resistance.",
            targetRace: Race.Erin, priority: 26, conditional: s => Cursed(s) && Lewd(s) && HasGreatEscape(s)),

        };

        RandomDigestionMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Target.Name}</b>’s struggles can be clearly seen inside <b>{i.Unit.Name}</b>’ transparent {i.preyLocation.ToSyn()}.",
            actorRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Target.Name}</b> is suspended inside <b>{i.Unit.Name}</b>, barely able to move.",
            actorRace: Race.Slimes, priority: 10),

            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} is fizzing with the angry slime.",
            targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} is emitting bubbling noises.",
            targetRace: Race.Slimes, priority: 9),

            new EventString((i) => $"<b>{i.Target.Name}</b> struggles within <b>{i.Unit.Name}</b>'s semi-translucent {i.preyLocation.ToSyn()}.",
            actorRace: Race.Vagrants, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s {i.preyLocation.ToSyn()} wiggles and wobbles with <b>{i.Target.Name}</b> struggling inside.",
            actorRace: Race.Vagrants, priority: 9),

            new EventString((i) => $"<b>{i.Target.Name}</b> struggles inside <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}, trying to avoid becoming fresh scat.",
            priority: 8, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> daydreams of taking a nice, big dump. {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} gurgles ominously with prey that has a long way to go.",
            priority: 8, conditional: Scat),

            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} gurgles louder, slathering <b>{i.Target.Name}</b> in digestive juices.",
            priority: 8, conditional: HardVoreInStomach),

            new EventString((i) => $"<b>{i.Unit.Name}</b> moans, and massages {GPPHis(i.Unit)} full {i.preyLocation.ToSyn()}",
            priority: 8),

            new EventString((i) => $"<b>{i.Target.Name}</b> explains that this isn’t how it’s supposed to work. <b>{i.Unit.Name}</b> stretches and purrs.",
            actorRace: Race.Cats, targetRace: Race.Dogs, priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> scratches the stomach walls. <b>{i.Unit.Name}</b> begins thumping {GPPHis(i.Unit)} leg.",
            actorRace: Race.Dogs, targetRace: Race.Cats, priority: 8),

            new EventString((i) => $"<b>{i.Unit.Name}</b> cries aloud as {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} angrily rumble{PluralForPart(i.preyLocation)} as it digests its first large meal",
            priority: 8, conditional: FirstTime),

            //Panther pred 
            new EventString((i) => $"<b>{i.Unit.Name}</b> doesn’t even seem to notice <b>{i.Target.Name}</b>’s desperate struggles, only acknowledging the fight transpiring within {GPPHim(i.Unit)} with a short burp as {GPPHis(i.Unit)} meal jostles {GPPHis(i.Unit)} gut side-to-side.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pats {GPPHis(i.Unit)} distended stomach gently and softly speaks, “Though you will soon die, worry not. Our spirits will become as one soon enough.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"A fierce attempt to escape the sweltering gut digesting {GPPHim(i.Target)} almost succeeds as <b>{i.Target.Name}</b> gets an arm up into the mouth. {Capitalize(GPPHis(i.Target))} hope is dashed however as <b>{i.Unit.Name}</b> clamps {GPPHis(i.Unit)} sharp fangs together, biting {GPPHis(i.Target)} hand and sending it reeling back into {GPPHis(i.Unit)} gullet.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"There is no escape from me,” <b>{i.Unit.Name}</b> says, digging a clawed paw into {GPPHis(i.Unit)} massive abdomen. “Your struggling only delays the inevitable.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"Fighting a warrior like me was your first blunder,” <b>{i.Unit.Name}</b> taunts {GPPHis(i.Unit)} gut as it pulsates with fading life. “Your second was entering my gut!But worry not, it won’t be something you repeat!",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $" Struggle less,” <b>{i.Unit.Name}</b> chides {GPPHis(i.Unit)} prey as {GPPHe(i.Target)} attempt{SIfSingular(i.Target)} to break free. “I don’t want you hurting yourself! A clean digestion will make for better bone trophies!",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> purrs in satisfaction as <b>{i.Target.Name}</b> sloshes about within {GPPHim(i.Unit)}.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> chants a silent prayer to {GPPHis(i.Unit)} ancestors to keep {GPPHis(i.Unit)} rebellious meal within {GPPHim(i.Unit)} until {GPPHis(i.Unit)} stomach can silence {GPPHis(i.Target)} for good.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> mewls pitifully as the superior cat’s stomach works {GPPHim(i.Target)} over, sloshing {GPPHim(i.Target)} back and forth.  <b>{i.Unit.Name}</b>’s muscled stomach barely even shifts as the weak cat person stews within.",
            actorRace: Race.Panthers, targetRace: Race.Cats, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is forced to clench {GPPHis(i.Unit)} stomach tightly as <b>{i.Target.Name}</b> roars and thrashes about within {GPPHim(i.Unit)}. Vicious looking bulges jut out from {GPPHis(i.Unit)} gut as the tiger warrior within attempts to claw {GPPHimself(i.Unit)} out.",
            actorRace: Race.Panthers, targetRace: Race.Tigers, priority: 9),

            //Panther prey
            new EventString((i) => $"<b>{i.Target.Name}</b> roars, struggling inside <b>{i.Unit.Name}</b>'s gut, not willing to submit to the inferior creature. <b>{i.Unit.Name}</b> mockingly repeats the roar, enjoying {GPPHis(i.Unit)} arrogant prey's struggles.",
            actorRace: Race.Cats, targetRace: Race.Panthers, priority: 9),

            new EventString((i) => $"The huge dragoness looses a belch, little air being left for <b>{i.Target.Name}</b> within <b>{i.Unit.Name}</b>'s belly.",
            priority: 9, actorRace: Race.Abakhanskya, conditional: InStomach),
            new EventString((i) => $"Teasingly running a paw over her fed gut, <b>{i.Unit.Name}</b> hums as <b>{i.Target.Name}</b> melts away.",
            priority: 9, actorRace: Race.Abakhanskya, conditional: InStomach),
            new EventString((i) => $"Drooling wildly, <b>{i.Unit.Name}</b> can't wait to devour more like <b>{i.Target.Name}</b>!",
            priority: 9, actorRace: Race.Abakhanskya, conditional: InStomach),

            new EventString((i) => $"<b>{i.Unit.Name}</b> moans in delight, feeling <b>{i.Target.Name}</b> melting away within her {i.preyLocation.ToSyn()}, knowing it won't be long until {GPPHe(i.Target)}'d be but another layer on her frame.",
            priority: 9, actorRace: Race.Abakhanskya),
            new EventString((i) => $"The digestive fluids closing in, <b>{i.Target.Name}</b> has to accept that {GPPHis(i.Target)} time will come to an end within a greedy <b>{i.Unit.Name}</b>'s {i.preyLocation.ToSyn()}.",
            priority: 9, actorRace: Race.Abakhanskya),


            new EventString((i) => $"<b>{i.Target.Name}</b> is humiliated by being consumed by a first timer.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> gets wrapped up in the experience of {GPPHis(i.Unit)} first prey, swinging {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} back and forth as {GPPHe(i.Unit)} quiver{SIfSingular(i.Unit)} in excitement.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> turns green and almost throws up as <b>{i.Target.Name}</b> moves in {GPPHis(i.Unit)} virgin belly.",
            priority: 9, conditional: (s) => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> proudly sticks out {GPPHis(i.Unit)} full {i.preyLocation.ToSyn()} to show off {GPPHis(i.Unit)} first prey to all {GPPHis(i.Unit)} friends.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> {GPPHis(i.Target)} pushes inside <b>{i.Unit.Name}</b>'s {i.preyLocation.ToSyn()}, causing inexperienced predator to fall over and stand up embarassed.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> inadvertently lets out a satisfied moan as {GPPHe(i.Unit)} experience{SIfSingular(i.Unit)} {GPPHis(i.Unit)} first truly full {i.preyLocation.ToSyn()}.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Target.Name}</b> struggles but <b>{i.Unit.Name}</b>’s virgin {i.preyLocation.ToSyn()} is surprisingly resilient.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> begs {GPPHis(i.Unit)} first prey to settle down, but <b>{i.Target.Name}</b> keeps struggling.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Target.Name}</b> demands to be let out of the newbie's {i.preyLocation.ToSyn()}.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> looks down at {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} with disbelief, watching as <b>{i.Target.Name}</b> moves around within.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hesitantly looks down at {GPPHis(i.Unit)} engorged {i.preyLocation.ToSyn()} and with trepidation rests a hand on it, feeling <b>{i.Target.Name}</b> shifting within.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> doesn’t really know what to do after {GPPHis(i.Unit)} first feeding. {GPPHe(i.Unit)} speaks softly to {GPPHis(i.Unit)} gurgling {i.preyLocation.ToSyn()} asking if <b>{i.Target.Name}</b> is still alive. <b>{i.Target.Name}</b> screams loudly, scaring the poor predator.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tries to catch {GPPHis(i.Unit)} breath as <b>{i.Target.Name}</b> fights against the new predator’s acids.",
            priority: 9, conditional: (s) => FirstTime(s) && HardVore(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> threatens to devour <b>{i.Unit.Name}</b> once {GPPHe(i.Target)} get out. The thought terrifies {GPPHim(i.Unit)} so {GPPHe(i.Unit)} firmly hold{SIfSingular(i.Unit)} {GPPHis(i.Unit)} shifting guts to keep {GPPHis(i.Unit)} agitated prey down.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> presses a hand into {GPPHis(i.Unit)} extended midsection and is surprised by just how soft it is.",
            priority: 9, conditional: (s) => FirstTime(s) && InStomachOrWomb(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hopes that finishing off <b>{i.Target.Name}</b> will make {GPPHis(i.Unit)} mentor proud.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wonders if {GPPHe(i.Unit)}’ll be beautiful like the older warriors after digesting {GPPHis(i.Unit)} first prey.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> and <b>{i.Target.Name}</b> both start to panic as neither are sure what to do.",
            priority: 9, conditional: FirstTime),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lets out a belch as {GPPHis(i.Unit)} gut sways back and forth. Resting a hand on {GPPHis(i.Unit)} upset tummy as {GPPHe(i.Unit)} digest{SIfSingular(i.Unit)} {GPPHis(i.Unit)} first meal, the rookie pred moans, “Ugh, I’m starting to think this was a bad idea. I feel like I’m going to pop.”",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s)),
            new EventString((i) => $"“I’m sorry, is that uncomfortable?” <b>{i.Unit.Name}</b> asks {GPPHis(i.Unit)} first prey as {GPPHe(i.Unit)} attempt{SIfSingular(i.Unit)} to rearrange {GPPHis(i.Unit)} meal to a more accommodating position. “Please be patient, this is my first time!”",
            priority: 9, conditional: s => FirstTime(s) && ActorHumanoid(s)),
            new EventString((i) => $"“O-oh! So this is what being a predator feels like!” <b>{i.Unit.Name}</b> declares as a smile crosses {GPPHis(i.Unit)} lips and {GPPHis(i.Unit)} hands sensually caress {GPPHis(i.Unit)} wobbling gut, “Yeah, I could get used to this!”",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"“Eh! Why are you so heavy!” <b>{i.Unit.Name}</b> asks {GPPHis(i.Unit)} meal as {GPPHis(i.Unit)} virgin belly tries its best to melt down its first prey. “I’m getting stretch marks from how much you’re making my gut sag! Come on!”",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"“Ugh, I hope this gets easier,” <b>{i.Unit.Name}</b> complains as {GPPHis(i.Unit)} gut bounces high before sinking down low into a deep sag as {GPPHis(i.Unit)} first prey fights for {GPPHis(i.Target)} life.",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"“Please be gentle with me,” <b>{i.Unit.Name}</b> shyly mumbles to {GPPHis(i.Unit)} trembling belly as the form of {GPPHis(i.Unit)} prey sloshes around beneath the skin, “It’s my first time.”",
            priority: 9, conditional: s => FirstTime(s) && InStomach(s) && ActorHumanoid(s)),


            new EventString((i) => $"<b>{i.Unit.Name}</b>’s stomach barely moves as <b>{i.Target.Name}</b> happily relaxes in {GPPHis(i.Target)} new home.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> desperately thanks {GPPHis(i.Target)} consumer for devouring one as unworthy as {GPPHimself(i.Target)}. <b>{i.Unit.Name}</b> gives a confused pat to the top of {GPPHis(i.Unit)} grateful {i.preyLocation.ToSyn()} in response.",
            priority: 9, conditional: Cursed),
            new EventString((i) => $"<b>{i.Target.Name}</b> lovingly massages <b>{i.Unit.Name}</b>’s tummy from the inside, giving the predator a unique experience as {GPPHis(i.Unit)} meal cares for {GPPHis(i.Unit)} guts.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> hopes that {GPPHeIsAbbr(i.Unit)} tasty enough. <b>{i.Unit.Name}</b>’s stomach gives a reassuring gurgle and flow of acids in response.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),

            new EventString((i) => $"“Oh Gods! Yes, digest me!” <b>{i.Target.Name}</b> screams out as {GPPHe(i.Target)} furiously play{SIfSingular(i.Target)} with {GPPHimself(i.Unit)} as <b>{i.Unit.Name}</b>’s stomach walls pulsate around {GPPHim(i.Target)} and digestive juices cover {GPPHis(i.Unit)} melting flesh. “I’m just food now! Agh this is hot!”",
            priority: 9, conditional: s => Cursed(s) && InStomach(s) && Lewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> rubs {GPPHis(i.Target)} chest and ass, slathering the digestive juices across {GPPHis(i.Target)} entire body, the sting exciting {GPPHim(i.Target)} to no end as the curse holds sway over {GPPHis(i.Target)} broken mind. {Capitalize(GPPHis(i.Target))} self-defeating actions ease the process of digestion for <b>{i.Unit.Name}</b> who gratefully pats the sides of {GPPHis(i.Unit)} sagging gut.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> screams out in ecstasy as the stomach wall around {GPPHim(i.Target)} convulses, pressing {GPPHim(i.Target)} into the acids within. “Please, don’t digest me!” the prey screams out, making <b>{i.Unit.Name}</b> wonder if the curse had worn off but {GPPHe(i.Target)} continue{SIfSingular(i.Target)}, alleviating the worry, “Keep me as your gut slut, Gods I never want to leave your belly!”",
            priority: 9, conditional: s => Cursed(s) && InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> pants heavily as {GPPHe(i.Unit)} tr{IesIfSingular(i.Target)} to elicit an orgasm but the motion of <b>{i.Unit.Name}</b>’s stomach, while exciting {GPPHim(i.Target)} even further prevents {GPPHim(i.Target)} from getting a good grip. The excited prey attempts to grab onto the slippery stomach lining, but keeps losing {GPPHis(i.Target)} grip and sloshing around in digestive juices.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s) && Lewd(s)),
            new EventString((i) => $"As <b>{i.Target.Name}</b> blissfully sloshes back and forth in the gut digesting {GPPHim(i.Target)}. {Capitalize(GPPHe(i.Target))} begin{SIfSingular(i.Target)} to rub {GPPHis(i.Target)} own belly, contemplating those that {GPPHe(i.Target)} {HasHave(i.Target)} also eaten. Their screams always made digestion seem terrifying, but now that {GPPHeIsAbbr(i.Target)} making a bulge on <b>{i.Unit.Name}</b>’s frame, the experience is better than anything {GPPHe(i.Target)} could have imagined.",
            priority: 9, conditional: s => Cursed(s) && InStomach(s)),


            new EventString((i) => $"<b>{i.Unit.Name}</b> lifts up {GPPHis(i.Unit)} stomach, swinging it side to side, making <b>{i.Target.Name}</b> dizzy.",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Target.Name}</b> almost gets back up but <b>{i.Unit.Name}</b> grabs {GPPHis(i.Target)} face as {GPPHe(i.Target)} reach{EsIfSingular(i.Target)} the mouth and push{EsIfSingular(i.Unit)} {GPPHim(i.Target)} back down.",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Unit.Name}</b> punches {GPPHis(i.Unit)} stomach to stop {GPPHis(i.Unit)} meal’s squirming.",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Unit.Name}</b> asks <b>{i.Target.Name}</b> to move a bit as {GPPHeIsAbbr(i.Target)} giving {GPPHim(i.Unit)} indigestion",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"With a violent tummy wobble <b>{i.Unit.Name}</b> loses {GPPHis(i.Unit)} balance for a moment",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pokes {GPPHis(i.Unit)} belly and asks if <b>{i.Target.Name}</b> is still alive",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s face appears through <b>{i.Unit.Name}</b>’s belly for a moment but vanishes as <b>{i.Unit.Name}</b> shifts {GPPHis(i.Unit)} weight.",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wonders if {GPPHe(i.Unit)} too will end up in a belly one day and tries to soothe {GPPHis(i.Unit)} wailing stomach",
            priority: 8, conditional: InStomachOrWomb),
            new EventString((i) => $"<b>{i.Target.Name}</b> manages to get an arm back up the esophagus but <b>{i.Unit.Name}</b> suddenly swallows hard, forcing {GPPHim(i.Target)} back down curled into a ball at the bottom of {GPPHis(i.Unit)} belly.",
            priority: 8, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s tummy bounces furiously as <b>{i.Target.Name}</b> tickles {GPPHim(i.Unit)} from within, bringing a smile to {GPPHis(i.Unit)} face.",
            priority: 8, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> uses both hands to squeeze {GPPHis(i.Unit)} stomach tracing out <b>{i.Target.Name}</b>’s form through {GPPHis(i.Unit)} churning stomach walls.",
            priority: 8, conditional: InStomach),
            new EventString((i) => $"<b>{i.Target.Name}</b> sees an ominous bulge press inwards as <b>{i.Unit.Name}</b> pushes a hand firmly against {GPPHis(i.Unit)} wobbling belly.",
            priority: 8, conditional: InStomach),

            new EventString((i) => $"<b>{i.Target.Name}</b> barely struggles and begins crying after being betrayed by <b>{i.Unit.Name}</b>.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Target.Name}</b> accusations of betrayal are masked by <b>{i.Unit.Name}</b>’s gurgling {i.preyLocation.ToSyn()}!",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Target.Name}</b> screams for someone to help {GPPHim(i.Target)} but {GPPHis(i.Target)} screams are indistinguishable from a digesting enemy.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> feels for {GPPHis(i.Unit)} digesting ally through {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} walls, but can’t tell the difference between {GPPHim(i.Target)} and {GPPHis(i.Unit)} enemies.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> laughs as <b>{i.Target.Name}</b> demands to know why and reminds {GPPHis(i.Unit)} former ally that a meal’s a meal.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> can’t stand to look at {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} as it finishes off {GPPHis(i.Unit)} comrade.",
            priority: 8, conditional: Friendly),
           
            //Bodyparts - Thigh fat - Tail fat - thickening {GPPHis(i.Unit)} tentacles - drooping down {GPPHis(i.Unit)} midsection - any else?
            new EventString((i) => $"<b>{i.Unit.Name}</b> realizes what {GPPHe(i.Unit)} {HasHave(i.Unit)} done and tries to expel {GPPHis(i.Unit)} ally but it’s too late, <b>{i.Target.Name}</b> is already thigh fat.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"Pushing {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} with the hilt of {GPPHis(i.Unit)} weapon, <b>{i.Unit.Name}</b> accuses <b>{i.Target.Name}</b> of being too weak.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Target.Name}</b> understands and submits {GPPHimself(i.Target)} to becoming nutrients for {GPPHis(i.Target)} ally.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> assures <b>{i.Target.Name}</b> that {GPPHe(i.Target)} shall better serve the empire as {GPPHis(i.Unit)} meal than as a warrior.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Target.Name}</b> laughs at the joke but begins to panic as <b>{i.Unit.Name}</b> doesn’t laugh and acids begin to pour in.",
            priority: 8, conditional: (s) => Friendly(s) && HardVore(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hides {GPPHis(i.Unit)} traitorous {i.preyLocation.ToSyn()} from {GPPHis(i.Unit)} allies as <b>{i.Target.Name}</b> screams for help.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wonders where <b>{i.Target.Name}</b> went and starts to look for {GPPHim(i.Target)}, forgetting that {GPPHeIsAbbr(i.Target)} swinging just in front of {GPPHim(i.Unit)}.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wonders what {GPPHe(i.Unit)}’ll tell <b>{i.Target.Name}</b>’s family as {GPPHe(i.Target)} still struggle{SIfSingular(i.Target)} inside {GPPHis(i.Unit)} wobbling midsection.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> cruelly laughs, resting a hand on {GPPHis(i.Unit)} engorged {i.preyLocation.ToSyn()}. {GPPHe(i.Unit)} had been waiting for this moment for a long time.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shivers as <b>{i.Target.Name}</b> struggles. {GPPHe(i.Unit)} never knew treason tasted so good.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> laughs. With <b>{i.Target.Name}</b> out of the way, everything will become much easier for {GPPHim(i.Unit)}.",
            priority: 8, conditional: Friendly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tries {GPPHis(i.Unit)} best to get <b>{i.Target.Name}</b> out, but can’t manage it.",
            priority: 8, conditional: Friendly),


            new EventString((i) => $"<b>{i.Target.Name}</b> kicks <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} angrily.",
            priority: 8),
            new EventString((i) => $"Not to be defeated so easily, <b>{i.Target.Name}</b> struggles with all {GPPHis(i.Target)} might.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} quiver{PluralForPart(i.preyLocation)} and shake{PluralForPart(i.preyLocation)}, as <b>{i.Target.Name}</b> tries to make {GPPHim(i.Unit)} {i.preyLocation.ToVerb()} {GPPHim(i.Target)} out.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> shuffles inside <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}.",
            priority: 8),
            new EventString((i) => $"Angry protests of <b>{i.Target.Name}</b> can be heard from within <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> squirms and cries for help within <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}.",
            priority: 8),
            //"They" for balls/breasts - "It" for belly/womb, also rumble/rumbles
            //new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} rumble{PluralForPart(i.preyLocation)} as it greets fresh prey, eager for more.",
            //priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} loudly churn{PluralForPart(i.preyLocation)}, getting started on digesting <b>{i.Target.Name}</b>.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> groans quietly when {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} lets out a mild gurgle of indigestion, with <b>{i.Target.Name}</b> yet to settle in.",
            priority: 8, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} wobble{PluralForPart(i.preyLocation)} vigorously as <b>{i.Target.Name}</b> is trying to escape.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> starts feeling dizzy inside <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s struggles are weakening somewhat.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> might not see the light of day again if <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} continues to be left in charge of {GPPHim(i.Target)}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} contract{PluralForPart(i.preyLocation)} around <b>{i.Target.Name}</b>, steadily digesting {GPPHis(i.Unit)} meal.",
            priority: 8),
            new EventString((i) => $"All this digestion is making <b>{i.Unit.Name}</b> thirsty. {GPPHe(i.Unit)} hopes there will be water nearby after the battle.",
            priority: 8, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> urrrps, {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} tightening around <b>{i.Target.Name}</b>.",
            priority: 8, conditional: (s) => InStomach(s) && CanBurp(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> is really starting to feel the pressure from <b>{i.Unit.Name}</b>'s {i.preyLocation.ToSyn()}, the {i.preyLocation.ToSyn()} lewdly churning over the { GetRaceDescSingl(i.Target)}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} wobble{PluralForPart(i.preyLocation)} as <b>{i.Target.Name}</b> still tries to get out.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> enjoys the added weight of <b>{i.Target.Name}</b> in {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} contract{PluralForPart(i.preyLocation)} around <b>{i.Target.Name}</b>, trying to squish {GPPHim(i.Target)}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slaps {GPPHis(i.Unit)} full {i.preyLocation.ToSyn()}, taunting the prey inside.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} let{PluralForPart(i.preyLocation)} out a satisfied gurgle as it works.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans softly while {GPPHis(i.Unit)} stuffed {i.preyLocation.ToSyn()} rumble{PluralForPart(i.preyLocation)} ominously.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s knees buckle for a moment as <b>{i.Target.Name}</b> struggles.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> says {GPPHe(i.Target)} {HasHave(i.Target)} a {GetRandomStringFrom("daughter", "son", "family")} back home and begs to be let out. <b>{i.Unit.Name}</b> apologizes and rubs {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}.",
            priority: 8, conditional: HardVore),
            new EventString((i) => $"<b>{i.Target.Name}</b> struggles inside <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tells <b>{i.Target.Name}</b> that {GPPHe(i.Unit)} didn’t want this. <b>{i.Target.Name}</b> is not consoled.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> holds out hope, knowing that {GPPHis(i.Target)} fellow warriors will free {GPPHim(i.Target)} soon.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> offers <b>{i.Unit.Name}</b> a bribe. <b>{i.Unit.Name}</b> considers it for a moment but politely declines.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> asks <b>{i.Target.Name}</b> if {GPPHeIsAbbr(i.Unit)} eaten {GPPHim(i.Target)} before. <b>{i.Target.Name}</b> says {GPPHe(i.Unit)} do{EsIfSingular(i.Unit)}n’t think so.",
            priority: 8, conditional: (s) => s.Target.HasTrait(Traits.Eternal)),
            new EventString((i) => $"<b>{i.Target.Name}</b> promises that {GPPHe(i.Target)} will eat <b>{i.Unit.Name}</b> once {GPPHe(i.Target)} escapes. <b>{i.Unit.Name}</b> laughs and pats {GPPHis(i.Unit)} belly.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> winces in pain as <b>{i.Target.Name}</b> tries to claw {GPPHis(i.Target)} way out.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s struggles excite <b>{i.Unit.Name}</b>.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> thanks <b>{i.Target.Name}</b> for being {GPPHis(i.Unit)} meal.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> rubs {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} trying to alleviate the indigestion from <b>{i.Target.Name}</b>’s fighting.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s screams are covered by a loud gurgle from <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> squeezes<b>{i.Target.Name}</b>’s genitals, boasting that it all belongs to {GPPHim(i.Unit)} now.",
            priority: 8, conditional: Lewd),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hopes<b>{i.Target.Name}</b> will go to all the right places.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hates the feeling of <b>{i.Target.Name}</b> inside {GPPHim(i.Unit)} but keeps {GPPHim(i.Target)} down anyway.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> says this was all part of {GPPHis(i.Target)} plan <b>{i.Unit.Name}</b> is confused.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> demands that <b>{i.Target.Name}</b> digest quickly.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> begs <b>{i.Unit.Name}</b> to just end it but only receives a cruel laugh in response.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans as <b>{i.Target.Name}</b> massages {GPPHim(i.Unit)} with {GPPHis(i.Target)} struggles.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tells <b>{i.Target.Name}</b> to be thankful as {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} {IsAre(i.Unit)} far more gentle than {GPPHis(i.Unit)} friends.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> kicks at the walls of the {i.preyLocation.ToSyn()} confining {GPPHim(i.Target)}, sending ripples of pleasure across <b>{i.Unit.Name}</b>'s body.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s struggling gives <b>{i.Unit.Name}</b> indigestion.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lovingly wraps {GPPHis(i.Unit)} arms around {GPPHis(i.Unit)} midsection, gently squeezing <b>{i.Target.Name}</b> as {GPPHe(i.Target)} melt{SIfSingular(i.Target)} away.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> angrily punches {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} as <b>{i.Target.Name}</b>’s struggling persists.",
            priority: 8),

            new EventString((i) => $"<b>{i.Unit.Name}</b> scratches the top of {GPPHis(i.Unit)} engorged tummy, licking {GPPHis(i.Unit)} lips while looking at <b>{AttractedWarrior(i.Unit).Name}</b>.",
            priority: 8, conditional: ReqOSWStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> imagines pressing {GPPHimself(i.Unit)} against <b>{AttractedWarrior(i.Unit).Name}</b> as <b>{i.Target.Name}</b> struggles between them.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> blushes when <b>{AttractedWarrior(i.Unit).Name}</b> stares at {GPPHis(i.Unit)} flailing stomach.",
            priority: 8, conditional: ReqOSWStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wonders if {GPPHe(i.Unit)} should ask <b>{AttractedWarrior(i.Unit).Name}</b> to come play with {GPPHis(i.Unit)} new assets after the battle.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hopes that <b>{i.Target.Name}</b> will go to all the right places so that <b>{AttractedWarrior(i.Unit).Name}</b> will finally notice {GPPHim(i.Unit)}!",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> smirks when {GPPHis(i.Unit)} catch{EsIfSingular(i.Unit)} <b>{AttractedWarrior(i.Unit).Name}</b> glancing at {GPPHis(i.Unit)} thrashing gut.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tries to get {GPPHis(i.Unit)} wobbling belly to settle down so that <b>{AttractedWarrior(i.Unit).Name}</b> doesn’t notice.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> waves at <b>{AttractedWarrior(i.Unit).Name}</b> while thrusting {GPPHis(i.Unit)} stomach out, hoping <b>{GPPHis(AttractedWarrior(i.Unit))}</b> recognize how virile {GPPHeIs(i.Unit)}.",
            priority: 8, conditional: (s) => ReqOSWBelly(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> asks <b>{AttractedWarrior(i.Unit).Name}</b> to rub {GPPHis(i.Unit)} belly so {GPPHe(i.Unit)} won’t get indigestion.",
            priority: 8, conditional: ReqOSWStomach),
            new EventString((i) => $"As <b>{i.Unit.Name}</b>’s insides are rearranged by {GPPHis(i.Unit)} most recent meal {GPPHe(i.Unit)} can’t help but think about thrashing about with <b>{AttractedWarrior(i.Unit).Name}</b>.",
            priority: 8, conditional: ReqOSWLewd),
            new EventString((i) => $"<b>{i.Unit.Name}</b> rests {GPPHis(i.Unit)} elbows on {GPPHis(i.Unit)} packed gut and admires <b>{AttractedWarrior(i.Unit).Name}</b>.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> looks down at {GPPHis(i.Unit)} packed belly and wishes that <b>{AttractedWarrior(i.Unit).Name}</b> would fill {GPPHim(i.Unit)} in other ways.",
            priority: 8, conditional: s => ReqOSWBelly(s) && s.Unit.GetGender() == Gender.Female && Lewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sees <b>{CompetitionWarrior(i.Unit).Name}</b> flirting with <b>{AttractedWarrior(i.Unit).Name}</b> and thinks about cramming {GPPHim(CompetitionWarrior(i.Unit))} in {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} alongside <b>{i.Target.Name}</b>.",
            priority: 8, conditional: ReqSSWAndOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> strikes a suggestive pose with {GPPHis(i.Unit)} belly swaying to get <b>{AttractedWarrior(i.Unit).Name}</b>’s attention.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wants nothing more than to ravage <b>{AttractedWarrior(i.Unit).Name}</b> like {GPPHis(i.Unit)} acids are ravaging <b>{i.Target.Name}</b>!",
            priority: 8, conditional: s =>ReqOSWLewd(s) && HardVore(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> thinks about using {GPPHis(i.Unit)} newfound weight to pin <b>{AttractedWarrior(i.Unit).Name}</b> to the ground and have {GPPHis(i.Unit)} way with {GPPHim(AttractedWarrior(i.Unit))}, but decides not to.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wishes {GPPHeWas(i.Unit)} as close to <b>{AttractedWarrior(i.Unit).Name}</b> as {GPPHeWas(i.Unit)} with the prey in {GPPHis(i.Unit)} <b>{i.Target.Name}</b>-stuffed guts. But with less digesting and death.",
            priority: 8, conditional: s => ReqOSWLewd(s) && HardVore(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slaps {GPPHis(i.Unit)} tummy and says with confidence \"<b>{i.Target.Name}</b>’s not the only thing I can fit in me!\" <b>{AttractedWarrior(i.Unit).Name}</b> blushes and looks away.",
            priority: 8, conditional: s => ReqOSWBelly(s) && Lewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> looks at {GPPHis(i.Unit)} bubbling, shifting tummy and hopes that <b>{AttractedWarrior(i.Unit).Name}</b> doesn’t think {GPPHe(i.Unit)} look{SIfSingular(i.Unit)} fat.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"Watching <b>{AttractedWarrior(i.Unit).Name}</b>, <b>{i.Unit.Name}</b> gives a longing sigh and asks {GPPHis(i.Unit)} pulsating stomach for love advice. <b>{i.Target.Name}</b>’s mouth is too full of acids to give a proper answer.",
            priority: 8, conditional: ReqOSWStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> begs <b>{i.Target.Name}</b> to hurry and digest so {GPPHis(i.Unit)} new assets can impress <b>{AttractedWarrior(i.Unit).Name}</b>.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"Even though <b>{i.Target.Name}</b> fills {GPPHis(i.Unit)} belly, <b>{i.Unit.Name}</b> still feels empty without <b>{AttractedWarrior(i.Unit).Name}</b>’s attentions.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"<b>{i.Unit.Name}</b> feels queasy as {GPPHis(i.Unit)} stomach gurgles angrily; {GPPHe(i.Unit)} hates the feeling of a live meal, but gladly does it to protect <b>{AttractedWarrior(i.Unit).Name}</b>.",
            priority: 8, conditional: ReqOSWStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sees <b>{CompetitionWarrior(i.Unit).Name}</b> proudly displaying {GPPHis(CompetitionWarrior(i.Unit))} body to <b>{AttractedWarrior(i.Unit).Name}</b>. <b>{i.Unit.Name}</b>’s belly starts to thrash wildly as <b>{i.Target.Name}</b> tries to escape the rage induced digestion.",
            priority: 8, conditional: s => ReqSSWAndOSW(s) && ReqOSWBelly(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hates the taste of {GetRaceDescSingl(i.Target)} but knows they always melt into all the places <b>{AttractedWarrior(i.Unit).Name}</b> likes so {GPPHe(i.Unit)} pats {GPPHis(i.Unit)} stomach with content.",
            priority: 8, conditional: ReqOSWStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b> heard that <b>{AttractedWarrior(i.Unit).Name}</b> likes females with big breasts. So, {GPPHe(i.Unit)} starts pushing her stomach upward, hoping that it will encourage the fat to go to her tits.",
            priority: 8, conditional: (s) => ReqOSWBelly(s) && s.Unit.HasBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is extremely jealous of <b>{AttractedWarrior(i.Unit).Name}</b>’s stunning body and hopes <b>{i.Target.Name}</b> will make {GPPHis(i.Unit)} {(i.Unit.HasBreasts ? "breasts" : "muscles")} bigger. ",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"As {GPPHis(i.Unit)} belly lets out an intimidating roar, <b>{i.Unit.Name}</b> wonders why <b>{AttractedWarrior(i.Unit).Name}</b> is so tense around {GPPHim(i.Unit)}.",
            priority: 8, conditional: ReqOSWBelly),
            new EventString((i) => $"<b>{AttractedWarrior(i.Unit).Name}</b> is terrified of becoming a layer of fat on <b>{i.Unit.Name}</b>. Meanwhile, <b>{i.Unit.Name}</b> just knows that this digestion will be the one to finally entice <b>{AttractedWarrior(i.Unit).Name}.</b>",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"<b>{i.Unit.Name}</b> licks the top of {GPPHis(i.Unit)} wobbling belly with {GPPHis(i.Unit)} tongue, trying to entice <b>{AttractedWarrior(i.Unit).Name}</b>, whom goes pale as {GPPHe(AttractedWarrior(i.Unit))} mistake{SIfSingular(AttractedWarrior(i.Unit))} the gesture as a foreboding threat!",
            priority: 8, conditional: ReqOSWBelly),
            //Panther flirt
            new EventString((i) => $"<b>{i.Unit.Name}</b> knows that it is often best to hunt alone, but {GPPHe(i.Unit)} can’t help but accompany <b>{AttractedWarrior(i.Unit).Name}</b> wherever {GPPHe(AttractedWarrior(i.Unit))} go{EsIfSingular(AttractedWarrior(i.Unit))}.",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $"As {GPPHis(i.Unit)} meal squirms within {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}{PluralForPart(i.preyLocation)}, <b>{i.Unit.Name}</b> catches <b>{AttractedWarrior(i.Unit).Name}</b> staring at {GPPHim(i.Unit)} just as {GPPHe(i.Unit)} finishes licking {GPPHis(i.Unit)} paw clean. In a bit of a panic, {GPPHe(i.Unit)} flashes a toothy grin. Surprisingly {GPPHe(AttractedWarrior(i.Unit))} do{EsIfSingular(AttractedWarrior(i.Unit))}n’t seem all that scared and even smile back!",
            priority: 8, conditional: ReqOSW),
            new EventString((i) => $" Displaying {GPPHis(i.Unit)} hunting prowess, <b>{i.Unit.Name}</b> arrogantly struts about, displaying {GPPHis(i.Unit)} full tum to everyone watching. {GPPHis(i.Unit)} becomes somewhat nervous however when <b>{AttractedWarrior(i.Unit).Name}</b> begins watching but is encouraged as {GPPHe(AttractedWarrior(i.Unit))} too congratulate{SIfSingular(AttractedWarrior(i.Unit))} {GPPHim(i.Unit)} on the catch!",
            priority: 8, conditional: ReqOSW),

            new EventString((i) => $"<b>{i.Unit.Name}</b>’s breasts wobble vigorously as <b>{i.Target.Name}</b> attempts to escape {GPPHis(i.Target)} prison.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> rubs {GPPHis(i.Unit)} breast, trying to alleviate the soreness from absorbing <b>{i.Target.Name}</b>.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s rubs {GPPHis(i.Unit)} back as her breasts jiggle as {GPPHis(i.Unit)} prey fights for {GPPHis(i.Target)} life.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grips {GPPHis(i.Unit)} now engorged breasts in both hands and squeezes them, forcing a bit of fatty milk to trickle from {GPPHis(i.Unit)} nipples.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s pinches {GPPHis(i.Unit)} sore nipples as the essence of her most recent prey fills {GPPHis(i.Unit)} tits with creamy milk.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> begins to lactate as {GPPHis(i.Unit)} prey begins to break down, the process excites {GPPHis(i.Unit)} and {GPPHe(i.Unit)} can’t help but suckle {GPPHis(i.Unit)} own tits.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans as {GPPHis(i.Unit)} breasts heave. The struggles of {GPPHis(i.Unit)} prey dying inside of {GPPHis(i.Unit)} arouses the busty warrior to no end.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> finally feels like a true woman with her flourishing bosom.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s breasts become so laden with prey that every step becomes a struggle, {GPPHis(i.Unit)} can barely move with the swell of life struggling within her.",
            priority: 8, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wishes that <b>{AttractedWarrior(i.Unit).Name}</b> would come over and hold {GPPHis(i.Unit)} breasts as {GPPHe(i.Unit)} break{SIfSingular(i.Unit)} down {GPPHis(i.Unit)} prey.",
            priority: 8, conditional: s => InBreasts(s) && ReqOSW(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> smacks {GPPHis(i.Unit)} breasts to make the struggling prey within settle down, but {GPPHis(i.Unit)} breasts are overly sensitive as they absorb <b>{i.Target.Name}</b>, causing {GPPHis(i.Unit)} to let out a shocked gasp.",
            priority: 8, conditional: InBreasts),

            new EventString((i) => $"Slowly turning an opponent into nothing more than a slushie of meat and bile inside of {GPPHim(i.Unit)} really turns <b>{i.Unit.Name}</b> on. {Capitalize(GPPHe(i.Unit))} begin fingering {GPPHimself(i.Unit)}. The new wave of movement results in panicked screams from within {GPPHis(i.Unit)} stomach as <b>{i.Target.Name}</b> is covered in digestive juices.",
            priority: 8, conditional: s => Lewd(s) && HardVore(s) ),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s futile attempts to free {GPPHimself(i.Target)} from an acidic fate lights a fire in {GPPHis(i.Target)} predator’s loins. <b>{i.Unit.Name}</b>, in a flurry of passion places {GPPHis(i.Unit)} thrashing bulge of a gut against {GPPHis(i.Unit)} most sensitive parts.",
            priority: 8, conditional: s => Lewd(s) && InStomachOrWomb(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> knows that the only way to increase {GPPHis(i.Unit)} attractiveness is for the struggling meal within {GPPHis(i.Unit)} curvaceous gut to fill {GPPHim(i.Unit)} out. Both to encourage growth and derive pleasure, {GPPHe(i.Unit)} begins to massage {GPPHis(i.Unit)} {(i.Unit.HasDick ? "dick" : "breasts")}.",
            priority: 8, conditional: s => Lewd(s) && InStomachOrWomb(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> uses {GPPHis(i.Target)} entire body to thrash and struggle against <b>{i.Unit.Name}</b>’s stomach walls. The predator lets out a scream of pleasure and has to steady {GPPHimself(i.Unit)}. Thankfully {GPPHis(i.Unit)} sagging gut covers {GPPHis(i.Unit)} now moistened nethers.",
            priority: 8, conditional: s => Lewd(s) && InStomach(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> tenses his gut a little and says: \"Settle down in there already! The only noise I want to hear from you is a series of blunt splatting sounds when I shit you out.\"",
            priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tenses his gut a little and says: \"Settle down in there already! The only way I want to notice you is as noise under my tail.\"",
            priority: 9, conditional: s => InStomach(s) && Farts(s) && ActorTail(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> winces as {GPPHis(i.Unit)} gut, riddled with handprints and strained bulges, attempts to break down <b>{i.Target.Name}</b> who still fights from within {GPPHim(i.Unit)}. \"Please, settle down!\" {GPPHe(i.Unit)} says, attempting to smooth {GPPHis(i.Unit)} swaying stomach, \"It’ll be over much sooner if you just stop fighting!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Oh my!\" <b>{i.Unit.Name}</b> exclaims as {GPPHis(i.Unit)} belly wobbles as <b>{i.Target.Name}</b> delivers a powerful kick against the stomach’s walls. \"You’re so strong!\" {GPPHe(i.Unit)} says coyly with an admiring smack against {GPPHis(i.Unit)} bouncing gut. \"Just not strong enough.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Ugh, stop moving already!\" <b>{i.Unit.Name}</b> shouts with distain at {GPPHis(i.Unit)} barely wobbling gut. \"You were a pathetic warrior and a pathetic meal! The best thing you can do now is just settle in and accept your fate.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"You were hard to get down, I’ll give you that,\" <b>{i.Unit.Name}</b> says to {GPPHis(i.Unit)} midsection, sagging low from the weight of the living meal within. \"But it’s about time you become part of a real warrior,\" {GPPHe(i.Unit)} says proudly rubbing {GPPHis(i.Unit)} rounded gut. ",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"You better not go to my thighs,\" <b>{i.Unit.Name}</b> warns {GPPHis(i.Unit)} meal as it thrashes about inside of {GPPHim(i.Unit)}. \"I’m trying to maintain a certain figure,\" {GPPHe(i.Unit)} taunts as {GPPHe(i.Unit)} run{SIfSingular(i.Unit)} {GPPHis(i.Unit)} hands across {GPPHis(i.Unit)} chest and bulging gut.",
            priority: 8, conditional:s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"I thought your race was supposed to be intimidating, not delicious!\" <b>{i.Unit.Name}</b> teases {GPPHis(i.Unit)} meal followed by a burp, \"But hey, I’m not complaining!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"You thought you were so special,\" <b>{i.Unit.Name}</b> states with disdain at the swaying gut hanging off {GPPHim(i.Unit)}, \"But now you’re nothing more than food. But don’t worry,\" {GPPHe(i.Unit)} chuckles as a cruel smile crosses {GPPHis(i.Unit)} lips, \"You’ll be hot shit soon enough!\"",
            priority: 8, conditional: s => InStomach(s) && Scat(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Once you soften up, I’m going to let <b>{AttractedWarrior(i.Unit).Name}</b> use my belly as a pillow,\" <b>{i.Unit.Name}</b> states as {GPPHis(i.Unit)} prey makes {GPPHis(i.Unit)} enticing gut wobble.",
            priority: 8, conditional: s => ReqOSWBelly(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"A warrior must keep their body strong,\" <b>{i.Unit.Name}</b> declares with a smile as {GPPHis(i.Unit)} hands caress {GPPHis(i.Unit)} prey filled gut, \"Your meat will be very useful to me.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"I think it’s about time I found someplace to rest and digest, what do you think?\" <b>{i.Unit.Name}</b> questions {GPPHis(i.Unit)} writhing belly. A response comes but the words are muffled by {GPPHis(i.Unit)} stomach walls and the roars of digestion. \"I’m glad you agree!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"O-Oh my!\" <b>{i.Unit.Name}</b> exclaims as {GPPHis(i.Unit)} sagging belly begins to sway back and forth with muffled lust filled moans as <b>{i.Target.Name}</b> pleasures {GPPHimself(i.Target)}. \"You’re a naughty one, aren’t you!\"",
            priority: 8, conditional: s => InStomach(s) && Lewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"You know,\" <b>{i.Unit.Name}</b> idly muses while stroking {GPPHis(i.Unit)} jiggling belly, \"It’s hard to feel lonely when you’ve got someone so close by.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Ha! You thought you could beat me?\" <b>{i.Unit.Name}</b> proclaims as {GPPHis(i.Unit)} stomach shakes violently from the agitated foe within. \"Well, enjoy my acids for as long as you have left! You’re nothing but pudge in the making now!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"Despite being eaten <b>{i.Target.Name}</b> thrashes wildly, causing {GPPHis(i.Target)} devourer to begin burping as {GPPHis(i.Target)} struggles bring on indigestion. \"Ugh, maybe if you fought this hard before I ate you the tables would be turned right now!\" <b>{i.Unit.Name}</b> states, wincing from {GPPHis(i.Unit)} meal’s efforts.",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"I know it may not be the best for you,\" <b>{i.Unit.Name}</b> says, rubbing {GPPHis(i.Unit)} rounded gut as {GPPHis(i.Unit)} prey settles in, \"but I really do hope you enjoy your stay in my belly!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Any last words?\" <b>{i.Unit.Name}</b> asks {GPPHis(i.Unit)} shifting gut as bulges appear and disappear from the struggles within. The only response {GPPHe(i.Unit)} get{SIfSingular(i.Unit)} being a prolonged, muffled scream. \"I suppose not.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Fighting as much as you are almost makes it seem that you think you have any chance of escaping, but you don’t.\" <b>{i.Unit.Name}</b> states very plainly to {GPPHis(i.Unit)} wobbling gut as {GPPHis(i.Unit)} prey tries {GPPHis(i.Target)} best to escape. \"You should probably stop embarrassing yourself and just give up. Or at least try and make this pleasurable for me.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"<b>{AttractedWarrior(i.Unit).Name}</b> better appreciate how big I’m going to get from digesting you or I swear I’m going to flatten {GPPHim(AttractedWarrior(i.Unit))}.\" <b>{i.Unit.Name}</b> says lifting {GPPHis(i.Unit)} belly with both hands as {GPPHis(i.Unit)} prey sloshes about inside.",
            priority: 8, conditional: s => ReqOSWBelly(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"I’ve got to say, having a big belly feels amazing and it’s all thanks to you,\" <b>{i.Unit.Name}</b> says smiling and running {GPPHis(i.Unit)} hands across {GPPHis(i.Unit)} humungous gut, \"It almost makes me not want to digest you… almost.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"If it makes the acids sting a little less, you were absolutely delicious!\" <b>{i.Unit.Name}</b> compliments {GPPHis(i.Unit)} still living meal as {GPPHis(i.Unit)} bulging gut shifts from the fight going on within.",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b>’s ravenous tummy lets out a low gurgle, the predator speaks to the furious prey within {GPPHis(i.Unit)} stomach, \"Hey, look at the bright side! At least I didn’t chew,\" {GPPHe(i.Unit)} says trying to contain the bulges appearing on the surface of {GPPHis(i.Unit)} belly, \"you’ve got a bit more time before you’re gone!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"A horrified scream emits from <b>{i.Unit.Name}</b>’s sagging belly as it swings back and forth. The predator laughs and rubs the top of {GPPHis(i.Unit)} gut, \"You can yell all you want. Nobody’s going to hear you form inside there.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Ahh… yeah, move just like that. Keep struggling in there!\" <b>{i.Unit.Name}</b> shouts, encouraging {GPPHis(i.Unit)} wiggling prey to keep moving as {GPPHe(i.Unit)} pinches {GPPHis(i.Unit)} nipples, almost drawing blood as {GPPHis(i.Unit)} belly bounces. \"Oh yeah! I-I’m so C-Close! Aaah!\"",
            priority: 8, conditional: s => InStomach(s) && Lewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"With a heavy breath and a hoisting of {GPPHis(i.Unit)} overstuffed gut, <b>{i.Unit.Name}</b> stands ready to continue fighting, \"Rest assured, eating you wasn’t what I wanted. But killing you is my responsibility.\" {GPPHis(i.Unit)} stomach gurgles angrily as it starts to break down the living meal within, \"Digestion is my duty.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"As {GPPHis(i.Unit)} stomach bounces and sways from {GPPHis(i.Unit)} meal’s desperate attempt to escape, <b>{i.Unit.Name}</b> talks to the prey melting filling {GPPHim(i.Unit)}, \"Don’t worry, my innards will make short work of you and you’ll soon be a part of my wonderful figure.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Tell you what,\" <b>{i.Unit.Name}</b> says to the prey making a bulge in {GPPHis(i.Unit)} stomach, \"If you can last for a few hours in there I’ll let you out! If not…\" {GPPHe(i.Unit)} grab{SIfSingular(i.Unit)} the sides of {GPPHis(i.Unit)} full belly and shake{SIfSingular(i.Unit)} it side to side, disorienting its contents, \"You’ll make my ass bigger!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"A violent kick form within sends ripples across <b>{i.Unit.Name}</b>’s curvaceous gut bringing a belch and pained sigh from {GPPHis(i.Unit)} lips, \"Come on now, there’s no shame in losing. You fought well, but that’s over now. So stop embarrassing yourself and just let this happen. This is no easy task for me either, I imagine I’ll have indigestion for hours because of you!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"After another futile attempt to break free, the prey inside <b>{i.Unit.Name}</b> breaks down and begins to cry. \"Hey now, no need for that. Don’t worry,\" {GPPHe(i.Unit)} says as {GPPHe(i.Unit)} press{EsIfSingular(i.Unit)} {GPPHis(i.Unit)} hands into {GPPHis(i.Unit)} sobbing belly, \"after I’m done digesting you we’ll go to your home and reunite the whole family, doesn’t that sound nice?\" With that, {GPPHis(i.Unit)} belly returns to a mess of ripples and bulges.",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"Ugh, why do I keep doing this to myself?\" <b>{i.Unit.Name}</b> asks {GPPHis(i.Unit)} ominously rumbling gut as it sags from the heavy prey inside. \"I’m supposed to be on a diet but instead of a healthy meal I eat a fat idiot like you instead!\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"You must be lonely in there!\" <b>{i.Unit.Name}</b> says to the prey making {GPPHis(i.Unit)} gut jiggle, \"Don’t worry, we’ll find you a friend very soon, I’m sure of it.\"",
            priority: 8, conditional: s => InStomach(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> tells <b>{i.Target.Name}</b> that {GPPHe(i.Unit)}'ll enjoy spraying {GPPHis(i.Target)} remains all over the ground",
            priority: 8, conditional: s =>  InBalls(s) && Lewd(s)),

            new EventString((i) => $"<b>{i.Target.Name}</b> sits in {GPPHis(i.Target)} leader's belly, rubbing around. <b>{i.Unit.Name}</b> reassures {GPPHim(i.Target)} \"You have served me well, warrior. Soon you will be forever with me as a layer of fat hugging my belly.\" ",
            priority: 9, conditional: s => ActorLeader(s) && InStomach(s) && Friendly(s)), //This one is meant to imply that the eaten unit sees being eaten by the leader as a great honour

            new EventString((i) => $"\"You have been a thorn in my side for a long time\" says <b>{i.Unit.Name}</b>. \"But soon...you will only be flab on my gut\" ",
            priority: 9, conditional: s => ActorLeader(s) && InStomach(s) && TargetLeader(s)),


            new EventString((i) => $"<b>{i.Target.Name}</b> moans and groans blissfully in <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)} ",
            priority: 9, conditional: Cursed),

            new EventString((i) => $"<b>{i.Target.Name}</b> knows what will happen to {GPPHim(i.Target)} if {GPPHe(i.Target)} do{EsIfSingular(i.Target)} not escape from <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)}, but {GPPHeIsAbbr(i.Target)} just too horny to care.",
            priority: 9, conditional: s=> Cursed(s) && Lewd(s)),

            new EventString((i) => $" \"Look at you, the Big Bad Wolf reduced to a mere bulge in my Big Fat Gut\" <b>{i.Unit.Name}</b> laughs, taunting <b>{i.Target.Name}</b> as the wolf squirms within {GPPHis(i.Unit)} big fat gut.",
            targetRace: Race.Wolves, priority: 10, conditional: s => ActorLeader(s) && InStomach(s) && TargetLeader(s)),

            new EventString((i) => $"\"You better get comfy, I have a long digestive system\" <b>{i.Unit.Name}</b> teases <b>{i.Target.Name}</b> as the {GetRaceDescSingl(i.Target)} tries frantically to escape.",
            actorRace: Race.Taurus, priority: 9, conditional: InStomach),

            new EventString((i) => $" \"Tell me, how does it feel knowing you're lower on the food chain?\" <b>{i.Unit.Name}</b> taunts <b>{i.Target.Name}</b> as the {GetRaceDescSingl(i.Target)} squirms within {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}.",
            priority: 8),

        };

        BellyRubMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Unit.Name}</b> stretches and shifts on the ground, moving bulges in {GPPHis(i.Unit)} belly to more comfortable positions.",
            actorRace: Race.Serpents, priority: 11, conditional: s => s.Target == s.Unit),
            new EventString((i) => $"<b>{i.Unit.Name}</b> stretches and shifts on the ground, moving bulges in {GPPHis(i.Unit)} belly to more comfortable positions.",
            actorRace: Race.Lamia, priority: 11, conditional: s => s.Target == s.Unit),

            new EventString((i) => $"<b>{i.Unit.Name}</b> massages {GPPHis(i.Unit)} own belly, its contents shifting under {GPPHis(i.Unit)} hands.",
            priority: 10, conditional: s => s.Target == s.Unit),
            new EventString((i) => $"<b>{i.Unit.Name}</b> massages {GPPHimself(i.Unit)}, moving bulges in {GPPHis(i.Unit)} belly to more comfortable positions.",
            priority: 10, conditional: s => s.Target == s.Unit),
            new EventString((i) => $"{i.Unit.Name} puts {GPPHis(i.Unit)} hands on {GPPHis(i.Unit)} belly, feeling the prey struggling within.",
            priority: 10, conditional: s => s.Target == s.Unit),
            new EventString((i) => $"{i.Unit.Name} slaps {GPPHis(i.Unit)} belly, taunting the prey within.",
            priority: 10, conditional: s => s.Target == s.Unit),
            new EventString((i) => $"{i.Unit.Name} squishes {GPPHis(i.Unit)} gurgling gut, enjoying its wobble.",
            priority: 10, conditional: s => s.Target == s.Unit),

            new EventString((i) => $"<b>{i.Unit.Name}</b> coils around <b>{i.Target.Name}</b>'s {PreyLocStrings.ToSyn(PreyLocation.stomach)}, enjoying its warmth and movement.",
            actorRace: Race.Serpents, priority: 9, conditional: s => s.Target == s.Unit),
            new EventString((i) => $"<b>{i.Unit.Name}</b> coils around <b>{i.Target.Name}</b>'s {PreyLocStrings.ToSyn(PreyLocation.stomach)}, enjoying its warmth and movement.",
            actorRace: Race.Lamia, priority: 9, conditional: s => s.Target == s.Unit),

            new EventString((i) => $"<b>{i.Target.Name}</b> nervously asks <b>{i.Unit.Name}</b> for help as {GPPHis(i.Target)}'s body makes a squelching noise, the <b>{i.Unit.Name}</b> laughs and pats the newbie’s full belly.",
            priority: 9, conditional: s=> TargetFirstTime(s) && (s.Target.Side == s.Unit.Side)),
            
            // romance
            new EventString((i) => $"<b>{i.Target.Name}</b>’s nipples perk in anticipation as <b>{i.Unit.Name}</b> presses {GPPHimself(i.Unit)} into {GPPHis(i.Target)} sensitive midsection.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Target.HasBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> straddles <b>{i.Target.Name}</b>’s sloshing stomach, using {GPPHis(i.Unit)} weight to pack the stomach’s contents tighter. <b>{i.Target.Name}</b> quivers as {GPPHis(i.Target)} crush bounces up and down on top of {GPPHim(i.Target)}.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> touches {GPPHim(i.Target)}, <b>{i.Target.Name}</b> places {GPPHis(i.Target)} hands atop {GPPHis(i.Unit)} and starts exploring {GPPHis(i.Target)}'s body’s most sensitive areas.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> pinches {GPPHis(i.Target)} nipples as {GPPHis(i.Target)} body is assailed by an intoxicating sensuality as <b>{i.Unit.Name}</b> moves {GPPHis(i.Unit)} digits across {GPPHis(i.Target)} form.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Target.HasBreasts),
            new EventString((i) => $"\"You know I’ve eaten people for less than touching me, right?\" <b>{i.Target.Name}</b> confidently asks in a husky voice, \"Don’t stop.\"",
            priority: 8, conditional: s => ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"After massaging for a time, <b>{i.Unit.Name}</b> slowly reaches beneath the gurgling tummy and starts to rub {GPPHis(i.Unit)} hand against <b>{i.Target.Name}</b>’s sensitive nethers, eliciting a ragged gasp from the fearsome predator.",
            priority: 8, conditional: s => ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> pushes <b>{i.Unit.Name}</b> over and promptly sits {GPPHimself(i.Target)} onto {GPPHis(i.Unit)} face while resting {GPPHis(i.Unit)} engorged tummy against {GPPHis(i.Target)} chest.",
            priority: 8, conditional: s => ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> fumbles while massaging the predator’s convulsing bulge, distracted by {GPPHis(i.Target)} endowments. <b>{i.Target.Name}</b> finds the shy display absolutely adorable.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Target.HasDick),
            new EventString((i) => $"<b>{i.Unit.Name}</b> presses {GPPHimself(i.Unit)} close and hoists <b>{i.Target.Name}</b>’s tummy, making its contents slosh deeper into the blushing predator’s bowels.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"Coming from behind, <b>{i.Unit.Name}</b> grabs onto <b>{i.Target.Name}</b>’s hips and slowly works {GPPHis(i.Unit)} way up to {GPPHis(i.Target)} waist, around {GPPHis(i.Target)} bloated belly and up to {GPPHis(i.Target)} heaving chest.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> can’t resist {GPPHis(i.Unit)} urges any longer and forcefully thrust{SIfSingular(i.Unit)} {GPPHimself(i.Unit)} against the writhing midsection. <b>{i.Target.Name}</b> had been waiting to elicit such an eager response and immediately capitalizes on the amorous advance.",
            priority: 8, conditional: s => (s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s))),
            new EventString((i) => $"<b>{i.Target.Name}</b> was about to have {GPPHis(i.Target)} way with <b>{i.Unit.Name}</b>, but is caught off guard by just how caring {GPPHeIs(i.Unit)} in caressing {GPPHis(i.Target)} body and decides to let the affectionate massage continue.",
            priority: 8, conditional: s => ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> can’t help but get lost in <b>{i.Target.Name}</b>’s eyes as {GPPHe(i.Unit)} rub{SIfSingular(i.Unit)} {GPPHim(i.Target)}.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> lets out a soft moan as <b>{i.Unit.Name}</b>’s hands wrap around {GPPHis(i.Target)} engorged belly.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> refuses to let <b>{i.Unit.Name}</b> touch {GPPHim(i.Target)} but instantly swoons as {GPPHis(i.Unit)} hands press into {GPPHis(i.Target)} soft midsection.",
            priority: 8, conditional: s => ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> eyes <b>{i.Unit.Name}</b> seductively as {GPPHis(i.Unit)} hands press against {GPPHis(i.Target)} midsection.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs onto <b>{i.Target.Name}</b> pressing {GPPHimself(i.Unit)} against {GPPHis(i.Target)} soft body and forcing {GPPHim(i.Target)} into a deep kiss.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> begins to lean into <b>{i.Target.Name}</b> for a kiss as {GPPHe(i.Unit)} knead{SIfSingular(i.Unit)} {GPPHis(i.Target)} stomach. <b>{i.Target.Name}</b> doesn’t resist.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> wishes this moment could last forever as <b>{i.Unit.Name}</b> presses {GPPHis(i.Unit)} hands against {GPPHis(i.Target)} sensitive body.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> begins to pant heavily as <b>{i.Unit.Name}</b> handles {GPPHis(i.Target)} body.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"\"S - Stop, I’m s…sensitive!\" <b>{i.Target.Name}</b> protests as <b>{i.Unit.Name}</b> squeezes {GPPHis(i.Target)} midsection, relieving digestion.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"\"You know, while I might be full, I still have plenty of room in my bed for one more,\" <b>{i.Target.Name}</b> says to <b>{i.Unit.Name}</b> as {GPPHe(i.Unit)} feel{SIfSingular(i.Unit)} {GPPHis(i.Target)} enticing body.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> gives <b>{i.Target.Name}</b> an amorous hug, pushing the contents of {GPPHis(i.Target)} gut deeper into {GPPHis(i.Target)} intestines.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> lets out an unexpected moan as <b>{i.Unit.Name}</b> presses {GPPHis(i.Unit)} finger into {GPPHis(i.Target)} belly’s neglected underside.",
            priority: 8, conditional: s => ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> shakily tells <b>{i.Unit.Name}</b> to leave {GPPHim(i.Target)} alone, but {GPPHis(i.Target)} gurgling body melts into {GPPHis(i.Unit)} hands as soon as {GPPHe(i.Unit)} touch{EsIfSingular(i.Unit)} {GPPHim(i.Target)}.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> meticulously massages <b>{i.Target.Name}</b>’s engorged midsection, fearing that a poor job would add {GPPHim(i.Unit)} to the gurgling stew. <b>{i.Target.Name}</b> loves <b>{i.Unit.Name}</b>’s amorous attention.",
            priority: 8, conditional: s => ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> can’t believe {GPPHis(i.Target)} luck when {GPPHis(i.Target)} crush begins massaging {GPPHim(i.Target)}. <b>{i.Unit.Name}</b> fears for {GPPHis(i.Unit)} life as {GPPHe(i.Unit)} consider{SIfSingular(i.Unit)} the tummy’s current occupant.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),

            new EventString((i) => $"<b>{i.Target.Name}</b> grabs onto <b>{i.Unit.Name}</b> and presses {GPPHis(i.Unit)} face against {GPPHis(i.Target)} gurgling gut, resting one hand against the small of {GPPHis(i.Target)} back to push {GPPHis(i.Target)} stomach out even further while the other lovingly rubs the back of {GPPHis(i.Target)} potential mate’s head. While <b>{i.Unit.Name}</b> was initially nervous, the methodic sloshing coming from within the squishy belly calms {GPPHim(i.Unit)} and {GPPHe(i.Target)} begin{SIfSingular(i.Unit)} rubbing it with awe.",
            priority: 8, conditional: s => ReqTargetCompatible(s) && ActorHumanoid(s)),
            new EventString((i) => $"Exhausted from the process of digestion, <b>{i.Target.Name}</b> lies down, breathing heavily as {GPPHis(i.Target)} gut pulsates. <b>{i.Unit.Name}</b> takes the opportunity to help. Going over, {GPPHe(i.Unit)} push{EsIfSingular(i.Unit)} the bloated stomach towards the lying predator’s chest creating a chorus of sloshing. With the huge gut out of the way, <b>{i.Unit.Name}</b> begins to go to work relieving {GPPHim(i.Target)} stress via carnal pleasures. <b>{i.Target.Name}</b> soon returns to the battlefield, renewed and ready to continue.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),

            new EventString((i) => $"<b>{i.Target.Name}</b> can’t believe {GPPHis(i.Target)} luck when {GPPHis(i.Target)} crush begins massaging {GPPHim(i.Target)}. <b>{i.Unit.Name}</b> fears for {GPPHis(i.Unit)} life as {GPPHe(i.Unit)} consider{SIfSingular(i.Unit)} the tummy’s current occupant.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> massages <b>{i.Target.Name}</b>'s belly, its contents shifting under {GPPHis(i.Unit)} hands.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> basks in <b>{i.Unit.Name}</b>’s attentions as {GPPHe(i.Unit)} massage{SIfSingular(i.Unit)} {GPPHis(i.Target)} tummy.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> sweats nervously as <b>{i.Unit.Name}</b> presses {GPPHis(i.Unit)} hands into {GPPHis(i.Target)} deadly stomach.",
            priority: 8, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> is surprised when <b>{i.Unit.Name}</b> suddenly starts massaging {GPPHis(i.Target)} wobbling belly. {Capitalize(GPPHe(i.Target))} look{SIfSingular(i.Target)} embarrassed but do{EsIfSingular(i.Target)}n’t recoil from the touch.",
            priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> tenses when <b>{i.Unit.Name}</b> starts touching {GPPHim(i.Target)} but begins to relax as {GPPHis(i.Target)} digestion is relieved.",
            priority: 8),

             new EventString((i) => $"\"Who's a good {GetGenderString(i.Target, "boy", "girl", "wolf")}?\" <b>{i.Unit.Name}</b> says as {GPPHe(i.Unit)} rubs <b>{i.Target.Name}</b>'s belly. The wolf lies down, {GPPHis(i.Target)} tongue lolling out, tail rapidly wagging, and leg kicking as <b>{i.Unit.Name}</b> rubs {GPPHis(i.Target)} belly in all the right places.",
            targetRace: Race.Wolves, priority: 9, conditional: s => (s.Target != s.Unit)),
            new EventString((i) => $"\"Who's a good {GetGenderString(i.Target, "boy", "girl", "wolf")}?\" <b>{i.Unit.Name}</b> says as {GPPHe(i.Unit)} rubs <b>{i.Target.Name}</b>'s belly. The wolf lies down, {GPPHis(i.Target)} tongue lolling out, tail rapidly wagging, and leg kicking as <b>{i.Unit.Name}</b> rubs {GPPHis(i.Target)} belly in all the right places. \"I am!\" <b>{i.Target.Name}</b> exclaims.",
            targetRace: Race.Wolves, priority: 9, conditional: s => (s.Target != s.Unit)),

            new EventString((i) => $"\"Who's a good {GetGenderString(i.Target, "boy", "girl", "wolf")}?\" <b>{i.Unit.Name}</b> says as {GPPHe(i.Unit)} rubs <b>{i.Target.Name}</b>'s belly. The wolf lies down, {GPPHis(i.Target)} tongue lolling out, tail rapidly wagging, and leg kicking as <b>{i.Unit.Name}</b> rubs {GPPHis(i.Target)} belly in all the right places.",
            targetRace: Race.FeralWolves, priority: 9, conditional: s => (s.Target != s.Unit)),
            new EventString((i) => $"\"Who's a good {GetGenderString(i.Target, "boy", "girl", "wolf")}?\" <b>{i.Unit.Name}</b> says as {GPPHe(i.Unit)} rubs <b>{i.Target.Name}</b>'s belly. The wolf lies down, {GPPHis(i.Target)} tongue lolling out, tail rapidly wagging, and leg kicking as <b>{i.Unit.Name}</b> rubs {GPPHis(i.Target)} belly in all the right places. \"You are! Yes you are!\" <b>{i.Unit.Name}</b> exclaims.",
            targetRace: Race.FeralWolves, priority: 9, conditional: s => (s.Target != s.Unit)),

            new EventString((i) => $"\"Who's a good {GetGenderString(i.Target, "boy", "girl", "dog")}?\" <b>{i.Unit.Name}</b> says as {GPPHe(i.Unit)} rubs <b>{i.Target.Name}</b>'s belly. The dog lies down, {GPPHis(i.Target)} tongue lolling out, tail rapidly wagging, and leg kicking as <b>{i.Unit.Name}</b> rubs {GPPHis(i.Target)} belly in all the right places.",
            targetRace: Race.Dogs, priority: 9, conditional: s => (s.Target != s.Unit) ),
            new EventString((i) => $"\"Who's a good {GetGenderString(i.Target, "boy", "girl", "dog")}?\" <b>{i.Unit.Name}</b> says as {GPPHe(i.Unit)} rubs <b>{i.Target.Name}</b>'s belly. The dog lies down, {GPPHis(i.Target)} tongue lolling out, tail rapidly wagging, and leg kicking as <b>{i.Unit.Name}</b> rubs {GPPHis(i.Target)} belly in all the right places. \"I am! I am a good {GetGenderString(i.Target, "boy", "girl", "dog")}!\" <b>{i.Target.Name}</b> happily exclaims.",
            targetRace: Race.Dogs, priority: 9, conditional: s => (s.Target != s.Unit)),

            new EventString((i) => $"\"Who’s a good {BoyGirl(i.Target)}?\" <b>{i.Unit.Name}</b> asks while pressing {GPPHis(i.Unit)} fingers into {GPPHis(i.Target)} belly. <b>{i.Target.Name}</b> cocks {GPPHis(i.Target)} head to the side, wondering to the answer. \"You are!\" The answer sends the canine into an excited, jiggling jumping spree.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s attentive massage of <b>{i.Target.Name}</b>’s stuffed midsection convinces the voracious canine to make {GPPHim(i.Unit)} {GPPHis(i.Target)} master no matter the cost.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side || (s.Unit.HasTrait(Traits.SeductiveTouch) && TacticalUtilities.Units.Where(actor => actor.Unit == s.Target)?.FirstOrDefault().TurnsSinceLastParalysis <= 0) ),
            new EventString((i) => $"<b>{i.Target.Name}</b> just loves <b>{i.Unit.Name}</b> so much! {GPPHe(i.Target)} can’t help {GPPHimself(i.Target)} from using {GPPHis(i.Target)} sloshing belly weight to push {GPPHim(i.Unit)} over and assaulting {GPPHim(i.Unit)} in a flurry of licks.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> lolls out {GPPHis(i.Target)} tongue and gently starts to lick <b>{i.Unit.Name}</b>’s face as {GPPHe(i.Unit)} work{SIfSingular(i.Unit)} to satisfy {GPPHis(i.Target)} gurgling tummy.",
            targetRace: Race.Dogs, priority: 9),
            new EventString((i) => $"Rubbing {GPPHis(i.Target)} distended tummy causes <b>{i.Target.Name}</b> to start thumping {GPPHis(i.Target)} leg. <b>{i.Unit.Name}</b> finds this insanely cute and redoubles {GPPHis(i.Unit)} efforts.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> lies on {GPPHis(i.Target)}back, presenting {GPPHis(i.Target)} enormous gut to <b>{i.Unit.Name}</b> in hopes of receiving a belly rub and maybe something a bit lewder. <b>{i.Unit.Name}</b> obliges both desires.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"\"I don’t like feeling empty,\" <b>{i.Target.Name}</b> says as her master presses into her belly, \"After this meal’s all gone, can you fill me with puppies?\" the dog girl asks with surprising innocence. <b>{i.Unit.Name}</b> stutters for a moment but can’t see a good reason why {GPPHe(i.Unit)} shouldn’t.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s) && s.Target.GetGender() == Gender.Female),

            new EventString((i) => $"<b>{i.Target.Name}</b> wrap{SIfSingular(i.Target)} {GPPHimself(i.Target)} around <b>{i.Unit.Name}</b>, pressing {GPPHis(i.Target)} sloshing gut against {GPPHim(i.Unit)} while digging {GPPHis(i.Target)} claws into {GPPHis(i.Unit)} back.",
            targetRace: Race.Cats, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s slender tail flicks back and forth, swaying with <b>{i.Unit.Name}</b>’s rubbing motions across {GPPHis(i.Target)} prey filled gut.",
            targetRace: Race.Cats, priority: 9),
            new EventString((i) => $"The attentions on {GPPHis(i.Target)} bursting gut from the one {GPPHe(i.Target)} love{SIfSingular(i.Target)} cause <b>{i.Target.Name}</b> to inadvertently start purring. The feline predator catches {GPPHimself(i.Target)} and stops but it’s too late, <b>{i.Unit.Name}</b> already thinks {GPPHe(i.Target)} is adorable.",
            targetRace: Race.Cats, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> lifts {GPPHis(i.Target)} arms high, stretching {GPPHis(i.Target)} body out as <b>{i.Unit.Name}</b> caresses {GPPHis(i.Target)} most recent meal.",
            targetRace: Race.Cats, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> stares imperiously with {GPPHis(i.Target)} slit eyes as <b>{i.Unit.Name}</b> attends to her greedy belly’s physical needs while thinking about what other parts of her body could be satisfied also.",
            targetRace: Race.Cats, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Target.GetGender() == Gender.Female),
            new EventString((i) => $"<b>{i.Target.Name}</b> lies on {GPPHis(i.Target)} side and tries to lick {GPPHis(i.Target)} nethers, but {GPPHis(i.Target)} prey packed tummy prevents the flexible predator from doing so. \"I can’t reach,\" {GPPHe(i.Target)} says, lifting {GPPHis(i.Target)} leg high while seductively looking at <b>{i.Unit.Name}</b> \"Would you mind?\" The {GetRaceDescSingl(i.Unit)} quickly gets to work satisfying the demanding feline.",
            targetRace: Race.Cats, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"\"You know,\" <b>{i.Target.Name}</b> says, licking her paw as <b>{i.Unit.Name}</b> tends to {GPPHis(i.Target)} gurgling midsection, \"we would make beautiful kittens.\"",
            targetRace: Race.Cats, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s) && s.Target.GetGender() == Gender.Female),


            new EventString((i) => $"<b>{i.Target.Name}</b> growls and shows {GPPHis(i.Target)} teeth as <b>{i.Unit.Name}</b> approaches but starts to wag {GPPHis(i.Target)} tail and softly whine as {GPPHis(i.Target)} indigestion riddled stomach is caressed.",
            targetRace: Race.Wolves, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is surprised when <b>{i.Target.Name}</b> pushes {GPPHim(i.Unit)} over and starts to writhe against {GPPHim(i.Unit)} as the long-fanged predator’s belly wobbles and sloshes while {GPPHe(i.Unit)} presses {GPPHimself(i.Unit)} closer.",
            targetRace: Race.Wolves, priority: 9),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> is passing by, the wolfen suddenly starts to aggressively rub {GPPHis(i.Target)} gorged stomach against {GPPHim(i.Unit)}. {GPPHe(i.Target)} doesn’t want to share {GPPHis(i.Target)} new mate with {GPPHis(i.Target)} pack mates so {GPPHeIsAbbr(i.Target)} marking {GPPHim(i.Unit)} with {GPPHis(i.Target)} scent.",
            targetRace: Race.Wolves, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> thrusts {GPPHis(i.Target)} distended, prey filled gut into <b>{i.Unit.Name}</b>’s face. After a moment’s pause the {GetRaceDescSingl(i.Target)} says, \"Well? It’s not going to massage itself! Or would you rather do so from the inside?\" With urgency, the gurgling belly is attended to.",
            targetRace: Race.Wolves, priority: 9, conditional: s => ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tries {GPPHis(i.Unit)} best to pay attention to the vicious wolfen’s tummy as it gurgles away its occupant but the {GetRaceDescSingl(i.Target)}’s heavy panting and long stares keep distracting {GPPHim(i.Unit)}. As {GPPHe(i.Unit)} lift{SIfSingular(i.Unit)} the packed guts for an underside rub {GPPHe(i.Unit)} find{SIfSingular(i.Unit)} that the predator is most certainly in heat.",
            targetRace: Race.Wolves, priority: 9, conditional: s => ReqTargetCompatibleLewd(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> starts to rub the short demon’s overpacked tummy, eliciting a mischievous cackle from {GPPHim(i.Target)}.",
            targetRace: Race.Imps, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sits with <b>{i.Target.Name}</b> in {GPPHis(i.Unit)} lap, {GPPHe(i.Unit)} reach{EsIfSingular(i.Unit)} over and massage{SIfSingular(i.Unit)} the Imp but {GPPHe(i.Target)} starts grinding {GPPHis(i.Target)} hips back and forth, making {GPPHis(i.Target)} meal slosh in {GPPHis(i.Target)} guts and {GPPHis(i.Target)} ass press against {GPPHis(i.Target)} attendant.",
            targetRace: Race.Imps, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> feels {GPPHis(i.Unit)} hand across <b>{i.Target.Name}</b>’s smooth stomach scales as {GPPHis(i.Target)} guts work away its last occupant. The lizard predator watches the {GetRaceDescSingl(i.Unit)} do so with unknown intent.",
            targetRace: Race.Lizards, priority: 9),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> explores the scaled predator’s body, {GPPHeIs(i.Unit)} startled when suddenly propositioned, \"You will mate with <b>{i.Target.Name}</b>,\" <b>{i.Unit.Name}</b> stumbles with {GPPHis(i.Unit)} words for a moment before {GPPHe(i.Target)} slaps {GPPHis(i.Target)} wiggling stomach, \"Or you will join them.\" This prompts a swift agreement.",
            targetRace: Race.Lizards, priority: 9, conditional: s => ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b>’s tummy, padded with the melting flesh of gurgling warriors, roars angrily as indigestion sets in. {GPPHe(i.Target)} quickly approaches {GPPHis(i.Target)} favorite mate and thrust{SIfSingular(i.Target)} the bulging mass against {GPPHim(i.Unit)}. <b>{i.Unit.Name}</b> starts to massage {GPPHim(i.Target)} without question.",
            targetRace: Race.Lizards, priority: 9, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is suddenly knocked over by a violent tummy wobble as <b>{i.Target.Name}</b>’s prey makes one last attempt at escaping the lizard’s guts. With a reptilian rage, {GPPHe(i.Target)} slams {GPPHis(i.Target)} fist into {GPPHis(i.Target)} belly, knocking the poor prey out. {GPPHe(i.Target)} then hugs {GPPHis(i.Target)} hurt mate, desperately apologizing.",
            targetRace: Race.Lizards, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"\"<b>{i.Target.Name}</b> will never eat you, even if you become weak\" the lizard predator says while <b>{i.Unit.Name}</b> caresses {GPPHis(i.Target)} sloshing stomach. Before a thanks can be offered, the reptile continues, \"Reward this warrior’s compassion with passionate mating.\"",
            targetRace: Race.Lizards, priority: 9, conditional: s => ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> can tell the process of digesting {GPPHis(i.Target)} last victim is no easy task for the herbivorous rabbit person and works diligently to help <b>{i.Target.Name}</b> break down {GPPHis(i.Target)} prey.",
            targetRace: Race.Bunnies, priority: 9, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> holds {GPPHis(i.Target)} belly and groans miserably as it breaks down the meat within. {GPPHis(i.Target)} pain soon elicits the aid of <b>{i.Unit.Name}</b> who comes and gently massages the carrot-eater’s unusual meal.",
            targetRace: Race.Bunnies, priority: 9),
            new EventString((i) => $"Even though <b>{i.Target.Name}</b> has a gurgling gut and indigestion {GPPHe(i.Target)} hops onto <b>{i.Unit.Name}</b> with abandon and starts fucking like {GPPHis(i.Target)} namesake.",
            targetRace: Race.Bunnies, priority: 9,  conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"\"Urgh,\" <b>{i.Target.Name}</b> moans while <b>{i.Unit.Name}</b> caresses {GPPHis(i.Target)} rumbling bulge, \"Why can’t we fight something tasty from now on? Like carrot people! Or cabbage patch monsters!\" The joke goes over well, bringing out a chuckle from {GPPHis(i.Target)} loving attendant.",
            targetRace: Race.Bunnies, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> rubs the rabbit’s stomach, {GPPHeIsAbbr(i.Unit)} suddenly stopped when <b>{i.Target.Name}</b> says, \"Wait, can you hear that?\" As {GPPHe(i.Unit)} lowers {GPPHis(i.Unit)} head to rest against {GPPHis(i.Target)} rumbling gut. \"They’re saying something! It sounds like…\" A moment passes before the bucktoothed carnivore lets lose a loud belch followed by a fit of laughing.",
            targetRace: Race.Bunnies, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ActorHumanoid(s)),


            new EventString((i) => $"“I ate them, master! I ate them just for you!” <b>{i.Target.Name}</b> declares as <b>{i.Unit.Name}</b> tends to {GPPHis(i.Target)} gut while it dutifully turns its content into a slurry of nutritious paste. “Please love me! I’m a good mate, I swear it!” To emphasize the point, {GPPHe(i.Target)} start{SIfSingular(i.Target)} swaying {GPPHis(i.Target)} body, making {GPPHis(i.Target)} most sensual aspects stand out to {GPPHis(i.Target)} prospective mate.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"“You know,” <b>{i.Target.Name}</b> says imperiously as {GPPHis(i.Target)} beloved presses into {GPPHis(i.Target)} soft, supple body, “I’m actually very naughty.” {GPPHe(i.Target)} rub{SIfSingular(i.Target)} the side of {GPPHis(i.Target)} bloated meat filled gut as it lets out a groan and leans against <b>{i.Unit.Name}</b> while whispering into {GPPHis(i.Unit)} ear, “You should probably put a collar on me. Then we can start to work on... discipline.”",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"“You know I can smell it on you, right?” <b>{i.Target.Name}</b> states as <b>{i.Unit.Name}</b> lovingly tends to {GPPHis(i.Target)} tummy, full of digestive juices and melting flesh. “Your lusting, your desire to have your way with me. The air’s thick with the scent,” the canine predator continues as lovingly tended gut rumbles, “Don’t worry. I want that too.”",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> wastes no time in accepting the advances of {GPPHis(i.Target)} favorite lover. Soon enough the two are working furiously to satiate one another’s carnal desires, <b>{i.Target.Name}</b>’s bulging stomach bouncing wildly the whole time. As they’re wrapped together, <b>{i.Unit.Name}</b> begins to rub behind the canine warrior’s ears, eliciting a shocked gasp and belch from {GPPHis(i.Unit)} partner.",
            targetRace: Race.Dogs, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),

            new EventString((i) => $"<b>{i.Target.Name}</b>, with great melodrama, begins to wail about {GPPHis(i.Target)} pained stomach as it bubbled away its last victim. <b>{i.Unit.Name}</b>, naïve against the wiley fox’s acting, immediately starts lathering the bushy tailed warrior with attention and rubbing.",
            targetRace: Race.Foxes, priority: 9),
            new EventString((i) => $"\"You know,\" <b>{i.Target.Name}</b> muses with a sly grin as <b>{i.Unit.Name}</b> tends to {GPPHis(i.Target)} sloshing tummy, \"You could always massage me from the inside, it’d be much more relaxing. I promise I wouldn’t let you melt away!\" The trickster is surprised and flattered when {GPPHis(i.Target)} attendant doesn’t oppose the idea because {GPPHe(i.Unit)} trust{SIfSingular(i.Unit)} {GPPHim(i.Target)} so much.",
            targetRace: Race.Foxes, priority: 9, conditional: s => ReqTargetCompatible(s) && ActorHumanoid(s)),

            new EventString((i) => $"At <b>{i.Target.Name}</b>'s behest, <b>{i.Unit.Name}</b> begins to tend to the great dragon’s mighty belly as it melts {GPPHis(i.Target)} foes away into a fine sludge.",
            targetRace: Race.Dragon, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> has to scratch hard against the huge dragon’s shifting tummy scales to even be felt. <b>{i.Target.Name}</b> cares not, simply basking in {GPPHis(i.Target)} favorite servant’s attention.",
            targetRace: Race.Dragon, priority: 9, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"\"This is simply divine,\" <b>{i.Target.Name}</b> says haughtily as <b>{i.Unit.Name}</b> deftly attends to {GPPHis(i.Target)} prey laden belly. \"I’ll need to grant you a reward from my horde or perhaps you would prefer a… different prize,\" {GPPHe(i.Target)} says coyly with a seductive look while raising {GPPHis(i.Target)} tail.",
            targetRace: Race.Dragon, priority: 9, conditional: s =>  s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"Servants like <b>{i.Unit.Name}</b> remind <b>{i.Target.Name}</b> why {GPPHe(i.Target)} do{EsIfSingular(i.Target)}n’t simply devour everyone in {GPPHis(i.Target)} path. The thought however conflicts with {GPPHis(i.Target)} sloshing midsection melting a number of mortals' flesh and bone as the aforementioned servant rubs it.",
            targetRace: Race.Dragon, priority: 9, conditional: s =>  s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> can’t help but stare lovingly at <b>{i.Unit.Name}</b> as {GPPHe(i.Unit)} tend{SIfSingular(i.Unit)} to {GPPHis(i.Target)} burgeoning mass filled with the foolish and weak.",
            targetRace: Race.Dragon, priority: 9, conditional: s =>  s.Target.Side == s.Unit.Side),
            new EventString((i) => $"While <b>{i.Unit.Name}</b> is certainly weaker than the great dragon, <b>{i.Target.Name}</b> can’t help but obey {GPPHis(i.Unit)} every command when it comes to massaging {GPPHis(i.Target)} imperious guts. At times like this {GPPHe(i.Target)} would even let {GPPHis(i.Unit)} make more sensual desires come to the light.",
            targetRace: Race.Dragon, priority: 9, conditional: s =>  s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"\"Come closer,\" <b>{i.Target.Name}</b> says to <b>{i.Unit.Name}</b> as {GPPHis(i.Target)} filled guts rumble ominously. \"I can think of nothing better than your touch against my scales. Touch me as you please... wherever you please.\" The imposing dragon rolls over, inviting {GPPHis(i.Target)} favorite servant to explore {GPPHis(i.Target)} vast body.",
            targetRace: Race.Dragon, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),
            new EventString((i) => $"Laying on {GPPHis(i.Target)} back, <b>{i.Target.Name}</b> lifts <b>{i.Unit.Name}</b> up and places {GPPHim(i.Unit)} in the middle of {GPPHis(i.Target)} sloshing scaled belly. The servant goes to work massaging the powerful entity before {GPPHeIsAbbr(i.Unit)} interrupted by a booming voice, \"There’s an itch a bit lower, lower… even lower,\" soon enough the servant finds {GPPHimself(i.Unit)} tending to {GPPHis(i.Unit)} master’s most sensitive parts.",
            targetRace: Race.Dragon, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Target.Name}</b> watches <b>{i.Unit.Name}</b> as {GPPHe(i.Unit)} tr{IesIfSingular(i.Target)} to massage {GPPHis(i.Target)} sturdy, gurgling midsection in vain. Eventually {GPPHis(i.Target)} attendant just slams into {GPPHis(i.Target)} gut with a shoulder, sending {GPPHis(i.Target)} meal into another stomach chamber and a wave of pleasure through the bovine predator.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> grabs <b>{i.Unit.Name}</b> as {GPPHe(i.Unit)} pass{EsIfSingular(i.Unit)} by and forces {GPPHis(i.Unit)} lips onto {GPPHis(i.Target)} breast. {Capitalize(GPPHis(i.Target))} latest burbling tummy occupant quickly becoming a thick, creamy milk that pained her engorged breast. <b>{i.Unit.Name}</b>, initially surprised, now greedily sucks down the delicious milk and elicits pleasured moans from the cowgirl.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Unit.HasBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b>, with a surprising amount of confidence, grabs the bovine predator by the horns and forces {GPPHim(i.Target)} onto {GPPHis(i.Target)} hands and knees. With {GPPHis(i.Target)} gut pressed against the ground, <b>{i.Target.Name}</b> is shocked but excited when {GPPHis(i.Target)} attendant begins to milk {GPPHim(i.Target)}.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Unit.HasBreasts),
            new EventString((i) => $"With a wobbling tummy, <b>{i.Target.Name}</b> wraps <b>{i.Unit.Name}</b> into a deep, soft, warm hug. As it goes on, <b>{i.Unit.Name}</b> inadvertently calls <b>{i.Target.Name}</b> {(i.Target.HasBreasts ? "mommy" : "daddy")} while one party is mortified by this, the other beams with pride and no small bit of arousal.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> sits down on {GPPHis(i.Target)} fat laden ass, letting {GPPHis(i.Target)} belly rest on the ground in an attempt to relive {GPPHis(i.Target)} indigestion as {GPPHis(i.Target)} meal passes from one stomach to the next. <b>{i.Unit.Name}</b>, seeing {GPPHis(i.Target)} discomfort arrives and attends to {GPPHis(i.Target)} sloshing belly. <b>{i.Target.Name}</b> basks in the attention, leaning back on {GPPHis(i.Target)} elbows as {GPPHeIsAbbr(i.Target)} rubbed down.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side),
            new EventString((i) => $"<b>{i.Unit.Name}</b> begins to explore the bovine’s massive form, subtly venturing further away from the gurgling midriff. <b>{i.Target.Name}</b> pretends not to notice but can’t hide {GPPHis(i.Target)} excitement as the massage reaches {GPPHis(i.Target)} more sensitive regions.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> thinks {GPPHe(i.Target)} wouldn’t mind being milked by such diligent hands as <b>{i.Unit.Name}</b> tends to {GPPHis(i.Target)} prey filled guts.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && s.Unit.HasBreasts),
            new EventString((i) => $"As <b>{i.Unit.Name}</b>’s hands disappear into the soft belly flesh, <b>{i.Target.Name}</b> asks {GPPHim(i.Unit)} if {GPPHe(i.Unit)}'d like to be trapped in {GPPHis(i.Target)} ‘Labyrinth’ later, but the euphemism is lost on the bovine’s prospective lover.",
            targetRace: Race.Taurus, priority: 9, conditional: s => s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s)),


            new EventString((i) => $"<b>{i.Unit.Name}</b> quickly gets to work rubbing <b>{i.Target.Name}</b>'s belly like a good little kobold.",
            actorRace: Race.Kobolds,  priority: 9, conditional: s =>  s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),
            new EventString((i) => $"<b>{i.Unit.Name}</b> hops onto <b>{i.Target.Name}</b>'s belly and begins to knead and rub it, ellicting a pleasured groan from {GPPHis(i.Unit)} draconic master.",
            actorRace: Race.Kobolds,  priority: 9, conditional: s =>  s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),
            new EventString((i) => $"<b>{i.Unit.Name}</b> can feel the contents of <b>{i.Target.Name}</b>'s gut shift as {GPPHe(i.Unit)} presses {GPPHis(i.Unit)} hands on that big scaly gut.",
            priority: 9, conditional: s =>  s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),

            new EventString((i) => $"<b>{i.Unit.Name}</b> moans and groans as {GPPHe(i.Unit)} rub{SIfSingular(i.Unit)} {GPPHis(i.Unit)} full gut.",
            priority: 10, conditional: s => Lewd(s) && (s.Target == s.Unit)),
            new EventString((i) => $"\"Ohhhh...you were soooooo tasty!\" <b>{i.Unit.Name}</b> moans in pleasure as {GPPHe(i.Unit)} rub{SIfSingular(i.Unit)} {GPPHis(i.Unit)} gut.",
            priority: 10, conditional: s => s.Target == s.Unit),

            new EventString((i) => $"\"Mmmmmmmm\" <b>{i.Target.Name}</b> moans as <b>{i.Unit.Name}</b> rubs and prods the {GetRaceDescSingl(i.Target)}'s full gut. ",
            priority: 8),

            new EventString((i) => $"<b>{i.Unit.Name}</b> can't believe {GPPHe(i.Unit)} got the honour to rub the gut of the famous warrior, <b>{i.Target.Name}</b>.",
            priority: 9, conditional: s => s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"\"Oh gods this is actually happening!\" <b>{i.Unit.Name}</b> thinks to {GPPHimself(i.Unit)} as {GPPHe(i.Unit)} knead{SIfSingular(i.Unit)} the belly of thier idol, <b>{i.Target.Name}</b>.",
            priority: 9, conditional: s => s.Target.Side == s.Unit.Side && s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s eyes light up as the great warrior, <b>{i.Target.Name}</b>, lets {GPPHim(i.Unit)} rub {GPPHis(i.Target)} prey-filled gut. ",
            priority: 9, conditional: s => s.Target.Side == s.Unit.Side && s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"<b>{i.Unit.Name}</b> excitedly rubs <b>{i.Target.Name}</b>'s belly. The famous, battle-hardened warrior closing {GPPHis(i.Target)} eyes and nodding as {GPPHis(i.Target)} prey-filled belly is tended to.",
            priority: 9, conditional: s => s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"<b>{i.Unit.Name}</b> vigorously kneads <b>{i.Target.Name}</b>'s belly hoping that the famous warrior will be impressed with {GPPHis(i.Unit)} work.",
            priority: 9, conditional: s => s.Unit.Level + 9 < s.Target.Level),

            new EventString((i) => $"<b>{i.Unit.Name}</b> excitedly rubs the belly of <b>{i.Target.Name}</b>. The famous {(TargetHumanoid(i) ? "warrior" : "beast")} seems very pleased indeed and <b>{i.Unit.Name}</b> hopes it's enough to possibly get a 'special reward' later tonight. ",
            priority: 9, conditional: s => s.Target.Side == s.Unit.Side && Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> seductively rubs the belly of their idol, <b>{i.Target.Name}</b>. <b>{i.Unit.Name}</b> makes sure that {GPPHis(i.Unit)} assets draw the strong {(TargetHumanoid(i) ? "warrior" : "beast")}'s attention and hopes that <b>{i.Target.Name}</b> will want to be interested in {GPPHim(i.Unit)}. ",
            priority: 9, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> eagerly rubs the belly of <b>{i.Target.Name}</b>, the famous {(TargetHumanoid(i) ? "warrior" : "beast")} whispers to {GPPHim(i.Unit)}, \"If you keep that up, I might make time for a 'special session' with you tonight.\" <b>{i.Unit.Name}</b>'s eyes go wide and {GPPHe(i.Unit)} goes back to rubbing with twice the vigor!",
            priority: 9, conditional: s => s.Target.Side == s.Unit.Side && Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> rubs the belly of <b>{i.Target.Name}</b>, the renowned {(TargetHumanoid(i) ? "warrior" : "beast")} whispers something into {GPPHis(i.Unit)} ear. <b>{i.Unit.Name}</b>'s eyes light up as {GPPHe(i.Unit)} begin{SIfSingular(i.Unit)} to rub ever harder, hoping to make their idol's prey digest even faster so {GPPHe(i.Unit)} can spend more time with <b>{i.Unit.Name}</b> alone.",
            priority: 9, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"\"Keep that up, and I'll give you a passionate night you'll never forget~.\" <b>{i.Target.Name}</b> tells <b>{i.Unit.Name}</b> who immediately rubs harder and faster, still somewhat in disbelief that {GPPHis(i.Unit)} idol is willing to mate with them.",
            priority: 9, conditional: s => s.Target.Side == s.Unit.Side && Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),

            //enemy rubs
            new EventString((i) => $"<b>{i.Target.Name}</b> is confused about <b>{i.Unit.Name}</b>'s sudden affection while {GPPHeIs(i.Target)} digesting {GPPHis(i.Unit)} kin, but takes it as a peace offering considering the obvious difference in might. How adorable.",
            priority: 9, conditional: s => s.Target.Side != s.Unit.Side && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"<b>{i.Target.Name}</b> assumes the enemy lost {GPPHis(i.Unit)} mind out of pure attraction, causing {GPPHis(i.Target)} ego to be is stroked as much as {GPPHis(i.Target)} gurgling {i.preyLocation.ToSyn()} as {GPPHe(i.Target)} poses and stretches under the intensely pleasing touch.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side && ReqTargetCompatible(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> dextrously closes in and starts kneading <b>{i.Target.Name}</b>'s {i.preyLocation.ToSyn()} in the middle of battle, making sure their struggling former ally is processed nicely within the predator.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b>'s reflexes don't trigger to fend off <b>{i.Unit.Name}</b>, as they lack any killing intent or sudden movements. A careful caress turns into vigorous rubbing that's definitely causing <b>{i.Unit.Name}</b>'s comrade to be broken down faster. <b>{i.Target.Name}</b> cannot help but muse that maybe this one's useful as more than food.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side),
            new EventString((i) => $"Instead of surrendering, <b>{i.Unit.Name}</b> seems to be offering a massage. A really good massage. <b>{i.Target.Name}</b> completely has {GPPHis(i.Target)} guard down as {GPPHe(i.Target)} get{SIfSingular(i.Prey)} lost in the bliss of feeling {GPPHis(i.Target)} prey melt away in record time, but <b>{i.Unit.Name}</b> doesn't take advantage, tending to the {i.preyLocation.ToSyn()} dutifully.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side),
            new EventString((i) => $"<b>{i.Target.Name}</b> doesn't regret letting an unarmed <b>{i.Unit.Name}</b> touch {GPPHis(i.Target)} noisy {i.preyLocation.ToSyn()}. The {GetRaceDescSingl(i.Unit)} expertly shifts and softens the {GetRaceDescSingl(i.Target)} food that {GPPHe(i.Unit)} used to call an ally. Perhaps this is a sign {GPPHe(i.Unit)} wishes to be brought inside too!",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> casually commits high treason by affectionately helping the enemy's {i.preyLocation.ToSyn()} churn down <b>{i.Prey.Name}</b> into more useful stuff.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side),
            new EventString((i) => $"<b>{i.Unit.Name}</b> adds insult to injury for <b>{i.Prey.Name}</b>, by vigorously massaging {GPPHis(i.Prey)} voracious captor's {i.preyLocation.ToSyn()} instead of even attempting to help at all.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side && !s.Prey.IsDead),
            new EventString((i) => $"\"Help!!\", cries <b>{i.Prey.Name}</b> from within an ever softening bulge in <b>{i.Target.Name}</b>'s {i.preyLocation.ToSyn()}. <b>{i.Unit.Name}</b> heeds {GPPHis(i.Prey)} call by helping {GPPHim(i.Prey)} digest into gut slush. Any further protest are drowned out by <b>{i.Target.Name}</b>'s ensuing carnal symphony.",
            priority: 9, conditional: s =>  s.Target.Side != s.Unit.Side && !s.Prey.IsDead),

            new EventString((i) => $"<b>{i.Unit.Name}</b> caresses <b>{i.Target.Name}</b>'s {i.preyLocation.ToSyn()} with so much devotion, that <b>{i.Prey.Name}</b> can't help but despair as {GPPHe(i.Prey)} understand{SIfSingular(i.Prey)} {GPPHis(i.Prey)} allies wrote {GPPHim(i.Prey)} off, only considering {GPPHim(i.Prey)} food now, which should just hurry up and liquefy. At least they strive to make it quick and painless?",
            priority: 9, conditional: s =>  !s.Prey.IsDead && (s.Target.Side != s.Unit.Side || s.Prey.Side == s.Target.Side && s.Target.Side == s.Unit.Side)),

            //new EventString((i) => "",
            //targetRace: Race.Dragon, priority: 9),

            
        };

        BreastRubMessages = new List<EventString>()
        {
            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} full breasts.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} full breasts, milk leaking out of {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} engorged nipples.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> grabs {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}, lifting them up and then letting them bounce against {GPPHis(i.Unit)} chest, sloshing their contents about!", priority: 8, conditional: s => s.Target == s.Unit),
            new EventString((i) =>$"With a happy sigh, <b>{i.Unit.Name}</b> places {GPPHis(i.Unit)} arms behind {GPPHis(i.Unit)} back and shakes {GPPHis(i.Unit)} torso vigorusly, giving {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} a good stirring. ", priority: 8, conditional: s => s.Target == s.Unit),
            
            //succs
            new EventString((i) =>$"The mere touch of <b>{i.Unit.Name}</b> is enough to make <b>{i.Target.Name}</b> gasp and quiver in pleasure.", actorRace: Race.Succubi, priority: 9, conditional: s=> s.Target.Side == s.Unit.Side),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> moans and hugs <b>{i.Target.Name}</b> tightly and grinds their breasts against one another, sending tingling bliss down both their spines.", actorRace: Race.Succubi, priority: 9, conditional: s=> s.Target.Side == s.Unit.Side && ReqTargetCompatibleLewd(s) && ActorBoobs(s)),
        };

        TailRubMessages = new List<EventString>()
        {
            new EventString((i) =>$"<b>{i.Unit.Name}</b> strokes {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} tail, feeling it clench and squeeze its delicious contents.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> eagerly strokes {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} tail, feeling the tasty burden getting lighter with each stroke.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> soothes the sloshing in {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} tail with a quick rub.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> squeezes {GPPHis(i.Unit)} tail hard and signs contently, feels the large mass finally slither into {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.stomach)}.", priority: 8, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"With a grunt, <b>{i.Unit.Name}</b> pats {GPPHis(i.Unit)} tail to guide the digesting food on its way to {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.stomach)}.", priority: 8, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> shifts {GPPHimself(i.Unit)} sideways so that {GPPHe(i.Unit)} can press down hard on {GPPHis(i.Unit)} tail, shifting it deeper into {GPPHis(i.Unit)} body for digestion.", priority: 8, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> curls back to stroke the bulge on {GPPHis(i.Unit)} tail, slowly guiding the lump deeper inside {GPPHim(i.Unit)}.", priority: 8, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"<b>{i.Unit.Name}</b>'s tail exhales a puff of acrid gas from the steady massages <b>{i.Unit.Name}</b> is giving it. <b>{i.Target.Name}</b> soon finds {GPPHimself(i.Unit)} dunked into strong stomach acids.", priority: 8, conditional:s=> HardVore(s) && CanBurp(s)),

            // Can we do an OR for actorrace? 
            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages each lumpy bump on {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} tail, enjoying each bulge slowly soften and flatten.", actorRace: Race.Vipers, priority: 9),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages each lumpy bump on {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} tail, enjoying each bulge slowly soften and flatten.", actorRace: Race.Lamia, priority: 9),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> swishes {GPPHis(i.Unit)} filled tail against the ground, eager for it to squeeze its prey into {GPPHis(i.Unit)} stomach for digestion.", priority: 9, actorRace: Race.Lamia, conditional:s => s.Target == s.Unit),
            

            //succs
            new EventString((i) =>$"The mere touch of <b>{i.Unit.Name}</b> is enough to make <b>{i.Target.Name}</b> gasp and quiver in pleasure.", actorRace: Race.Succubi, priority: 9),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> passionately caresses the bulging length of {GPPHis(i.Unit)} tail, sending a burst of sensual sensations rippling down {GPPHis(i.Unit)} spine. \"Enjoy the ride my little snack.\" {GPPHe(i.Unit)} coos softly.", actorRace: Race.Succubi, priority: 9, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> stretches {GPPHis(i.Unit)} wings backwards, rubbing the wing span against {GPPHis(i.Unit)} stuffed tail to guide the contents into {GPPHis(i.Unit)} belly.", actorRace: Race.Succubi, priority: 9, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> tucks {GPPHis(i.Unit)} bloated tail between {GPPHis(i.Unit)} legs then grinds {GPPHis(i.Unit)} {(i.Unit.HasDick ? "dick" : "pussy")} depravedly against it for <b>{AttractedWarrior(i.Unit)}</b> to see. \"Come join me my dear!\"", actorRace: Race.Succubi, priority: 9, conditional:s => s.Target == s.Unit && ReqOSWLewd(s)),
        };

        BallMassageMessages = new List<EventString>()
        {
            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages <b>{i.Target.Name}</b>'s full scrotum.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages <b>{i.Target.Name}</b>'s full scrotum, a bit of pre leaking out of {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} erect {PreyLocStrings.ToCockSyn()}.", priority: 8),

            new EventString((i) =>$"<b>{i.Unit.Name}</b> forcefully strokes <b>{i.Target.Name}</b>'s erect cock, either waiting for the prey inside {GPPHis(i.Target)} balls to melt down faster, or for {GPPHim(i.Target)} to spurt it out for <b>{i.Unit.Name}</b> to snatch it.", priority: 9, conditional:s=>s.Target != s.Unit && ReqTargetCompatibleLewd(s)),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> puts <b>{i.Target.Name}</b>'s throbbing member between {GPPHis(i.Unit)} breasts, stroking and squishing it, waiting for release.", priority: 9, conditional: s=> ReqTargetCompatibleLewd(s) && ActorBoobs(s)),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> quickly strokes {(i.Unit == i.Target ? GPPHis(i.Target) : $"<b>{i.Target.Name}</b>'s")} cock, trying to relieve {GPPHis(i.Target)} engorged balls of their contents.", priority: 9, conditional: s=> ReqTargetCompatibleLewd(s) && ActorBoobs(s)),
            //succs
            new EventString((i) =>$"The mere touch of <b>{i.Unit.Name}</b> is enough to make <b>{i.Target.Name}</b> gasp and quiver in pleasure.", actorRace: Race.Succubi, priority: 9),
            new EventString((i) =>$"\"My, my, what a beautiful ensemble we have here~\" coos <b>{i.Unit.Name}</b> straddling <b>{i.Target.Name}</b>'s sloshing balls and stroking {GPPHis(i.Unit)} erection - \"Now, would you let me take care of it?\"", actorRace: Race.Succubi, priority: 9, conditional: ReqTargetCompatibleLewd),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> gives a gentle kiss to <b>{i.Target.Name}</b>'s cockhead, giggling at {GPPHis(i.Unit)} muffled gasp of pleasure.", actorRace: Race.Succubi, priority: 9, conditional: ReqTargetCompatibleLewd),

             new EventString((i) => $"<b>{i.Unit.Name}</b> begins massaging <b>{i.Target.Name}</b>'s full {PreyLocStrings.ToSyn(PreyLocation.balls)}, causing the dragon to groan in pleasure.",
            actorRace: Race.Kobolds,  priority: 10, conditional: s => Lewd(s) && s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),
            new EventString((i) => $"<b>{i.Unit.Name}</b> prods <b>{i.Target.Name}</b>'s full {PreyLocStrings.ToSyn(PreyLocation.stomach)}, {GPPHis(i.Unit)} hands sinking into the organ as he rubs it. The increasing drops of precum falling on {GPPHis(i.Unit)} back and the pleasured groans of the dragon signal to {GPPHim(i.Unit)} that {GPPHe(i.Unit)}'s doing a good job.",
            actorRace: Race.Kobolds,  priority: 10, conditional: s =>  Lewd(s) && s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),
            new EventString((i) => $"<b>{i.Unit.Name}</b> begins massaging <b>{i.Target.Name}</b>'s full {PreyLocStrings.ToSyn(PreyLocation.balls)}, kneading deep and trying to massage every inch {GPPHe(i.Unit)} can. The deep, pleasured rumbling the dragon is making is a sign that the kobold is doing a fantastic job.",
            actorRace: Race.Kobolds,  priority: 10, conditional: s => Lewd(s) && s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),
            new EventString((i) => $"<b>{i.Target.Name}</b> beckons <b>{i.Unit.Name}</b> over, the kobold excitedly complies. <b>{i.Unit.Name}</b> digs {GPPHimself(i.Unit)} onto the outside of the dragon's {PreyLocStrings.ToSyn(PreyLocation.balls)}, rubbing {GPPHis(i.Unit)} head, hands, and entire body on as much of the sack as possible; <b>{i.Target.Name}</b> rumbling in great pleasure the whole time. <b>{i.Unit.Name}</b> might not be a dragon, but after {GPPHe(i.Unit)}'s done, {GPPHe(i.Unit)} sure smells like one.",
            actorRace: Race.Kobolds,  priority: 10, conditional: s => Lewd(s) && s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),

            new EventString((i) => $"<b>{i.Unit.Name}</b> begins massaging <b>{i.Target.Name}</b>'s full balls, causing the dragon to groan in pleasure.",
            actorRace: Race.Kobolds,  priority: 10, conditional: s =>  s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),
            new EventString((i) => $"<b>{i.Unit.Name}</b> prods <b>{i.Target.Name}</b>'s full scrotum, {GPPHis(i.Unit)} hands sinking into the organ as {GPPHe(i.Unit)} rubs it. The increasing drops of precum falling on {GPPHis(i.Unit)} back signal to {GPPHim(i.Unit)} that {GPPHeIsAbbr(i.Unit)} doing a good job.",
            actorRace: Race.Kobolds,  priority: 10, conditional: s =>  s.Target.Race == Race.Dragon || s.Target.Race == Race.EasternDragon),

            new EventString((i) =>$"<b>{i.Unit.Name}</b> massages {(i.Unit == i.Target ? GPPHis(i.Target) : "<b>" + i.Target.Name + "</b>'s")} full scrotum, a bit of pre leaking out of {GPPHis(i.Target)} erect {PreyLocStrings.ToCockSyn()}.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> settles down and kneads {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.balls)}, feeling them slosh with increasing amounts of {PreyLocStrings.ToFluid(PreyLocation.balls)}.", priority: 11, conditional:s => s.Target == s.Unit),
            new EventString((i) =>$"Dropping to the ground, <b>{i.Unit.Name}</b> spreads {GPPHis(i.Unit)} legs and rubs {GPPHis(i.Unit)} thighs against {GPPHis(i.Unit)} {PreyLocStrings.ToCockSyn()}, enjoying the deep burbling within.", priority: 11, conditional:s => s.Target == s.Unit),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> rubs {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.balls)}, {GPPHe(i.Unit)} begins to ponder how big of a puddle {GPPHis(i.Unit)} prey will make.",
            priority: 11, conditional: s => s.Target == s.Unit),

            new EventString((i) => $"<b>{i.Unit.Name}</b> can't believe {GPPHe(i.Unit)} got the honour to rub the balls of the famous {(TargetHumanoid(i) ? "warrior" : "beast")}, <b>{i.Target.Name}</b>.",
            priority: 15, conditional: s => s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"\"Oh gods this is actually happening!\" <b>{i.Unit.Name}</b> thinks to {GPPHimself(i.Unit)} as {GPPHe(i.Unit)} kneads the prey-filled {PreyLocStrings.ToSyn(PreyLocation.balls)} of thier idol, <b>{i.Target.Name}</b>.",
            priority: 15, conditional: s => s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s eyes light up as the great {(TargetHumanoid(i) ? "warrior" : "beast")}, <b>{i.Target.Name}</b>, lets {GPPHim(i.Unit)} rub {GPPHis(i.Target)} prey-filled {PreyLocStrings.ToSyn(PreyLocation.balls)}. ",
            priority: 15, conditional: s => s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"<b>{i.Unit.Name}</b> excitedly rubs <b>{i.Target.Name}</b>'s belly. The famous, battle-hardened {(TargetHumanoid(i) ? "warrior" : "beast")} closing {GPPHis(i.Target)} eyes and groans in pleasure as <b>{i.Target.Name}</b> is clearly doing a good job.",
            priority: 15, conditional: s => s.Unit.Level + 9 < s.Target.Level),
            new EventString((i) => $"<b>{i.Unit.Name}</b> kneads and prods the {PreyLocStrings.ToSyn(PreyLocation.balls)} of <b>{i.Target.Name}</b>'s belly hoping that the famous {(TargetHumanoid(i) ? "warrior" : "beast")} will be impressed with {GPPHis(i.Unit)} work.",
            priority: 15, conditional: s => s.Unit.Level + 9 < s.Target.Level),

            new EventString((i) => $"<b>{i.Unit.Name}</b> excitedly rubs the {PreyLocStrings.ToSyn(PreyLocation.balls)} of <b>{i.Target.Name}</b>. The famous {(TargetHumanoid(i) ? "warrior" : "beast")} groans lustfully and <b>{i.Unit.Name}</b> hopes it's enough to possibly get a 'special reward' later tonight. ",
            priority: 15, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> seductively rubs the {PreyLocStrings.ToSyn(PreyLocation.balls)} of their idol, <b>{i.Target.Name}</b>. <b>{i.Unit.Name}</b> makes sure that {GPPHis(i.Unit)} assets draw the strong {(TargetHumanoid(i) ? "warrior" : "beast")}'s attention and hopes that <b>{i.Target.Name}</b> will think of taking {GPPHim(i.Unit)} tonight.",
            priority: 15, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> eagerly rubs the {PreyLocStrings.ToSyn(PreyLocation.balls)} of <b>{i.Target.Name}</b>, the renowned {(TargetHumanoid(i) ? "warrior" : "beast")} whispers to {GPPHim(i.Unit)}, \"If you keep that up, I might test out my new potency on you tonight~.\" <b>{i.Unit.Name}</b>'s eyes go wide and {GPPHe(i.Unit)} goes back to rubbing with thrice the vigor!",
            priority: 15, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"As <b>{i.Unit.Name}</b> rubs the {PreyLocStrings.ToSyn(PreyLocation.balls)} of <b>{i.Target.Name}</b>, the strong {(TargetHumanoid(i) ? "warrior" : "beast")} whispers something into {GPPHis(i.Unit)} ear. <b>{i.Unit.Name}</b>'s eyes light up as {GPPHe(i.Unit)} begins to rub ever harder, hoping to make their idol's prey churn even faster so {GPPHe(i.Unit)} can spend more time with <b>{i.Unit.Name}</b> alone.",
            priority: 15, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"\"Keep that up, and I'll give you a mating session you will never forget.\" <b>{i.Target.Name}</b> tells <b>{i.Unit.Name}</b> who immediately rubs harder and faster, clearly unable to hide their excitement after hearing that <b>{i.Target.Name}</b> wants to have sex with {GPPHim(i.Unit)}.",
            priority: 15, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> excitedly rubs the {PreyLocStrings.ToSyn(PreyLocation.balls)} of the battle-hardened {(TargetHumanoid(i) ? "warrior" : "beast")}, <b>{i.Target.Name}</b>. The smell of such a powerful {(TargetHumanoid(i) ? "warrior" : "beast")}'s nether regions is too arousing for <b>{i.Unit.Name}</b> and {GPPHis(i.Unit)} other hand drifts down to {GPPHis(i.Unit)} crotch.",
            priority: 15, conditional: s => Lewd(s) && (s.Unit.Level + 9 < s.Target.Level)),

        };

        BreastVoreMessages = new List<EventString>()
        {
            new EventString((i) =>$"<b>{i.Target.Name}</b> lets out a distressed yelp as {GPPHeIs(i.Target)} shoved headfirst into <b>{i.Unit.Name}</b>’s nipple!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> casts aside {GPPHis(i.Unit)} weapon and grabs onto <b>{i.Target.Name}</b>, stuffing {GPPHim(i.Target)} into {GPPHis(i.Unit)} cleavage!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> swiftly jumps on <b>{i.Target.Name}</b>, {GPPHis(i.Target)} form engulfed by <b>{i.Unit.Name}</b>'s breasts!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> onto the ground and shoves {GPPHim(i.Target)} legfirst into {GPPHis(i.Unit)} nipple!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> overpowers <b>{i.Target.Name}</b> and swiftly shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} bust!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} supple, hungry bosom.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b>’s nipples ache after stretching far enough to consume {GPPHis(i.Unit)} rival warrior.", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> holds <b>{i.Target.Name}</b> tightly, preparing to swallow them, but just as {GPPHe(i.Unit)} goes to swallow them, {GPPHe(i.Unit)} sees {GPPHis(i.Unit)} prey disappearing into {GPPHis(i.Unit)} engorged cleavage.", priority: 8),
            //Slime pred
            new EventString((i) => $"<b>{i.Unit.Name}</b> launches {GPPHimself(i.Unit)} at <b>{i.Target.Name}</b>, engulfing them in {GPPHis(i.Unit)} bosom!", actorRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> seems to hug <b>{i.Target.Name}</b>, engulfing them in {GPPHis(i.Unit)} chest!", actorRace: Race.Slimes, priority: 10),

            //Slime prey
            new EventString((i) => $"<b>{i.Unit.Name}</b> reaches for <b>{i.Target.Name}</b>’s core, shoving it into {GPPHis(i.Unit)} cleavage!", targetRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b>’s viscous form and slurps {GPPHim(i.Target)} into {GPPHis(i.Unit)} nipples!", targetRace: Race.Slimes, priority: 10),

            new EventString((i) => $"<b>{i.Unit.Name}</b> and <b>{i.Target.Name}</b> grapple each other tightly, grinding their breasts hard against one another until <b>{i.Target.Name}</b> finds sinks into <b>{i.Unit.Name}</b>'s bosom!", priority: 8, conditional: s => ActorBoobs(s) && TargetBoobs(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> presses {GPPHis(i.Unit)} breasts over <b>{i.Target.Name}</b>'s head, muffling the voice of the leader of the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)} and absorbing {GPPHim(i.Unit)} into the hungry breast flesh.", priority: 8, conditional: s => Lewd(s) && TargetLeader(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> stuffs <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} boobs.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sucks <b>{i.Target.Name}</b> up {GPPHis(i.Unit)} breasts, the engorged bosom jiggling as prey struggles inside.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"Not a moment after being sucked in, <b>{i.Target.Name}</b> is already plotting {GPPHis(i.Target)} escape from <b>{i.Unit.Name}</b>'s bosom.",priority:25, conditional: HasGreatEscape),
        };

        CockVoreMessages = new List<EventString>()
        {
            //Generic messages
            new EventString((i) => $"<b>{i.Target.Name}</b> lets out a distressed yelp as {GPPHeIs(i.Target)} shoved headfirst into <b>{i.Unit.Name}</b>’s penis!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> casts aside {GPPHis(i.Unit)} weapon and grabs onto <b>{i.Target.Name}</b>, stuffing {GPPHim(i.Target)} into {GPPHis(i.Unit)} testicles!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> swiftly jumps on <b>{i.Target.Name}</b>, {GPPHis(i.Target)} form engulfed by <b>{i.Unit.Name}</b>'s cock!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> onto the ground and shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} dick!",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> overpowers <b>{i.Target.Name}</b> and swiftly shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} balls!",
            priority: 8),
            new EventString((i) => $"Grunting with each throb, <b>{i.Unit.Name}</b> sees <b>{i.Target.Name}</b> disappear further and further into {GPPHis(i.Unit)} cock.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lewdly sways {GPPHis(i.Unit)} hips, giving friend and foe alike the sight of <b>{i.Target.Name}</b>'s body disappearing into into {GPPHis(i.Unit)} cock.",
            priority: 8),
            new EventString((i) => $"Tired of <b>{i.Target.Name}</b>'s struggling, <b>{i.Unit.Name}</b> grabs {GPPHim(i.Target)} by {GPPHis(i.Target)} shoulders and swiftly humps <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} cock.",
            priority: 8),
            new EventString((i) => $"Just bringing <b>{i.Target.Name}</b> close enough to glans causes <b>{i.Unit.Name}</b>'s cock to swell and instantly swallow {GPPHis(i.Unit)} prey.",
            priority: 8),

            //Slime pred
            new EventString((i) => $"<b>{i.Unit.Name}</b> launches {GPPHimself(i.Unit)} at <b>{i.Target.Name}</b>, engulfing {GPPHim(i.Target)} in {GPPHis(i.Unit)} testicles!",
            actorRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> cascades over <b>{i.Target.Name}</b>'s body, sucking {GPPHim(i.Target)} into {GPPHis(i.Unit)} dick!",
            actorRace: Race.Slimes, priority: 10),
            new EventString((i) => $" Everyone on the battlefield gets a good view of <b>{i.Target.Name}</b> sliding through translucent member of <b>{i.Unit.Name}</b> into {GPPHis(i.Unit)} balls.",
            actorRace: Race.Slimes, priority: 10),
            new EventString((i) => $"With a naughty grin, <b>{i.Unit.Name}</b> swells {GPPHis(i.Unit)} dick to grotesque proportions and slams it down on <b>{i.Target.Name}</b>, stunning and absorbing them through it.",
            actorRace: Race.Slimes, priority: 10),

            //Slime prey
            new EventString((i) => $"<b>{i.Unit.Name}</b> reaches for <b>{i.Target.Name}</b>’s core, shoving it into {GPPHis(i.Unit)} penis!",
            targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b>’s viscous form and slurps {GPPHim(i.Target)} into {GPPHis(i.Unit)} dick!",
            targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shoves {GPPHis(i.Unit)} dick into <b>{i.Target.Name}</b>. Before <b>{i.Target.Name}</b> could react, <b>{i.Unit.Name}</b> slurps {GPPHim(i.Target)} up {GPPHis(i.Unit)} throbbing member.",
            targetRace: Race.Slimes, priority: 9, conditional: Lewd),

            new EventString((i) => $"<b>{i.Unit.Name}</b> smirks at <b>{i.Target.Name}</b>'s fearful face before {GPPHe(i.Unit)} crams the so-called leader down {GPPHis(i.Unit)} mighty cock.",
            priority: 8, conditional: s => TargetLeader(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs the shrunken form of <b>{i.Target.Name}</b> and holds {GPPHim(i.Target)} tight against {GPPHis(i.Unit)} crotch. <b>{i.Unit.Name}</b> makes sure that the struggling {GetRaceDescSingl(i.Target)} gets good whiff before bringing {GPPHim(i.Target)} up to {GPPHis(i.Unit)} shaft and sticking {GPPHim(i.Target)} inside.",
            priority: 11, conditional: s => Shrunk(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs the shrunken form of <b>{i.Target.Name}</b>, getting all sorts of dirty thoughts of what to do with {GPPHim(i.Target)}. <b>{i.Unit.Name}</b> gets an idea and holds the wiggling {GetRaceDescSingl(i.Target)} against {GPPHis(i.Unit)} length, stroking up and down for a bit before shoving <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} dick.",
            priority: 11, conditional: s => Shrunk(s) && Lewd(s)),
            new EventString((i) => $"A shrunken <b>{i.Target.Name}</b> screams in horror as the massive hand of <b>{i.Unit.Name}</b> envelops {GPPHis(i.Target)} entire body. <b>{i.Unit.Name}</b> smirks and gives the scared {GetRaceDescSingl(i.Target)} a good look at {GPPHis(i.Unit)} member before stuffing {GPPHim(i.Target)} inside.",
            priority: 11, conditional: Shrunk),

            new EventString((i) => $"<b>{i.Unit.Name}</b> stuffs <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} cock.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"In a humping frenzy accompanying <b>{i.Target.Name}</b>'s descent into <b>{i.Unit.Name}</b>'s gurgling balls', <b>{i.Unit.Name}</b> doesn't notice <b>{i.Target.Name}</b>'s smug expression. \"Enjoy it while it lasts\" - the prey says disappearing inside.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s shaft greedily consumes <b>{i.Target.Name}</b>, {GPPHis(i.Target)} form engorging the pred's balls. <b>{i.Target.Name}</b> settles within and starts probing the walls of {GPPHis(i.Target)} fleshy prison, looking for a way out.",priority:25, conditional: HasGreatEscape),

        };

        AnalVoreMessages = new List<EventString>()
        {
           new EventString((i) => $"<b>{i.Unit.Name}</b> leaps high into the air before coming down onto <b>{i.Target.Name}</b> ass-first, slurping the terrified prey up {GPPHis(i.Unit)} tailhole.", priority: 8),
           new EventString((i) => $"<b>{i.Target.Name}</b> leaves skidmarks on the ground as {GPPHe(i.Target)} is being pulled up <b>{i.Unit.Name}</b>’s booty.", priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> clenches <b>{i.Target.Name}</b>’s head between {GPPHis(i.Unit)} asscheeks and pulls {GPPHim(i.Target)} in.", priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> sits on <b>{i.Target.Name}</b>’s face, letting the lucky prey feel {GPPHis(i.Unit)} plump booty before quickly pulling them inside.", priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> smooshes <b>{i.Target.Name}</b>’s face under {GPPHis(i.Unit)} rump, grinding indulgently and working {GPPHim(i.Target)} into {GPPHis(i.Unit)} anus among muffled protests, before getting up and stretching {GPPHis(i.Unit)} butt in the air, letting gravity do the rest.", priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> baits a sneak attack from <b>{i.Target.Name}</b>, only to let {GPPHim(i.Target)} collide with {GPPHis(i.Unit)} gaping tailhole, which consumes {GPPHim(i.Target)} in a series of powerful anal clenches.", priority: 8, conditional: s => ActorTail(s)),
           new EventString((i) => $"<b>{i.Target.Name}</b> is pulled screaming and thrashing into <b>{i.Unit.Name}</b>’s behind!",priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> lies on the ground watching <b>{i.Target.Name}</b> disappear up {GPPHis(i.Unit)} ass.",priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> knocks <b>{i.Target.Name}</b> onto the ground and swiftly shoves the terrified prey’s lower body up {GPPHis(i.Unit)} ass. After a few contractions, {i.Unit.Name} leans over, caressing their now enormous belly.", priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> slides <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.anal)}!", priority: 8),
           new EventString((i) => $"<b>{i.Unit.Name}</b> backs up into <b>{i.Target.Name}</b> until {GPPHe(i.Target)} trip{SIfSingular(i.Target)}, dooming {GPPHim(i.Target)} to getting face-first engulfed in ass.", priority: 8),
           new EventString((i) => $"Once <b>{i.Target.Name}</b> finds {GPPHimself(i.Target)} immobilized, {GPPHe(i.Target)} can only watch in fear as <b>{i.Unit.Name}</b>'s sphincter slurps up {GPPHis(i.Target)} body like a noodle", priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> looks up at <b>{i.Unit.Name}</b>'s pucker in awe, realizing {GPPHe(i.Target)} want{SIfSingular(i.Target)} to slip up that hole so bad – which makes {GPPHim(i.Target)} rather cooperative when the butt thwumps down.", priority: 10, conditional: s=> Cursed(s) && Shrunk(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sits down on the shrunken <b>{i.Target.Name}</b> and grinds {GPPHis(i.Unit)} butt on the ground. After {GPPHe(i.Unit)} stand{SIfSingular(i.Unit)} up, <b>{i.Target.Name}</b> is nowhere to be seen.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sits down on the shrunken <b>{i.Target.Name}</b> and clenches {GPPHis(i.Unit)} ass hard. After {GPPHe(i.Unit)} stands up, <b>{i.Target.Name}</b> is nowhere to be seen.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $"<b>{i.Unit.Name}</b> unceremoniously turns around and thwumps down on the shrunken <b>{i.Target.Name}</b>, making {GPPHim(i.Target)} slip up into {GPPHis(i.Unit)} anal depths.",
            priority: 10, conditional: Shrunk),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs the shrunken <b>{i.Target.Name}</b> {(ActorHumanoid(i) ? "" : "with a swift tail motion ")}and brings {GPPHim(i.Target)} to {GPPHis(i.Unit)} ass. Using {(ActorHumanoid(i) ? "a finger" : "the tail tip")}, {GPPHe(i.Unit)} shove{SIfSingular(i.Unit)} the struggling {GetRaceDescSingl(i.Target)} inside.",
            priority: 10, conditional: s => Shrunk(s) && (ActorTail(s) || ActorHumanoid(s))),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs the shrunken <b>{i.Target.Name}</b> {(ActorHumanoid(i) ? "" : "with a swift tail motion ")}and brings {GPPHim(i.Target)} to {GPPHis(i.Unit)} ass, shoving {GPPHim(i.Target)} halfway inside {GPPHis(i.Unit)} hole. Using {(ActorHumanoid(i) ? "a finger" : "the tail tip")}, {GPPHe(i.Unit)} shove{SIfSingular(i.Unit)} the struggling {GetRaceDescSingl(i.Target)} inside completely.",
            priority: 10, conditional: s => Shrunk(s) && (ActorTail(s) || ActorHumanoid(s))),
            new EventString((i) => $"<b>{i.Unit.Name}</b> reaches down and grabs the shrunken <b>{i.Target.Name}</b> and holds {GPPHim(i.Target)} in front of {GPPHis(i.Unit)} ass. <b>{i.Unit.Name}</b> lets the 'lucky' {GetRaceDescSingl(i.Target)} get a nice long view of {GPPHis(i.Unit)} assets before shoving {GPPHim(i.Target)} into {GPPHis(i.Unit)} hole.",
            priority: 10, conditional: s => Shrunk(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> squats over <b>{i.Target.Name}</b> and stuffs {GPPHim(i.Target)} into {GPPHis(i.Unit)} ass. <b>{i.Target.Name}</b> takes notice on how stretchy and easy to slide through it is.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"\"Wanna go out the same way you went in?\"- asks <b>{i.Unit.Name}</b> {GPPHis(i.Unit)} prey smugly. <b>{i.Target.Name}</b> shrugs and dives up. if <b>{i.Unit.Name}</b> wants it, so be it.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"Not the one to reject free lodging, <b>{i.Target.Name}</b> climbs up <b>{i.Unit.Name}</b>'s ass with vigor, prompting gasps of pleasure from his predator.",priority:25, conditional: s => HasGreatEscape(s) && Cursed(s)),
        };

        UnbirthMessages = new List<EventString>()
        {
            new EventString((i) =>$"<b>{i.Target.Name}</b> lets out a distressed yelp as {GPPHeIs(i.Target)} shoved headfirst into <b>{i.Unit.Name}</b>’s vagina!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> casts aside {GPPHis(i.Unit)} weapon and grabs onto <b>{i.Target.Name}</b>, stuffing {GPPHim(i.Target)} into {GPPHis(i.Unit)} womb!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> swiftly jumps on <b>{i.Target.Name}</b>, {GPPHis(i.Target)} form engulfed by <b>{i.Unit.Name}</b>'s pussy!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> onto the ground and shoves {GPPHim(i.Target)} legfirst into {GPPHis(i.Unit)} vulva!", priority: 8),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> overpowers <b>{i.Target.Name}</b> and swiftly shoves {GPPHim(i.Target)} into {GPPHis(i.Unit)} womb!", priority: 8),
            new EventString((i) =>$"In the heat of {GPPHis(i.Unit)} needs, <b>{i.Unit.Name}</b> desides to use <b>{i.Target.Name}</b> as a living dildo – Unfortunately, {GPPHis(i.Unit)} toys have a tendency to disappear.", priority: 8, conditional: Lewd),

            //Shrunken Prey
            new EventString((i) => $"<b>{i.Unit.Name}</b> unceremoniously turns around and thwumps down on the shrunken <b>{i.Target.Name}</b>, making {GPPHim(i.Target)} slip up into {GPPHis(i.Unit)} wet folds.",
            priority: 9, conditional: Shrunk),
            
            //Abakhanskya 
            new EventString((i) => $"Feeling lewd, and dominant, <b>{i.Unit.Name}</b> looks over a helpless <b>{i.Target.Name}</b>. It wasn't long until {GPPHeIsAbbr(i.Target)} grappled by the dragoness, and shoved in headfirst through her slit, <b>{i.Unit.Name}</b> cooing softly throughout the i.", actorRace: Race.Abakhanskya, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b>, wishing to play with her food, spins herself about on the spot, grabbing her prey with her tail and sending <b>{i.Target.Name}</b> on a trip to her womb, for {GPPHis(i.Target)} demise.", actorRace: Race.Abakhanskya, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> feels an erotic itch needing to be scratched, and with <b>{i.Target.Name}</b> in front of her, it wasn't long until {GPPHeIsAbbr(i.Target)} used to scratch it. Up and through her slit {GPPHe(i.Target)} went, legs first!", actorRace: Race.Abakhanskya, priority: 10),
            new EventString((i) => $"After having sensed she had gained the advantage, <b>{i.Unit.Name}</b> seals <b>{i.Target.Name}</b> away lewdly, taking {GPPHim(i.Target)} in head first into her womb, gasping in delight as {GPPHe(i.Target)} passed through her slit.", actorRace: Race.Abakhanskya, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> stands perfectly still for a moment, menacingly overlooking her fearful prey. Quickly feigning an incoming swallow, she instead rotates to show her rump and sits down upon <b>{i.Target.Name}</b>, causing {GPPHim(i.Target)} to pass straight in to her womb.", actorRace: Race.Abakhanskya, priority: 10),
            
            //Slime Pred
            new EventString((i) => $"<b>{i.Unit.Name}</b> launches {GPPHimself(i.Unit)} at <b>{i.Target.Name}</b>, engulfing {GPPHim(i.Target)} in {GPPHis(i.Unit)} womb!", actorRace: Race.Slimes, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> cascades over <b>{i.Target.Name}</b>'s body, sucking {GPPHim(i.Target)} into {GPPHis(i.Unit)} womb!", actorRace: Race.Slimes, priority: 10),
            
            //Slime Prey
            new EventString((i) => $"<b>{i.Unit.Name}</b> reaches for <b>{i.Target.Name}</b>’s core, shoving it into {GPPHis(i.Unit)} vulva!", targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b>’s viscous form and slurps {GPPHim(i.Target)} into {GPPHis(i.Unit)} pussy!", targetRace: Race.Slimes, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> stuffs <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} pussy.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shoves <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} pussy and goes back to battle, unbothered by cries of protest from within.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"After a short struggle <b>{i.Unit.Name}</b> is now sporting a fat belly, {GPPHis(i.Unit)} womb full of surprisingly stubborn  <b>{i.Target.Name}</b>. \"Some people just refuse to admit they lost\" - sighs <b>{i.Unit.Name}</b>.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"\" Want to be my baby for a while?\" - smugly asks <b>{i.Unit.Name}</b> her half-unbirthed quarry. \"Sure, for a while\" - equally smugly answers <b>{i.Target.Name}</b>.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"You'd think the prospect of being melted into cum would shaken <b>{i.Target.Name}</b>'s spirit, but {GetRaceDescSingl(i.Target)} is surprisingly calm, already calculating {GPPHis(i.Target)} escape.",priority:25, conditional: HasGreatEscape),
        };

        TailVoreMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Unit.Name}</b> catches <b>{i.Target.Name}</b> off guard and crams {GPPHim(i.Target)} down {GPPHis(i.Unit)} tail.", priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slurps up <b>{i.Target.Name}</b> with {GPPHis(i.Unit)} tail!", priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> yanks <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} hungry tail maw.", priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b>'s struggles are for naught as {GPPHe(i.Target)} vanishes into <b>{i.Unit.Name}</b>'s ravenous tail.", priority: 8),

            new EventString((i) => $"<b>{i.Unit.Name}</b> blows <b>{i.Target.Name}</b> a kiss before sliding the tip of {GPPHis(i.Unit)} tail over <b>{i.Target.Name}</b>'s head and slurping the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)} up!", actorRace: Race.Succubi, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> distracts <b>{i.Target.Name}</b> with a tight sensual hug, just enough for {GPPHis(i.Unit)} to suck <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} tail!", actorRace: Race.Succubi, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> flutters over <b>{i.Target.Name}</b> and causally drops {GPPHis(i.Unit)} tail down far enough to vaccum <b>{i.Target.Name}</b> into the hungry opening.", actorRace: Race.Succubi, priority: 9),
            new EventString((i) => $"<b>{i.Target.Name}</b> finds {GPPHimself(i.Target)} stunned with steamy thoughts when <b>{i.Unit.Name}</b> blows {GPPHim(i.Target)} a sensuous kiss; long enough for <b>{i.Unit.Name}</b> to plunge {GPPHim(i.Target)} into <b>{i.Unit.Name}</b>'s hungry tail.", actorRace: Race.Succubi, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> flashes {GPPHis(i.Unit)} {(i.Unit.HasDick ? "dick" : "breasts")} at <b>{i.Target.Name}</b>, distracting {GPPHim(i.Target)} long enough to swallow the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)} feet first with {GPPHis(i.Unit)} tail.", actorRace: Race.Succubi, priority: 9, conditional: s => Lewd(s) && TargetHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> seizes <b>{i.Target.Name}</b> in a tight hug, grinding {GPPHis(i.Unit)} {(i.Unit.HasDick ? "dick" : "breasts")} against <b>{i.Target.Name}</b> before pulling <b>{i.Target.Name}</b> down into the awaiting tail maw.", actorRace: Race.Succubi, priority: 9, conditional: s => Lewd(s) && TargetHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slams {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.anal)} down on <b>{i.Target.Name}</b>'s face, smothering <b>{i.Target.Name}</b> while {GPPHe(i.Unit)} devour{SIfSingular(i.Unit)} with <b>{i.Unit.Name}</b>'s tail maw.", actorRace: Race.Succubi, priority: 9, conditional: s => Lewd(s) && TargetHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> coils {GPPHimself(i.Unit)} around <b>{i.Target.Name}</b>, feeding {GPPHim(i.Unit)} into the greedy hungry tail mouth!", actorRace: Race.Vipers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s tail lashes out with a loud crack and <b>{i.Target.Name}</b> promptly finds {GPPHimself(i.Target)} slithering into its long hot slimy depths.", actorRace: Race.Vipers, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> grabs <b>{i.Target.Name}</b> and stuffs {GPPHim(i.Target)} down into an awaiting {PreyLocStrings.ToSyn(PreyLocation.womb)}. <b>{i.Target.Name}</b> quickly slides down it, not into a womb but into <b>{i.Unit.Name}</b>'s voracious tail stomach.", actorRace: Race.Lamia, priority: 9, conditional: s => Lewd(s) && s.Unit.GetGender() == Gender.Female),
            new EventString((i) => $"<b>{i.Target.Name}</b> finds {GPPHimself(i.Target)} trapped in <b>{i.Unit.Name}</b>'s crushing tail coils and the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)} is devoured by the hungry tail maw.", actorRace: Race.Lamia, priority: 9),
            new EventString((i) => $"With a loud thud, <b>{i.Target.Name}</b> is knocked over by <b>{i.Unit.Name}</b>'s lashing tail then reeled into its greedy tail maw!", actorRace: Race.Lamia, priority: 9),

            new EventString((i) => $"<b>{i.Unit.Name}</b> latches {GPPHis(i.Unit)} stinger into <b>{i.Target.Name}</b> and uses it like a straw to drink the {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)} down!", actorRace: Race.Bees, targetRace: Race.Slimes, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s tail tip expands wide, engulfing <b>{i.Target.Name}</b> and trapping {GPPHim(i.Target)} in <b>{i.Unit.Name}</b> honey filled abdomen!", actorRace: Race.Bees, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lunges {GPPHim(i.Unit)} stinger rapidly, jabbing it into <b>{i.Target.Name}</b>. While <b>{i.Target.Name}</b> reels from the pain, the stinger widens to a huge cone and {PreyLocStrings.GetOralVoreVPT()} {GPPHim(i.Target)}!", actorRace: Race.Bees, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> stings <b>{i.Target.Name}</b> and it swiftly swells wide enough to swallow up <b>{i.Target.Name}</b> whole!", actorRace: Race.Bees, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> throws {GPPHimself(i.Unit)} butt first at <b>{i.Target.Name}</b> with an armed tail stinger that wraps around <b>{i.Target.Name}</b> and swallows {GPPHim(i.Target)} upon impact!", actorRace: Race.Bees, priority: 9),

            new EventString((i) => $"<b>{i.Target.Name}</b>'s world goes dark as <b>{i.Unit.Name}</b>'s rapacious tail descends on <b>{i.Target.Name}</b>'s head, engulfing {GPPHim(i.Target)} completely.", priority: 9, conditional:TargetHumanoid),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s tail tip expands around <b>{i.Target.Name}</b>'s legs and reels {GPPHim(i.Target)} down greedily.", priority: 9, conditional:TargetHumanoid),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pushes <b>{i.Target.Name}</b> over and snickers as {GPPHis(i.Unit)} tail consumes {GPPHis(i.Unit)} prey.", priority: 9, conditional:TargetHumanoid),

            new EventString((i) => $"<b>{i.Target.Name}</b>'s struggling form is seen through <b>{i.Unit.Name}</b>'s crop as they slide down into terror bird's waiting stomach", actorRace: Race.Terrorbird, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is struggling to swallow <b>{i.Target.Name}</b> in a single bite, and so waits until {GetPreyDesc(i.Target)} {GetRaceDescSingl(i.Target)} tires themselves out", actorRace: Race.Terrorbird, priority: 10),
            new EventString((i) => $"A brief scream sounds through the battlefield and the only reminder of <b>{i.Target.Name}</b> ever being there is <b>{i.Unit.Name}</b>'s bulging crop", actorRace: Race.Terrorbird, priority: 10),
            new EventString((i) => $"Every time <b>{i.Unit.Name}</b> opens their mouth, anyone can see <b>{i.Target.Name}</b>'s terrified face, still struggling to get out of terror bird's esophagus", actorRace: Race.Terrorbird, priority: 10),

            new EventString((i) => $"<b>{i.Unit.Name}</b> stuffs <b>{i.Target.Name}</b> into {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Target.Name}</b> worries surprisingly little about being shoved into <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)}.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)} welcomes another guest, this one livelier than the most.",priority:25, conditional: HasGreatEscape),
    };

        DigestionDeathMessages = new List<EventString>()
        {
            //Generic strings
            new EventString((i) => $"The muffled protests in <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} grow silent, replaced with content gurgling. <b>{i.Target.Name}</b> is dead.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} churns, its content finally turning into {i.preyLocation.ToFluid()} for the predator.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} contracts, turning <b>{i.Target.Name}</b> into {i.preyLocation.ToFluid()}.",
            priority: 8),
            new EventString((i) => $"Inside <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}, <b>{i.Target.Name}</b> finally falls silent.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} kills off <b>{i.Target.Name}</b>. The remaining sludge bubbles in the pit of {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> smiles as {GPPHe(i.Unit)} feel{SIfSingular(i.Unit)} {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} burble pleasurably, <b>{i.Target.Name}</b> having slipped under the acidic tides.",
            priority: 8),
            new EventString((i) => $"As <b>{i.Target.Name}</b>’s struggles cease, prompting an agitated, \"Wait, was that it, really?\" from <b>{i.Unit.Name}</b> as {GPPHe(i.Unit)} pokes and prods at {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}.",
            priority: 8),
            new EventString((i) => $"After a final desperate struggle that leaves <b>{i.Unit.Name}</b> sweating, <b>{i.Target.Name}</b> finds all {GPPHis(i.Target)} energy drained and soon succumbs to the convulsions of the victorious {PreyLocStrings.ToSyn(i.preyLocation)}. ",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sighs. Even though {GPPHe(i.Unit)} just ate a whole warrior, {GPPHe(i.Unit)} knows {GPPHe(i.Unit)}’ll just be hungry again ten minutes later.",
            priority: 8),
            new EventString((i) => $"As the digestion process finally finishes, <b>{i.Unit.Name}</b> compliments {GPPHis(i.Unit)} resilient prey for being so hardy.",
            priority: 8),
            new EventString((i) => $"<b>{i.Unit.Name}</b> breathes a sigh of relief as {GPPHis(i.Unit)} violently wobbling {PreyLocStrings.ToSyn(i.preyLocation)} finally stop moving. <b>{i.Target.Name}</b> simply can’t continue the fight and allows nature to take its course.",
            priority: 8),
            new EventString((i) => $"As <b>{i.Target.Name}</b> is converted into a nutritious stew in the pit of {GPPHis(i.Unit)} stomach, <b>{i.Unit.Name}</b> hopes {GPPHe(i.Unit)}’ll still be given rations after the battle.",
            priority: 8, conditional: InStomach),
            //Cum digestion - works, but need to actually bring the whole absorption thing here
            //new EventString((i) => $"BEPIS",
            //priority: 9, conditional: InBalls),
            //Scat strings
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} contracts, sending <b>{i.Target.Name}</b>'s remains towards {GPPHis(i.Unit)} rectum to be released as fresh feces.",
            priority: 9, conditional: s=> Scat(s) && InStomach(s)),
            //Burp strings
            new EventString((i) => $"<b>{i.Unit.Name}</b> looses a lewd, wet burp as <b>{i.Target.Name}</b> begins to pump into {GPPHis(i.Unit)} rumbling bowels.",
            priority: 9, conditional: s=> Scat(s) && CanBurp(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lets out a rolling belch, feeling <b>{i.Target.Name}</b> perish within {GPPHim(i.Unit)} as it's released.",
            priority: 9, conditional: s=> CanBurp(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> wraps {GPPHis(i.Unit)} arms around {GPPHis(i.Unit)} swollen midsection and squeezes hard, causing {GPPHim(i.Unit)} to let out a rolling belch. The burp soon deprives the belly chamber of air, causing <b>{i.Target.Name}</b> to pass out face first into the roaring acids.",
            priority: 9, conditional: s=> HardVore(s) && ActorHumanoid(s) && CanBurp(s) && InStomach(s)),
            //Hard strings
            new EventString((i) => $"With the final contraction, <b>{i.Target.Name}</b> falls apart into a bunch of {i.preyLocation.ToFluid()}.",
            priority: 9, conditional:HardVore),
            new EventString((i) => $"<b>{i.Target.Name}</b> punches the walls of {GPPHis(i.Target)} squishy prison but much to {GPPHis(i.Target)} dismay, <b>{i.Unit.Name}</b>’s stomach fights back, filling itself with bubbling acid which swiftly turns the warrior into a goopy mess.",
            priority: 9, conditional: s=> HardVore(s) && InStomach(s)),
            new EventString((i) => $"Not wanting to experience the horror of digestion, <b>{i.Target.Name}</b> draws a small blade and plunges it into {GPPHis(i.Target)} heart. <b>{i.Unit.Name}</b> is surprised by the sudden ceasing of movement but just gives {GPPHis(i.Unit)} happy tummy a pat as it works.",
            priority: 9, conditional: s=> HardVore(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>’s stomach clenches, breaking <b>{i.Target.Name}</b>’s stance and plunging {GPPHim(i.Target)} into the acids frothing about the bottom of {GPPHis(i.Unit)} belly.",
            priority: 9, conditional: s=> HardVore(s) && InStomach(s)),
            new EventString((i) => $"The ripples and convulsions across <b>{i.Unit.Name}</b>’s gurgling stomach die down as <b>{i.Target.Name}</b> takes a deep breath of acid, leaving a smooth, calm bulge.",
            priority: 9, conditional: s=> HardVore(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> tries to survive by pressing {GPPHimself(i.Target)} against the top stomach walls but {GPPHis(i.Target)} efforts are in vein as <b>{i.Unit.Name}</b>’s voracious gut is fully filled with flesh eating acid.",
            priority: 9, conditional: s=> HardVore(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> manages to avoid the deadly stomach acids for a time but <b>{i.Unit.Name}</b>, frustrated with {GPPHis(i.Target)} fighting grabs the side of {GPPHis(i.Unit)} tummy and shakes it violently, splashing the unsuspecting prey with powerful juices.",
            priority: 9, conditional: s=> HardVore(s) && InStomach(s)),
            //Humanoid pred
            new EventString((i) => $"\"Shh, it’s okay. You’re right where you belong,\" <b>{i.Unit.Name}</b> says soothingly while stroking {GPPHis(i.Unit)} belly. <b>{i.Target.Name}</b>, too weak to fight by this point, gives in to the loving words and dies with a final tummy wobble.",
            priority: 9, conditional: ActorHumanoid),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tries to draw out the process of digesting {GPPHis(i.Unit)} wiggling prey a little longer, but {GPPHis(i.Unit)} voracious {i.preyLocation.ToSyn()} overcome{PluralForPart(i.preyLocation)} the living meal before {GPPHe(i.Target)} can stop it.",
            priority: 9, conditional: ActorHumanoid),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grows tired of {GPPHis(i.Unit)} meal’s incessant struggling and decides to stop playing with {GPPHis(i.Unit)} food. <b>{i.Target.Name}</b> doesn’t stand a chance as the well trained {i.preyLocation.ToSyn()} begin{PluralForPart(i.preyLocation)} to convulse and fill with juices as the charade ends.",
            priority: 9, conditional: ActorHumanoid),
            //Friendly prey
            new EventString((i) => $"<b>{i.Unit.Name}</b> can’t stand to look at {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} as it finishes off {GPPHis(i.Unit)} comrade.",
            priority: 9, conditional: s=> Friendly(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> realizes what {GPPHeIsAbbr(i.Unit)} done and tries to expel {GPPHis(i.Unit)} ally but it’s too late, <b>{i.Target.Name}</b> is already thigh fat.",
            priority: 9, conditional: s=> Friendly(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lifts a leg, making {GPPHis(i.Unit)} full belly wobble angrily. {GPPHe(i.Unit)} smile{SIfSingular(i.Unit)} as {GPPHis(i.Unit)} promotion is all but assured with <b>{i.Target.Name}</b> out of the way.",
            priority: 9, conditional: s=> Friendly(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> laughs. With <b>{i.Target.Name}</b> out of the way, everything will become much easier for {GPPHim(i.Unit)}.",
            priority: 9, conditional: s=> Friendly(s) && ActorHumanoid(s)),
            //vagrant pred
            new EventString((i) => $"The silhouette of <b>{i.Target.Name}</b> inside <b>{i.Unit.Name}</b> loses coherency and dissolves into {i.preyLocation.ToFluid()}.",
            actorRace: Race.Vagrants, priority: 10),
            new EventString((i) => $"<b>{i.Unit.Name}</b> stops wobbling, the prey inside pacified forever.",
            actorRace: Race.Vagrants, priority: 10),
            new EventString((i) => $"As <b>{i.Unit.Name}</b>'s body flattens, its prey softening into {i.preyLocation.ToFluid()}, it spits out the only thing left of <b>{i.Target.Name}</b> -{GPPHis(i.Target)} clothes.",
            actorRace: Race.Vagrants, priority: 10, conditional: ReqTargetClothingOn),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s body flattens, its prey softening into {i.preyLocation.ToFluid()}, nothing remains of <b>{i.Target.Name}</b>",
            actorRace: Race.Vagrants, priority: 10, conditional: ReqTargetClothingOff),
            //Slime pred
            new EventString((i) => $"The silhouette of <b>{i.Target.Name}</b> inside <b>{i.Unit.Name}</b> loses coherency and dissolves into slime.",
            actorRace: Race.Slimes, priority: 8),
            //Panther pred
            new EventString((i) => $"A final rumble from <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} signals that <b>{i.Target.Name}</b> has passed from this world to the next. ",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"Resting a hand on {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}, <b>{i.Unit.Name}</b> can feel <b>{i.Target.Name}</b>’s final motions, as {GPPHis(i.Unit)} juices dissolve {GPPHim(i.Target)}, letting <b>{i.Target.Name}</b> join the spirits in the next life.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grows tired of waiting for {GPPHis(i.Unit)} lazy {i.preyLocation.ToSyn()} to finish <b>{i.Target.Name}</b> off. Deciding to speed the process along, <b>{i.Unit.Name}</b> leans on {GPPHis(i.Unit)} sloshing {i.preyLocation.ToSyn()}, slathering {GPPHis(i.Unit)} meal with juices. A sudden muffled scream followed by stillness ensures the fight to be over.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"/“You’ve been fighting for a long time in there, maybe I’ll get you back to town to sell you off as a sla-/” <b>{i.Unit.Name}</b> says before {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} make{PluralForPart(i.preyLocation)} such thoughts moot by finishing the meal off.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"A belch that shortly turns into a gleeful roar signals the end of <b>{i.Target.Name}</b> and the victory of the tribal warrior.",
            actorRace: Race.Panthers, priority: 9, conditional: s=> InStomach(s) && CanBurp(s)),
            new EventString((i) => $"Despite a few weak motions within {GPPHis(i.Unit)} softening {i.preyLocation.ToSyn()}, <b>{i.Unit.Name}</b> can already feel the strength of {GPPHis(i.Unit)} meal surge into {GPPHis(i.Unit)} body. <b>{i.Target.Name}</b> is too far gone now.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> gives an admiring rub to {GPPHis(i.Unit)} furred {i.preyLocation.ToSyn()} as <b>{i.Target.Name}</b>’s defiant struggle comes to an end.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is loath to admit it, but as {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} settle{PluralForPart(i.preyLocation)} and begin{PluralForPart(i.preyLocation)} to soften, {GPPHe(i.Unit)}’ll miss the energetic struggles of {GPPHis(i.Unit)} meal.",
            actorRace: Race.Panthers, priority: 9),
            new EventString((i) => $"<b>{i.Unit.Name}</b> can’t help but rub {GPPHis(i.Unit)} dick and curse with delight as <b>{i.Target.Name}</b> attempts a final desperate plan of escape. It does naught but excite the panting panther into orgasm.",
            actorRace: Race.Panthers, priority: 9, conditional: InBalls),
            new EventString((i) => $"{i.Unit.Name}</b> can’t help but rub {GPPHis(i.Unit)} nipples and curse with delight as <b>{i.Target.Name}</b> attempts a final desperate plan of escape. It does naught but excite the panting panther into orgasm.",
            actorRace: Race.Panthers, priority: 9, conditional: InBreasts),

            new EventString((i) => $"The violent jiggling in <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()} becomes still, much to <b>{i.Unit.Name}</b>'s displeasure. \"You can't even survive my {i.preyLocation.ToSyn()}. How weak.\"",
            priority: 9, conditional: s => InBreasts(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> arches {GPPHis(i.Unit)} back, revealing the busty outline of <b>{i.Target.Name}</b> pressing against the inside of {GPPHis(i.Unit)} mighty bosom before the struggling of {GPPHis(i.Unit)} prey ends.",
            priority: 9, conditional: s => InBreasts(s) && ActorHumanoid(s) && TargetBoobs(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pelvic thrusts as the movement in {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} stops. \"You make a fine load of {i.preyLocation.ToFluid()}.\"",
            priority: 9, conditional: s => InBalls(s) && ActorHumanoid(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> feels a great pride as {GPPHe(i.Unit)} digest{EsIfSingular(i.Unit)} the leader of the {GetPreyDesc(i.Target)} {i.Target.Race}: <b>{i.Target.Name}</b>.",
            priority: 10, conditional:TargetLeader),
            new EventString((i) => $"<b>{i.Target.Name}</b> thrashes and screams futilely to no avail for {GPPHis(i.Target)} followers to free {GPPHim(i.Target)} as {GPPHe(i.Target)} melt{SIfSingular(i.Target)} away within <b>{i.Unit.Name}</b>.",
            priority: 10, conditional:TargetLeader),
            new EventString((i) => $"<b>{i.Unit.Name}</b> proudly bellows {GPPHis(i.Unit)} triumph as the leader of the {GetPreyDesc(i.Target)} {i.Target.Race} is claimed by {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}.",
            priority: 10, conditional: s => TargetLeader(s) && ActorHumanoid(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> squeezes {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} together proudly as <b>{i.Target.Name}</b> is drowned by all {GPPHis(i.Unit)} bubbling {i.preyLocation.ToFluid()}. \"I hope you enjoyed the royal accomodation <b>{i.Target.Name}</b>.\"",
            priority: 10, conditional: s => TargetLeader(s) && InBreasts(s) && ActorHumanoid(s)),


            new EventString((i) => $"<b>{i.Unit.Name}</b> massages {GPPHis(i.Unit)} soft {PreyLocStrings.ToSyn(PreyLocation.balls)} as <b>{i.Target.Name}</b> melts into {GetRandomStringFrom("fresh", "warm", "creamy")} {GetRaceDescSingl(i.Unit)} batter",
            priority: 8, conditional: s => InBalls(s) && Lewd(s)),

            new EventString((i) => $"\"It was a valiant effort, <b>{i.Target.Name}</b>, but your struggles were futile since the start.\" <b>{i.Unit.Name}</b> tells the {GetRaceDescSingl(i.Target)} leader. \"Goodbye, <b>{i.Target.Name}</b>\" With that farewell, <b>{i.Target.Name}</b> finally succumbs to the {GetRaceDescSingl(i.Unit)} leader's {PreyLocStrings.ToSyn(i.preyLocation)}.",
            priority: 11, conditional: s => ActorLeader(s) && TargetLeader(s)),


            new EventString((i) => $"\"The time has come, <b>{i.Target.Name}</b>.\" says <b>{i.Unit.Name}</b> as the {GetRaceDescSingl(i.Target)} leader's struggles begin to wane. \"It is time for you to churn into my seed.\" Unable to withstand any longer, <b>{i.Target.Name}</b> finally lets {GPPHe(i.Target)}  head sink below the fluids. ",
            priority: 11, conditional: s => ActorLeader(s) && InBalls(s) && TargetLeader(s)),
            new EventString((i) => $"\"I greatly enjoyed our time together, <b>{i.Target.Name}</b>. But it's time to end this\" says <b>{i.Unit.Name}</b> as {GPPHe(i.Unit)} begins to knead {GPPHis(i.Unit)} massive balls. \"Now melt, turn into my load.\" Unable to withstand any longer, <b>{i.Target.Name}</b> sinks into the fluids. ",
            priority: 11, conditional: s => ActorLeader(s) && InBalls(s) && TargetLeader(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> burps as <b>{i.Target.Name}</b> goes quiet in {GPPHis(i.Unit)} belly. <b>{i.Target.Name}</b> was a tasty noodle.", //
            targetRace: Race.EasternDragon, priority: 9,  conditional: s=> CanBurp(s) && InStomach(s)),

            //The instant digestion ones haven't been fully tested. They should work but if something's wrong with them just take them out and tell me
            new EventString((i) => $"A loud shriek is silenced by a crunch as <b>{i.Target.Name}</b> is instantly digested in the belly of <b>{i.Unit.Name}</b>",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InStomach(s) && HardVore(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s belly goes soft as <b>{i.Target.Name}</b> is instantly digested",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> contracts {GPPHis(i.Unit)} abs and <b>{i.Target.Name}</b> is digested instantly",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InStomach(s)),
            new EventString((i) => $"A loud scream, a gutteral crunch, and a soft belly describes what <b>{i.Unit.Name}</b>'s belly did to <b>{i.Target.Name}</b> in such a short time ",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InStomach(s) && HardVore(s)),
            new EventString((i) => $"As quickly as <b>{i.Unit.Name}</b> swallowed <b>{i.Target.Name}</b> was as quick as <b>{i.Unit.Name}</b>'s belly turned <b>{i.Target.Name}</b> into mush",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InStomach(s)),
            new EventString((i) => $"The solid mass of <b>{i.Target.Name}</b> turns to mush in <b>{i.Unit.Name}</b>'s belly within seconds of being swallowed",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InStomach(s) && HardVore(s)),

            new EventString((i) => $"<b>{i.Target.Name}</b> doesn't spend more than five seconds in the {PreyLocStrings.ToSyn(i.preyLocation)} of <b>{i.Unit.Name}</b> before being turned into {PreyLocStrings.ToFluid(PreyLocation.balls)} ",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InBalls(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s balls stop wiggling as <b>{i.Target.Name}</b> is instantly digested",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InBalls(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s balls squeeze in on <b>{i.Target.Name}</b>, instantly digesting {GPPHim(i.Target)}",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InBalls(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> cries out in horror as {GPPHe(i.Target)} realizes that within seconds of being sucked into <b>{i.Unit.Name}</b>'s balls, {GPPHeIs(i.Target)} already melting ",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InBalls(s) && HardVore(s)),
            new EventString((i) => $"As quickly as <b>{i.Unit.Name}</b> sucked in <b>{i.Target.Name}</b> was as quick as <b>{i.Unit.Name}</b>'s balls turned <b>{i.Target.Name}</b> into {PreyLocStrings.ToFluid(PreyLocation.balls)}",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InBalls(s)),
            new EventString((i) => $"The solid mass of <b>{i.Target.Name}</b> turns into {PreyLocStrings.ToFluid(PreyLocation.balls)} in <b>{i.Unit.Name}</b>'s balls within seconds of being sucked down",
            priority: 10, conditional: s => s.Unit.HasTrait(Traits.InstantDigestion) && InBalls(s)),


            //Content Warning: the below are incredibly lewd
            new EventString((i) => $"<b>{i.Unit.Name}</b> lets out a soft moo as {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)} turn <b>{i.Target.Name}</b> into {GetRandomStringFrom("a fresh batch of", "a warm serving of", "high-protein", "rich, creamy", "warm, creamy")} bull milk",
            actorRace: Race.Taurus, priority: 11, conditional: s => InBalls(s) && Lewd(s) && s.Unit.GetGender() == Gender.Male),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)} turn <b>{i.Target.Name}</b> into {GetRandomStringFrom("a fresh batch of", "a warm serving of", "high-protein", "rich, creamy", "warm, creamy")} bull milk",
            actorRace: Race.Taurus, priority: 11, conditional: s => InBalls(s) && Lewd(s) && s.Unit.GetGender() == Gender.Male),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moos as {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)} turn <b>{i.Target.Name}</b> into dairy ... the bull kind",
            actorRace: Race.Taurus, priority: 11, conditional: s => InBalls(s) && Lewd(s) && s.Unit.GetGender() == Gender.Male),
            //Content Warning: the above are incredibly lewd

            //If your headcanon is that kobolds feel great honour in being digested by a dragon, then keep these, if not, delete them and tell me you don't like that. 
            new EventString((i) => $"<b>{i.Target.Name}</b> moans in bliss knowing that {GPPHe(i.Target)} will become pudge and forever hug {GPPHis(i.Target)} dragon's belly",
            actorRace: Race.Dragon, targetRace: Race.Kobolds, priority: 10, conditional: InStomach),
            new EventString((i) => $"<b>{i.Target.Name}</b> moans in bliss knowing that {GPPHe(i.Target)} will become {GPPHis(i.Target)} dragon's {PreyLocStrings.ToFluid(PreyLocation.balls)}",
            actorRace: Race.Dragon, targetRace: Race.Kobolds, priority: 10, conditional: InBalls),
            new EventString((i) => $"<b>{i.Target.Name}</b> blissfully sinks under the surrounding {PreyLocStrings.ToFluid(PreyLocation.balls)} knowing {GPPHe(i.Target)} will become {GPPHis(i.Target)} dragon master's next hot load ",
            actorRace: Race.Dragon, targetRace: Race.Kobolds, priority: 10, conditional: s => InBalls(s) && Lewd(s)),


            new EventString((i) => $"\"Can't you see, <b>{i.Target.Name}</b>? Your kind were always meant to be food to us.\" <b>{i.Unit.Name}</b> tells the {GetRaceDescSingl(i.Target)}'s leader inside {GPPHim(i.Unit)}. \"...and you know what happens to food...\" Defeated, <b>{i.Target.Name}</b> braces for the inevitable. \"...it digests.\" and with that <b>{i.Unit.Name}</b> squeezes {GPPHis(i.Unit)} gut, finishing off <b>{i.Target.Name}</b>.",
            priority: 11, conditional: s => InStomach(s) && ActorLeader(s) && TargetLeader(s)),
            new EventString((i) => $"\"It's been fun, <b>{i.Target.Name}</b>, but our time together must end.\" <b>{i.Unit.Name}</b> tells the {GetRaceDescSingl(i.Target)}'s leader inside {GPPHim(i.Unit)}. <b>{i.Target.Name}</b> knows that {GPPHe(i.Target)}'s completely out of energy, {GPPHe(i.Target)} knows that {GPPHe(i.Target)} can't resist {GPPHis(i.Target)} fate anymore. \"It's time to digest\" <b>{i.Unit.Name}</b> says, hugging {GPPHis(i.Unit)} belly tightly, feeling <b>{i.Target.Name}</b> finally digest.",
            priority: 11, conditional: s => InStomach(s) && ActorLeader(s) && TargetLeader(s)),

            new EventString((i) => $"\"Oh, here it comes!\" <b>{i.Unit.Name}</b> says excitedly as <b>{i.Target.Name}</b>'s struggles begin to wane. \"BUUUUUUUAAAAAARRRRRRPPP!\" The massive belch that <b>{i.Unit.Name}</b> released finishes off the {GetRaceDescSingl(i.Target)}, the struggling bumps sinking down as <b>{i.Unit.Name}</b>'s belly rounds out.",
            priority: 9, conditional: s => CanBurp(s) && InStomach(s)),
            
            //If they were feeling pleasure from being eaten while cursed, it would be just absolute cruelty to not let them climax before digesting away...
            //Tell me if you like these and I'll make some more along this theme
            new EventString((i) => $"\"Glad you enjoyed yourself\" <b>{i.Unit.Name}</b> chuckles while rubbing {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)} after hearing <b>{i.Target.Name}</b>'s loud, {GetRandomStringFrom("pleasured moan", "orgasmic cry", "cry of ecstasy")} before being digested.",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> orgasms in the {PreyLocStrings.ToSyn(i.preyLocation)} of <b>{i.Unit.Name}</b> as digestion finally claims the oddly willing prey.",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> chuckles to {GPPHimself(i.Unit)} as the willing <b>{i.Target.Name}</b> climaxes inside {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)} as {GPPHe(i.Target)} melts away",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> releases a weak moan as {GPPHe(i.Target)} orgasm{SIfSingular(i.Target)} inside the {PreyLocStrings.ToSyn(i.preyLocation)} of <b>{i.Unit.Name}</b>",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> has the strongest, as well as last orgasm of {GPPHis(i.Target)} life as {GPPHe(i.Target)} melts away in the {PreyLocStrings.ToSyn(i.preyLocation)} of <b>{i.Unit.Name}</b>",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> has a massive orgasm inside of <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)} and passes out from the pleasure, sinking peacefully into the surrounding fluids.",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b>'s body shudders as {GPPHe(i.Target)} climaxes inside of <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)}. So glad that {GPPHe(i.Target)} was able to orgasm before being churned up, {GPPHe(i.Target)} peacefully lets {GPPHimself(i.Target)} sink into the fluids.",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> only feels a wave of pleasure crash into {GPPHim(i.Target)} as {GPPHe(i.Target)} climaxes inside of <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)} mere seconds before melting away.",
            priority: 9, conditional: s => Cursed(s) && Lewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> has no troubly getting off when thinking about becoming {InfoPanel.RaceSingular(i.Unit)} shit, and <b>{i.Unit.Name}</b>'s innards promptly churn down {GPPHis(i.Target)} body to make {GPPHe(i.Target)} wish come true.",
            priority: 9, conditional: s => Cursed(s) && Lewd(s) && InStomach(s) && Scat(s)),

        };

        AbsorptionMessages = new List<EventString>()
        {
            //generic
            new EventString((i) => $"<b>{i.Unit.Name}</b> enjoys the added weight of <b>{i.Target.Name}</b> on {GPPHis(i.Unit)} body.", priority: 8),
            new EventString((i) => $"The last of <b>{i.Target.Name}</b> is absorbed into <b>{i.Unit.Name}</b>’s {i.preyLocation.ToSyn()}.", priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> is now completely gone, soon to be forgotten as just another meal.", priority: 8),
            new EventString((i) => $"<b>{i.Target.Name}</b> is now completely gone, soon to be forgotten as just another meal.", priority: 8),

            //Weight gain generic
            new EventString((i) => $"<b>{i.Unit.Name}</b> smirks, noticing {GPPHis(i.Unit)} assets feel heavier after absorbing <b>{i.Target.Name}</b>.", priority: 8, conditional: WeightGain),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slaps {GPPHis(i.Unit)} ass, enjoying its newfound roundness and wobbliness.", priority: 8, conditional: WeightGain),
            new EventString((i) => $"<b>{i.Unit.Name}</b> checks {GPPHis(i.Unit)} new curves, wondering if <b>{AttractedWarrior(i.Unit)}</b> would find {GPPHim(i.Unit)} more attractive now.", priority: 8, conditional: s=> WeightGain(s) && ReqOSW(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> slaps {GPPHis(i.Unit)} newly made flab with a laugh ", actorRace: Race.Hippos, priority: 9, conditional: WeightGain),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lifts {GPPHis(i.Unit)} belly and drops it with a bounce, it being pudgier after absorbing <b>{i.Target.Name}</b>.", priority: 8, conditional: WeightGain),
            new EventString((i) => $"<b>{i.Unit.Name}</b> caresses {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} now that <b>{i.Target.Name}</b>'s substance has become part of it.", priority: 8, conditional: WeightGain),
            new EventString((i) => $"<b>{i.Unit.Name}</b> enjoys kneading {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} with its added heft from <b>{i.Target.Name}</b>, but wishes <b>{PotentialNextPrey(i.Unit).Name}</b> would add to it even more", priority: 8, conditional: s=> WeightGain(s) && CanAddressPlayer(s)),

            //stomach-exclusive
            new EventString((i) => $"With <b>{i.Target.Name}</b> all sucked up by <b>{i.Unit.Name}</b>'s intestines, {GPPHe(i.Unit)} immediately misses the sloshy feeling and looks around for {GPPHis(i.Unit)} next meal", priority: 9, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s stomach flattens, having absorbed the last of <b>{i.Target.Name}</b>.", priority: 9, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s stomach flattens, having absorbed the last of - what was {GPPHis(i.Target)} name, again?", priority: 9, conditional: InStomach),
            new EventString((i) => $"Gurgling gives way to silence, as last bits of <b>{i.Target.Name}</b> find themselves a better purpose - to provide <b>{i.Unit.Name}</b> with nutrients.", priority: 9, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s belly finally contracts as if <b>{i.Target.Name}</b> never existed.", priority: 9, conditional: InStomach),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s belly finally contracts as if - what was {GPPHis(i.Target)} name, again? never existed.", priority: 9, conditional: InStomach),

            //Scat disposal
            new EventString((i) => $"<b>{i.Unit.Name}</b> enjoys feeling the weight of <b>{i.Target.Name}</b>'s remains squeeze out of {GPPHis(i.Unit)} ass.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> takes a lewd pleasure in <b>{i.Target.Name}</b> sliding out of {GPPHis(i.Unit)} rectum as several thick turds.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> smirks, noticing {GPPHis(i.Unit)} ass feels heavier after absorbing and eliminating <b>{i.Target.Name}</b>.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> takes a quick dump to purge <b>{i.Target.Name}</b>'s foul remains from {GPPHis(i.Unit)} guts.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shits out a large, steamy batch of fresh manure. This wasn't the escape <b>{i.Target.Name}</b> had been looking for in {GPPHis(i.Unit)} dark stomach.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans in disgust and pleasure, feeling <b>{i.Target.Name}</b> squeeze through {GPPHis(i.Unit)} colon and out of {GPPHis(i.Unit)} plump {PreyLocStrings.ToSyn(PreyLocation.anal)}.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> presses down on {GPPHis(i.Unit)} {i.preyLocation.ToSyn()}, lolling out {GPPHis(i.Unit)} tongue in relief while letting loose one huge log of shit. There's <b>{i.Target.Name}</b>...", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Target.Name}</b> is completely digested, dumped out as a gross scat pile behind <b>{i.Unit.Name}</b>'s paws.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Target.Name}</b> finishes digesting in <b>{i.Unit.Name}</b>'s bowels, and is promptly eliminated from {GPPHis(i.Unit)} body.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> squats to deposit <b>{i.Target.Name}</b>'s used-up remains in a messy pile below {GPPHis(i.Unit)} tail.", priority: 9, conditional: s => Scat(s) && ActorTail(s)),
            new EventString((i) => $"After squeezing out <b>{i.Target.Name}</b> as a nice, {GetRandomStringFrom("nasty", "creamy", "warm", "greasy")} {GetRandomStringFrom("log", "turd", "brown lump")}, <b>{i.Unit.Name}</b> winks at <b>{PotentialNextPrey(i.Unit).Name}</b>, as if to say \"You're next!\".\n...{GPPHis(i.Unit)} sphincter seems to do the same.", priority: 9, conditional: s=> Scat(s) && CanAddressPlayer(s)),
            new EventString((i) => $"After squeezing out ...What was {GPPHis(i.Target)} name, again? as a nice, {GetRandomStringFrom("nasty", "creamy", "warm", "greasy")} {GetRandomStringFrom("log", "turd", "brown lump")}, <b>{i.Unit.Name}</b> winks at <b>{PotentialNextPrey(i.Unit).Name}</b>, as if to say \"You're next!\".\n...{GPPHis(i.Unit)} sphincter seems to do the same.", priority: 9, conditional: s=> Scat(s) && CanAddressPlayer(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> looks back at the vile mass of <b>{i.Target.Name}</b> that just slid out {GPPHis(i.Unit)} behind, then at <b>{PotentialNextPrey(i.Unit).Name}</b>, then back at {GPPHis(i.Unit)} {GetRandomStringFrom("dump", "shit", "crap")}, before licking his lips at <b>{PotentialNextPrey(i.Unit).Name}.</b>", priority: 9, conditional: s=> Scat(s) && CanAddressPlayer(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> is proud of the steaming mound of shit he turned <b>{i.Target.Name}</b> into, and desires to add <b>{PotentialNextPrey(i.Unit).Name}</b> on top while it's warm.", priority: 9, conditional: s=> Scat(s) && CanAddressPlayer(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> is nothing but {GetRandomStringFrom("a gross brown mass", "stinking turds", "fresh, warm shit", "disgusting waste")} that's now leaving <b>{i.Unit.Name}</b>'s rectum", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s guts are done with <b>{i.Target.Name}</b>, leaving {GPPHim(i.Target)} to be disposed of as nothing but {GetRandomStringFrom("a gross brown pile", "stinking turds", "fresh, warm shit", "disgusting waste", "a creamy dump")}.", priority: 9, conditional: Scat),
            new EventString((i) => $"With a squint and a push from <b>{i.Unit.Name}</b>, <b>{i.Target.Name}</b> manages to achieve {GPPHis(i.Target)} final form as {InfoPanel.RaceSingular(i.Unit)} droppings.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b> grunts, and the filthy remains of {GPPHis(i.Unit)} now used-up prey splat onto the ground behind {GPPHim(i.Unit)} <b>{i.Unit.Name}</b>, completely unrecognizable.", priority: 9, conditional: Scat),
            new EventString((i) => $"All that's left of <b>{i.Target.Name}</b> is a {InfoPanel.RaceSingular(i.Target)}-sized load of <b>{i.Unit.Name}</b>'s feces. Good fight.", priority: 9, conditional: Scat),
            new EventString((i) => $"<b>{i.Unit.Name}</b>:*PFRRRRRT*. <b>{i.Target.Name}</b>:*Splat*", priority: 9, conditional: s => Scat(s) && Farts(s)),
            new EventString((i) => $"A cacophonous fart out of <b>{i.Unit.Name}</b>'s hind side heralds <b>{i.Target.Name}</b>'s smelly return to the battlefield...", priority: 9, conditional: s => Scat(s) && Farts(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> relieves {GPPHimself(i.Unit)}...\nThe obscene release of gas will have to suffice as <b>{i.Target.Name}</b>'s eulogy.", priority: 9, conditional: s => Scat(s) && Farts(s)),
            new EventString((i) => $"With the amount of shit piling up under <b>{i.Unit.Name}</b>' ass, one could think <b>{i.Target.Name}</b> was mostly waste to begin with.", priority: 9, conditional: s => Scat(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> exerts {GPPHimself(i.Unit)}, letting loose one juicy burst of gas, before having <b>{i.Target.Name}</b> emerge and drop out of that same hole to form a pile of {GetRandomStringFrom("nasty", "creamy", "warm", "greasy")} {GetRandomStringFrom("logs", "turds", "brown lumps")}", priority: 9, conditional: s => Scat(s) && Farts(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> has been reduced to {GetRandomStringFrom("shit", "waste", "a dump", "scat")} by <b>{i.Unit.Name}</b>'s digestive system.", priority: 9, conditional: s => Scat(s)),
            new EventString((i) => $"Just like that, <b>{i.Target.Name}</b> has been casually shat onto the ground as mere {InfoPanel.RaceSingular(i.Unit)} waste, and <b>{i.Unit.Name}</b> certainly won't remember what {GPPHe(i.Unit)} ate.", priority: 9, conditional: s => Scat(s)),
            new EventString((i) => $"The friction from <b>{i.Target.Name}</b>'s remains sliding past {GPPHis(i.Unit)} anal sphincter sends arousing tingles through <b>{i.Unit.Name}</b>'s body.", priority: 9, conditional: s => Scat(s) && Lewd(s)),

            new EventString((i) => $"<b>{i.Unit.Name}</b> passes gas – <b>{i.Target.Name}</b> passes on to the next life.", priority: 9, conditional: s => Farts(s) && !Scat(s)),
            //CV disposal             
            new EventString((i) => $"<b>{i.Unit.Name}</b> sways {GPPHis(i.Unit)} hips side to side as what is left of <b>{i.Target.Name}</b> gushes from {GPPHis(i.Unit)} {PreyLocStrings.ToCockSyn()} onto the ground.", priority: 9, conditional: s => ActorHumanoid(s) && InBalls(s)),
            new EventString((i) => $"Not being able to hold anymore, <b>{i.Unit.Name}</b> orgasms, <b>{GPPHis(i.Unit)}</b> {PreyLocStrings.ToCockSyn()} expelling in spurts what once was <b>{i.Target.Name}.</b>", priority: 9, conditional: InBalls),
            new EventString((i) => $"With nothing to absorb anymore, <b>{i.Unit.Name}</b> cums out the puddle of <b>{i.Target.Name}</b>'s remains.", priority: 9, conditional: InBalls),
            new EventString((i) => $"<b>{i.Unit.Name}</b> demonstrates {GPPHis(i.Unit)} virility to <b>{AttractedWarrior(i.Unit)}</b> by splattering <b>{i.Target.Name}</b>'s remains across the ground in an impressive puddle.", priority: 9, conditional: s=> InBalls(s) && ReqOSWLewd(s)),

            new EventString((i) => $"With a monstrous roar, <b>{i.Unit.Name}</b> ejaculates, {GPPHis(i.Unit)} cock spurting out in thick globs the remains of <b>{i.Target.Name}</b>",
            actorRace: Race.Dragon, priority: 9, conditional: InBalls),

            new EventString((i) => $"<b>{i.Unit.Name}</b>'s dick spurts out the remains of <b>{i.Target.Name}</b>. Then {GPPHe(i.Unit)} proudly gazes at the puddle {GPPHis(i.Unit)} former servant made.",
            actorRace: Race.Dragon, targetRace: Race.Kobolds, priority: 9, conditional: s => InBalls(s) && Friendly(s)),

            new EventString((i) => $"\"Attention {GetRaceDescSingl(i.Target)}, this is what has become of your 'mighty' leader.\" <b>{i.Unit.Name}</b> taunts as {GPPHe(i.Unit)} jerks {GPPHimself(i.Unit)} to orgasm, splattering the remains of <b>{i.Target.Name}</b> all over the ground in an impressive puddle.",
            priority: 9, conditional: s => ActorLeader(s) && InBalls(s) && TargetLeader(s)),
            
            //BV disposal
            new EventString((i) => $"<b>{i.Unit.Name}</b> leans forward and moans, letting what's left of <b>{i.Target.Name}</b> pour out of {GPPHis(i.Unit)} nipples onto the ground.", priority: 9, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s breasts finally settle, only streams of milk pouring from them reminding of what happened to <b>{i.Target.Name}</b>.", priority: 9, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> basks in the triump of {GPPHis(i.Unit)} mighty breasts as <b>{i.Target.Name}</b> is totally absorbed into them without a trace.", priority: 9, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Target.Name}</b> cleavage is absorbed into the <b>{i.Unit.Name}</b>'s victorious {i.preyLocation.ToSyn()}. \"Mine are bigger and badder.\", <b>{i.Unit.Name}</b> scorns.", priority: 9, conditional: s => InBreasts(s) && TargetBoobs(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> pants as {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} squirts out the milky remains of <b>{i.Target.Name}</b>, making them wobble pleasurably in the process.", priority: 9, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> thrusts {GPPHis(i.Unit)} {i.preyLocation.ToSyn()} at <b>{AttractedWarrior(i.Unit)}</b>, then gives them a good squeeze to coat {GPPHis(i.Unit)} lover with fresh milk made from <b>{i.Target.Name}</b>.", priority: 9, conditional: s=>InBreasts(s) && ReqOSWLewd(s)),
            new EventString((i) => $"<b>With a loud and tense grunt, <b>{i.Unit.Name}</b> squeezes out the undigested bones of <b>{i.Target.Name}</b> out of {GPPHis(i.Unit)}, the process well lubricated by gushing breast milk.", priority: 9, conditional: s=>InBreasts(s) && BonesDisposal(s)),

            //Bone Disposal
            new EventString((i) => $"<b>{i.Unit.Name}</b> takes a lewd pleasure in <b>{i.Target.Name}</b> sliding out of {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(PreyLocation.anal)} as clean bleached bones.", priority: 9, conditional: s=> InStomach(s) && BonesDisposal(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans in pleasure, feeling <b>{i.Target.Name}</b>'s bones squeeze out of {GPPHis(i.Unit)} plump {PreyLocStrings.ToSyn(PreyLocation.anal)}.", priority: 9, conditional: s=> InStomach(s) && BonesDisposal(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> finishes digesting in <b>{i.Unit.Name}</b>'s bowels, and {GPPHis(i.Target)}'s skeletal remains are promptly eliminated from predator's body.", priority: 9, conditional: s=> InStomach(s) && BonesDisposal(s)),

            //Slimes exclusive
            new EventString((i) => $"The dark lump in <b>{i.Unit.Name}</b>'s translucent form disappears completely.",  actorRace: Race.Slimes, priority: 10),
        };
        TransferMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Unit.Name}</b> pumps the cum left of {GPPHis(i.Unit)} prey into <b>{i.Target.Name}</b>'s mouth, {i.Target.Name} is happy with the result. ",
            priority: 10, conditional: s => InStomach(s) && s.Target.IsDead ),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> pumps <b>{i.Prey.Name}</b> into <b>{i.Target.Name}</b>'s {i.preyLocation.ToSyn()}, {i.Target.Name} is happy with the result. ", priority: 8),           

            //Very lewd
            new EventString((i) => $"<b>{i.Unit.Name}</b> beckons <b>{i.Target.Name}</b> over and tells {GPPHim(i.Unit)} to start sucking. <b>{i.Target.Name}</b> is just too good with {GPPHis(i.Target)} mouth and <b>{i.Unit.Name}</b> {GetRandomStringFrom("climaxes", "orgasms", "goes over the edge", "cums", "ejaculates")} soon after, bloating out the {GetRaceDescSingl(i.Target)}'s belly as {GPPHis(i.Unit)} balls shrink down to normal.",
            priority: 11, conditional: s => InStomach(s) && Lewd(s)),
            //Very lewd

            new EventString((i) => $"<b>{i.Target.Name}</b> wraps {GPPHis(i.Unit)} mouth around <b>{i.Unit.Name}</b>'s member and begins servicing it. <b>{i.Unit.Name}</b> can't hold on any longer and blows {GPPHis(i.Unit)} load into <b>{i.Target.Name}</b>'s mouth. <b>{i.Target.Name}</b> swallows most of it and {GPPHis(i.Target)} belly bloats out",
            priority: 11, conditional: s => InStomach(s) && Lewd(s)),
        };
        VoreStealMessages = new List<EventString>()
        {
            // If you can make this not lewd, please advise, otherwise I may need to lock the whole mechanic behind lewd texts
            new EventString((i) => $"<b>{i.Unit.Name}</b> tackles <b>{i.Target.Name}</b> and rides {GPPHim(i.Target)} until {GPPHe(i.Target)} releases <b>{i.Prey.Name}</b> into {GPPHis(i.Unit)} womb.",
            priority: 10, conditional: s => s.oldLocation == PreyLocation.balls && s.preyLocation == PreyLocation.womb),
            new EventString((i) => $"<b>{i.Target.Name}</b> can't bring {GPPHimself(i.Target)} to fight off <b>{i.Unit.Name}</b> when {GPPHe(i.Unit)} starts sucking on {GPPHis(i.Target)} breasts; {i.Unit.Name} doesn't relent until <b>{i.Prey.Name}</b> is released from {GPPHis(i.Target)} bust.",
            priority: 10, conditional: s => (s.oldLocation == PreyLocation.breasts||s.oldLocation == PreyLocation.leftBreast||s.oldLocation == PreyLocation.rightBreast) && s.preyLocation == PreyLocation.stomach),
            new EventString((i) =>$"<b>{i.Unit.Name}</b> knocks down <b>{i.Target.Name}</b> and begins sucking {GPPHis(i.Target)} rod. Astonished, {i.Target.Name} doesn't even realize it when {GPPHe(i.Target)} feeds <b>{i.Prey.Name}</b> to {i.Unit.Name}.", priority: 8),
        };
        BreastFeedMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Target.Name}</b> suckles on {(i.Unit == i.Target ? GPPHis(i.Target) + " own" : ApostrophizeWithOrWithoutS(i.Unit.Name) + "'s")} full breasts, eagerly gulping down a mouthful of {PreyLocStrings.ToFluid(PreyLocation.breasts)}.",priority:8),
        };
        CumFeedMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Target.Name}</b> lovingly fellates <b>{i.Unit.Name}</b>, and is rewarded with a mouthful of {GPPHis(i.Target)} lover's {PreyLocStrings.ToFluid(PreyLocation.balls)}.",priority: 10, conditional: s => ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> tells <b>{i.Target.Name}</b> to open wide as {GPPHe(i.Unit)} thrust{SIfSingular(i.Unit)} {GPPHis(i.Unit)} cock into <b>{i.Target.Name}</b>'s mouth and unloads {GPPHis(i.Unit)} {PreyLocStrings.ToFluid(PreyLocation.balls)}.",priority: 10, conditional: s => ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> deliriously beckons <b>{i.Unit.Name}</b> and begins playing with {GPPHis(i.Unit)} penis before feasting on {GPPHis(i.Unit)} {PreyLocStrings.ToFluid(PreyLocation.balls)}.",priority: 10, conditional: s => ReqTargetCompatibleLewd(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> unloads {GPPHis(i.Unit)} {PreyLocStrings.ToFluid(PreyLocation.balls)} into <b>{i.Target.Name}</b>'s mouth.",priority: 8),
        };
       
        GreatEscapeKeepMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Unit.Name}</b> jiggles their {PreyLocStrings.ToSyn(i.preyLocation)} with <b>{i.Target.Name}</b> stashed inside. \"What are you waiting for in there?\" - {GPPHe(i.Unit)} asks, annoyed.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)} is quiet. Too quiet. \"Did you digest in there?\" - <b>{i.Unit.Name}</b> asks prodding {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}. Nope, <b>{i.Target.Name}</b> is still there.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> stretches, feeling the pleasant heaviness of <b>{i.Target.Name}</b> in {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Target.Name}</b> kicks the {PreyLocStrings.ToSyn(i.preyLocation)} holding {GPPHim(i.Target)} from the inside. <b>{i.Unit.Name}</b> considers getting rid of {GPPHis(i.Unit)} rude occupant right now.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Target.Name}</b> curls up within claustrophobic confines of <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)}.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> has a hard time keeping <b>{i.Target.Name}</b> in {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}.",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans, rubbing {GPPHis(i.Unit)} {PreyLocStrings.ToSyn(i.preyLocation)}. <b>{i.Target.Name}</b> proves to be a hardy catch",priority:25, conditional: HasGreatEscape),
            new EventString((i) => $"<b>{i.Unit.Name}</b> listens to the gurgling of {GPPHis(i.Unit)} <b>{i.Target.Name}</b>-filled stomach. To {GPPHis(i.Unit)} dismay, there's little actual gurgling.",priority:25, conditional: s => HasGreatEscape(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shows off {GPPHis(i.Unit)} belly to the others. <b>{i.Target.Name}</b> within said belly is waiting, having already devised plan to embarass {GPPHis(i.Target)} captor.",priority:25, conditional: s => HasGreatEscape(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> listens to the gurgling of {GPPHis(i.Unit)} <b>{i.Target.Name}</b>-filled stomach. To {GPPHis(i.Unit)} dismay, there's little actual gurgling.",priority:25, conditional: s => HasGreatEscape(s) && InStomach(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> gropes {GPPHis(i.Unit)} <b>{i.Target.Name}</b>-stuffed midsection. To {GPPHis(i.Unit)} annoyance, <b>{i.Target.Name}</b> still refuses to melt.",priority:25, conditional: s => HasGreatEscape(s) && InWomb(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> lets out a quiet gasp with each movement of surprisingly resilient <b>{i.Target.Name}</b> within {GPPHis(i.Unit)} womb.",priority:25, conditional: s => HasGreatEscape(s) && InWomb(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> sighs in resignation as <b>{i.Target.Name}</b> continues squirming within {GPPHis(i.Unit)} vagina. Looks like this annoyance isn't gonna melt away for a while.",priority:25, conditional: s => HasGreatEscape(s) && InWomb(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s full breasts jiggle as <b>{i.Target.Name}</b> struggles inside.",priority:25, conditional: s => HasGreatEscape(s) && InBreasts(s)),
            new EventString((i) => $"\"Enjoying your time there?\" - <b>{i.Unit.Name}</b> asks the prisoner of {GPPHis(i.Unit)} bosom.",priority:25, conditional: s => HasGreatEscape(s) && InBreasts(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> is tucked comfortably within <b>{i.Unit.Name}</b>'s engorged breasts, the ideal environment to be plotting escape.",priority:25, conditional: s => HasGreatEscape(s) && InBreasts(s)),
            new EventString((i) => $"<b>{i.Target.Name}</b> turns around within <b>{i.Unit.Name}</b>'s breasts, getting {GPPHimself(i.Target)} a nice gulp of milk.",priority:25, conditional: s => HasGreatEscape(s) && InBreasts(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> moans in pleasure as <b>{i.Target.Name}</b> massages {GPPHis(i.Unit)} nutsack from the inside, trying to make {GPPHis(i.Target)} adversary cum {GPPHim(i.Target)} out.",priority:25, conditional: s => HasGreatEscape(s) && InBalls(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b>'s balls slosh and quiver, <b>{i.Target.Name}</b> assuming more comfortable position within.",priority:25, conditional: s => HasGreatEscape(s) && InBalls(s)),
            new EventString((i) => $"<b>{i.Unit.Name}</b> can't wait to humiliate {GPPHis(i.Unit)} prey by splattering its remains across the ground. <b>{i.Target.Name}</b> can't wait to humiliate {GPPHis(i.Target)} pred by coming out intact.",priority:25, conditional: s => HasGreatEscape(s) && InBalls(s)),
            new EventString((i) => $"<b>Erin</b> lets out a horrified shriek as <b>{i.Unit.Name}</b> begins to rub at <b>Erin</b>'s nethers through their belly.",
          targetRace: Race.Erin, priority: 26, conditional: s => Lewd(s) && HasGreatEscape(s) && InStomach(s)),
          new EventString((i) => $"<b>{i.Unit.Name}</b> starts groping at the bulge that is <b>Erin</b>, squeezing and rubbing her most sensitive parts, trying to coerce an orgasm out of the terrified Nyangel.",
          targetRace: Race.Erin, priority: 26, conditional: s => Lewd(s) && HasGreatEscape(s)),
          new EventString((i) => $"Even though she should be terrified, the curse controlling <b>Erin</b>'s mind removes any doubt as the indigestible Nyangel begins fingering herself within <b>{i.Unit.Name}</b>'s lewd {PreyLocStrings.ToSyn(i.preyLocation)}.",
          targetRace: Race.Erin, priority: 26, conditional: s => Cursed(s) && Lewd(s)),
          new EventString((i) => $"<b>{i.Unit.Name}</b>'s stomach isn't doing much to <b>Erin</b>, unlike the mind-altering spell bringing her closer and closer to another orgasm.",
          targetRace: Race.Erin, priority: 26, conditional: s => Cursed(s) && Lewd(s) && InStomach(s)),
          new EventString((i) => $"<b>{i.Unit.Name}</b>'s stinging acids burn away at <b>Erin</b>, causing her to scream in agony even though they fail to break her body down.",
          targetRace: Race.Erin, priority: 26, conditional: s => HardVore(s) && HasGreatEscape(s) && InStomach(s)),
          new EventString((i) => $"<b>{i.Unit.Name}</b>'s stomach tries to melt <b>Erin</b> down, but the Nyangel's body is staying intact.",
          targetRace: Race.Erin, priority: 26, conditional: s => HasGreatEscape(s) && InStomach(s)),
          new EventString((i) => $" \"Let me out! Please!\" <b>Erin</b> screams, to no avail, as <b>{i.Unit.Name}</b> seems content to keep the poor Nyangel in there.",
          targetRace: Race.Erin, priority: 26, conditional: HasGreatEscape),
          new EventString((i) => $"<b>Erin</b>'s body doesn't melt away, but her claustrophobia makes sure that her mind isn't staying intact.",
          targetRace: Race.Erin, priority: 26, conditional: HasGreatEscape),
          new EventString((i) => $"<b>Erin</b> squirms in <b>{i.Unit.Name}</b>'s belly, hoping to escape the terrifyingly small space.",
          targetRace: Race.Erin, priority: 26, conditional: s => HasGreatEscape(s) && InStomach(s)),
          new EventString((i) => $"<b>Erin</b> doesn't make any move to resist as <b>{i.Unit.Name}</b>'s acids try-and fail-to eat away at her flesh.",
          targetRace: Race.Erin, priority: 26, conditional: s => Cursed(s) && HasGreatEscape(s) && InStomach(s)),
        };

        GreatEscapeFleeMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Unit.Name}</b> stretches and goes to pat {GPPHis(i.Unit)} full {PreyLocStrings.ToSyn(i.preyLocation)}, only to notice with horror the emptiness where <b>{i.Target.Name}</b> was supposed to be.",priority:25),
            new EventString((i) => $"\"Curse you, <b>{i.Target.Name}</b>, you won't get away next time!\" - yells <b>{i.Unit.Name}</b>, shaking {GPPHis(i.Unit)} fist at the heavens.",priority:25),
            new EventString((i) => $"Tired of not being to digest <b>{i.Target.Name}</b>, <b>{i.Unit.Name}</b> decides to let {GPPHim(i.Target)} out.",priority:25),
            new EventString((i) => $"<b>{i.Target.Name}</b> slips out of <b>{i.Unit.Name}</b>'s {PreyLocStrings.ToSyn(i.preyLocation)}.",priority:25),
            new EventString((i) => $"Somehow, <b>{i.Target.Name}</b> managed to escape without anyone noticing, the only thing reminding <b>{i.Unit.Name}</b> of {GPPHim(i.Target)} being {GetRaceDescSingl(i.Unit)}'s aching butt.",priority:25, conditional: InStomach),
            new EventString((i) => $"<b>{i.Target.Name}</b> pulls out a smoke bomb and lights it up. Amid the coughing fit, <b>{i.Unit.Name}</b> is too distracted to notice {GPPHis(i.Unit)} prey sneaking out and away.",priority:25, conditional: InStomach),
            new EventString((i) => $"After the battle, <b>{i.Unit.Name}</b> decides to take a quick nap to digest <b>{i.Target.Name}</b>. Waking up, {GPPHe(i.Unit)} notices in horror that {GPPHis(i.Unit)} belly is now completely empty, with no signs of <b>{i.Target.Name}</b> ever being there.",priority:25, conditional: InStomach),
            new EventString((i) => $"Somehow, <b>{i.Target.Name}</b> managed to escape without anyone noticing, the only thing reminding <b>{i.Unit.Name}</b> of {GPPHim(i.Target)} being {GetRaceDescSingl(i.Unit)}'s aching pussy.",priority:25, conditional: InWomb),
            new EventString((i) => $"<b>{i.Target.Name}</b>'s squirming around has droven <b>{i.Unit.Name}</b> to orgasm. The {GetRaceDescSingl(i.Target)} triumphantly slides out as {GPPHis(i.Target)} captor quivers on the ground.",priority:25, conditional: InWomb),
            new EventString((i) => $"<b>{i.Unit.Name}</b>, tired of <b>{i.Target.Name}</b> hogging the valuable space within {GPPHis(i.Unit)} womb, decides to evict the freeloader.",priority:25, conditional: InWomb),
            new EventString((i) => $"Somehow, <b>{i.Target.Name}</b> managed to escape without anyone noticing, the only thing reminding <b>{i.Unit.Name}</b> of {GPPHim(i.Target)} being {GetRaceDescSingl(i.Unit)}'s aching nipples.",priority:25, conditional: InBreasts),
            new EventString((i) => $"<b>{i.Unit.Name}</b> shakes {GPPHis(i.Unit)} breasts trying to see if <b>{i.Target.Name}</b> has been digested already. Apparently not, as the sly prey frees {GPPHimself(i.Target)} from <b>{i.Unit.Name}</b>'s bosom and runs away.'",priority:25, conditional: InBreasts),
            new EventString((i) => $"Frustrated with <b>{i.Target.Name}</b> not digesting, <b>{i.Unit.Name}</b> squeezes {GPPHis(i.Unit)} breasts, evicting the freeloader into a nearby thorny bush.",priority:25, conditional: InBreasts),
            new EventString((i) => $"Somehow, <b>{i.Target.Name}</b> managed to escape without anyone noticing, the only thing reminding <b>{i.Unit.Name}</b> of {GPPHim(i.Target)} being {GetRaceDescSingl(i.Unit)}'s aching cockhole.",priority:25, conditional: InBalls),
            new EventString((i) => $"<b>{i.Target.Name}</b> pokes, prods and massages <b>{i.Unit.Name}</b>'s ballsack from the inside, nearly bringing {GPPHim(i.Unit)} to orgasm several times. Frustrated, <b>{i.Unit.Name}</b> decides to take matters in {GPPHis(i.Unit)} own hands, soon unleashing a massive load of all the pent-up semen... in which {GPPHis(i.Unit)} prey escapes.",priority:25, conditional: InBalls),
            new EventString((i) => $"Suddenly, <b>{i.Unit.Name}</b>'s cock expands to immense size and releases <b>{i.Target.Name}</b> back into the world. Before <b>{i.Unit.Name}</b> could react, his prey, slippery from cum covering it, escapes.",priority:25, conditional: InBalls),
        };

        BreastFeedMessages = new List<EventString>()
        {
            new EventString((i) => $"<b>{i.Target.Name}</b> suckles on {(i.Unit == i.Target ? GPPHis(i.Target) + "own" : i.Target.Name + "'s")} full breasts, eagerly gulping down a mouthful of {PreyLocStrings.ToFluid(PreyLocation.breasts)}.",priority:8),
        };
    }

}
