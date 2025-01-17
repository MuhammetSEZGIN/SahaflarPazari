using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SahaflarPazari.Models
{
    public class ComplaintsViewModel
    {
        public string ComplaintText { get; set; }
        public string ComplaintDate { get; set; }
        
        public string BookName { get; set; }
        public string BookOwner { get; set; }
        public string ComplaintUserName { get; set; }
        public int BookId { get; set; }
        public List<string> BookImagesPath { get; set; }
    }
}