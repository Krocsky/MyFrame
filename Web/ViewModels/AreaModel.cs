using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web.ViewModels
{
    public class AreaModel
    {
        public string Name { get; set; }

        public int Code { get; set; }

        public List<AreaModel> Children { get; set; }
    }
}