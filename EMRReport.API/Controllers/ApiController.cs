using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace EMRReport.API.Controllers
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected readonly IMapper _mapper;
        protected ApiController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}