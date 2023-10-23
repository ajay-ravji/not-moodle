async function deleteSlot(id) {
    await fetch(`/api/slot/delete`, {
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

async function createNewSlot(scheduleId) {
    let slotTime = document.getElementById("slot-time-input").value;

    let response = await fetch("/api/slot/create", {
        method: "POST",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "scheduleId": scheduleId.toString(),
            "time": slotTime,
        })
    });

    if (response.ok) {
        location.reload();
    }
}

async function claimSlot(slotId) {
    let response = await fetch("/api/slot/claim", {
        method: "POST",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "id": slotId.toString(),
        })
    });

    if (response.ok) {
        location.reload();
    }
}

async function resetSlot(slotId) {
    let response = await fetch("/api/slot/reset", {
        method: "POST",
        headers: {
            "Content-type": "application/json;"
        },
        body: JSON.stringify({
            "id": slotId.toString(),
        })
    });

    if (response.ok) {
        location.reload();
    }
}