'use strict';

const messageBody = document.getElementById('messageBody');
const submitButton = document.getElementById('submitButton');
const timeline = document.getElementById('timeline');

const msgElement = document.createElement('div');
msgElement.textContent = 'Saluton!';
timeline.appendChild(msgElement);
