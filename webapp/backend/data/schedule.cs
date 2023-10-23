public class Schedule {
    public int ScheduleId { get; set; }
    public string Name { get; set; }
    public int ClassId { get; set; }
    public virtual Class Class { get; set; }
};