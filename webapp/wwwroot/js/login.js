async function handleLogin() {
    let username = document.getElementById("username-input").value;
    let password = document.getElementById("password-input").value;

    let response = await fetch(`/api/auth/login`, {
        method: "POST",
        headers: {
            "Content-type": "application/json"
        },
        body: JSON.stringify({
            username: username,
            password: password
        })
    });

    if (response.ok) {
        window.location = "/";
    }
}