package dto;

public class Student extends user{
    String id;
    String personId;
    String LPGld;
    Boolean active = false;

    public Student() {
    }

    public Student(String username, String password, int roleId, String personId) {
        super(username, password, roleId);
        this.personId = personId;
    }

    public Student(String username, String password, int roleId) {
        super(username, password, roleId);
    }

    public Student(String username, String password, int roleId, Boolean active) {
        super(username, password, roleId);
        this.active = active;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getPersonId() {
        return personId;
    }

    public void setPersonId(String personId) {
        this.personId = personId;
    }

    public String getLPGld() {
        return LPGld;
    }

    public void setLPGld(String LPGld) {
        this.LPGld = LPGld;
    }

}
