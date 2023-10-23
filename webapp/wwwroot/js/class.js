async function deleteSchedule(id) {
    await fetch(`/api/schedule/delete`, {
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

async function createNewSchedule(classId) {
    let scheduleName = document.getElementById("schedule-name-input").value;

    let response = await fetch("/api/schedule/create", {
        method: "POST",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "classId": classId.toString(),
            "name": scheduleName,
        })
    });

    if (response.ok) {
        location.reload();
    }
}