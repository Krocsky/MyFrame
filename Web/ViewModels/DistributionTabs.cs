using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    [Serializable]
    public class DistributionTabs
    {
        public DistributionTabs()
        {
            title = "";
            key = 0;
            folder = false;
            selected = false;
            children = new List<DistributionTabs>();
        }

        public string title { get; set; }

        public int key { get; set; }

        public bool folder { get; set; }

        public bool selected { get; set; }
        public List<DistributionTabs> children { get; set; }
    }
}