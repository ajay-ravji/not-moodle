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
    });

    coursesElement.innerHTML += `<div class="course">
        <div class="course-header">
            <div>
                <p> Add new course </p>
            </div>
        </div>
        <p class="course-name-header"> Name </p>
        <input id="course-name-input" name="name">
        <button id="course-add-button" onclick="createNewCourse();"> Add </button>
    </div>`;

    courses.forEach(course => {
        let deleteButton = document.getElementById(`delete-button-${course.courseId}`);
        
        deleteButton.onclick = async () => {
            await fetch(`/api/course/delete?id=${course.courseId}`, {
                method: "DELETE",
                headers: {
                    "Content-type": "application/json; charset=UTF-8"
                }
            });

            window.location.reload();
        };
    })
}

async function createNewCourse() {
    let courseName = document.getElementById("course-name-input").value;

    let response = await fetch("/api/course/create", {
        method: "POST",
        body: new URLSearchParams({name: courseName})
    });

    if (response.ok) {
        location.reload();
    }
}

renderCourses();