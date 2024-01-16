using Demo.Filter.ActionFilters;
using Demo.Filter.ExceptionFilters;
using Demo.Models;
using Demo.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    
    public class ShirtsController : ControllerBase
    {
        

        [HttpGet]
        //[Route("/shirts")]
        public IActionResult GetShirts()
        {
            return Ok(ShirtRepository.GetShirts());
        }
        [HttpGet("{id}")]
        //[Route("/shirts/{id}")]
        [Shirt_ValidateShirtIdFilter]
        public IActionResult GetShirtbyId(int id)
        {
            //if(id <= 0)
            //    return BadRequest();
            //var shirt =  ShirtRepository.GetShirtById(id);
            //if (shirt == null)
            //    return NotFound();
            return Ok(ShirtRepository.GetShirtById(id));
        }
        [HttpPost]
        //[Route("/shirts")]
        [Shirt_ValidationCreateShirt]
        public IActionResult CreateShirt([FromBody] Shirt shirt)

        {

            ShirtRepository.AddShirt(shirt);
            return CreatedAtAction(nameof(GetShirtbyId), new
            {
                id = shirt.ShirtId
            }, shirt);
        }
        [HttpPut("{id}")]
        // [Route("/shirts/{id}")]
        //[Shirt_ValidateShirtIdFilter]
        [Shirt_ValidateUpdateShirtFilter]
        [Shirt_HandleUpdateExceptionsFilter]
        public IActionResult UpdateShirt(int id, Shirt shirt)
        {
           
            //try
            //{
            ShirtRepository.UpdateShirt(shirt);

            //}catch(Exception e){
               // if (!ShirtRepository.ShirtExist(id))
                   // return NotFound();
                //throw;
            //}
            return NoContent();
        }

        [HttpDelete("{id}")]
        // [Route("/shirts/{id}")]
        [Shirt_ValidateShirtIdFilter]
        public IActionResult DeleteShirt(int id)
        {
            var shirt = ShirtRepository.GetShirtById(id);
            ShirtRepository.DeleteShirt(id);
            return  Ok(shirt);
        }
    }
}
