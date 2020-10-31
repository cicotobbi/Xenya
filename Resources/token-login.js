setInterval(() => {
document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `"%TOKEN_HERE%"`
}, 50);
setTimeout(() => {
location.reload();
}, 2500);