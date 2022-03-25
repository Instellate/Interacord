using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitFields;

namespace Interacord.Components
{
    /// <summary>
    /// A enum for message flag bitfields.
    /// </summary>
    [Flags]
    public enum MessageFlag : ulong
    {
        Clear = 0x00,
        Crossposted = 0x01,
        IsCrosspost = 0x01 << 1,
        SuppressEmbeds = 0x01 << 2,
        SourceMessageDeleted = 0x01 << 3,
        Urgent = 0x01 << 4,
        HasThread = 0x01 << 5,
        Ephemeral = 0x01 << 6,
        Loading = 0x01 << 7,
        FailedToMentionSomeRolesInThread = 0x01 << 8,
    }
}
