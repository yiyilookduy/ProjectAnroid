package utils;

import android.util.Log;

import org.json.JSONArray;
import org.json.JSONObject;

import dao.StudentDAO;
import dao.TeacherDAO;
import dto.Student;
import dto.Teacher;
import dto.user;

public class JSONManipulator {
    private String usernameJsonData = "";
    private String passwordJsonData = "";
    private int roleIdJsonData = 0;
    private Boolean activeJsonData = false;
    StudentDAO studentDAO = new StudentDAO();
    TeacherDAO teacherDAO = new TeacherDAO();


}
