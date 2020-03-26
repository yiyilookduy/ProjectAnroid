package dao;

import dto.Teacher;

public class TeacherDAO {
    public Teacher setTeacherData(String username,String password,int roleIdJson, boolean active){
        Teacher teacher = null;
            teacher.setUsername(username);
            teacher.setPassword(password);
            teacher.setRoleId(roleIdJson);
            teacher.setActive(active);
            return teacher;
        }
}
