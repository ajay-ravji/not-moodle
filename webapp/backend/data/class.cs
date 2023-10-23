public class Class {
    public int ClassId { get; set; }
    public string Name { get; set; }
    public int CourseId { get; set; }
    public virtual Course Course { get; set; }
    public int LecturerId { get; set; }
    public virtual User Lecturer { get; set; }
};