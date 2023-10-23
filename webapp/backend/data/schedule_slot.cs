public class ScheduleSlot {
    public int ScheduleSlotId { get; set; }
    public int ScheduleId { get; set; }
    public virtual Schedule Schedule { get; set; }
    public DateTime Time { get; set; }
    public int? StudentId { get; set; }
    public virtual User Student { get; set; }
}