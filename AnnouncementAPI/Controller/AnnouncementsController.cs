using Microsoft.AspNetCore.Mvc;

using AnnouncementAPI.Services;

using AnnouncementAPI.DTOs;

using AnnouncementAPI.Entities;





namespace AnnouncementAPI.Controllers

{



    [Route("api/[controller]")]

    [ApiController]

    public class AnnouncementsController : ControllerBase

    {

        private readonly IAnnouncementService _service;



        public AnnouncementsController(IAnnouncementService repository)

        {

            _service = repository;

        }





        [HttpGet]

        public async Task<IActionResult> Get()

        {

            var data = await _service.GetAllAsync();

            return Ok(data);

        }





        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AnnouncementDto announcementDto)

        {

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }



            var id = await _service.CreateAsync(announcementDto);

            return Ok(id);

        }





        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] AnnouncementDto announcementDto)

        {

            if (!ModelState.IsValid)

            {

                return BadRequest(ModelState);

            }



            await _service.UpdateAsync(id, announcementDto);

            return NoContent();

        }



        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)

        {

            await _service.DeleteAsync(id);

            return NoContent();

        }

    }

}