using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolastaBardClass
{
    [Flags]
    public enum ExtraTargetFilteringTag : byte
    {
        No = 0,
        Unarmored = 1,
        NonCaster = 128
    }


    public enum ExtraRitualCasting
    {
        None,
        Prepared,
        Spellbook,
        Spontaneous
    }
}
