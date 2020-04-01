package dto;

public class Ticket {
    String id, content, startDate, EndDate, Status;

    public Ticket(String id, String content, String startDate, String endDate, String status) {
        this.id = id;
        this.content = content;
        this.startDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    public Ticket() {
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public String getStartDate() {
        return startDate;
    }

    public void setStartDate(String startDate) {
        this.startDate = startDate;
    }

    public String getEndDate() {
        return EndDate;
    }

    public void setEndDate(String endDate) {
        EndDate = endDate;
    }

    public String getStatus() {
        return Status;
    }

    public void setStatus(String status) {
        Status = status;
    }
}
