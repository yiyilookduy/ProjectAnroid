using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceApi2.ModelAPIs;
using FaceApi2.Models;
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
            faceClient = new FaceClient(new ApiKeyServiceClientCredentials("f6e8602133a243918bc8af7e71df8856"));
            faceClient.Endpoint = "https://duy1.cognitiveservices.azure.com/";
        }

        [HttpPost("/Face/CreatePersonGroup")]
        public IActionResult CreatePersonGroup(string personGroupId, string personGroupName)
        {
            try
            {
                var personGroups = faceClient.PersonGroup.ListAsync().Result;

                if (!personGroups.Any(x => x.PersonGroupId == personGroupId))
                {
                    faceClient.PersonGroup.CreateAsync(personGroupId, personGroupName).Wait();
                }

                return Ok(new BaseResponse(personGroupId, "Success!!!", true));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }


        [HttpPost("/Face/DefineAPerson")]
        public IActionResult DefineAPerson(string personGroupId, string personName, string url)
        {
            try
            {
                var person = faceClient.PersonGroupPerson.CreateAsync(personGroupId, personName).Result;

                faceClient.PersonGroupPerson.AddFaceFromUrlAsync(personGroupId, person.PersonId, url);

                return Ok(new BaseResponse(null, "Success!!!", true));

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));

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

                return Ok(new BaseResponse(null, "Success!!!", true));

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));

            }
        }



        [HttpPost("/Face/IdentifyFace")]
        public IActionResult IdentifyFace(string url, string personGroupId)
        {
            try
            {
                List<Guid> sourceFaceIds = new List<Guid>();
                // Detect faces from source image url.
                List<DetectedFace> detectedFaces = DetectFaceRecognize(faceClient, url, RecognitionModel.Recognition01).Result;

                // Add detected faceId to sourceFaceIds.
                foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }

                // Identify the faces in a person group. 
                var identifyResults = faceClient.Face.IdentifyAsync(sourceFaceIds, personGroupId).Result;

                Person person = faceClient.PersonGroupPerson.GetAsync(personGroupId, identifyResults[0].Candidates[0].PersonId).Result;

                UpdateAttendance(person.Name, true);

                return Ok(new BaseResponse(person, "Success!!!", true));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));

            }
        }


        private int getSlot(DateTime timeRequest)
        {
            if (TimeBetween(timeRequest, new TimeSpan(07, 0, 0), new TimeSpan(8, 30, 0)))
            {
                return 1;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(08, 45, 0), new TimeSpan(10, 15, 0)))
            {
                return 2;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(10, 30, 0), new TimeSpan(12, 00, 0)))
            {
                return 3;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(12, 30, 0), new TimeSpan(2, 0, 0)))
            {
                return 4;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(2, 15, 0), new TimeSpan(3, 45, 0)))
            {
                return 5;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(4, 00, 0), new TimeSpan(5, 30, 0)))
            {
                return 6;
            }
            else
                return 0;
        }

        private List<ClassSubject> GetClassSubjectByStudentId(string studentId)
        {
            var context = new FaceIOContext();
            var classSubjectId = context.StudentStudy.Where(s => s.StudentId == studentId).ToList<StudentStudy>();
            var listClassSubject = new List<ClassSubject>();
            foreach (StudentStudy ss in classSubjectId)
            {
                var classSubject = context.ClassSubject.Where(c => c.Id == ss.ClassSubjectId).FirstOrDefault();
                listClassSubject.Add(classSubject);
            }

            return listClassSubject;
        }

        private bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        private List<Schedule> GetScheduleByClassSubjectId(int ClassSubjectId)
        {
            var context = new FaceIOContext();

            var ListClassSubjectSchedule = context.ClassSubjectSchedule.Where(x => x.ClassSubjectId == ClassSubjectId).ToList<ClassSubjectSchedule>();

            var ListSchedule = new List<Schedule>();

            foreach (var ClassSubjectSchedule in ListClassSubjectSchedule)
            {
                var Schedule = context.Schedule.Where(x => x.Id == ClassSubjectSchedule.ScheduleId).FirstOrDefault();
                ListSchedule.Add(Schedule);
            }

            return ListSchedule;
        }
        public bool UpdateAttendance(string studentId, bool attendance)
        {
            CheckValid valid = new CheckValid();
            var timeRequest = DateTime.Now;
            var slotJoin = getSlot(timeRequest);
            var check = false;

            try
            {
                if (!valid.IsExistedStudentId(studentId))
                    valid.IsValid = false;
                if (valid.StringIsNullOrEmpty(attendance.ToString()))
                    valid.IsValid = false;

                if (valid.IsValid)
                {
                    var listClassSubject = GetClassSubjectByStudentId(studentId);

                    if (listClassSubject.Count > 0)
                    {
                        foreach (var classSubject in listClassSubject)
                        {
                            var ListSchedule = GetScheduleByClassSubjectId(classSubject.Id).Where(x => x.Date == timeRequest.Date);
                            foreach (var schedule in ListSchedule)
                            {
                                if (schedule.Slot == slotJoin)
                                {
                                    var context = new FaceIOContext();
                                    var classSubjectSchedule = context.ClassSubjectSchedule.Where(x =>
                                        x.ClassSubjectId == classSubject.Id && x.ScheduleId == schedule.Id).FirstOrDefault();

                                    var studentStudyAttendance =
                                        context.StudentStudyAttendance.Where(x =>
                                            x.ClassSubjectScheduleId == classSubjectSchedule.Id).FirstOrDefault();

                                    if (studentStudyAttendance != null)
                                    {
                                        studentStudyAttendance.Attendance = true;

                                        context.StudentStudyAttendance.Update(studentStudyAttendance);

                                        context.SaveChanges();

                                        check = true;
                                        break;
                                    }
                                }

                                else continue;
                            }

                            if (check) break;
                        }

                        if (check == false)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return false;
                }

                return false;


            }
            catch (Exception e)
            {
                return false;
            }
        }




        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string RECOGNITION_MODEL1)
        {
            IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: RECOGNITION_MODEL1);
            return detectedFaces.ToList();
        }


    }
}