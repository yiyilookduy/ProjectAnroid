using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceDetector
{
    class Program
    {
        private static IFaceClient faceClient;
        private static CloudStorageAccount storageAccount;



        static async Task Main(string[] args)
        {
            try
            {
                
                string choice = "";
                do
                {
                    Console.Clear();
                    Console.Write("1. Init\n2. Create Person Group\n3. Define A Person\n4. Training A Person Group\n5. Identify face\n6. Exit\nChoose a function: ");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            {
                                //Console.Write("Enter FaceKey: ");
                                //faceClient = new FaceClient(new ApiKeyServiceClientCredentials(Console.ReadLine()));
                                //Console.Write("Enter FaceEndPoint: ");
                                //faceClient.Endpoint = Console.ReadLine();

                                faceClient = new FaceClient(new ApiKeyServiceClientCredentials("a8619b52f7764b22bbdf8b37255e0335"));
                                faceClient.Endpoint = @"https://duy.cognitiveservices.azure.com/";
                                //Console.Write("Enter ConnectionString: ");
                                //storageAccount = CloudStorageAccount.Parse(Console.ReadLine());
                                Console.WriteLine("Init succeeded");
                                break;
                            }
                        case "2":
                            {
                                Console.Write("Enter ID of person group: ");
                                var id = Console.ReadLine();
                                Console.Write("Enter name of person group: ");
                                await CreatePersonGroupAsync(id, Console.ReadLine());
                                Console.WriteLine("Create succeeded");
                                break;
                            }
                        case "3":
                            {
                                Console.Write("Enter person group ID: ");
                                var id = Console.ReadLine();
                                Console.Write("Enter name of a person you want to assign: ");
                                var personName = Console.ReadLine();
                                Console.Write("Enter folder path contain pictures of the person you want to assign(.jpeg, .jpg, .png): ");
                                var folderPath = Console.ReadLine();
                                Console.Write("Enter Blobname: ");
                                await DefineAPersonAsync(id, personName, folderPath,Console.ReadLine());
                                Console.WriteLine("Assign succeeded");
                                break;
                            }
                        case "4":
                            {
                                Console.Write("Enter person group ID: ");
                                await TrainModel(Console.ReadLine());
                                Console.WriteLine("Train succeeded");
                                break;
                            }

                        case "5":
                            {
                                Console.Write("Enter folder path contain pictures of the person you want to assign(.jpeg, .jpg, .png): ");
                                var folderPath = Console.ReadLine();
                                await IdentifyFaceAsync(folderPath);
                                break;
                            }

                    }
                    Console.ReadLine();
                   
                } while (!choice.Equals("6"));

            }
            catch (Exception e)
            {
                Console.WriteLine("Connection string may have error, please init again");
                Console.ReadLine();
            }
        }

        private static async Task IdentifyFaceAsync(string folderPath)
        {
            List<Guid> sourceFaceIds = new List<Guid>();
            // Detect faces from source image url.
            List<DetectedFace> detectedFaces = await DetectFaceRecognize(faceClient, folderPath, RecognitionModel.Recognition01);

            // Add detected faceId to sourceFaceIds.
            foreach (var detectedFace in detectedFaces) { sourceFaceIds.Add(detectedFace.FaceId.Value); }

            // Identify the faces in a person group. 
            var identifyResults = await faceClient.Face.IdentifyAsync(sourceFaceIds, "test1");

            foreach (var identifyResult in identifyResults)
            {
                Person person = await faceClient.PersonGroupPerson.GetAsync("test1", identifyResult.Candidates[0].PersonId);
                Console.WriteLine($"Person '{person.Name}' is identified for face in: - {identifyResult.FaceId}," +
                                  $" confidence: {identifyResult.Candidates[0].Confidence}.");
            }
            Console.WriteLine();
        }

        //private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string url, string RECOGNITION_MODEL1)
        //{
        //    // Detect faces from image URL. Since only recognizing, use the recognition model 1.
        //    IList<DetectedFace> detectedFaces = await faceClient.Face.DetectWithUrlAsync(url, recognitionModel: RECOGNITION_MODEL1);
        //    Console.WriteLine($"{detectedFaces.Count} face(s) detected from image `{Path.GetFileName(url)}`");
        //    return detectedFaces.ToList();
        //}

        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, string folderPath, string RECOGNITION_MODEL1)
        {
            // Detect faces from image URL. Since only recognizing, use the recognition model 1.
            IList<DetectedFace> detectedFaces = new List<DetectedFace>();

            foreach (string imagePath in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpeg") || s.EndsWith(".jpg") || s.EndsWith(".png")))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    try
                    {

                        //await InteractAccountStorageAsync(imagePath,personName,blobname);
                        detectedFaces = await faceClient.Face.DetectWithStreamAsync(s, recognitionModel: RECOGNITION_MODEL1);
                        Console.WriteLine($"{detectedFaces.Count} face(s) detected from image ");

                    }
                    catch (APIErrorException ex)
                    {
                        if (ex.Response.StatusCode != System.Net.HttpStatusCode.BadRequest && ex.Response.Content.Contains("No Face Detected", StringComparison.OrdinalIgnoreCase))
                        {
                            throw;
                        }
                        Console.WriteLine($"{imagePath} not readable by face regconition");
                        continue;
                    }

                }
            }

            return (List<DetectedFace>) detectedFaces;
        }
        


        static async Task<string> CreatePersonGroupAsync(string personGroupId, string personGroupName)
        {
            var personGroups = await faceClient.PersonGroup.ListAsync();
            if (!personGroups.Any(x => x.PersonGroupId == personGroupId))
            {
                await faceClient.PersonGroup.CreateAsync(personGroupId, personGroupName);
            }
            return personGroupId;
        }


        static async Task DefineAPersonAsync(string personGroupId, string personName, string folderPath, string blobname)
        {
            var person = await faceClient.PersonGroupPerson.CreateAsync(personGroupId, personName);
            await RegisterAsync(personGroupId, person.PersonId, folderPath, blobname, personName);

        }

        static async Task TrainModel(string personGroupId)
        {
            await faceClient.PersonGroup.TrainAsync(personGroupId);
            TrainingStatus trainingStatus = null;
            while (true)
            {
                trainingStatus = await faceClient.PersonGroup.GetTrainingStatusAsync(personGroupId);

                if (trainingStatus.Status != TrainingStatusType.Running)
                {
                    break;
                }

                await Task.Delay(1000);
            }
        }


        static async Task RegisterAsync(string personGroupId, Guid personID, string folderPath, string blobname, string personName)
        {
            foreach (string imagePath in Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpeg") || s.EndsWith(".jpg") || s.EndsWith(".png")))
            {
                using (Stream s = File.OpenRead(imagePath))
                {
                    try
                    {
                        //await InteractAccountStorageAsync(imagePath,personName,blobname);
                        await faceClient.PersonGroupPerson.AddFaceFromStreamAsync(personGroupId, personID, s);
                    }
                    catch (APIErrorException ex)
                    {
                        if (ex.Response.StatusCode != System.Net.HttpStatusCode.BadRequest && ex.Response.Content.Contains("No Face Detected", StringComparison.OrdinalIgnoreCase))
                        {
                            throw;
                        }
                        Console.WriteLine($"{imagePath} not readable by face regconition");
                        continue;
                    }

                }
            }

        }
    }
}
