async function logout() {
    let response = await fetch("/api/auth/logout", {
        method: "POST"
    });

    if (response.ok) {
        window.location = "/";
    }
}