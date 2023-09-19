using System.ComponentModel.DataAnnotations.Schema;

public class Enrolment {
    public int ClassId { get; set; }
    public virtual Class Class { get; set; }
    public int StudentId { get; set; }
    public virtual User Student { get; set; }
}