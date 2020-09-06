using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRUDWithWebAPI.Models
{
    public class State
    {
        public int StateId { get; set; }

        public string Name { get; set; }

        public int CountryId { get; set; }
    }
}