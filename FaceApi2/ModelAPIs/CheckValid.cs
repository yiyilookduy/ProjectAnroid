using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using FaceApi2.Models;

namespace FaceApi2.ModelAPIs
{
    public class CheckValid
    {
        public bool IsValid { get; set; }

        public string StudentId { get; set; }

        public string Username { get; set; }

        public string TeacherId { get; set; }

        public string ErrorMessage { get; set; }

        private StringBuilder ErrorMessageBuilder { get; set; }

        public String Type { get; set; }

        public CheckValid()
        {
            ErrorMessageBuilder = new StringBuilder();
            ErrorMessage = string.Empty;
            IsValid = true;
        }


        public bool IsExistedStudentId(string studentId)
        {
            StudentId = studentId;

            FaceIOContext context = new FaceIOContext();

            var result = context.Student.Where(x => x.Id == StudentId).FirstOrDefault() != null;

            if (!result)
            {
                ErrorMessageBuilder.Append("Student ID is not found \n");
                ErrorMessage = ErrorMessageBuilder.ToString();
            }

            return result;
        }

        public bool IsExistedUsername(string username)
        {
            Username = username;

            FaceIOContext context = new FaceIOContext();

            var result = context.Users.Where(x => x.Username == Username).FirstOrDefault() != null;

            if (!result)
            {
                ErrorMessageBuilder.Append("Username is not found \n");
                ErrorMessage = ErrorMessageBuilder.ToString();
            }

            return result;

        }

        public bool IsExistedTeacherId(string teacherId)
        {
            TeacherId = teacherId;

            FaceIOContext context = new FaceIOContext();

            var result = context.Teacher.Where(x => x.Id == TeacherId).FirstOrDefault() != null;

            if (!result)
            {
                ErrorMessageBuilder.Append("Teacher Id is not found \n");
                ErrorMessage = ErrorMessageBuilder.ToString();
            }

            return result;

        }

        public bool StringIsNullOrEmpty(string data)
        {
            var result = string.IsNullOrEmpty(data);

            if (result)
            {
                ErrorMessageBuilder.Append("Data is can not be blank \n");
                ErrorMessage = ErrorMessageBuilder.ToString();
            }

            return result;
        }
    }
}
