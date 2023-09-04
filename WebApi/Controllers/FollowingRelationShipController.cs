using System.Net;
using Domain.Dtos.FollowingRelationshipDto;
using Domain.Filters.FollowingRelationShipFilter;
using Domain.Responses;
using Infrastructure.Services.FollowingRelationShipService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class FollowingRelationShipController : BaseController
{
    private readonly IFollowingRelationShipService _service;

    public FollowingRelationShipController(IFollowingRelationShipService service)
    {
        _service = service;
    }

    [HttpGet("get-FollowingRelationShips")]
    public async Task<IActionResult> GetFollowingRelationShips(FollowingRelationShipFilter filter)
    {
        var result = await _service.GetFollowingRelationShip(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-FollowingRelationShip-by-id")]
    public async Task<IActionResult> GetFollowingRelationShipById(int id)
    {
        var result = await _service.GetFollowingRelationShipById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-FollowingRelationShip")]
    public async Task<IActionResult> AddFollowingRelationShip([FromBody]AddFollowingRelationShipDto followingRelationShip)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddFollowingRelationShip(followingRelationShip);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<FollowingRelationShipDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-FollowingRelationShip")]
    public async Task<IActionResult> DeleteFollowingRelationShip(string userId, string followingId)
    {
        var result = await _service.DeleteFollowingRelationShip(userId, followingId);
        return StatusCode(result.StatusCode, result);
    }
}