using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VCMS.MVC4.Web
{
    public class LocationModel
    { 
        public int City { get; set; }    
        public int State { get; set; }
        public int Wards { get; set; }      
        public int Street { get; set; }   
    }
}