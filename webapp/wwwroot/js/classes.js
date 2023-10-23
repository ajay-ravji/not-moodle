async function deleteClass(id) {
    await fetch(`/api/class/delete`, {
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

async function createNewClass() {
    let className = document.getElementById("class-name-input").value;
    let courseId = document.getElementById("class-course-input").value;
    let lecturerId = document.getElementById("class-lecturer-input").value;

    let response = await fetch("/api/class/create", {
        method: "POST",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "name": className,
            "courseId": courseId,
            "lecturerId": lecturerId,
        })
    });

    if (response.ok) {
        location.reload();
    }
}