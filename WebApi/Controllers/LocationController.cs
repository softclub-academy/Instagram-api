﻿using System.Net;
using Domain.Dtos.LocationDto;
using Domain.Filters.LocationFilter;
using Domain.Responses;
using Infrastructure.Services.LocationDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("[controller]")]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ILocationService _service;

    public LocationController(ILocationService service)
    {
        _service = service;
    }

    [HttpGet("get-Locations")]
    public async Task<IActionResult> GetLocations([FromQuery]LocationFilter filter)
    {
        var result = await _service.GetLocations(filter);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("get-Location-by-id")]
    public async Task<IActionResult> GetLocationById(int id)
    {
        var result = await _service.GetLocationById(id);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("add-Location")]
    public async Task<IActionResult> AddLocation([FromQuery]AddLocationDto location)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.AddLocation(location);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<LocationDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }
    
    [HttpPut("update-Location")]
    public async Task<IActionResult> UpdateLocation([FromQuery]UpdateLocationDto location)
    {
        if (ModelState.IsValid)
        {
            var result = await _service.UpdateLocation(location);
            return StatusCode(result.StatusCode, result);
        }

        var errors = ModelState.SelectMany(e => e.Value.Errors.Select(er => er.ErrorMessage)).ToList();
        var response = new Response<LocationDto>(HttpStatusCode.BadRequest, errors);
        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("delete-Location")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var result = await _service.DeleteLocation(id);
        return StatusCode(result.StatusCode, result);
    }
}