async function deleteCourse(id) {
    await fetch(`/api/course/delete`, {
        method: "DELETE",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "id": id.toString()
        })
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