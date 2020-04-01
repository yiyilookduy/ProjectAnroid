package dto;

public class Class_Subject extends Subject{
    String StartTime, EndTime, Day, Date;
    int slot;

    public Class_Subject(String id, String name, String startTime, String endTime) {
        super(id, name);
        StartTime = startTime;
        EndTime = endTime;
    }

    public Class_Subject(String id, String name, String day, String date, int slot) {
        super(id, name);
        Day = day;
        Date = date;
        this.slot = slot;
    }

    public Class_Subject(String id, String name, String day, int slot) {
        super(id, name);
        Day = day;
        this.slot = slot;
    }

    public Class_Subject(String day, int slot) {
        Day = day;
        this.slot = slot;
    }

    public Class_Subject(String day, String date, int slot) {
        Day = day;
        Date = date;
        this.slot = slot;
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
