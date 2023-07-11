using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IosEnviroment.Services;
using Microsoft.AspNetCore.Mvc;

namespace IosEnviroment.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocketOSController : ControllerBase
{
    private readonly ILogger<DocketOSController> _logger;
    private readonly DockerService _dockerService;

    public DocketOSController(ILogger<DocketOSController> logger, DockerService dockerService)
    {
        _logger = logger;
        _dockerService = dockerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetDocketOSEnvironments()
    {
        try
        {
            var environments = await _dockerService.GetDockerOSEnvironments();
            return Ok(environments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Docket OS environments.");
            return StatusCode(500, "An error occurred while retrieving Docket OS environments.");
        }
    }
}