window.homenestAuth = {
    login: async function (email, password) {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password }),
            credentials: 'include'
        });
        return response.ok;
    },
    logout: async function () {
        await fetch('/api/auth/logout', {
            method: 'POST',
            credentials: 'include'
        });
    }
};
