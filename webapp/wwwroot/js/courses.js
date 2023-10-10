<<<<<<< HEAD
async function renderCourses() {
    let response = await fetch('/api/course/all');
    if (!response.ok) return;
    console.log(response);

    let courses = (await response.json());
    console.log(courses);
    if (courses === undefined) return;
    
    let coursesElement = document.getElementById('courses');
    
    courses.forEach(course => {
        coursesElement.innerHTML += `<div class="course">
        <div class="course-header">
            <div>
                <p> ${course.name} </p>
            </div>
            <button id="delete-button-${course.courseId}" class="course-delete-button"> X </button>
        </div>
        <p class="course-name-header"> Name </p>
        <p class="course-name"> ${course.name}  </p>
    </div>`;
=======
async function deleteCourse(id) {
    await fetch(`/api/course/delete`, {
        method: "DELETE",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "id": id.toString()
        })
>>>>>>> main
    });
    location.reload();
}

async function createNewCourse() {
    let courseName = document.getElementById("course-name-input").value;

    let response = await fetch("/api/course/create", {
        method: "POST",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "name": courseName
        })
    });

    if (response.ok) {
        location.reload();
    }
}