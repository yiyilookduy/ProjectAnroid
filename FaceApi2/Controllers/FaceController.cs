using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceApi2.ModelAPIs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.VisualBasic;

namespace FaceApi2.Controllers
{
    public class FaceController : Controller
    {
        private static IFaceClient faceClient;

        static FaceController()
        {
            faceClient = new FaceClient(new ApiKeyServiceClientCredentials("a8619b52f7764b22bbdf8b37255e0335"));
            faceClient.Endpoint = "https://duy.cognitiveservices.azure.com/";
        }

        [HttpPost("/Face/CreatePersonGroup")]
        public IActionResult CreatePersonGroup(string personGroupId, string personGroupName)
        {
            try
            {
                var personGroups =  faceClient.PersonGroup.ListAsync().Result;
                if (!personGroups.Any(x => x.PersonGroupId == personGroupId))
                {
                     faceClient.PersonGroup.CreateAsync(personGroupId, personGroupName).Wait();
                }
                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error", false));
            }
        }

        [HttpPost("/Face/DefineAPerson")]
        public IActionResult DefineAPerson(string personGroupId, string personName, string url)
        {
            try
            {
                var person = faceClient.PersonGroupPerson.CreateAsync(personGroupId, personName).Result;

                faceClient.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId,  person.PersonId, url);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error", false));
            }
        }

        [HttpPost("/Face/TrainAPersonGroup")]
        public IActionResult TrainAPersonGroup(string personGroupId)
        {
            try
            {
                faceClient.PersonGroup.TrainAsync(personGroupId);

                TrainingStatus trainingStatus = null;

                while (true)
                {
                    trainingStatus = faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId).Result;

                    if (trainingStatus.Status != TrainingStatusType.Running)
                    {
                        break;
                    }

                    Task.Delay(1000).Wait();
                }
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error", false));
            }
        }


    }
}