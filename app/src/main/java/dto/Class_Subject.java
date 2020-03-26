package dto;

public class Class_Subject extends Subject{
    String StartTime, EndTime;

    public Class_Subject(String id, String name, String startTime, String endTime) {
        super(id, name);
        StartTime = startTime;
        EndTime = endTime;
    }

    public String getStartTime() {
        return StartTime;
    }

    public void setStartTime(String startTime) {
        StartTime = startTime;
    }

    public String getEndTime() {
        return EndTime;
    }

    public void setEndTime(String endTime) {
        EndTime = endTime;
    }
}
