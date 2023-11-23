using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMRReport.API.DataTranserObject.EncounterType
{
    public sealed class GetEncounterTypeDDLResponseDto
    {
        public int EncounterTypeID { get; set; }
        public string EncounterTypeName { get; set; }
    }
}
