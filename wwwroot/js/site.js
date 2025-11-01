// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

<script>
    const signupPanel = document.getElementById("signup-panel");
    const loginPanel = document.getElementById("login-panel");
    document.getElementById("show-login").onclick = () => {
        signupPanel.style.transform = "translateX(-100%)";
    loginPanel.style.transform = "translateX(0%)";
    };
    document.getElementById("show-signup").onclick = () => {
        signupPanel.style.transform = "translateX(0%)";
    loginPanel.style.transform = "translateX(100%)";
    };

</script>
