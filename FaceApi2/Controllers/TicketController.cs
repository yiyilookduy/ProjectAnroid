using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FaceApi2.ModelAPIs;
using FaceApi2.Models;
using Microsoft.AspNetCore.Mvc;

namespace FaceApi2.Controllers
{
    public class TicketController : Controller
    {
        [HttpPost("/Ticket/CreateTicket")]
        public IActionResult CreateTicket(string studentId, string teacherId, string content)
        {
            try
            {
                var context = new FaceIOContext();
                var student = context.Users.Where(q => q.Username == studentId).FirstOrDefault();
                var teacher = context.Teacher.Where(t => t.Id == teacherId).FirstOrDefault();

                if (student == null || teacher == null || string.IsNullOrEmpty(content))
                {
                    return NotFound(new BaseResponse(null, "Teacher or student not found or content is blank", false));
                }
                else
                {
                    context.Ticket.Add(new Ticket()
                    {
                        Content = content,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddDays(10),
                        Status = "Open"
                    });


                    context.SaveChanges();

                    context.StudentTeacherTicket.Add(new StudentTeacherTicket()
                    {
                        StudentId =  studentId,
                        TeacherId = teacherId,
                        TicketId = context.Ticket.Where(t => t.Content == content).FirstOrDefault().Id
                    });

                    context.SaveChanges();
                }

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        [HttpGet("/Ticket/GetTicketByStudentId")]
        public IActionResult GetTicketByStudentId(string studentId)
        {
            try
            {
                var context = new FaceIOContext();
                if (string.IsNullOrEmpty(studentId))
                {
                    return NotFound(new BaseResponse(null, "Student ID cannot be blank", false));
                }
                else
                {
                    var listIdTicket = context.StudentTeacherTicket.Where(x => x.StudentId == studentId).Select(x => x.TicketId)
                        .ToList();

                    var tickets = context.Ticket.Where(x => listIdTicket.Contains(x.Id)).ToList();

                    var result = new List<Object>();

                    foreach (var ticket in tickets)
                    {
                        result.Add(new
                        {
                            id = ticket.Id,
                            content = ticket.Content,
                            startDate = ticket.StartDate,
                            endDate = ticket.EndDate,
                            status = ticket.Status,
                            teacherId = context.StudentTeacherTicket.Where(x => x.TicketId == ticket.Id).FirstOrDefault()
                                .TeacherId
                    });
                    }

                    return Ok(new BaseResponse(result, "Success", true));
                }

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        [HttpGet("/Ticket/GetTicketByTeacherId")]
        public IActionResult GetTicketByTeacherId(string teacherId)
        {
            try
            {
                var context = new FaceIOContext();
                if (string.IsNullOrEmpty(teacherId))
                {
                    return NotFound(new BaseResponse(null, "teacher ID cannot be blank", false));
                }
                else
                {
                    var listIdTicket = context.StudentTeacherTicket.Where(x => x.TeacherId == teacherId).Select(x => x.TicketId)
                        .ToList();

                    var tickets = context.Ticket.Where(x => listIdTicket.Contains(x.Id)).ToList();

                    var result = new List<Object>();

                    foreach (var ticket in tickets)
                    {
                        result.Add(new
                        {
                            id = ticket.Id,
                            content = ticket.Content,
                            startDate = ticket.StartDate,
                            endDate = ticket.EndDate,
                            status = ticket.Status,
                            studentId = context.StudentTeacherTicket.Where(x => x.TicketId == ticket.Id).FirstOrDefault()
                                .StudentId
                        });
                    }

                    return Ok(new BaseResponse(result, "Success", true));
                }

                return Ok();

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));

            }
        }

        [HttpPost("/Ticket/UpdateTicket")]
        public IActionResult UpdateTicket(Ticket ticket)
        {
            try
            {
                var context = new FaceIOContext();
                if (ticket == null)
                {
                    return NotFound(new BaseResponse(null, "Ticket cannot be Null", false));
                }
                else
                {
                    context.Ticket.Update(ticket);

                    context.SaveChanges();

                    return Ok(new BaseResponse(null, "Success", true));
                }

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));

            }
        }
    }
}