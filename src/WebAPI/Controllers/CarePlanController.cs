using Application.Common.Interfaces.IServices;
using Domain.Constants;
using Domain.Exceptions;
using Application.Dto;
using Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarePlanController : ControllerBase
    {
        private readonly IPatientCarePlanService _carePlanService;

        public CarePlanController(IPatientCarePlanService carePlanService)
        {
            _carePlanService = carePlanService;
        }


        [HttpPost("AddPatientCarePlan")]

        [ProducesResponseType(typeof(ResponseVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddPatientCarePlan([FromBody] PatientCarePlanDto carePlan)
        {
            try
            {
                switch (!ModelState.IsValid)
                {
                    case true: 
                        return BadRequest(new ResponseVM
                        {
                            Message = ResponseConstants.ModelStateInvalid,
                            Success = false
                        });
                }

                var result = await _carePlanService.AddPatientCarePlan(carePlan);
                return result.Success ? Ok(result) : BadRequest(result);

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
                return BadRequest(new ResponseVM { Message = ErrorConstants.Error });
            }
        }


        [HttpGet("GetAllPatientCarePlans")]
        [ProducesResponseType(typeof(ResponseVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPatientCarePlans()
        {
            try
            {
                var result = await _carePlanService.GetAllPatientCarePlans();
                switch (result.Success)
                {
                    case false:
                        return BadRequest(result);

                    case true:
                        switch (result.Message)
                        {
                            case ResponseConstants.NotFound:
                                return NotFound(result);

                            case ResponseConstants.Found:
                                return Ok(result);
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
            }
            return BadRequest(new ResponseVM { Message = ErrorConstants.Error });
        }



        [HttpGet("GetCarePlanById")]
        [ProducesResponseType(typeof(ResponseVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCarePlanById([FromQuery] long carePlanId)
        {
            try
            {

                switch (carePlanId)
                {
                    case 0:
                    case <0:
                        return BadRequest(new ResponseVM
                        {
                            Message = ResponseConstants.InvalidId,
                            Success = false
                        });

                }


                var result = await _carePlanService.GetCarePlanById(carePlanId);
                switch (result.Success)
                {
                    case false:
                        return BadRequest(result);

                    case true:
                        switch (result.Message)
                        {
                            case ResponseConstants.NotFound:
                                return NotFound(result);

                            case ResponseConstants.Found:
                                return Ok(result);
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
            }
            return BadRequest(new ResponseVM { Message = ErrorConstants.Error });
        }




        [HttpPut("UpdateCarePlanById")]
        [ProducesResponseType(typeof(ResponseVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCarePlanById([FromBody] PatientCarePlanDto carePlan, [FromQuery] long carePlanId)
        {
            try
            {
                switch (carePlanId)
                {
                    case 0:
                    case < 0:
                        return BadRequest(new ResponseVM
                        {
                            Message = ResponseConstants.InvalidId,
                            Success = false
                        });

                }
                switch (!ModelState.IsValid)
                {
                    case true:
                        return BadRequest(new ResponseVM
                        {
                            Message = ResponseConstants.ModelStateInvalid,
                            Success = false
                        });
                }


                var result = await _carePlanService.UpdateCarePlanById(carePlan, carePlanId);
                switch (result.Success)
                {
                    case false:
                        return BadRequest(result);

                    case true:
                        switch (result.Message)
                        {
                            case ResponseConstants.NotFound:
                                return NotFound(result);

                            case ResponseConstants.Updated:
                                return Ok(result);
                        }
                        break;
                }


            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
            }
            return BadRequest(new ResponseVM { Message = ErrorConstants.Error });
        }



        [HttpDelete("DeleteCarePlanById")]
        [ProducesResponseType(typeof(ResponseVM), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCarePlanById([FromQuery] long carePlanId, [FromQuery] string ipAddress)
        {
            try
            {
                switch (carePlanId)
                {
                    case 0:
                    case < 0:
                        return BadRequest(new ResponseVM
                        {
                            Message = ResponseConstants.InvalidId,
                            Success = false
                        });

                }

                var result = await _carePlanService.DeleteCarePlanById(carePlanId, ipAddress);
                switch (result.Success)
                {
                    case false:
                        return BadRequest(result);

                    case true:
                        switch (result.Message)
                        {
                            case ResponseConstants.NotDeleted:
                                return NotFound(result);

                            case ResponseConstants.Deleted:
                                return Ok(result);
                        }
                        break;
                }


            }
            catch (Exception ex)
            {
                LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
            }

            return BadRequest(new ResponseVM { Message = ErrorConstants.Error });
        }
    }
}
