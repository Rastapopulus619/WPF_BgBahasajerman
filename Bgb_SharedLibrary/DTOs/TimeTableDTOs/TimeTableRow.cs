using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bgb_SharedLibrary.DTOs.TimeTableDTOs
{
    public class TimeTableRow
    {
        public string Zeiten { get; set; }    // Time column

        public SlotEntry Montag { get; set; } = new SlotEntry();
        public SlotEntry Dienstag { get; set; } = new SlotEntry();
        public SlotEntry Mittwoch { get; set; } = new SlotEntry();
        public SlotEntry Donnerstag { get; set; } = new SlotEntry();
        public SlotEntry Freitag { get; set; } = new SlotEntry();
        public SlotEntry Samstag { get; set; } = new SlotEntry();
        public SlotEntry Sonntag { get; set; } = new SlotEntry();
    }
}
