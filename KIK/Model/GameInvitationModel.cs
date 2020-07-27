using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIK.Model
{
    public class GameInvitationModel
    {
        public Guid Id { get; set; }
        public string EmailTo { get; set; }
        public string InvitedBy { get; set; }
        public bool isConfirmed { get; set; }
        public DateTime ConfirmationDate { get; set; }

    }
}
