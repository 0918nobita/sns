'use strict';

const messageBody = document.getElementById('messageBody');
const submitButton = document.getElementById('submitButton');
const timeline = document.getElementById('timeline');

void (async () => {
    const res = await fetch('/timeline');
    const comments = await res.json();
    for (const comment of comments) {
        const msgElement = document.createElement('div');
        msgElement.textContent = comment;
        timeline.appendChild(msgElement);
    }
})();
