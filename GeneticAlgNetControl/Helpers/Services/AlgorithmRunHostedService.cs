﻿using GeneticAlgNetControl.Data;
using GeneticAlgNetControl.Data.Enumerations;
using GeneticAlgNetControl.Data.Models;
using GeneticAlgNetControl.Helpers.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeneticAlgNetControl.Helpers.Services
{
    /// <summary>
    /// Represents the hosted service corresponding to an algorithm run.
    /// </summary>
    public class AlgorithmRunHostedService : BackgroundService
    {
        /// <summary>
        /// Represents the service scope factory.
        /// </summary>
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Represents the logger.
        /// </summary>
        private readonly ILogger<AlgorithmRunHostedService> _logger;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="serviceScopeFactory">Represents the service scope factory.</param>
        /// <param name="logger">Represents the logger.</param>
        public AlgorithmRunHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<AlgorithmRunHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// Launches the algorithm run execution.
        /// </summary>
        /// <param name="stopToken">The cancellation token corresponding to the task.</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            // Get the application context.
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            // Go over each algorithm in the database that are ongoing at start.
            foreach (var algorithm in context.Algorithms.Where(item => item.Status == AlgorithmStatus.Ongoing))
            {
                // Update its status.
                algorithm.Status = AlgorithmStatus.Scheduled;
            }
            // Save the changes to the database.
            await context.SaveChangesAsync();
            // Repeat the task.
            while (!stopToken.IsCancellationRequested)
            {
                // Get the first scheduled algorithms in the database.
                var algorithm = context.Algorithms.FirstOrDefault(item => item.Status == AlgorithmStatus.Scheduled);
                // Check if there wasn't any algorithm found.
                if (algorithm == null)
                {
                    // Wait for 30 seconds.
                    await Task.Delay(30000);
                    // Continue.
                    continue;
                }
                // Mark the algorithm for updating.
                context.Update(algorithm);
                // Update the algorithm status and stats.
                algorithm.Status = AlgorithmStatus.Ongoing;
                algorithm.DateTimeStarted = DateTime.Now;
                algorithm.DateTimePeriods = algorithm.DateTimePeriods.Append(new DateTimePeriod { DateTimeStarted = algorithm.DateTimeStarted, DateTimeEnded = null }).ToList();
                // Save the changes in the database.
                await context.SaveChangesAsync();
                // Reload it for a fresh start.
                await context.Entry(algorithm).ReloadAsync();
                // Get the edges, nodes, target nodes, preferred nodes and parameters.
                var nodes = algorithm.Nodes;
                var edges = algorithm.Edges;
                var targetNodes = algorithm.TargetNodes;
                var preferredNodes = algorithm.PreferredNodes;
                var parameters = algorithm.Parameters;
                // Get the additional needed variables.
                var nodeIndex = Algorithm.GetNodeIndex(nodes);
                var nodeIsPreferred = Algorithm.GetNodeIsPreferred(nodes, preferredNodes);
                var matrixA = Algorithm.GetMatrixA(nodeIndex, edges);
                var matrixC = Algorithm.GetMatrixC(nodeIndex, targetNodes);
                var powersMatrixA = Algorithm.GetPowersMatrixA(matrixA, parameters.MaximumPathLength);
                var powersMatrixCA = Algorithm.GetPowersMatrixCA(matrixC, powersMatrixA);
                var targetAncestors = Algorithm.GetTargetAncestors(powersMatrixA, targetNodes, nodeIndex);
                // Set up the current iteration.
                var random = new Random(parameters.RandomSeed);
                var currentIteration = algorithm.CurrentIteration;
                var currentIterationWithoutImprovement = algorithm.CurrentIterationWithoutImprovement;
                var population = !algorithm.Population.Chromosomes.Any() ? new Population(nodeIndex, targetNodes, targetAncestors, powersMatrixCA, parameters, random) : algorithm.Population;
                var bestFitness = population.HistoricBestFitness.Max();
                // Save the changes in the database.
                await context.SaveChangesAsync();
                // Move through the generations.
                while (!stopToken.IsCancellationRequested && algorithm != null && algorithm.Status == AlgorithmStatus.Ongoing && currentIteration < parameters.MaximumIterations && currentIterationWithoutImprovement < parameters.MaximumIterationsWithoutImprovement)
                {
                    // Move on to the next iterations.
                    currentIteration += 1;
                    currentIterationWithoutImprovement += 1;
                    // Move on to the next population.
                    population = new Population(population, nodeIndex, targetNodes, targetAncestors, powersMatrixCA, nodeIsPreferred, parameters, random);
                    // Get the best fitness of the current population.
                    var fitness = population.HistoricBestFitness.Last();
                    // Check if the current solution is better than the previous solution.
                    if (bestFitness < fitness)
                    {
                        // Update the fitness.
                        bestFitness = fitness;
                        currentIterationWithoutImprovement = 0;
                    }
                    // Update the iteration count.
                    algorithm.CurrentIteration = currentIteration;
                    algorithm.CurrentIterationWithoutImprovement = currentIterationWithoutImprovement;
                    // Save the changes in the database.
                    await context.SaveChangesAsync();
                    // Reload it for a fresh start.
                    await context.Entry(algorithm).ReloadAsync();
                }
                // Check if the algorithm doesn't exist anymore (if it has been deleted).
                if (algorithm == null)
                {
                    // End the function.
                    continue;
                }
                // Update the solutions, end time and the status.
                algorithm.Population = population;
                algorithm.Status = algorithm.Status == AlgorithmStatus.ScheduledToStop ? AlgorithmStatus.Stopped : AlgorithmStatus.Completed;
                algorithm.DateTimeEnded = DateTime.Now;
                algorithm.DateTimePeriods = algorithm.DateTimePeriods.SkipLast(1).Append(new DateTimePeriod { DateTimeStarted = algorithm.DateTimeStarted, DateTimeEnded = algorithm.DateTimeEnded }).ToList();
                // Save the changes in the database.
                await context.SaveChangesAsync();
            }
        }
    }
}
