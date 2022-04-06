'use strict';

console.log('Hello from JavaScript');

void (async () => {
    const res = await fetch('/index.js');
    const text = await res.text();
    console.log(text);
})();
