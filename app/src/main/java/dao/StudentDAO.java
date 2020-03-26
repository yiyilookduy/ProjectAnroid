package dao;

import dto.Student;

public class StudentDAO {
    public Student setStudentData(String usernameJsonData,String passwordJsonData,int roleIdJsonData,boolean activeJsonData){
        Student student = null;
        student.setUsername(usernameJsonData);
        student.setPassword(passwordJsonData);
        student.setRoleId(roleIdJsonData);
        student.setActive(activeJsonData);
        return student;
    }
}
