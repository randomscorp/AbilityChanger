using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbilityChanger.Base
{
    public static class CommonStates
    {
        public static class DreamNail
        {
            public static string Init { get; } = "Init";
            public static string Incative { get; } = "Inactive";
            public static string CanDreamNail { get; } = "Can Dream Nail?";
            public static string FsmCancel { get; } = "Fsm Cancel";
            public static string DreamConvo { get; } = "Dream Convo";
            public static string RegainControl { get; } = "Regain Control";
            public static string TakeConterol { get; } = "Take Control";
            public static string Start { get; } = "Start";
            public static string IdleAnim { get; } = "Idem Anim";
            public static string EntryCancelCheck { get; } = "Entry Cancel Check";
            public static string Queuing { get; } = "Queuing";
            public static string ChargeCancel { get; } = "Charge Cancel";
            public static string Charge { get; } = "Charge";
            public static string DreamGate { get; } = "Dream Gate?";
        }
        public static class SpellControl
        {
            public static string Init { get; } = "Init";
            public static string Pause { get; } = "Pause";
            public static string Inactive { get; } = "Inactive";
            public static string Cancel { get; } = "Cancel";
            public static string BackIn { get; } = "Back In?";
            public static string RegainControl { get; } = "Regain Control";
            public static string ButtonDown { get; } = "Button Down";
            public static string CanCastQC { get; } = "Can Cast? QC";
            public static string QC { get; } = "QC";
            public static string CanCast { get; } = "Can Cast?";
            public static string SpellEnd { get; } = "Spell End";
        }
        public static class NailArts
        {
            public static string Init { get; } = "Init";
            public static string CancelAll { get; } = "Cancel All";
            public static string Inactive { get; } = "Inactive";
            public static string CanNailArt { get; } = "Can Nail Art?";
            public static string Wallside { get; } = "Wallside?";
            public static string TakeControl { get; } = "Take Control";
            public static string MoveChoice { get; } = "Move Choice";
            public static string RegainControl { get; } = "Regain Control";


        }
    }

}
