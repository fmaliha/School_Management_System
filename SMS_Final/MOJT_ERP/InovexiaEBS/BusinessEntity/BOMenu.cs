﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InovexiaEBS
{
    public class BOMenu
    {

       // public BOMenu();

        public int Level { get; set; }
        public string MenuCode { get; set; }
        public int MenuId { get; set; }
        public string MenuText { get; set; }
        public int ParentMenuId { get; set; }
        public string Url { get; set; }

        public string CssClass { get; set; }

        public string ChkBox { get; set; }

        public string Module { get; set; }

        public string SubModule { get; set; }

        public string MenuName { get; set; }

        public string ActiveButton { get; set; }

        public string DeActiveButton { get; set; }

        public bool IsActive { get; set; }

        public bool IsExpand { get; set; }

        public string IconCss { get; set; }
    }
}
