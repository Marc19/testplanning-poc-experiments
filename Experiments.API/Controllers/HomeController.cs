using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Experiments.Application.IServices;
using Experiments.Core;
using Experiments.Core.DTOs;
using Experiments.Core.IKafka;
using Experiments.Core.Messages.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Experiments.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Homecontroller : ControllerBase
    {
        private readonly IKafkaProducer _kafkaProducer;
        private readonly IConfiguration Configuration;
        private readonly string EXPERIMENT_TOPIC;

        public Homecontroller(IKafkaProducer kafkaProducer, IConfiguration configuration)
        {
            _kafkaProducer = kafkaProducer;
            Configuration = configuration;
            EXPERIMENT_TOPIC = Configuration["ExperimentsTopic"];
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Up and running..." + EXPERIMENT_TOPIC);
        }

        [HttpPost]
        public IActionResult CreateExperiment([FromBody] ExperimentDTO experimentDTO)
        {
            long loggedInUserId = GetLoggedInUserIdMockUp();

            if (loggedInUserId == -1)
                return Unauthorized();

            CreateExperiment createExperiment =
                new CreateExperiment(experimentDTO.Creator, experimentDTO.Name, loggedInUserId);

            _kafkaProducer.Produce(createExperiment, EXPERIMENT_TOPIC);

            return Ok("Currently processing your request...");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExperiment(long id)
        {
            long loggedInUserId = GetLoggedInUserIdMockUp();

            if (loggedInUserId == -1)
                return Unauthorized();

            DeleteExperiment deleteExperiment = new DeleteExperiment(id, loggedInUserId);

            _kafkaProducer.Produce(deleteExperiment, EXPERIMENT_TOPIC);

            return Ok("Currently processing your request...");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateExperiment(long id, [FromBody]ExperimentDTO experimentDTO)
        {
            long loggedInUserId = GetLoggedInUserIdMockUp();

            if (loggedInUserId == -1)
                return Unauthorized();

            UpdateExperiment updateExperiment =
                new UpdateExperiment(id, experimentDTO.Creator, experimentDTO.Name, loggedInUserId);

            _kafkaProducer.Produce(updateExperiment, EXPERIMENT_TOPIC);

            return Ok("Currently processing your request...");
        }

        private long GetLoggedInUserIdMockUp()
        {
            var authorizationHeader = Request.Headers[HeaderNames.Authorization].ToString();
            if (authorizationHeader == "")
                return -1;

            string jwtInput = authorizationHeader.Split(' ')[1];

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(jwtInput)) throw new Exception("The token doesn't seem to be in a proper JWT format.");

            var token = jwtHandler.ReadJwtToken(jwtInput);

            var jwtPayload = JsonConvert.SerializeObject(token.Claims.Select(c => new { c.Type, c.Value }));

            JArray rss = JArray.Parse(jwtPayload);
            var firstChild = rss.First;
            var lastChild = firstChild.Last;
            var idString = lastChild.Last.ToString();

            long.TryParse(idString, out long id);

            return id;
        }

    }
}

/*
 * Token with id 1:
 * eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MSwibmFtZSI6Im1hcmMifQ.aQ9IVm74fEFgxBJY244kXy7XvzFikimzMu9MfGGfavs
 *
 * Token with id 2:
 * eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MiwibmFtZSI6InNoZXJpZiJ9.VDc9DFmDA_3Tcy5KkjGLUkLpug0v4orLdBdOJ_L8XL0
 */
