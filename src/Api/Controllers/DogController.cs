namespace DogShelterService.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using DogShelterService.Api.DTOs;
    using DogShelterService.Api.Mappers.Interfaces;
    using DogShelterService.Application.DTOs;
    using DogShelterService.Application.Queries;
    using DogShelterService.Application.Services.Interfaces;
    using EnsureThat;
    using Microsoft.AspNetCore.Mvc;

    [ApiController()]
    [Route("[controller]")]
    public class DogsController : ControllerBase
    {
        private readonly IDogService dogService;
        private readonly IDogDtoMapper dogDtoMapper;

        public DogsController(IDogService dogService, IDogDtoMapper dogDtoMapper)
        {
            EnsureArg.IsNotNull(dogService, nameof(dogService));
            EnsureArg.IsNotNull(dogDtoMapper, nameof(dogDtoMapper));

            this.dogService = dogService;
            this.dogDtoMapper = dogDtoMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DogDto>>> GetDogsAsync([FromQuery] SearchDogsRequestDTO searchDog, CancellationToken cancellationToken = default)
        {
            var foundDogs = await dogService.GetDogsAsync(searchDog, cancellationToken);

            if (foundDogs.Any())
            {
                var dogsDto = dogDtoMapper.Map(foundDogs);

                return Ok(dogsDto);
            }

            return NotFound(null);
        }

        [HttpPost("dogs")]
        public async Task<ActionResult<DogDto>> SaveDogsAsync([FromBody] DogCreationRequestDTO request, CancellationToken cancellationToken = default)
        {
            var savedDog = await dogService.PostDogAsync(request, cancellationToken);

            if (savedDog is null)
            {
                return BadRequest("Unable to save the dogs");
            }

            var dogDto = dogDtoMapper.Map(savedDog);

            return StatusCode((int)HttpStatusCode.Created, dogDto);
        }
    }
}