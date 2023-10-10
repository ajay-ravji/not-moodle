public class Class {
    public int ClassId { get; set; }
    public Course Course { get; set; }
    public int LecturerId { get; set; }
    public virtual User Lecturer { get; set; }
};