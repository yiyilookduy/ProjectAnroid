package dto;

public class Teacher extends user{
    String id;
    String personaId;
    String LPGld;
    Boolean active = false;

    public Teacher(String username, String password, int roleId) {
        super(username, password, roleId);
    }

    public Teacher(String username, String password, int roleId, Boolean active) {
        super(username, password, roleId);
        this.active = active;
    }

    public Teacher() {

    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getPersonaId() {
        return personaId;
    }

    public void setPersonaId(String personaId) {
        this.personaId = personaId;
    }

    public String getLPGld() {
        return LPGld;
    }

    public void setLPGld(String LPGld) {
        this.LPGld = LPGld;
    }

    @Override
    public Boolean getActive() {
        return active;
    }

    @Override
    public void setActive(Boolean active) {
        this.active = active;
    }
}
