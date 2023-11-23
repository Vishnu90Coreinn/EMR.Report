using System.Collections.Generic;

namespace EMRReport.API.DataTranserObject.ScrubberError
{
    public sealed class GetScrubberErrorGroupResponseDto
    {
        public int BasketGroupID { get; set; }
        public int TotalRows { get; set; }
        public List<GetScrubberErrorResponseDto> GetScrubberErrorResponseDtoList { get; set; }
    }
}
