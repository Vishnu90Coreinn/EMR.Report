﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.EncounterType
{
    public class CreateEncounterTypeResponseDto
    {
        public int ID { get; set; }
        public int EncounterTypeID { get; set; }
        public string EncounterType { get; set; }
    }
}
