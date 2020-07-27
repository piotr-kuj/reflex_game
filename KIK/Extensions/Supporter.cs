using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using javax.xml.crypto;

namespace KIK.Extensions
{
    public class Supporter
    {
        [UIHint("UTCTime")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime MyDateTime { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime DDate { get; set; }



    }
}
