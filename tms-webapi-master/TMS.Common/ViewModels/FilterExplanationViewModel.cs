using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.ViewModels
{
    public class FilterExplanationViewModel
    {
        /// <summary>
        /// This string arrays to filter explanations by creators
        /// </summary>
        public string[] Creators { get; set; }
        /// <summary>
        /// This string arrays to filter explanations by status
        /// </summary>
        public string[] StatusRequest { get; set; }

        /// <summary>
        /// This string arrays to filter explanations by reasons
        /// </summary>
        public string[] ReasonRequest { get; set; }

        /// <summary>
        /// This variable to filter explanations by created date(from date)
        /// </summary>
        public string FromDate { get; set; }

        /// <summary>
        /// This variable to filter explanations by created date(to date)
        /// </summary>
        public string ToDate { get; set; }

        /// <summary>
        /// This string arrays to filter explanations by creators
        /// </summary>
        public string[] ChosenCreatorFilterSuperAdmin { get; set; }
    }
}
