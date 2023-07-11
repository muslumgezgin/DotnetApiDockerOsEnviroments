using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace IosEnviroment.Services;

public class DockerService
{
    private readonly DockerClient _dockerClient;

    public DockerService(IConfiguration configuration)
    {
        var dockerHost = configuration["DockerHost"]; // Retrieve Docker host from configuration
        _dockerClient = new DockerClientConfiguration(new Uri(dockerHost)).CreateClient();
    }

    public async Task<List<string>> GetDockerOSEnvironments()
    {
        var containers = await _dockerClient.Containers.ListContainersAsync(
            new ContainersListParameters()
            {
                Filters = new Dictionary<string, IDictionary<string, bool>>()
                {
                    {
                        "label", new Dictionary<string, bool>()
                        {
                            { "com.docker.compose.project", true }
                        }
                    }
                }
            });

        var environments = new List<string>();
        foreach (var container in containers)
        {
            if (container.Labels.TryGetValue("com.docker.compose.project", out var environment))
            {
                environments.Add(environment);
            }
        }

        return environments;
    }
}