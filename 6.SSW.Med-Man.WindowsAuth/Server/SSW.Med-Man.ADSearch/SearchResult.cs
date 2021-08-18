using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Med_Man.ADSearch
{
    public class SearchResult
    {
        public string UsereName { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public List<string> GroupMemberships { get; set; }
    }
}
