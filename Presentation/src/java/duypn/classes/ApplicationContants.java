/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package duypn.classes;

/**
 *
 * @author duypnse63523
 */
public class ApplicationContants {
    public static final String ERROR = "error.jsp";
    public static final String ADMIN = "admin.jsp";
    public static final String PAGE_ATTENDANCE = "Attendance.jsp";
    
    //IP
    public static final String IP = "http://171.245.197.16:8080";

    
    //API Link
    public static final String URLPOSTLOGIN = IP + "/Home/Login";
    public static final String URL_GET_SCHEDULE_CURRENTWEEK = IP + "/GetScheduleOnCurrentWeek";
    public static final String URL_GET_SCHEDULE_ONWEEK = IP + "/Attendance/GetScheduleOnWeek";

    
    //
    public static final String PROFILECONTROLLER ="ProfileController";
    public static final String PROFILE = "Profile.jsp";
    public static final String LOGINCONTROLLER = "LoginController";
    public static final String LOGIN = "Login.jsp";
    
    //
    public static final String UPDATEPROFILECONTROLLER = "UpdateProfileController";
    public static final String ATTENDANCE_CONTROLLER = "AttendanceController";
    public static final String SEARCH_CONTROLLER = "SearchScheduleController";

    
    // CRUD
    public static final String EDITCONTROLLER = "EditController";
} 
