﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgNetControl.Helpers.Services
{
    public class AnalysisRunDefaultHostedService : BackgroundService
    {
        /// <summary>
        /// Represents the configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Represents the logger.
        /// </summary>
        private readonly ILogger<AnalysisRunDefaultHostedService> _logger;

        /// <summary>
        /// Represents the host application lifetime.
        /// </summary>
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="configuration">Represents the application configuration.</param>
        /// <param name="logger">Represents the logger.</param>
        /// <param name="hostApplicationLifetime">Represents the application lifetime.</param>
        public AnalysisRunDefaultHostedService(IConfiguration configuration, ILogger<AnalysisRunDefaultHostedService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _configuration = configuration;
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        /// <summary>
        /// Executes the background service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token corresponding to the task.</param>
        /// <returns>A runnable task.</returns>
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Log a message.
            _logger.LogInformation(string.Concat(
                "\n\tWelcome to the GeneticAlgNetControl application!",
                "\n\t",
                "\n\t---",
                "\n\t",
                "\n\tThe following arguments can be provided:",
                "\n\t--Help\tUse this argument to display this help message.",
                "\n\t--Mode\tUse this argument to apecify the mode in which the application will run. The possible values are \"Web\" (the application will run as a local web server) and \"Cli\" (the application will run in the command-line). The default value is \"Web\".",
                "\n\t--Urls\tUse this argument to specify the local address on which to run the web server. It can also be configured in the \"appsettings.json\" file. The default value is \"http://localhost:5000\".",
                "\n\t--Edges\t(only for CLI) Use this argument to specify the path to the file containing the edges of the network. Each edge should be on a new line, with its source and target nodes being separated by a semicolon character. This argument has no default value.",
                "\n\t--Targets\t(only for CLI) Use this argument to specify the path to the file containing the target nodes of the network. Only nodes appearing in the network will be considered. Each node should be on a new line. This argument has no default value.",
                "\n\t--Preferred\t(only for CLI, optional) Use this argument to specify the path to the file containing the preferred nodes of the network. Only nodes appearing in the network will be considered. Each node should be on a new line. This argument has no default value.",
                "\n\t--Parameters\t(only for CLI) Use this argument to specify the path to the file containing the parameter values for the analysis. The file should be in JSON format, as shown in the \"DefaultParameters.json\" file, containing the default parameter values. This argument has no default value.",
                "\n\t",
                "\n\t---",
                "\n\t",
                "\n\tExamples of posible usage:",
                "\n\t--Help \"True\"",
                "\n\t--Mode \"Web\"",
                "\n\t--Mode \"Cli\" --Edges \"Path/To/FileContainingEdges.extension\" --Targets \"Path/To/FileContainingTargetNodes.extension\" --Parameters \"Path/To/FileContainingParameters.extension\"",
                "\n\t"));
            // Wait for a completed task, in order to not get a warning about having an async method.
            await Task.CompletedTask;
            // Stop the application.
            _hostApplicationLifetime.StopApplication();
            // Return a successfully completed task.
            return;
        }
    }
}
