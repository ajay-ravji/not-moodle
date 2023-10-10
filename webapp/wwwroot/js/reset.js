async function handleReset() {
    let username = document.getElementById("username-input").value;
    let password = document.getElementById("password-input").value;

    let response = await fetch(`/api/auth/reset`, {
        method: "POST",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({
            username: username,
            password: password
        })
    });

    let errorMessage = document.getElementById("form-message");
    errorMessage.classList.remove("hidden");

    if (response.ok) {
        errorMessage.innerHTML = "Reset successful";
    } else {
        errorMessage.innerHTML = "Unknown error";
    }
}