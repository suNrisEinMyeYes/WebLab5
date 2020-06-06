﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLab5.Models
{
    public class HospitalDoctor
    {
        public Int32 HospitalId { get; set; }

        public Hospital Hospital { get; set; }

        public Int32 DoctorId { get; set; } 

        public Doctor Doctor { get; set; }
    }
}
